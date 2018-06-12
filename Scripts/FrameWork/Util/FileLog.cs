using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FrameWork
{
    public class FileLog
    {
        #region Variable
        private StreamWriter sw = null;
        public string path { get; set; }
        #endregion
        public FileLog(string logFile)
        {
            path = logFile;
            InitStreamWriter(logFile);
        }
        #region Public Method
        /// <summary>
        /// 写日志，不得抛出异常，以防LogExecutor异常嵌套
        /// </summary>
        /// <param name="str"></param>
        public void WriteLog(string str)
        {
            try
            {
                sw.WriteLine(str);
            }
            catch (Exception ex)
            {
#if UNITY_EDITOR
                UnityEngine.Debug.LogError(string.Format("write log err:{0}", ex.Message));
#endif
            }
        }
        public void Close()
        {
            if (null != sw)
                sw.Close();
        }
        #endregion

        #region Private Method
        //设置streamwriter
        private void InitStreamWriter(string logfile)
        {
            if (null != sw)
                return;
            if (File.Exists(logfile))
			{
                sw = File.AppendText(logfile);//using utf-8
			}
            else
			{
                sw = File.CreateText(logfile);//using utf-8
			}
            sw.AutoFlush = true;
        }
        #endregion
    }
}
