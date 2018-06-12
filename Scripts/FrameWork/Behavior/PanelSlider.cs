using System;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace FrameWork
{
    public class PanelSlider : MonoBehaviour
    {
        #region Use Enums
        public enum AniType : int
        {
            NONE = 0,
            //显示
            SHOW_MOVE_LEFT = 1,
            SHOW_MOVE_RIGHT = 2,

            //关闭
            HIDE_MOVE_LEFT = 3,
            HIDE_MOVE_RIGHT = 4,
        }
        #endregion
        #region Var
        public AniType testType = AniType.NONE;
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
        #endregion
        #region Public Method
        public void Play(AniType aniType,Action handler = null)
        {
            bool playAni = true;
            Vector2 targetPos = Vector2.zero;
            switch(aniType)
            {
                case AniType.SHOW_MOVE_LEFT:
                    RcTrans.anchoredPosition = new Vector2(RcTrans.rect.width, 0);
                    targetPos = Vector2.zero;
                    break;
                case AniType.SHOW_MOVE_RIGHT:
                    RcTrans.anchoredPosition = new Vector2(-RcTrans.rect.width, 0);
                    targetPos = Vector2.zero;
                    break;
                case AniType.HIDE_MOVE_LEFT:
                    RcTrans.anchoredPosition = Vector2.zero;
                    targetPos = new Vector2(-RcTrans.rect.width, 0);
                    break;
                case AniType.HIDE_MOVE_RIGHT:
                    RcTrans.anchoredPosition = Vector2.zero;
                    targetPos = new Vector2(RcTrans.rect.width, 0);
                    break;
                default:
                    playAni = false;
                    break;
            }
            if (playAni)
            {
                Global.Instance.UiTopMask.SetActive(true);
                RcTrans.DOAnchorPos(targetPos, 0.5f).SetEase(Ease.InQuart).OnComplete(() =>
                {
                    Global.Instance.UiTopMask.SetActive(false);
                    if (null != handler)
                        handler();
                });
            }
        }
        #endregion
        #region Private Method
        [ContextMenu("Test")]
        private void Test()
        {
            Play(testType);
        }
        #endregion
    }
}
