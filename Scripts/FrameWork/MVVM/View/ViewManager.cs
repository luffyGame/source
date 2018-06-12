using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork.MVVM
{
    public class ViewManager : Singleton<ViewManager>
    {
        private List<IView> views = new List<IView>();
        
        public T FindView<T>() where T : MonoBehaviour,IView
        {
            foreach(IView view in views)
            {
                if (view is T)
                    return view as T;
            }
            return null;
        }

        public void AddView(IView view)
        {
            views.Add(view);
        }
        public void RemoveView(IView view)
        {
            views.Remove(view);
        }

        public T ForceGetView<T>(string res) where T : MonoBehaviour,IView
        {
            T view = FindView<T>();
            if (view == null)
                view = ViewLocator.Instance.LoadView<T>(res);
            return view;
        }
    }
}
