using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FrameWork
{
	[AddComponentMenu("FrameWork/BundleLoader")]
	public class BundleLoader : SingleBehavior<BundleLoader>
	{
		#region Sub Class
		private class ProgressLoadTask
		{
			private List<BundleHolder> loadingHolders = null;
			public ProgressLoadTask(List<BundleHolder> _loadings)
			{
				loadingHolders = _loadings;
			}
			public float GetProgress(ref bool isAllLoaded)
			{
				isAllLoaded = true;
				if(null == loadingHolders||loadingHolders.Count == 0)
					return 1f;
				float progress = 0f;
				for(int i=0;i<loadingHolders.Count;++i)
				{
					progress += loadingHolders[i].SelfLoadingProgress;
					if(!loadingHolders[i].IsLoaded())
						isAllLoaded = false;
				}
				return progress/loadingHolders.Count;
			}
		}
		#endregion
		#region Variables
		private ProgressLoadTask progressTask = null;//唯一的进度任务
		//private List<BundleLoadTask> tasks = new List<BundleLoadTask>();
		//private List<BundleLoadTask> tasksSwap = new List<BundleLoadTask>();
		private float clearDeltaTime = 0;
		private const float CLEAR_PERIOD = 5f;//无用资源的存在时间超过这个时间的才删除，
		#endregion
		#region Public Method
		public void StartProgressTask(List<BundleHolder> loadingHoladers)
		{
			if(null == progressTask)
				progressTask = new ProgressLoadTask(loadingHoladers);
		}
		/*public void StartLoadTask(BundleLoadTask task)
		{
			if(null!=task)
			{
				StartCoroutine(task.Run());
				tasks.Add(task);
			}
		}*/
		#endregion
		#region Private Method
		/*private void CheckAndSwapTasks()
		{
			for(int i=0;i<tasks.Count;++i)
			{
				BundleLoadTask task = tasks[i];
				if(task.IsFinished)
				{
					if(null!=task.Holder)
						task.Holder.OnLoaded();
				}
				else
					tasksSwap.Add(task);
			}
			tasks.Clear();
			List<BundleLoadTask> tmp = tasks;
			tasks = tasksSwap;
			tasksSwap = tmp;
		}*/
		#endregion
		void Update ()
		{
			if(null!=progressTask)
			{
				bool isAllLoaded = false;
				float progress = progressTask.GetProgress(ref isAllLoaded);
				//LuaEntrance.Instance.OnProgressLoad(progress);
				if(isAllLoaded)
					progressTask = null;
			}
			//this.CheckAndSwapTasks();
			//关闭尝试只是在切换场景的时候delete
			float deltaTime = Time.deltaTime;
			clearDeltaTime += deltaTime;
			if(clearDeltaTime>CLEAR_PERIOD)
			{
				clearDeltaTime = 0f;
				StartCoroutine(GameObjPool.Instance.PeriodClean());
				StartCoroutine(BundleMgr.Instance.TryDelete());
			}
			BundleMgr.Instance.Update(deltaTime);
		}
	}
}
