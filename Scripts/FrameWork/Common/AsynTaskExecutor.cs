using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace FrameWork
{
    public class AsynTaskExecutor
    {
        #region Variables
        private string m_name = null;
        private Thread m_workThread = null;
        protected volatile bool m_isRunning = false; 

        private Queue<AsynTask> m_runningTaskQueue = new Queue<AsynTask>();
        private Queue<AsynTask> m_waitingTaskQueue = new Queue<AsynTask>();

        private object m_waitingTaskQueueLock = new object();
        #endregion

        #region Cons
        public AsynTaskExecutor(string name,bool startNow)
        {
            m_name = name;
			if(startNow)
            	Start();
        }
        #endregion

        #region Public Method
        public virtual void AddTask(AsynTask task)
        {
            if (!m_isRunning)
                Start();
            lock(m_waitingTaskQueueLock)
            {
                this.m_waitingTaskQueue.Enqueue(task);
                Monitor.Pulse(m_waitingTaskQueueLock);
            }
        }
        public void ShutDown()
        {
            m_isRunning = false;
	        lock (m_waitingTaskQueueLock)
	        {
		        Monitor.PulseAll(m_waitingTaskQueueLock);
	        }
			m_workThread = null;
        }
        #endregion

        #region Self Called
        private void Start()
        {
            if (m_isRunning)
                return;
            try
            {
				m_isRunning = true;
                m_workThread = new Thread(Run);
                m_workThread.Name = m_name;
                m_workThread.Start();
                OnStartSuccess();
            }
            catch
            {
				m_isRunning = false;
            }
        }

        private void Run()
        {
            while(m_isRunning)
            {
                if(0 == m_runningTaskQueue.Count)
                {
                    lock(m_waitingTaskQueueLock)
                    {
                        if(0 == m_waitingTaskQueue.Count)
                        {
                            Monitor.Wait(m_waitingTaskQueueLock);
	                        if(!m_isRunning)
		                        break;
                        }
                        Queue<AsynTask> temp = m_runningTaskQueue;
                        m_runningTaskQueue = m_waitingTaskQueue;
                        m_waitingTaskQueue = temp;
                    }
                }
                else
                {
                    while(m_runningTaskQueue.Count>0)
                    {
                        AsynTask task = m_runningTaskQueue.Dequeue();
                        try
                        {
                            task.Execute();
                        }
                        catch(Exception ex)
                        {
                            UnityEngine.Debug.LogException(ex);
                        }
                    }
                }
            }
            m_runningTaskQueue.Clear();
            m_waitingTaskQueue.Clear();
            OnClosed();
        }

        protected virtual void OnStartSuccess() { }
        protected virtual void OnClosed() { }
        #endregion
    }

	public class UnityCbTaskExecutor : AsynTaskExecutor
	{
		#region Vars
		private Queue<UnityCbTask> doneQueue = new Queue<UnityCbTask>();
		private object doneQueueLock = new object();
		private bool autoShutDown = true;//对于任务量少的可以设为自动关闭
		private int taskCounts = 0;
        public bool IsBusy { get { return taskCounts > 0; } }
		#endregion
		#region Cons
		public UnityCbTaskExecutor(string name,bool startNow,bool _autoShutDown):base(name,startNow)
		{
			autoShutDown = _autoShutDown;
		}
		#endregion
		#region Public Method
        public override void AddTask(AsynTask task)
		{
			++taskCounts;
			task.CallBackHandler = this.TaskDone;
			base.AddTask(task);
		}
		public void TaskDone(AsynTask task)
		{
			lock(doneQueueLock)
			{
				doneQueue.Enqueue(task as UnityCbTask);
			}
		}
		public void UpdateDone()
		{
			if(!m_isRunning||!IsBusy)
				return;
			lock(doneQueueLock)
			{
				if(doneQueue.Count>0)
				{
					UnityCbTask task = doneQueue.Dequeue();
					task.UpdateExec();
					--taskCounts;
					if(taskCounts == 0 && autoShutDown)
						ShutDown();
				}
			}
		}
		#endregion
	}
}
