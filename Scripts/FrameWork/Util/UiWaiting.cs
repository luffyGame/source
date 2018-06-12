using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public class UiWaiting : MonoBehaviour
    {
        public bool isShow { get; private set; }
        public FrameAnimation ani;
        public void Show(bool bShow)
        {
            isShow = bShow;
            gameObject.SetActive(bShow);
            if (bShow)
            {
                ani.Play();
            }
            else
                ani.Stop();
        }
    }
}
