using UnityEngine;
using System.Collections;
using System.Text;
using System.Net;
using System.IO;
using System.Collections.Generic;
using System;

namespace FrameWork
{
	#region WebTaskClass
    public delegate void OnTaskDone(bool isSuccess,string msg);
    public abstract class WebCbTask : UnityCbTask
    {
        protected OnTaskDone cb;
        protected string ret;
        protected bool isSuccess;
        public WebCbTask(OnTaskDone _cb)
        {
            cb = _cb;
            ret = null;
            isSuccess = false;
        }
        public override void UpdateExec()
        {
            if (cb != null)
            {
                cb(isSuccess, ret);
                cb = null;
            }
        }
    }
	public class WebTask : WebCbTask
	{
		private string url;
		private string head;
		private string body;
		private string method;
		private string contentType;
		private int timeOut;
		public WebTask(string _url,string _method,string _head, string _body,string _contentType,
		               int _timeOut,OnTaskDone _cb):base(_cb)
		{
			url = _url;
			method = _method;
			head = _head;
			body = _body;
			timeOut = _timeOut;
			contentType = _contentType;
		}
		
		public override void Execute ()
		{
			HttpRequest();
            base.Execute();
		}
		
		private void HttpRequest()
		{
			if(string.IsNullOrEmpty(method))
				method = "POST";
			if(!string.IsNullOrEmpty(head))
				url = url + '?' + head;
			try
			{
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = method;
				request.Timeout = timeOut;
				request.Proxy = null;
				if(method == "POST")
				{
					if(string.IsNullOrEmpty(body))
						request.ContentLength = 0;
					else
					{
						if(!string.IsNullOrEmpty(contentType))
							request.ContentType = contentType;
						byte[] postData = Encoding.UTF8.GetBytes(body);
						request.ContentLength = postData.Length;
						using(Stream writeStream = request.GetRequestStream())
						{
							writeStream.Write(postData,0,postData.Length);
						}
					}
				}
				using(HttpWebResponse response = request.GetResponse() as HttpWebResponse)
				{
					isSuccess = (response.StatusCode == HttpStatusCode.OK);
					if(isSuccess)
					{
						using(Stream dataStream = response.GetResponseStream())
						{
							using(StreamReader sr = new StreamReader(dataStream,Encoding.UTF8))
							{
								ret = sr.ReadToEnd();
							}
						}
					}
					else
						ret = string.Format("response code:{0}",response.StatusCode);
				}
			}
			catch(WebException ex)
			{
				isSuccess = false;
				ret = ex.Message;
				Debugger.LogError("HttpRequest url {0}: err = {1}",url,ex);
			}
		}
	}
	public class HttpUpLoadTask : WebCbTask
	{
		private string uploadFile;
		private string url;
        private string fileKey;
		private string saveName;
        private string[] header;//处文件信息外的其他参数，由key和value顺序构成
		private bool compress;
		private string contentType;
		private int timeOut;
		public HttpUpLoadTask(string _url,string[] header ,string _uploadFile, string fileKey,string _saveName,bool addInfo,
		                      bool _compress,string _contentType, int _timeOut,OnTaskDone _cb)
            :base(_cb)
		{
            url = _url;
            this.header = header;
			uploadFile = _uploadFile;
            this.fileKey = fileKey;
			
			if(addInfo)
			{
				string time = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
				saveName = string.Format("[{0}][{1}]{2}",time,Application.platform,_saveName);
			}
			else
				saveName = _saveName;
			compress = _compress;
			if(string.IsNullOrEmpty(_contentType))
				contentType = "application/octet-stream";
			else
				contentType = _contentType;
			timeOut = _timeOut;
		}
		
		public override void Execute ()
		{
			HttpUpload();
            base.Execute();
		}
		
		private void HttpUpload()
		{
			Stream inStream = null;
			try
			{
				if(!File.Exists(uploadFile))
				{
					ret = "file not exist";
					return;
				}
                //先关闭压缩
				if(compress)
					inStream = Utils.ZipFile(saveName,uploadFile);
				else
					inStream = new FileStream(uploadFile,FileMode.Open,FileAccess.Read,FileShare.ReadWrite);
				if(null == inStream)
					return;
                //分界线，用于服务器分割请求体，读取数据
				string boundary = "----------" + DateTime.Now.Ticks.ToString("x");
                //边界头
                string beginBoundary = string.Format("--{0}\r\n",boundary);
				StringBuilder sb = new StringBuilder();
                //1.开始分割线
				sb.Append(beginBoundary);
                //2.上传附带参数
                if (header != null)
                {
                    for (int i = 0; i < header.Length; i += 2)
                    {
                        sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}\r\n{2}", header[i], header[i + 1], beginBoundary);
                    }
                }
                //3.文件头数据体
                sb.AppendFormat("Content-Disposition: form-data; name=\"{0}\";filename=\"{1}\"\r\nContent-Type: {2}\r\n\r\n", fileKey,saveName, contentType);  
				
				string postHeader = sb.ToString();  
				byte[] postHeaderBytes = Encoding.UTF8.GetBytes(postHeader);
				// Build the trailing boundary string as a byte array  
				// ensuring the boundary appears on a line by itself  
                //4.边界结尾
				byte[] boundaryBytes =  Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Proxy = null;//no proxy used,or set to none?
				request.Method = "POST";
                request.KeepAlive = true;
				//对发送的数据不使用缓存   
				request.AllowWriteStreamBuffering = false;
				request.Timeout = timeOut;
				request.ContentType = "multipart/form-data; boundary=" + boundary;
				long length = postHeaderBytes.Length + inStream.Length +  boundaryBytes.Length;
                request.ContentLength = length;
				using(Stream postStream = request.GetRequestStream())
				{
					// Write out our post header  
					postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
					
					byte[] buffer = new Byte[Math.Min(4096,(int)inStream.Length)];  
					int bytesRead = 0;  
					while ( (bytesRead = inStream.Read(buffer, 0, buffer.Length)) != 0 )  
						postStream.Write(buffer, 0, bytesRead);
					
					// Write out the trailing boundary  
					postStream.Write(boundaryBytes, 0, boundaryBytes.Length);
				}
				using(HttpWebResponse response = request.GetResponse() as HttpWebResponse)
				{
					isSuccess = (response.StatusCode == HttpStatusCode.OK);
					if(isSuccess)
					{
						using(Stream s = response.GetResponseStream())
						{
							using(StreamReader sr = new StreamReader(s,Encoding.UTF8))
							{
								ret = sr.ReadToEnd();
							}
						}
					}
					else
						ret = string.Format("response code:{0}",response.StatusCode);
				}
			}
			catch(WebException ex)
			{
				isSuccess = false;
				ret = ex.Message;
				Debugger.LogError("HttpRequest url {0}: err = {1}",url,ex.Message);
			}
			finally
			{
				if(null!=inStream)
				{
					inStream.Close();
					inStream = null;
				}
				GC.Collect();
			}
		}
	}
	public class HttpDownloadTask : WebCbTask
	{
		private string url;
		private string savePath;
		private int timeOut;
		public HttpDownloadTask(string _url,string _savePath, int _timeOut,OnTaskDone _cb):base(_cb)
		{
			url = _url;
			savePath = _savePath;
			timeOut = _timeOut;
		}
		public override void Execute ()
		{
			HttpDownload();
            base.Execute();
		}
		
		private void HttpDownload()
		{
			try
			{
				using(FileStream fs = new FileStream(savePath,FileMode.Create,FileAccess.Write))
				{
					WebRequest request = WebRequest.Create(url);
					request.Timeout = timeOut;
					request.Proxy = null;
					using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
					{
						if (response.StatusCode == HttpStatusCode.OK)
						{
							using (Stream stream = response.GetResponseStream())
							{
								int bytesRead;
								byte[] buffer = new byte[4096];
								while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) > 0)
								{
									fs.Write(buffer, 0, bytesRead);
								}
								isSuccess = true;
							}
						}
					}
				}
			}
			catch(Exception ex)
			{
				isSuccess = false;
				Debugger.LogError("DownloadFile {0}:err {1}", url, ex.Message);
			}
			finally
			{
				GC.Collect();
			}
		}
	}
	#endregion
	public class WebExecutor : UnityCbTaskExecutor
	{
		#region Var
		private int id;
		public int Id {get{return id;}}
		#endregion
		#region Cons
		public WebExecutor(int _id):base(string.Format("Web_{0}",_id),false,true){id = _id;}
		#endregion
	}
}