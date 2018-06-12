using System;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

namespace FrameWork
{
    /// <summary>
    /// 鉴于web需求的行为可预期
    /// </summary>
    public class WebExecWorker
    {
        public const int DEFAULT_ID = 0;
        #region Vars
        private List<WebExecutor> workers = new List<WebExecutor>();
        private int idCount = 0;
        private static WebExecWorker instance = null;
        public static WebExecWorker Instance
        {
            get
            {
                if (null == instance)
                {
                    instance = new WebExecWorker();
                }
                return instance;
            }
        }
        #endregion
        #region Public Method
        public void OpenRequest(int id, string url, string method, string head, string body = null, string contentType = null,
                                int timeOut = 10000, OnTaskDone cb = null)
        {
            WebTask task = new WebTask(url, method, head, body, contentType, timeOut, cb);
            GetWorker(id).AddTask(task);
        }
        public void UploadFile(int id, string url,string uploadFile, string fileKey,string saveName, bool addPre =false, bool compress =false,
                               string contentType = null,string[] header = null, int timeOut = 10000, OnTaskDone cb = null)
        {
            HttpUpLoadTask task = new HttpUpLoadTask(url, header,uploadFile, fileKey,saveName, addPre, compress, contentType, timeOut, cb);
            GetWorker(id).AddTask(task);
        }
        public void DownloadFile(int id, string url, string savePath, int timeOut, OnTaskDone cb)
        {
            HttpDownloadTask task = new HttpDownloadTask(url, savePath, timeOut, cb);
            GetWorker(id).AddTask(task);
        }
        public void WebRequestGet(string url,OnTaskDone cb,int id = 0)
        {
            OpenRequest(id, url, "GET", null, null, null, 10000, cb);
        }
        public void UpdateDone()
        {
            for (int i = 0; i < idCount; ++i)
                workers[i].UpdateDone();
        }
        public void ShutDown()
        {
            for (int i = 0; i < idCount; ++i)
                workers[i].ShutDown();
        }
        #endregion
        #region Private Method
        private WebExecutor GetWorker(int id)
        {
            for (int i = 0; i < idCount; ++i)
            {
                if (workers[i].Id == id)
                    return workers[i];
            }
            WebExecutor we = new WebExecutor(id);
            workers.Add(we);
            ++idCount;
            return we;
        }
        #endregion
    }
}
