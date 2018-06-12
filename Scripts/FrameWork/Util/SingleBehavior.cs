using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public class SingleBehavior<T> : MonoBehaviour where T : SingleBehavior<T>
    {
        #region Var
        private static T s_instance = null;
        public static T Instance { get { return s_instance; } }
        #endregion
        #region Behaviro
        public void Awake()
        {
            if (null == s_instance)
            {
                s_instance = this as T;
                s_instance.OnInit();
            }
        }
        void Start()
        {
            if (null != s_instance)
                OnStart();
        }
        void OnDestroy()
        {
            if (null != s_instance)
            {
                OnQuit();
                s_instance = null;
            }
        }
        void OnApplicationQuit()
        {
            if (null != s_instance)
            {
                OnQuit();
                s_instance = null;
            }
        }
        #endregion
        #region Public Method
        public virtual void OnInit() { }
        public virtual void OnStart() { }
        public virtual void OnQuit() { }
        #endregion
    }
}
