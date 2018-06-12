using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public class BezierPosition : MonoBehaviour
    {
        #region Variables
        public Vector3 from;
        public Vector3 to;
        public Vector3 handler1;//开始点的控制点
        public Vector3 handler2;//结束点的控制点
        public bool is2B;//是否2次贝塞尔，只有1个控制点
        public bool useStart = true;
        public int handlerTag = 1;

        public bool worldSpace = true;
        private float duration;

        private Transform mTrans;
        private Transform mParent = null;
        private bool isParentInit = false;
        #endregion
        #region Properties
        public Transform cachedTransform { get { if (mTrans == null) mTrans = transform; return mTrans; } }
        private Transform parentTransform { get { if (!isParentInit) { mParent = cachedTransform.parent; isParentInit = true; } return mParent; } }
        public Vector3 value
        {
            get
            {
                return worldSpace ? cachedTransform.position : cachedTransform.localPosition;
            }
            set
            {
                if (worldSpace) cachedTransform.position = value;
                else cachedTransform.localPosition = value;
            }
        }
        #endregion
        void OnDrawGizmos()
        {
            GenDefaultHandler();
            Vector3 prevPos = from;
            if (!worldSpace && null != parentTransform)
            {
                prevPos = parentTransform.localToWorldMatrix.MultiplyPoint3x4(prevPos);
            }
            for (int c = 1; c <= 100; c++)
            {
                Vector3 curPos = Bezier3(from, handler1, handler2, to, (float)c / 100);
                if (!worldSpace && null != parentTransform)
                    curPos = parentTransform.localToWorldMatrix.MultiplyPoint3x4(curPos);
                Gizmos.color = Color.red;
                Gizmos.DrawLine(prevPos, curPos);
                prevPos = curPos;
            }
        }
        protected void OnUpdate(float factor, bool isFinished)
        {
            if (isFinished)
                value = to;
            else
            {
                if (is2B)
                    value = Bezier2(from, handler1, to, factor);
                else
                    value = Bezier3(from, handler1, handler2, to, factor);
            }
        }
        #region Private Method
        private Vector2 Bezier2(Vector2 s, Vector2 c, Vector2 e, float t)
        {
            return (((1 - t) * (1 - t)) * s) + (2 * t * (1 - t) * c) + ((t * t) * e);
        }
        private Vector3 Bezier2(Vector3 s, Vector3 c, Vector3 e, float t)
        {
            return (((1 - t) * (1 - t)) * s) + (2 * t * (1 - t) * c) + ((t * t) * e);
        }
        private Vector2 Bezier3(Vector2 s, Vector2 st, Vector2 et, Vector2 e, float t)
        {
            return (((-s + 3 * (st - et) + e) * t + (3 * (s + et) - 6 * st)) * t + 3 * (st - s)) * t + s;
        }
        private Vector3 Bezier3(Vector3 s, Vector3 st, Vector3 et, Vector3 e, float t)
        {
            return (((-s + 3 * (st - et) + e) * t + (3 * (s + et) - 6 * st)) * t + 3 * (st - s)) * t + s;
        }
        private void Bezier3DefaultHandler(Vector3 s, Vector3 e, ref Vector3 st, ref Vector3 et, int tag, bool useS)
        {
            switch (tag)
            {
                case 1:
                    st.x = s.x + (e.x - s.x) / 4f;
                    et.x = e.x - (e.x - s.x) / 4f;
                    st.y = useS ? s.y : e.y;
                    et.y = st.y;
                    st.z = useS ? s.z : e.z;
                    et.z = st.z;
                    break;
                case 2:
                    st.y = s.y + (e.y - s.y) / 4f;
                    et.y = e.y - (e.y - s.y) / 4f;
                    st.x = useS ? s.x : e.x;
                    et.x = st.x;
                    st.z = useS ? s.z : e.z;
                    et.z = st.z;
                    break;
                default:
                    st = s;
                    et = e;
                    break;
            }
        }
        #endregion
        #region Public Method
        public void GenDefaultHandler()
        {
            Bezier3DefaultHandler(from, to, ref handler1, ref handler2, handlerTag, useStart);
        }
        public void BezierTo(float duration, Vector3 pos)
        {
            this.duration = duration;
            this.from = this.value;
            this.to = pos;
            this.GenDefaultHandler();
        }
        static public BezierPosition Set(GameObject go, float duration, Vector3 pos, bool useStart, int handlerTag, bool worldSpace = false)
        {
            BezierPosition bp = go.GetComponent<BezierPosition>();
            if (null == bp)
                bp = go.AddComponent<BezierPosition>();
            bp.duration = duration;
            bp.worldSpace = worldSpace;
            bp.from = bp.value;
            bp.to = pos;
            bp.useStart = useStart;
            bp.handlerTag = handlerTag;
            bp.GenDefaultHandler();
            return bp;
        }
        #endregion
    }
}
