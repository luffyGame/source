using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public class HttpWorker : SingleBehavior<HttpWorker>
    {
        public delegate void OnHttpDone(bool isSuccess,string msg);
        public void HttpGet(OnHttpDone handler, string url, params string[] dataParams)
        {
            string paramStr = Utils.GetHttpHead(dataParams);
            if (!string.IsNullOrEmpty(paramStr))
                url = url + '?' + paramStr;
            StartCoroutine(httpDo(handler,url));
        }

        public void HttpPost(OnHttpDone handler, string url, params string[] dataParams)
        {
            WWWForm data = Utils.GetWWWPostData(dataParams);
            StartCoroutine(httpDo(handler,url, data));
        }

        public void HttpUpload(OnHttpDone handler, string url, string fileField, byte[] upFile, params string[] dataParams)
        {
            WWWForm data = Utils.GetWWWPostData(dataParams);
            data.AddBinaryData(fileField, upFile);
            StartCoroutine(httpDo(handler,url, data));
        }

        public void HttpDownload(OnHttpDone handler, string url, string filename, params string[] dataParams)
        {
            string paramStr = Utils.GetHttpHead(dataParams);
            if (!string.IsNullOrEmpty(paramStr))
                url = url + '?' + paramStr;
            StartCoroutine(httpDownload(handler, url, filename));
        }

        #region Private Method
        private IEnumerator httpDo(OnHttpDone handler,string url,WWWForm data = null)
        {
            using (WWW www = (null == data)? new WWW(url):new WWW(url,data))
            {
                yield return www;
                string msg;
                bool isSuc;
                if (www.error != null)
                {
                    isSuc = false;
                    msg = www.error;
                }
                else
                {
                    isSuc = true;
                    msg = www.text;
                }
                if (null != handler)
                {
                    handler(isSuc, msg);
                }
            }
        }

        private IEnumerator httpDownload(OnHttpDone handler, string url,string filename)
        {
            using (WWW www = new WWW(url))
            {
                yield return www;
                string msg;
                bool isSuc;
                if (www.error != null)
                {
                    isSuc = false;
                    msg = www.error;
                }
                else
                {
                    isSuc = true;
                    msg = BaseUtil.FileUtility.GetFileReadPath(filename, true);
                    BaseUtil.FileUtility.WriteFile(filename, www.bytes);
                }
                if (null != handler)
                {
                    handler(isSuc, msg);
                }
            }
        }
        #endregion
    }
}
