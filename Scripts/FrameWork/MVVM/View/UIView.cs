using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FrameWork.MVVM
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public abstract class UIView<T> : UIBehaviour,IView where T : ViewModelBase
    {
        public T viewModel { get; protected set; }
        private bool opened;
        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;
        private GameObject gameObj;
        protected readonly BindingSet bindingSet = new BindingSet();

        public GameObject GameObj
        {
            get
            {
                return (this.gameObj ?? (this.gameObj = base.gameObject));
            }
        }
        public virtual CanvasGroup CanvasGroup
        {
            get
            {
                return (this.canvasGroup ?? (this.canvasGroup = GetComponent<CanvasGroup>()));
            }
        }
        public virtual RectTransform RectTransform
        {
            get
            {
                return (rectTransform ?? (rectTransform = GetComponent<RectTransform>()));
            }
        }
        public virtual float Alpha
        {
            get
            {
                return this.CanvasGroup.alpha;
            }
            set
            {
                this.CanvasGroup.alpha = value;
            }
        }
        public virtual bool Interactable
        {
            get
            {
                return this.CanvasGroup.interactable;
            }
            set
            {
                this.CanvasGroup.interactable = value;
            }
        }
        public virtual bool Visibility
        {
            get
            {
                return GameObj.activeSelf;
            }
            set
            {
                GameObj.SetActive(value);
            }
        }

        public virtual string Name
        {
            get
            {
                return GameObj.name;
            }
            set
            {
                GameObj.name = value;
            }

        }

        public void Close()
        {
            Visibility = false;
            if(opened)
            {
                opened = false;
                ViewManager.Instance.RemoveView(this);
                OnClose();
            }
        }

        public void Open()
        {
            if(!opened)
            {
                opened = true;
                ViewManager.Instance.AddView(this);
                OnOpen();
            }
            Visibility = true;
        }

        protected virtual void OnOpen()
        {
        }

        protected virtual void OnClose()
        {
            bindingSet.Unbuild();
            bindingSet.Clear();
            viewModel = null;
        }
    }
}
