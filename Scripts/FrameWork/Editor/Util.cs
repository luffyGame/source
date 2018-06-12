using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using System.Diagnostics;
using System;
using System.Security.Cryptography;
using System.Text;

namespace FrameWork.Editor
{
	public class Util
	{
        private static readonly char[] seps = { '/', '\\' };
		public static List<FileInfo> GetFiles(string dirPath,string[] patterns = null,bool ignoreMeta = true)
		{
			DirectoryInfo directory = new DirectoryInfo(dirPath);
			List<FileInfo> ret = new List<FileInfo>();
			foreach(FileInfo f in directory.GetFiles("*.*",SearchOption.AllDirectories))
			{
				string fname = f.Name.ToLower();
				if(ignoreMeta && fname.EndsWith(".meta"))
					continue;
				if(null == patterns)
					ret.Add(f);
				else
				{
					foreach(string pattern in patterns)
					{
						if(f.Name.ToLower().EndsWith(pattern))
							ret.Add(f);
					}
				}
			}
			return ret;
		}

        public static bool RunCmd(string cmdExe, string cmdStr)
        {
            bool result = false;
            try
            {
                using (Process myPro = new Process())
                {
                    //指定启动进程是调用的应用程序和命令行参数
                    ProcessStartInfo psi = new ProcessStartInfo(cmdExe, cmdStr);
                    myPro.StartInfo = psi;
                    myPro.Start();
                    myPro.WaitForExit();
                    result = true;
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.LogError(string.Format("Is {0} exist?", cmdExe));
                UnityEngine.Debug.LogError(cmdStr);
                UnityEngine.Debug.LogError(ex);
            }
            return result;
        }
        /// <summary>
        /// 运行cmd命令
        /// 不显示命令窗口
        /// </summary>
        /// <param name="cmdExe">指定应用程序的完整路径</param>
        /// <param name="cmdStr">执行命令行参数</param>
        public static bool RunCmd2(string cmdExe, string cmdStr)
        {
            bool bRet = false;
            Process process = new Process();//创建进程对象
            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.FileName = "cmd.exe";//设定需要执行的命令
            startInfo.Arguments = "/c " + string.Format("{0} {1}", cmdExe, cmdStr);
            startInfo.UseShellExecute = false;//不使用系统外壳程序启动
            startInfo.RedirectStandardInput = true;//不重定向输入
            startInfo.RedirectStandardOutput = true; //重定向输出
            startInfo.RedirectStandardError = true;
            startInfo.CreateNoWindow = true;//不创建窗口
            process.StartInfo = startInfo;
            try
            {
                if (process.Start())//开始进程
                {
                    //process.StandardInput.WriteLine(string.Format("{0} {1}",cmdExe,cmdStr) + "&exit");
                    process.StandardInput.AutoFlush = true;
                    string output = process.StandardOutput.ReadToEnd();
                    UnityEngine.Debug.Log(output);
                    process.WaitForExit();
                    bRet = true;
                }
            }
            catch (Exception ex)
            {
                UnityEngine.Debug.Log(ex.Message);
                bRet = false;
            }
            finally
            {
                if (process != null)
                    process.Close();
            }
            return bRet;
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
            catch
            {
                return string.Empty;
            }
        }

		public static string GetObjectAbsolutPath(UnityEngine.Object obj)
		{
			string rootPath = Application.dataPath;
			rootPath = rootPath.Substring(0,rootPath.Length-"Assets".Length);
			return rootPath + AssetDatabase.GetAssetPath(obj);
		}

        public static void CreateDir(string dirPath, bool reIfExist)
        {
            if (reIfExist)
            {
                if (Directory.Exists(dirPath))
                    Directory.Delete(dirPath, true);
                Directory.CreateDirectory(dirPath);
            }
            else
            {
                if (!Directory.Exists(dirPath))
                    Directory.CreateDirectory(dirPath);
            }
        }
        public static string LowFileName(string filePath)
        {
            int first = filePath.LastIndexOfAny(seps)+1;
            string folderpath = filePath.Substring(0,first);
            int last = filePath.IndexOf('.',first);
            string fileName = last > 0 ? filePath.Substring(first, last - first) : filePath.Substring(first);
            string post = last > 0 ? filePath.Substring(last) : "";
            return folderpath + fileName.ToLower() + post;
        }

        public static string GenerateFilename(string dir, string extension)
        {
            string projectPath = System.IO.Path.GetDirectoryName(Application.dataPath) + "/";
            string path;
            do
            {
                string uuid = System.Guid.NewGuid().ToString();

                string Name = String.Format("{0}-{1}.{2}",
                        System.DateTime.Now.ToString("yyyyMMMdd-HHmmss"),uuid, extension);
                path = System.IO.Path.Combine(dir, Name);
            } while (System.IO.File.Exists(projectPath + path));
            return path;
        }
	}
}
