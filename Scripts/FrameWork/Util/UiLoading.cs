using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public class UiLoading : MonoBehaviour
    {
        public bool isShow { get; private set; }
        public RectTransform spin;
        void Update()
        {
            spin.Rotate(Vector3.forward, 90.0f * Time.deltaTime);
        }
        public void Show(bool bShow)
        {
            isShow = bShow;
            gameObject.SetActive(bShow);
            if (bShow)
                spin.localRotation = Quaternion.identity;
        }
    }
}
