using System;
using FrameWork;
using FrameWork.BaseUtil;

namespace Game
{
    public class Storage : SingleBehavior<Storage>
    {
        #region Def
        private class StorageProcessor : UnityCbTaskExecutor
        {
            public StorageProcessor() : base("storage", true, false){}
        }
        private class SaveTask : UnityCbTask
        {
            private readonly string file;
            private readonly string data;
            private readonly Action<string,bool> done;
            private bool isSuc;

            public SaveTask(string file, string data, Action<string,bool> done)
            {
                this.file = file;
                this.data = data;
                this.done = done;
            }
            public override void UpdateExec()
            {
                if (null != done)
                    done(file,isSuc);
            }

            public override void Execute()
            {
                isSuc = FuncUtility.SaveStrToFile(this.data, this.file);
                base.Execute();
            }
        }
        private class LoadTask : UnityCbTask
        {
            private readonly string file;
            private string data;
            private readonly Action<string,string> done;

            public LoadTask(string file, Action<string,string> done)
            {
                this.file = file;
                this.done = done;
            }
            public override void UpdateExec()
            {
                if (null != done)
                    done(file,data);
            }

            public override void Execute()
            {
                this.data = FuncUtility.GetStrFromFile(this.file);
                base.Execute();
            }
        }
        #endregion

        #region Var
        public string httpSyncUrl;
        private StorageProcessor processor;
        private Action<string,string> cachedLoadDone;
        private Action<string,string> CachedLoadDone
        {
            get { return cachedLoadDone ?? (cachedLoadDone = OnLoadDone); }
        }
        public Action<string,string> loadCb;
        
        private Action<string,bool> cachedSaveDone;
        private Action<string,bool> CachedSaveDone
        {
            get { return cachedSaveDone ?? (cachedSaveDone = OnSaveDone); }
        }
        public Action<string,bool> saveCb;
        
        private OnTaskDone cachedSyncDone;
        private OnTaskDone CachedSyncDone
        {
            get { return cachedSyncDone ?? (cachedSyncDone = OnSyncDone); }
        }

        public Action<bool, string> syncDone;
        #endregion
        public void Init()
        {
            processor = new StorageProcessor();
        }

        public void ShutDown()
        {
            processor.ShutDown();
        }

        public void OnUpdate()
        {
            processor.UpdateDone();
        }

        #region Load
        public void StartLoad(string file)
        {
            LoadTask task = new LoadTask(file,CachedLoadDone);
            processor.AddTask(task);
        }

        private void OnLoadDone(string file,string data)
        {
            if (null != loadCb)
                loadCb(file,data);
        }
        #endregion
        #region Save
        public void StartSave(string file,string data)
        {
            SaveTask task = new SaveTask(file,data,CachedSaveDone);
            processor.AddTask(task);
        }
        private void OnSaveDone(string file,bool bsuc)
        {
            if (null != saveCb)
                saveCb(file,bsuc);
        }
        #endregion

        #region HttpSync

        public void Sync(string data)
        {
            WebExecWorker.Instance.OpenRequest(0,httpSyncUrl,"POST",null,data,"application/json",10000,CachedSyncDone);
        }

        private void OnSyncDone(bool bsuc, string msg)
        {
            if (null != syncDone)
                syncDone(bsuc, msg);
        }
        #endregion
    }
}