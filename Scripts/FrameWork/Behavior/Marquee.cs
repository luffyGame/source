using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace FrameWork
{
    public class Marquee : MonoBehaviour
    {
        public Text text;
        public float speed = 20f;//每秒字数
        private RectTransform rT;
        public RectTransform Rt { get { if (null == rT) rT = transform as RectTransform; return rT; } }
        private RectTransform textRt;
        public RectTransform TextRt { get { if (null == textRt) textRt = text.rectTransform; return textRt; } }
        private GameObject go;
        public GameObject Go { get { if (null == go) go = gameObject; return go; } }
        #region public Method
        public void DoMarquee(string txt)
        {
            Go.SetActive(true);
            TextRt.localPosition = new Vector3(Rt.sizeDelta.x, 0);
            int length = Utils.GetTextLength(txt, text);
            text.text = txt;
            Tween tweener = TextRt.DOLocalMoveX(-length, (float)txt.Length / speed);
            tweener.OnComplete(() =>
            {
                Go.SetActive(false);
            });
        }
        #endregion
    }
}
