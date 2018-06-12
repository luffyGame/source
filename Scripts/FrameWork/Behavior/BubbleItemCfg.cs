using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using URandom = UnityEngine.Random;

namespace FrameWork{
    public class BubbleItemCfg : MonoBehaviour
    {
        public Text lable;
        public RectTransform baseTrans;
        public CanvasGroup canvasGroup;
        public void SetText(string text)
        {
            if (null != lable)
                lable.text = text;
        }

        public void SetTextColor(Color color)
        {
            if (null != lable)
                lable.color = color;
        }
        public void SetLocalScale(Vector3 scale)
        {
            if (null != baseTrans)
                baseTrans.localScale = scale;
        }
        public void SetLocalPos(Vector3 pos)
        {
            if (null != baseTrans)
                baseTrans.localPosition = pos;
        }
        public void SetAlpha(float alpha)
        {
            if (null != canvasGroup)
                canvasGroup.alpha = alpha;
        }
        public float GetLocalHeight()
        {
            return baseTrans.localScale.y * baseTrans.rect.height;
        }

        public float GetFinalHeight()
        {
            return baseTrans.rect.height;
        }
    }
}
