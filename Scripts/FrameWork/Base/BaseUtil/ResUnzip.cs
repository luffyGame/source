using ICSharpCode.SharpZipLib.Zip;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;

namespace FrameWork.BaseUtil
{
    public class ResUnzip
    {
        private struct EntryData
        {
            public string path;
            public ZipEntry zipEntry;
        }
        #region Var
        private static ResUnzip s_instance = new ResUnzip();
        public static ResUnzip Instance { get { return s_instance; } }
        private static readonly string APK_RES_ROOT = "assets/";
        private const int MAX_WRITE_THREAD_COUNT = 4;

        private bool isApkRes;
        private List<EntryData> data;
        private ZipFile zipFile;
        private int entryIndex = 0;
        private System.Object lockObj = new System.Object();
        private List<ManualResetEvent> writeEvents;
        private float zipReadProgress;
        private volatile bool isDone;
        private Action finishHandle;
        private string apkResDonePath;
        #endregion
        #region Public Method
        public void ReleaseApkRes(Action finishCb)
        {
            apkResDonePath = FileUtility.DirRoot2Path + "/done";
            if (File.Exists(apkResDonePath))
            {
                isDone = true;
                this.finishHandle = finishCb;
                return;
            }
            isApkRes = true;
            Unzip(Application.dataPath, FileUtility.DirRoot2Path, finishCb);
        }
        public void Unzip(string zipPath,string unZipPath,Action finishCb)
        {
            zipReadProgress = 0f;
            isDone = false;
            entryIndex = 0;
            this.finishHandle = finishCb;
            Thread thread = new Thread(new ParameterizedThreadStart(Unzip));
            thread.Start(new string[] { zipPath, unZipPath });
        }
        public float GetProgress()
        {
            if (isDone)
                return 1.0f;
            float progress = zipReadProgress * 0.3f;
            if(null!=data&&data.Count>0)
                progress += (entryIndex / (float)data.Count) * 0.7f;
            return progress;
        }
        #endregion
        #region Private Method
        private void Unzip(System.Object arg)
        {
            string[] args = (string[])arg;
            string zipPath = args[0];
            string unZipPath = args[1];
            data = new List<EntryData>();
            if (Directory.Exists(unZipPath) == false)
                Directory.CreateDirectory(unZipPath);
            zipFile = new ZipFile(zipPath);
                //new ZipFile(zipPath,Encoding.UTF8);
            float entryCount = (float)zipFile.Count;
            int index = 0;
            foreach (ZipEntry entry in zipFile)
            {
                ++index;
                zipReadProgress = index / entryCount;
                if (entry.IsDirectory)
                    continue;
                string entryName = entry.Name;
                if (isApkRes)
                {
                    if (entryName.StartsWith(APK_RES_ROOT))
                    {
                        entryName = entryName.Substring(APK_RES_ROOT.Length);
                        if (entryName.StartsWith("bin"))
                            continue;
                    }
                    else
                        continue;
                }
                string entryPath = unZipPath + "/" + entryName;
                string dirpath = Path.GetDirectoryName(entryPath);
                if (!Directory.Exists(dirpath))
                    Directory.CreateDirectory(dirpath);
                data.Add(new EntryData() { path = entryPath, zipEntry = entry });
            }
            int writeThreadNum = Math.Min(data.Count, MAX_WRITE_THREAD_COUNT);
            if (writeThreadNum == 0)
                Done();
            else
            {
                ExtractFile();
                Done();
//                 writeEvents = new List<ManualResetEvent>();
//                 for (int i = 0; i < writeThreadNum; ++i)
//                     writeEvents.Add(new ManualResetEvent(false));
//                 for (int i = 0; i < writeThreadNum; ++i)
//                 {
//                     Thread writeThread = new Thread(new ParameterizedThreadStart(WriteFile));
//                     writeThread.Start(i);
//                 }
//                 while (true)
//                 {
//                     bool writeComplete = true;
//                     for (int i = 0; i < writeThreadNum; ++i)
//                     {
//                         if (!writeEvents[i].WaitOne(0))
//                         {
//                             writeComplete = false;
//                         }
//                     }
//                     if (writeComplete)
//                     {
//                         Done();
//                         break;
//                     }
//                 }
            }
        }
        private void WriteFile(System.Object arg)
        {
            int threadIndex = (int)arg;
            int localIndex = 0;
            Stream input;
            ZipEntry entry;
            while (true)
            {
                lock (lockObj)
                {
                    if (entryIndex >= data.Count)
                        break;
                    localIndex = entryIndex++;
                    Debugger.Log("cur entry index = {0}", localIndex);
                    entry = data[localIndex].zipEntry;
                    input = zipFile.GetInputStream(entry);
                }
                FileStream output = File.Open(data[localIndex].path, FileMode.Create, FileAccess.Write);
                FileUtility.CopyStream(input, output);
                output.Close();
                input.Close();
            }
            Debugger.Log("write thread {0} done", threadIndex);
            writeEvents[threadIndex].Set();
        }
        private void ExtractFile()
        {
            int localIndex = 0;
            Stream input;
            ZipEntry entry;
            while (true)
            {
                if (entryIndex >= data.Count)
                    break;
                localIndex = entryIndex++;
                Debugger.Log("cur entry index = {0}", localIndex);
                entry = data[localIndex].zipEntry;
                input = zipFile.GetInputStream(entry);
                FileStream output = File.Open(data[localIndex].path, FileMode.Create, FileAccess.Write);
                FileUtility.CopyStream(input, output);
                output.Close();
                input.Close();
            }
        }
        private void Done()
        {
            Debugger.Log("Resunzip done");
            if(isApkRes)
            {
                FileStream fs = new FileStream(apkResDonePath, FileMode.Create);
                fs.Close();
                fs = null;
                isApkRes = false;
            }
            data = null;
            if (null != zipFile)
            {
                zipFile.Close();
                zipFile = null;
            }
            writeEvents = null;
            isDone = true;
            
        }
        public IEnumerator WaitFor()
        {
            while (!isDone)
                yield return null;
            if (null != finishHandle)
            {
                finishHandle();
                finishHandle = null;
            }
        }
        #endregion
    }
}
