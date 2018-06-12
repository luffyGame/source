using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace FrameWork.MVVM
{
    public class BindingSet
    {
        private readonly List<BindingBuilder> builders = new List<BindingBuilder>();
        public BingdingBuilderWithProperty<T> Bind<T>(BindableProperty<T> src)
        {
            BingdingBuilderWithProperty<T> builder = new BingdingBuilderWithProperty<T>(src);
            builders.Add(builder);
            return builder;
        }
        public BindingBuilderWithUnityEvent Bind(UnityEvent unityEvent)
        {
            BindingBuilderWithUnityEvent builder = new BindingBuilderWithUnityEvent(unityEvent);
            builders.Add(builder);
            return builder;
        }
        public BindingBuilderWithUnityEvent<T> Bind<T>(UnityEvent<T> unityEvent)
        {
            BindingBuilderWithUnityEvent<T> builder = new BindingBuilderWithUnityEvent<T>(unityEvent);
            builders.Add(builder);
            return builder;
        }
        public void Build()
        {
            foreach (BindingBuilder builder in builders)
                builder.Build();
        }
        public void Unbuild()
        {
            foreach (BindingBuilder builder in builders)
                builder.Unbuild();
        }
        public void Clear()
        {
            builders.Clear();
        }
    }
}
