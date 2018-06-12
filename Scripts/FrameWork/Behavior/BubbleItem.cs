using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine.UI;
using URandom = UnityEngine.Random;

namespace FrameWork
{
	public class BubbleItem : MonoBehaviour
	{
		#region Sub Def

		public enum PushType
		{
			NONE,
			HORIZONTAL_UP,
			HORIZONTAL_DOWN,
			VERTICAL_UP,
			VERTICAL_DOWN,
			RANDOM_POS,
		}
		public enum ItemType 
		{
			LABEL,ICON,LABEL_ICON,
		}
		public class Entry
		{
			public float time;			// 加上的时间
			public float stay = 0f;		// 在屏幕上呆多久
			public GameObject go;
			public Transform trans;
            public BubbleItemCfg cfg;
			public Vector2 locPos = Vector2.zero;
			public Vector2 initPos = Vector2.zero; 
			public void Init(GameObject _go, ItemType itemType)
			{
				go = _go;
				trans = go.transform;
                cfg = go.GetComponent<BubbleItemCfg>();
			}

			public void SetTextColor(Color color)
			{
				cfg.SetTextColor(color);
			}
		}
		#endregion
		#region Variables
		public ItemType itemType = ItemType.LABEL;
		public PushType pushType = PushType.NONE;
		public bool isPush
		{
			get { return pushType != PushType.NONE; }
		}

		public bool isEmpty
		{
			get { return mList.Count == 0; }
		}
		public GameObject template = null;
		public int initCount = 3;
		public float period = 0.5f;//间隔
		public float offsetScale = 1f;
		public Rect randomRange;

		//位移曲线
		public AnimationCurve offsetXCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(3f, 15f) });
		public AnimationCurve offsetYCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(3f, 15f) });
		//alpha曲线
		public AnimationCurve alphaCurve = new AnimationCurve(new Keyframe[] { new Keyframe(1f, 1f), new Keyframe(3f, 0f) });
		//缩放曲线
		public AnimationCurve scaleCurve = new AnimationCurve(new Keyframe[] { new Keyframe(0f, 0f), new Keyframe(0.25f, 1f) });
		
		private Transform cachedTrans = null;
		private GameObject cachedGo = null;

		private float totalEnd = 0f;//曲线结束时间
		private List<Entry> mList = new List<Entry>();
		private List<Entry> mUnused = new List<Entry>();

		public Transform CachedTrans
		{
			get{if(null==cachedTrans) cachedTrans = transform;return cachedTrans;}
		}
		public GameObject CachedGo
		{
			get{if(null==cachedGo) cachedGo=gameObject; return cachedGo;}
		}
		#endregion
		#region Private Method
		private Entry Create ()
		{
			if (mUnused.Count > 0)
			{
				Entry ent = mUnused[mUnused.Count - 1];
				mUnused.RemoveAt(mUnused.Count - 1);
				ent.time = Time.time;
				ent.go.SetActive(true);
                ent.cfg.SetLocalScale(new Vector3(0.001f, 0.001f, 0.001f));
				mList.Add(ent);
				return ent;
			}
			
			// New entry
			Entry ne = NewItem(true);
			if(null!=ne)
				mList.Add(ne);
			return ne;
		}
		private void Delete (int i)
		{
			Entry ent = mList[i];
			ent.go.SetActive(false);
			mList.RemoveAt(i);
			mUnused.Add(ent);
		}
		private Entry NewItem(bool active)
		{
			if(null == template)
				return null;
			Entry ne = new Entry();
			ne.time = Time.time;
			ne.Init(GameObject.Instantiate(template) as GameObject,itemType);
            ne.trans.SetParent(CachedTrans,false);
			//ne.trans.localPosition = Vector3.zero;
			//ne.trans.localScale = Vector3.one;
			ne.cfg.SetLocalScale(new Vector3(0.001f, 0.001f, 0.001f));// Make it small so that it's invisible to start with
			ne.go.SetActive(active);
			return ne;
		}
		#endregion
		#region Public Method
		public Entry Add(string param,float stayDuration)
		{
			Entry ne = Create();
			ne.stay = stayDuration;
			ne.initPos = GetInitPos(mList.Count-1);
            switch (itemType)
            {
                case ItemType.LABEL:
                    if (!string.IsNullOrEmpty(param))
                        ne.cfg.SetText(param);
                    break;
            }
			return ne;
		}
		public void Reserve(int count)
		{
			for(int i=0;i<count;++i)
			{
				Entry item = NewItem(false);
				if(null!=item)
					mUnused.Add(item);
			}
		}
		public float GetEndTime()
		{
			return totalEnd;
		}
		public void Follow(Transform followee)
		{
			Follow3DBy2D.FollowTrans(CachedGo,followee,new Vector3(0,0.5f,0));
		}

		public void StopFollow()
		{
			Follow3DBy2D.StopFollower(CachedGo);
		}
		#endregion
		#region Behavior Method
		void Awake()
		{
			if(null!=template)
				template.SetActive(false);
			Keyframe[] xOffsets = offsetXCurve.keys;
			Keyframe[] yOffsets = offsetYCurve.keys;
			Keyframe[] alphas = alphaCurve.keys;
			Keyframe[] scales = scaleCurve.keys;
			float[] ends = new float[4];
			ends[0] = xOffsets[xOffsets.Length-1].time;
			ends[1] = yOffsets[yOffsets.Length - 1].time;
			ends[2] = alphas[alphas.Length - 1].time;
			ends[3] = scales[scales.Length - 1].time;
			totalEnd = Mathf.Max(ends);
			Reserve(initCount);
		}
		void Update()
		{
			float time = Time.time;
			for(int i=mList.Count-1;i>=0;--i)
			{
				Entry ent = mList[i];
				float currentTime = time - ent.time;
				float x = offsetXCurve.Evaluate(currentTime);
				float y = offsetYCurve.Evaluate(currentTime);
				//ent.baseTrans.localPosition = new Vector3(x,y);
				ent.locPos = new Vector2(x,y) + ent.initPos;
                ent.cfg.SetAlpha(alphaCurve.Evaluate(currentTime));
				float s = scaleCurve.Evaluate(time-ent.time);
				if (s < 0.001f) s = 0.001f;
				ent.cfg.SetLocalScale(new Vector3(s, s, s));
				
				if (currentTime-ent.stay > totalEnd) Delete(i);
			}

			switch (pushType)
			{
				case PushType.VERTICAL_DOWN:
				case PushType.VERTICAL_UP:
					AdjustVerticalPos();
					break;
				default:
					AdjustPos();
					break;
			}
			
		}
		private Vector2 GetInitPos(int index)
		{
			switch (pushType)
			{
				case PushType.HORIZONTAL_UP:
					if (index > 0)
					{
						Entry pre = mList[index - 1];
						return pre.initPos + new Vector2(0,pre.cfg.GetFinalHeight() + 10f);
					}
					break;
				case PushType.HORIZONTAL_DOWN:
					if (index > 0)
					{
						Entry pre = mList[index - 1];
						return pre.initPos - new Vector2(0,pre.cfg.GetFinalHeight() + 10f);
					}
					break;
				case PushType.RANDOM_POS:
					return new Vector2(URandom.Range(randomRange.left,randomRange.right),URandom.Range(randomRange.bottom,randomRange.top));
					break;
			}
			return Vector2.zero;
		}
		//设置目标位置
		private void AdjustPos()
		{
			// Move the entries
			for (int i = 0; i<mList.Count; ++i )
			{
				Entry ent = mList[i];
				ent.cfg.SetLocalPos(new Vector3(ent.locPos.x, ent.locPos.y, 0f));
			}
		}
		//竖直方向push的，后出现的在原始位置，对已有的向上或向下推
		private void AdjustVerticalPos()
		{
			float offset = 0f;
			
			// Move the entries
			for (int i = mList.Count-1; i >= 0; --i )
			{
				Entry ent = mList[i];
				if(i == mList.Count-1)
					offset = ent.locPos.y;
				else
				{
					if(pushType == PushType.VERTICAL_UP)
						offset = Mathf.Max(offset, ent.locPos.y);
					else
						offset = Mathf.Min(offset, ent.locPos.y);
				}
				ent.cfg.SetLocalPos(new Vector3(ent.locPos.x, offset, 0f));
				float pushOffset = Mathf.Round(ent.cfg.GetLocalHeight())*offsetScale+10f;
				if(pushType == PushType.VERTICAL_UP)
					offset += pushOffset;
				else
					offset -= pushOffset;
			}
		}
		#endregion
	}
}