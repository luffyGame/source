using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace FrameWork
{
    public class TcpClient : NetClient
    {
        private Socket socket;
        private volatile NetState state;
        private volatile bool isSending;
        private volatile bool isReceiving;
        private ByteBuf remainder;//接收剩余
        private Queue<Protocol> sendPrtcls = new Queue<Protocol>();
        public Action<Message> messageHandler { get; set; }
        public TcpClient()
        {
            state = NetState.DISCONNECTED;
            remainder = ByteCache.Alloc(1024);
        }
        public void Connect(string host,int port)
        {
            if (state == NetState.CONNECTING || state == NetState.CONNECTED)
                return;
            state = NetState.CONNECTING;
            IPAddress ipAddress = Dns.GetHostAddresses(host)[0];
            AddressFamily addressFamily = GetAddressFamily(ipAddress);
            socket = new Socket(addressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.BeginConnect(ipAddress, port, asyncResult =>
                {
                    try
                    {
                        socket.EndConnect(asyncResult);
                        state = NetState.CONNECTED;
                        OnConnected();
                    }
                    catch(Exception ex)
                    {
                        state = NetState.DISCONNECTED;
                        OnConnectFail(ex.Message);
                    }
                }, null);
            }catch(Exception ex)
            {
                state = NetState.DISCONNECTED;
                OnConnectFail(ex.Message);
            }
        }
        private void OnConnected()
        {
            Thread run = new Thread(SendAndReceive);
            run.Name = "net";
            run.Start();
            HandleMessage(new NetStateMsg().ConnectSuccess());
        }
        private void OnConnectFail(string msg)
        {
            HandleMessage(new NetStateMsg().ConnectFail(msg));
        }
        private void OnError(string msg)
        {
            if(null != socket||state == NetState.CONNECTED)
            {
                HandleMessage(new NetStateMsg().Error(msg));
                Disconnect();
            }
        }
        private void SendAndReceive()
        {
            ByteBuf buffer = ByteCache.Alloc(1024);
            while(state == NetState.CONNECTED)
            {
                if (!isSending)
                    Send();
                if(!isReceiving)
                {
                    Receive(buffer);
                }
                Thread.Sleep(1);
            }
        }
        public void Disconnect()
        {
            if (state == NetState.DISCONNECTING || state == NetState.DISCONNECTED)
                return;
            state = NetState.DISCONNECTING;
            if (null != socket)
            {
                try
                {
                    socket.Close();
                }
                catch (Exception ex)
                {
                    Debugger.Log(ex);
                }
                socket = null;
            }

        }
        private void Send()
        {
            if (socket == null || state != NetState.CONNECTED)
                return;
            Protocol one = null;
            lock (sendPrtcls)
            {
                if(sendPrtcls.Count>0)
                    one = sendPrtcls.Dequeue();
            }
            if (null != one)
            {
                ByteBuf raw = EncodeAndPackage(one);
                DoSend(raw);
            }
        }
        private void DoSend(ByteBuf buf)
        {
            try
            {
                isSending = true;
                socket.BeginSend(buf.buf, 0, buf.len, SocketFlags.None, asyncResult =>
                    {
                        try
                        {
                            int l = socket.EndSend(asyncResult);
                            buf.Dispose();
                            isSending = false;
                        }catch(Exception ex)
                        {
                            buf.Dispose();
                            isSending = false;
                            OnError(ex.Message);
                        }
                    }, null);
            }catch(Exception ex)
            {
                buf.Dispose();
                isSending = false;
                OnError(ex.Message);
            }
        }
        private void Receive(ByteBuf buf)
        {
            if (socket == null || state != NetState.CONNECTED)
                return;
            try
            {
                isReceiving = true;
                socket.BeginReceive(buf.buf, 0, buf.maxSize, SocketFlags.None, asyncResult =>
                {
                    try
                    {
                        buf.len = socket.EndReceive(asyncResult);
                        HandleReceive(buf);
                        isReceiving = false;
                    } catch (Exception ex)
                    {
                        isReceiving = false;
                        OnError(ex.Message);
                    }
                }, null);
            }catch(Exception ex)
            {
                isReceiving = false;
                OnError(ex.Message);
            }
        }

        private void HandleReceive(ByteBuf data)
        {
            if(!remainder.IsEmpty)
            {
                if (!remainder.CanAppend(data))
                    remainder = ByteCache.Reserve(remainder,remainder.len + data.len);
                remainder.Append(data);
                data = remainder;
                remainder = ByteCache.Alloc(1024);
            }
            int cursor = 0;

            while(data.len-cursor>4)
            {
                int length = data.GetInt(cursor);
                if (data.len - cursor >= 4 + length)
                {
                    cursor += 4;
                    HandleRawData(data, cursor, length);
                    cursor += length;
                }
                else
                    break;
            }
            if (data.len > cursor)
            {
                if(!remainder.CanAppend(data,cursor))
                    remainder = ByteCache.Reserve(remainder, remainder.len + data.len - cursor);
                remainder.Append(data, cursor);
            }
        }
        public override void SendPrtcl(Protocol protocol)
        {
            if (state != NetState.CONNECTED)
                return;
            lock(sendPrtcls)
            {
                sendPrtcls.Enqueue(protocol);
            }
        }
        protected void HandleRawData(ByteBuf raw, int offset, int length)
        {
            if (state != NetState.CONNECTED)
                return;
            ByteBuf buf = ByteCache.Alloc(length);
            buf.Copy(raw, offset, length);
            Protocol p = Decode(buf);
            if(null!=p)
            {
                HandleMessage(p);
            }
        }
        protected void HandleMessage(Message m)
        {
            if (null != messageHandler)
                messageHandler(m);
        }
        
        private static AddressFamily GetAddressFamily(IPAddress ipAddress)
        {
            return ipAddress.AddressFamily == AddressFamily.InterNetworkV6 ? AddressFamily.InterNetworkV6 : AddressFamily.InterNetwork;
        }
    }
}
