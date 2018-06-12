using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FrameWork.MVVM
{
    public abstract class BindingBuilder
    {
        private bool builded;
        public virtual bool Build() { if (builded) return false; builded = true; return true; }
        public virtual bool Unbuild() { if (!builded) return false;builded = false;return true; }
    }
    public class BingdingBuilderWithProperty<T>: BindingBuilder
    {
        private BindableProperty<T> src;
        private BindableProperty<T>.ValueChangeHandler handler;
        public BingdingBuilderWithProperty(BindableProperty<T> src)
        {
            this.src = src;
        }
        public void Handle(BindableProperty<T>.ValueChangeHandler handler)
        {
            this.handler = handler;
        }
        public override bool Build()
        {
            if (!base.Build())
                return false;
            src.onValueChanged += handler;
            return true;
        }
        public override bool Unbuild()
        {
            if (!base.Unbuild())
                return false;
            src.onValueChanged -= handler;
            return true;
        }
    }
    public class BindingBuilderWithUnityEvent : BindingBuilder
    {
        private UnityAction handler;
        private UnityEvent unityEvent;
        public BindingBuilderWithUnityEvent(UnityEvent unityEvent)
        {
            this.unityEvent = unityEvent;
        }
        public void Handle(Action handler)
        {
            this.handler = new UnityAction(handler);
        }
        public override bool Build()
        {
            if (!base.Build())
                return false;
            unityEvent.AddListener(handler);
            return true;
        }
        public override bool Unbuild()
        {
            if (!base.Unbuild())
                return false;
            unityEvent.RemoveListener(handler);
            return true;
        }
    }
    public class BindingBuilderWithUnityEvent<T0> : BindingBuilder
    {
        private UnityAction<T0> handler;
        private UnityEvent<T0> unityEvent;
        public BindingBuilderWithUnityEvent(UnityEvent<T0> unityEvent)
        {
            this.unityEvent = unityEvent;
        }
        public void Handle(Action<T0> handler)
        {
            this.handler = new UnityAction<T0>(handler);
        }
        public override bool Build()
        {
            if (!base.Build())
                return false;
            unityEvent.AddListener(handler);
            return true;
        }
        public override bool Unbuild()
        {
            if (!base.Unbuild())
                return false;
            unityEvent.RemoveListener(handler);
            return true;
        }
    }
    public class BindingBuilderWithUnityEvent<T0, T1> : BindingBuilder
    {
        private UnityAction<T0, T1> handler;
        private UnityEvent<T0,T1> unityEvent;
        public BindingBuilderWithUnityEvent(UnityEvent<T0,T1> unityEvent)
        {
            this.unityEvent = unityEvent;
        }
        public override bool Build()
        {
            if (!base.Build())
                return false;
            unityEvent.AddListener(handler);
            return true;
        }
        public override bool Unbuild()
        {
            if (!base.Unbuild())
                return false;
            unityEvent.RemoveListener(handler);
            return true;
        }
        public void Handle(Action<T0,T1> handler)
        {
            this.handler = new UnityAction<T0,T1>(handler);
        }
    }
}
