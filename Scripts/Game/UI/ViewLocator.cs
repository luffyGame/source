using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Game
{
    public class ViewLocator : SingleBehavior<ViewLocator>
    {
        public Transform[] layers;
        public static ViewLocator Inst
        {
            get { return Instance; }
        }
        public void LoadView(string res,Action<DlgView> onLoaded,int layer)
        {
            DlgView view = new DlgView(res);
            view.Load(() =>
            {
                view.SetRoot(layers[layer]);
                onLoaded(view);
            });
        }
    }
}