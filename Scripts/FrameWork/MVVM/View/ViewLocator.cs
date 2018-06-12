using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork.MVVM
{
    public class ViewLocator : SingleBehavior<ViewLocator>
    {
        private Transform viewRoot;
        public Transform ViewRoot
        {
            get { return viewRoot ?? (viewRoot = transform); }
        }
        public virtual T LoadView<T>(string res) where T : MonoBehaviour,IView
        {
            Transform vt = ViewRoot.Find(res);
            if (null != vt)
                return vt.GetComponent<T>();
            return null;
        }
    }
}
