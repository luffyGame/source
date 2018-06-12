using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Security.Cryptography;

namespace FrameWork.BaseUtil
{
    /// <summary>
    /// 
    /// </summary>
    public static class FileUtility
    {
		//默认文件目录根
		private static readonly string DIR_ROOT = Application.streamingAssetsPath;
		//热更新的目录，拥有资源加载的更高优先级
		private static string DIR_ROOT_2 = null;
        //lua头
        public static readonly byte[] fileHead = { 0x11, 0x22, 0x33, 0x44 };

        //lua zip密码
        private static string zipPswd = null;//"p0Qi6+zl64FoQg6=21jeSBbXwF&JDSWHgK7!";
        public static string ZipPswd
        {
            get { return zipPswd; }
            set { zipPswd = value; }
        }
        public static string DirRoot2Path
        {
            get
            {
                if (null == DIR_ROOT_2)
                {
                    switch (Application.platform)
                    {
                        case RuntimePlatform.WindowsEditor:
                        case RuntimePlatform.OSXEditor:
                            DIR_ROOT_2 = Application.dataPath;
                            break;
                        default: DIR_ROOT_2 = Application.persistentDataPath;
                            break;
                    }
                }
                return DIR_ROOT_2;
            }
            set { DIR_ROOT_2 = value; }
        }
        public static string GetFileReadPath(string filename,bool senior)
        {
            if(senior)
                return Path.Combine(DirRoot2Path, filename);
            else
                return Path.Combine(DIR_ROOT, filename);
        }

        public static bool IsFileFullPathSenior(string filename)
        {
            string fileFullPath = Path.Combine (DirRoot2Path, filename);
            if (File.Exists(fileFullPath))
                return true;
            return false;
        }
		
		public static string GetFileReadFullPath (string filename,bool checkInside = true)
		{
			string fileFullPath = Path.Combine (DirRoot2Path, filename);
			if (File.Exists (fileFullPath))
				return fileFullPath;
		    if (checkInside)
		    {
			    fileFullPath = Path.Combine(DIR_ROOT, filename);
			    if (!fileFullPath.Contains("://") && File.Exists(fileFullPath))
				    return fileFullPath;
		    }
            return null;
		}

		public static string CreateDirectory(string relativePath,bool delExist = false)
		{
			string fullDirPath = Path.Combine (DirRoot2Path, relativePath);
            if (Directory.Exists(fullDirPath))
            {
                if (delExist)
                    Directory.Delete(fullDirPath, true);
                else
                    return fullDirPath;
            }
			Directory.CreateDirectory(fullDirPath);
			return fullDirPath;
		}

		public static string GetFileWriteFullPath(string filename)
		{
			return Path.Combine (DirRoot2Path, filename);
		}

        /// <summary>
        /// 文件打开，对于包内文件只用于非压缩格式的
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static Stream OpenFile(string filePath)
        {
            Stream stream = null;
            if (filePath.Contains("://"))
            {
				using(WWW www = new WWW(filePath))
				{
					while (!www.isDone) ;//注意：这个只用于非压缩格式的，而assetbundle(默认是7z）压缩
					if(null != www.error)
						throw new Exception(filePath);
					else
						stream = new MemoryStream(www.bytes);
					www.Dispose();
				}
            }
            else
            {
				if(File.Exists(filePath))
                	stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
            return stream;
        }

		public static bool IsFileExist(string filePath)
		{
			if (filePath.Contains("://"))
			{
				using(WWW www = new WWW(filePath))
				{
					while (!www.isDone) ;
					return www.error == null;
				}
			}
			else
			{
				return File.Exists(filePath);
			}
		}
		public static byte[] GetFileBytes(string filePath)
		{
			try
			{
				if (filePath.Contains("://"))
				{
					using(WWW www = new WWW(filePath))
					{
						while (!www.isDone) ;
						if(null != www.error)
							throw new Exception(filePath);
						else
							return www.bytes;
					}
				}
				else
				{
					using(FileStream fs = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
					{
						byte[] bytes = new byte[fs.Length];
						fs.Read(bytes,0,bytes.Length);
						return bytes;
					}
				}
			}
			catch(Exception ex)
			{
                Debugger.LogError("GetFileBytes err : {0}",ex.Message);
                return null;
			}
		}

        public static void WriteFile(string filePath, byte[] data)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (MemoryStream stream = new MemoryStream(data))
                {
                    CopyStream(stream, fs);
                }
            }
        }

        public static void CopyStream(Stream input, Stream output)
        {
            int bufferSize = 2048;
            bool isSuccessed = false;
            try
            {
                byte[] buffer = new byte[bufferSize];
                while (true)
                {
                    int read = input.Read(buffer, 0, bufferSize);
                    if (read <= 0)
                    {
                        isSuccessed = true;
                        break;
                    }
                    output.Write(buffer, 0, read);
                }
            }
            catch (Exception ex)
            {
                Debugger.Log("fail to copy stream: {0}", ex.Message);
            }
            if (isSuccessed)
                output.Flush();
        }

	    public static byte[] ReadAllBytes(this Stream stream)
	    {
		    if (stream is MemoryStream)
			    return ((MemoryStream) stream).ToArray();
		    using (MemoryStream ms = new MemoryStream())
		    {
			    CopyStream(stream,ms);
			    return ms.ToArray();
		    }
	    }
        public static long GetFileSize(string fileName)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    FileInfo info = new FileInfo(fileName);
                    return info.Length;
                }
            }
            catch
            {
                return -1;
            }
            return 0;
        }

        public static string GetFileMd5(string fileName)
        {
            try
            {
                byte[] fileBuffer = File.ReadAllBytes(fileName);
                MD5 md5Hash = MD5.Create();
                byte[] result = md5Hash.ComputeHash(fileBuffer);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < result.Length; i++)
                {
                    sb.AppendFormat("{0:x2}", result[i]);
                }
                return sb.ToString();
            }
            catch(Exception ex)
            {
                return ex.Message;
            }
        }

        public static bool RemoveFile(string fileName)
        {
            try
            {
                if(File.Exists(fileName))
                    File.Delete(fileName);
                return true;
            }
            catch(Exception ex)
            {
                Debugger.LogError("fail del file {0}:{1}", fileName, ex.Message);
                return false;
            }
        }

        public static bool RemoveDir(string dir)
        {
            try
            {
                if(Directory.Exists(dir))
                    Directory.Delete(dir, true);
                return true;
            }
            catch(Exception ex)
            {
                Debugger.LogError("fail del dir {0}:{1}", dir, ex.Message);
                return false;
            }
        }

        public static MemoryStream LZMACompress(Stream input)
        {
            SevenZip.Compression.LZMA.Encoder coder = new SevenZip.Compression.LZMA.Encoder();
            MemoryStream output = new MemoryStream();
            coder.WriteCoderProperties(output);
            output.Write(BitConverter.GetBytes(input.Length), 0, 8);
            coder.Code(input, output, input.Length, -1, null);
            output.Flush();
            return output;
        }
        public static MemoryStream LZMADecompress(Stream input)
        {
            SevenZip.Compression.LZMA.Decoder coder = new SevenZip.Compression.LZMA.Decoder();
            byte[] properties = new byte[5];
            input.Read(properties, 0, 5);
            byte[] lengthBytes = new byte[8];
            input.Read(lengthBytes, 0, 8);
            long length = BitConverter.ToInt64(lengthBytes, 0);
            coder.SetDecoderProperties(properties);
            MemoryStream output = new MemoryStream((int)length);
            coder.Code(input, output, input.Length, length, null);
            output.Flush();
            return output;
        }
     }
}
