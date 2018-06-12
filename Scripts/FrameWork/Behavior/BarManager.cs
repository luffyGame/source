using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace FrameWork
{
	public class BarManager : MonoBehaviour
	{
		#region Variables
		public GameObject template = null;
		
		public Transform cachedTrans = null;
		private Dictionary<int,GameObject> bars = new Dictionary<int, GameObject>();//objId做key
		private List<GameObject> unUsed = new List<GameObject>();

		private int itemIdx = 0;
		#endregion
		#region Behavior Method
		void Awake()
		{
            if(cachedTrans == null)
            {
                cachedTrans = transform;
            }

			if(null!=template)
				template.SetActive(false);
			itemIdx = 0;
		}
		#endregion
		#region Public Method
		public Transform AddBar(int ownerId,Transform followTrans,Vector3 followDelta)
		{
			GameObject bar = CreateBar();
			Follow3DBy2D.FollowTrans(bar,followTrans,followDelta);
            if (bars.ContainsKey(ownerId))
                Debug.LogError("Repeat KEY!!!:" + ownerId);
            if(!bars.ContainsKey(ownerId))
                bars.Add(ownerId,bar);
			return bar.transform;
		}
		
        public Transform AddBar(int ownerId, Vector3 pos)
		{
			GameObject bar = CreateBar();
			Follow3DBy2D.FollowPos(bar, pos);
			bars.Add(ownerId,bar);
			return bar.transform;
		}
		public void RemoveBar(int ownerId)
		{
			if(bars.ContainsKey(ownerId))
			{
				GameObject bar = bars[ownerId];
				Follow3DBy2D.StopFollower(bar);
				bar.SetActive(false);
				unUsed.Add(bar);
				bars.Remove(ownerId);
			}
		}
		public void BarChangeFollower(int ownerId,Transform followTrans)
		{
			if(bars.ContainsKey(ownerId))
			{
				GameObject bar = bars[ownerId];
				if(null!=bar)
					Follow3DBy2D.FollowTrans(bar,followTrans,Vector3.zero);
			}
		}
		#endregion
		#region Private Method
		private GameObject CreateBar()
		{
			if (unUsed.Count > 0)
			{
				GameObject bar = unUsed[unUsed.Count - 1];
				unUsed.RemoveAt(unUsed.Count - 1);
				bar.SetActive(true);
				return bar;
			}
			GameObject go = GameObject.Instantiate(template) as GameObject;
			go.name = string.Format("Bar_{0}",++itemIdx);
			Transform newItem = go.transform;
			newItem.SetParent(cachedTrans);
			newItem.localPosition = Vector3.zero;
			newItem.localScale = Vector3.one;
			go.SetActive(true);
			return go;
		}
		#endregion
	}
}