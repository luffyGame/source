using System;
using FrameWork;
using UnityEngine;

namespace Game
{
    public class DlgView
    {
        private AssetModel model;
        private DlgBehavior behavior;
        public LuaInjector injector { get; private set; }
        private string res;

        private RectTransform rectTrans
        {
            get { return null == model ? null : model.trans as RectTransform; }
        }

        public DlgView(string res)
        {
            this.res = res;
        }

        public void Load(Action onLoaded)
        {
            model = AssetModelFactory.CreateModel(AssetType.UI_PANEL, res);
            model.Load(OnLoaded+onLoaded);
        }

        private void OnLoaded()
        {
            behavior = model.GetComponent<DlgBehavior>();
            injector = model.GetComponent<LuaInjector>();
        }

        public void Release()
        {
            behavior = null;
            injector = null;
            if (null != model)
            {
                model.Release();
                model = null;
            }
        }

        public void SetRoot(Transform root)
        {
            if (null != model)
            {
                model.SetParent(root);
                RectTransform rectT = rectTrans;
                if (null != rectT)
                {
                    rectT.offsetMin = Vector2.zero;
                    rectT.offsetMax = Vector2.zero;
                }
            }
        }

        public void DisplayAsLast()
        {
            if(null!=rectTrans)
                rectTrans.SetAsLastSibling();
        }
        
        public void DisplayAsFirst()
        {
            if(null!=rectTrans)
                rectTrans.SetAsFirstSibling();
        }

        public void SetSlibingIndex(int index)
        {
            if (null != rectTrans)
            {
                if(index<0)
                    rectTrans.SetAsFirstSibling();
                else if(index<rectTrans.parent.childCount)
                    rectTrans.SetSiblingIndex(index);
                else
                    rectTrans.SetAsLastSibling();
            }
        }

        public int GetSlibingIndex()
        {
            if (null != rectTrans)
                return rectTrans.GetSiblingIndex();
            return -1;
        }

        public Component GetComponent(Type type)
        {
            if (null != model)
            {
                return model.GetComponent(type);
            }

            return null;
        }
    }
}