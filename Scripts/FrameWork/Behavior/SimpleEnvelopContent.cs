using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork{
    /// <summary>
    /// 注意anchor的各种影响
    /// </summary>
    public class SimpleEnvelopContent : MonoBehaviour
    {
        #region Var
        private RectTransform rectT;
        public RectTransform RectT { get { if (null == rectT) rectT = transform as RectTransform; return rectT; } }
        public RectTransform outline;//轮廓
        private Rect bound;
        public Vector2 gaping;
        public Vector2 Size
        {
            get { return bound.size; }
        }
        #endregion
        #region Public Method
        [ContextMenu("Execute")]
        public void Execute()
        {
            bound = GetChildBound();
            if(null!=outline)
            {
                outline.localPosition = new Vector3(bound.xMin, bound.yMax);
                outline.sizeDelta = bound.size;
            }
            //RectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, bound.size.x);
            //RectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, bound.size.y);
        }
        #endregion
        #region Private Method
        private Rect GetChildBound()
        {
            Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 max = new Vector2(float.MinValue, float.MinValue);
            for (int i = 0, imax = RectT.childCount; i < imax; i++)
            {
                RectTransform child = RectT.GetChild(i) as RectTransform;
                if (child == outline)
                    continue;
                Vector3[] corners = new Vector3[4];
                child.GetWorldCorners(corners);
                min.x = Mathf.Min(min.x, corners[0].x, corners[2].x);
                min.y = Mathf.Min(min.y, corners[0].y, corners[2].y);
                max.x = Mathf.Max(max.x, corners[0].x, corners[2].x);
                max.y = Mathf.Max(max.y, corners[0].y, corners[2].y);
            }
            Vector3 lmin = RectT.InverseTransformPoint(min);
            Vector3 lmax = RectT.InverseTransformPoint(max);
            lmin.x = Mathf.Min(lmin.x, 0f);
            lmin.y = Mathf.Min(lmin.y, 0f) - gaping.y;
            lmax.x = Mathf.Max(lmax.x, 0f) + gaping.x;
            lmax.y = Mathf.Max(lmax.y, 0f);
            return new Rect(lmin.x, lmin.y, lmax.x - lmin.x, lmax.y - lmin.y);
        }
        #endregion
    }
}
