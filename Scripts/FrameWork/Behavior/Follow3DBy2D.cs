using UnityEngine;
using System.Collections;
using System;

namespace FrameWork
{
	public class Follow3DBy2D : MonoBehaviour
	{
		#region SubDef
		public enum FollowType
		{
			FOLLOW_TRANS,
			FOLLOW_POINT,
		}
		#endregion
		#region Variables
		public FollowType mFollowType = FollowType.FOLLOW_TRANS;
		public Transform followTrans = null;
		public Vector3 followPosition = Vector3.zero;
		public Vector3 followDelta = Vector3.zero;//只用于followTrans，对于pos没必要

		private RectTransform followee = null;
        private RectTransform parent = null;

		private Action afterFollow = null;
		public Action AfterFollow
		{
			set{afterFollow = value;}
		}
		#endregion
		#region Behavior Method
		void Awake()
		{
			Init();
		}
		void LateUpdate()
		{
			RefreshPos();
		}
		#endregion
		#region Private Method
		private Vector3 GetFollowPos()
		{
			switch(mFollowType)
			{
			case FollowType.FOLLOW_POINT:
				return followPosition;
			case FollowType.FOLLOW_TRANS:
				return followTrans.position + followDelta;
			default:
				return Vector3.zero;
			}
		}
		#endregion
		#region Public Method
		public static Follow3DBy2D FollowPos(GameObject follower, Vector3 follow_pos)
		{
			Follow3DBy2D comp = follower.GetComponent<Follow3DBy2D>();
			if(null == comp) comp = follower.AddComponent<Follow3DBy2D>();
			comp.mFollowType = FollowType.FOLLOW_POINT;
			comp.followPosition = follow_pos;
			comp.followTrans = null;
			comp.enabled = true;
			comp.Init();
			comp.RefreshPos();
			return comp;
		}
		public static Follow3DBy2D FollowTrans(GameObject follower, Transform follow_trans,Vector3 follow_delta)
		{
			Follow3DBy2D comp = follower.GetComponent<Follow3DBy2D>();
			if(null == comp) comp = follower.AddComponent<Follow3DBy2D>();
			comp.mFollowType = FollowType.FOLLOW_TRANS;
			comp.followTrans = follow_trans;
			comp.followDelta = follow_delta;
			comp.enabled = true;
			comp.Init();
			comp.RefreshPos();
			return comp;
		}
		public static void StopFollower(GameObject follower)
		{
			Follow3DBy2D comp = follower.GetComponent<Follow3DBy2D>();
			if(null!=comp)
			{
				comp.StopFollow();
			}
		}
		public void Init()
		{
            if (null == followee)
            {
                followee = transform as RectTransform;
                parent = followee.parent as RectTransform;
            }
		}
		public void RefreshPos()
		{
			if(mFollowType == FollowType.FOLLOW_TRANS && null == followTrans)
				return;
			Vector3 followPos = GetFollowPos();
            Vector2 screenPos = RectTransformUtility.WorldToScreenPoint(Global.Instance.MainCamera, followPos);
            Vector2 posShow = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent, screenPos, Global.Instance.UiCamera, out posShow);

            followee.localPosition = posShow;
			if(null!=afterFollow)
				afterFollow();
		}
		public void StopFollow()
		{
			followTrans = null;
			enabled = false;
		}
		#endregion
	}
}
