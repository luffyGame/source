using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FrameWork
{
	public class CachedClone : MonoBehaviour
	{
		#region Variables
		private List<GameObject> unUsed = null;
		private Transform cachedTrans = null;
		private GameObject cachedGo = null;
		private int itemIdx = 0;
		private CachedClone cloneFrom = null;//从哪复制，标志是源还是复制体
		public int initSize = 0;//只在第一次运行时生效
		#endregion
		#region Properties
		private Transform CachedTrans {get {if(cachedTrans==null) cachedTrans=transform;return cachedTrans;}}
		private GameObject CachedGo {get {if(cachedGo == null) cachedGo=gameObject; return cachedGo;}}
		#endregion
		#region Behavior Method
		void Awake()
		{
			if(initSize > 0)
			{
				int reserve = initSize;
				initSize = 0;//清掉，防止复制体再复制
				unUsed = new List<GameObject>();
				Reserve(reserve);
			}
		}
		#endregion
		#region Private Method
		private GameObject CreateClone(bool bActive)
		{
			GameObject clone = GameObject.Instantiate(CachedGo) as GameObject;
			clone.name = string.Format("{0}_{1:D3}",clone.name,++itemIdx);
			Transform cloneTrans = clone.transform;
            cloneTrans.SetParent(CachedTrans.parent,false);
			//cloneTrans.parent = CachedTrans.parent;
			cloneTrans.localPosition = CachedTrans.localPosition;
			cloneTrans.localScale = CachedTrans.localScale;
			cloneTrans.localRotation = CachedTrans.localRotation;
			CachedClone comp = clone.GetComponent<CachedClone>();
			if(null == comp)
				comp = clone.AddComponent<CachedClone>();
			comp.cloneFrom = this;
			clone.SetActive(bActive);
			return clone;
		}
		private GameObject CloneSelf()
		{
			GameObject clone = null;
			if(null==unUsed)
				unUsed = new List<GameObject>();
			if(unUsed.Count > 0)
			{
				clone = unUsed[unUsed.Count-1];
				unUsed.RemoveAt(unUsed.Count-1);
				clone.SetActive(true);
			}
			else
			{
				clone = CreateClone(true);
			}
			return clone;
		}
		private void Remove(GameObject go, bool bDel)
		{
			go.SetActive(false);
			if(bDel||null==unUsed)
				Destroy(go);
			else
				unUsed.Add(go);
		}
		#endregion
		#region Public Method
		public static void Reserve(GameObject go,int reserveSize)
		{
			CachedClone comp = go.GetComponent<CachedClone>();
			if(null == comp)//如果没有则作为源
				comp = go.AddComponent<CachedClone>();
			if(null!=comp.cloneFrom)
				comp = comp.cloneFrom;
			comp.Reserve(reserveSize);
		}
		public static GameObject Clone(GameObject go)
		{
			CachedClone comp = go.GetComponent<CachedClone>();
			if(null == comp)//如果没有则作为源
				comp = go.AddComponent<CachedClone>();
			if(null == comp.cloneFrom)
				return comp.CloneSelf();
			else
				return comp.cloneFrom.CloneSelf();
		}
		public static void RemoveClone(GameObject go,bool bDel = false)
		{
			CachedClone comp = go.GetComponent<CachedClone>();
			if(null != comp&&null!=comp.cloneFrom)
				comp.cloneFrom.Remove(go,bDel);
		}
		public void RemoveCloneSelf()
		{
			if(null!=cloneFrom)
				cloneFrom.Remove(CachedGo,false);
		}
		public void Reserve(int reserveSize)
		{
			if(null!=unUsed)
			{
				for(int i=0;i<reserveSize;++i)
				{
					unUsed.Add(CreateClone(false));
				}
			}
			CachedGo.SetActive(false);
		}
		public void DelayRemove(float delay)
		{
			StartCoroutine(Utils.DelayAction(delay,this.RemoveCloneSelf,true));
		}
		#endregion
	}
}
