using System;
using System.Collections.Generic;
using System.Text;

using UnityEngine;

namespace FrameWork
{
    public class LogExecutor : AsynTaskExecutor, ILogger
    {
        /// <summary>
        /// log文件名，task中填的type为其索引，此处可修改添加
        /// </summary>
        private static readonly string s_logFile = "log";
        #region InsideClass
        private class LogTask : AsynTask
        {
            private string m_str;
            public FileLog m_flog { get; private set; }
            public LogTask(FileLog flog, string str)
			{
                m_flog = flog;
				m_str = str;
			}
			public override void Execute()
            {
                m_flog.WriteLog(m_str);
                base.Execute();
            }
        }
        #endregion
        #region Variables
        private static string s_LogPath = "/Log/";
		public static string LogPath {get {return s_LogPath;}}
        private FileLog flog;
        #endregion
        #region Cons
        public LogExecutor():base("Log",true){}
        #endregion
        #region Other Called
        public void Log(string str,bool upLoadLog = false)
        {
			LogTask task = new LogTask(flog,str);
			if(upLoadLog)
				task.CallBackHandler = Upload;
            AddTask(task);
        }
        public void Log(string msg, string stack, LogType type)
        {
            Log(msg);
        }
        #endregion
        #region Self Called
        private static void CreateLogDirectory()
        {
            s_LogPath = BaseUtil.FileUtility.DirRoot2Path + s_LogPath;
            if (!System.IO.Directory.Exists(s_LogPath))
                System.IO.Directory.CreateDirectory(s_LogPath);
        }
        protected override void OnStartSuccess()
        {
            CreateLogDirectory();
            System.IO.File.Delete(s_LogPath + s_logFile);
            flog = new FileLog(s_LogPath + s_logFile);
        }
        protected override void OnClosed()
        {
            if(null!=flog)
            {
                flog.Close();
                flog = null;
            }
        }
        private static void Upload(AsynTask task)
		{
			LogTask ltask = task as LogTask;
			string url = Global.Instance.LogServerUrl;
			if(string.IsNullOrEmpty(url))
				return;
			string saveName = "";//获取机器唯一标志
			if(string.IsNullOrEmpty(saveName))
				return;
			WebExecWorker.Instance.UploadFile(WebExecWorker.DEFAULT_ID,url,ltask.m_flog.path,"file",saveName,true,true);
		}
        #endregion
    }
}
