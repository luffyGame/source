using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public class PanelKeepSize : MonoBehaviour
    {
        #region Var
        public bool userGlobalSize = true;
        private RectTransform rcTrans;
        public RectTransform RcTrans
        {
            get
            {
                if (null == rcTrans)
                    rcTrans = transform as RectTransform;
                return rcTrans;
            }
        }
        private RectTransform parentCanvasRc;
        public RectTransform ParentCanvasRc
        {
            get
            {
                if (null == parentCanvasRc)
                    parentCanvasRc = GetCanvasRectTransInParent();
                return parentCanvasRc;
            }
        }
        #endregion
        #region Behavior Method
        void OnStart()
        {
            KeepSize();
        }
        #endregion
        #region Private Method
        [ContextMenu("KeepSize")]
        private void KeepSize()
        {
            if (Application.isPlaying)
            {
                if (userGlobalSize&&null!=Global.Instance)
                    RcTrans.sizeDelta = Global.Instance.CanvasSize;
                else
                    RcTrans.sizeDelta = ParentCanvasRc.sizeDelta;
            }
            else
                RcTrans.sizeDelta = ParentCanvasRc.sizeDelta;
        }
        [ContextMenu("ResetAnchor")]
        private void ResetAnchor()
        {
            RcTrans.anchorMin = new Vector2(0, 1);
            RcTrans.anchorMax = new Vector2(0, 1);
            RcTrans.pivot = new Vector2(0, 1);
            RcTrans.anchoredPosition = Vector2.zero;
        }

        private RectTransform GetCanvasRectTransInParent()
        {
            Canvas canvas = gameObject.GetComponentInParent<Canvas>();
            if (null != canvas)
                return canvas.GetComponent<RectTransform>();
            return null;
        }
        #endregion
    }
}
