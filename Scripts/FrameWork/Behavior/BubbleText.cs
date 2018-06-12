using UnityEngine;
using System.Collections.Generic;

namespace FrameWork
{
	public class BubbleText : MonoBehaviour
	{
		#region Def
		private class BubbleInfo
		{
			private string txt;
			private float duration;
			private bool useColor;
			private Color textColor;
			public BubbleInfo(string _txt,float _duration)
			{
				txt = _txt;
				duration = _duration;
				useColor = false;
			}

			public BubbleInfo(string _txt, float _duration, Color _color)
			{
				txt = _txt;
				duration = _duration;
				textColor = _color;
				useColor = true;
			}
			public void Bubble(BubbleItem item)
			{
				if(null == item)
					return;
				BubbleItem.Entry entry = item.Add(txt,duration);
				if(useColor)
					entry.SetTextColor(textColor);
			}
		}
		private class InfoQueue
		{
			private GameObject itemGo;
			public GameObject ItemGo
			{
				get {return itemGo;}
				set
				{
					itemGo = CachedClone.Clone(value);
					item = itemGo.GetComponent<BubbleItem>();
				}
			}
			private BubbleItem item;
			public float nextTime = 0f;
			public Queue<BubbleInfo> queue = new Queue<BubbleInfo>();
			private bool isFollowing = false;

			public void Push(BubbleInfo info)
			{
				if (!item.isEmpty&&!item.isPush)
					return;
				queue.Enqueue(info);
			}
			public bool CheckPop(float curTime)
			{
				if(queue.Count > 0)
				{
					if(nextTime<=curTime)
					{
						nextTime = curTime + item.period;
						BubbleInfo info = queue.Dequeue();
						info.Bubble(item);
					}
					return false;
				}
				else
				{
					//if(delTime<curTime)
					if(item.isEmpty)
					{
						ReleaseItem();
						return true;
					}
					return false;
				}
			}
			public void ReleaseItem()
			{
				if(null!=itemGo)
				{
					if (isFollowing)
					{
						isFollowing = false;
						Follow3DBy2D.StopFollower(itemGo);
					}
					CachedClone.RemoveClone(itemGo,false);
					itemGo = null;
					item = null;
				}
			}

			public void Follow(Transform trans)
			{
				if (!isFollowing&&null!=trans)
				{
					isFollowing = true;
					Follow3DBy2D.FollowTrans(itemGo, trans, Vector3.zero);
				}
			}
		}
		#endregion
		#region Variables
		public List<BubbleItem> kindItems = new List<BubbleItem>();
		public Color[] textColor;
		private Dictionary<long,InfoQueue> waitings = new Dictionary<long, InfoQueue>();
		private List<long> dels = new List<long>();

		public int testKind = 0;
		public string testTxt = null;
		public bool isTest = false;
		public float testPeriod = 0.5f;
		private float testNext = 0f;
		#endregion
		#region Public Method
		public BubbleItem GetKindItem(int kindIdx)
		{
			if(kindIdx>=kindItems.Count)
				return null;
			return kindItems[kindIdx];
		}
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tag">用于建立队列索引，弹出的内容会顶同一个索引队列，只有一个队列，填0</param>
        /// <param name="kindIdx">弹出类型</param>
        /// <param name="txt"></param>
        /// <param name="duration"></param>
		public void Bubble(int tag,int kindIdx,string txt,float duration)
		{
			if(kindIdx<0||kindIdx>=kindItems.Count)
				return;
			BubbleInfo info = new BubbleInfo(txt,duration);
			PushBubbleInfo(tag,kindIdx,info);
		}
		public void Bubble(int tag,int kindIdx,Transform trans,string txt,float duration)
		{
			if(kindIdx<0||kindIdx>=kindItems.Count)
				return;
			BubbleInfo info = new BubbleInfo(txt,duration);
			InfoQueue iqueue = PushBubbleInfo(tag,kindIdx,info);
			iqueue.Follow(trans);
		}
        public void Warning(string txt)
        {
            Bubble(0, 0, txt, 0f);
        }

		public void Tip(string txt)
		{
			Bubble(0,1,txt,0f);
		}

		public void PopHp(int tag, Transform trans, int hpType,int hp)
		{
			BubbleInfo info = new BubbleInfo(hp.ToString(),0f,textColor[hpType]);
			InfoQueue iqueue = PushBubbleInfo(tag,2,info);
			iqueue.Follow(trans);
		}

		public void RemoveHp(int tag)
		{
			RemoveBubbleInfo(tag,2);
		}
		public void Clear()
		{
			foreach (var kvp in waitings)
			{
				kvp.Value.ReleaseItem();
			}
			waitings.Clear();
		}
		public void RemoveBubbleInfo(int tag, int kindIdx)
		{
			if(kindIdx<0||kindIdx>=kindItems.Count)
				return;
			long key = GetKey(tag, kindIdx);
			if (waitings.ContainsKey(key))
			{
				InfoQueue iqueue = waitings[key];
				if (null != iqueue)
				{
					iqueue.ReleaseItem();
				}

				waitings.Remove(key);
			}
		}
		#endregion
		#region Private Method

		private static long TAG_MASK = 0xFFFFFFFF;
		private long GetKey(int tag, int kindIdx)
		{
			return ((long)kindIdx << 32 | tag);
		}

		public int GetTag(long key)
		{
			return (int) (key & TAG_MASK);
		}
		private InfoQueue PushBubbleInfo(int tag,int kindIdx,BubbleInfo info)
		{
			long key = GetKey(tag, kindIdx);
			InfoQueue infoQue = null;
			if(waitings.ContainsKey(key))
				infoQue = waitings[key];
			else
			{
				infoQue = new InfoQueue();
				infoQue.ItemGo = kindItems[kindIdx].CachedGo;
				waitings.Add(key,infoQue);
			}
			infoQue.Push(info);
			return infoQue;
		}
		#endregion
		void Update()
		{
            float curTime = Time.time;
			foreach(KeyValuePair<long,InfoQueue> element in waitings)
			{
				if(element.Value.CheckPop(curTime))
					dels.Add(element.Key);
			}
			if(dels.Count>0)
			{
				for(int i=0;i<dels.Count;++i)
				{
					waitings.Remove(dels[i]);
				}
				dels.Clear();
			}
#if UNITY_EDITOR
			if(isTest&&testNext<=curTime)
			{
				Bubble(0,testKind,testTxt,0f);
				testNext = curTime + testPeriod;
			}
#endif
		}
	}
}

