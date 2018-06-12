using System;
using System.Collections.Generic;

namespace FrameWork
{
    public class Singleton<T> where T:Singleton<T>
    {
        #region Var
        private static T s_instance = Activator.CreateInstance<T>();
        public static T Instance { get { return s_instance; } }
        #endregion
        protected Singleton() { }
    }
}
