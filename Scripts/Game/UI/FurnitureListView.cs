using System;
using UnityEngine;
using FrameWork;

namespace Game
{
    public class FurnitureListView : SimpleListView<LuaItem, LuaInjector>
    {
        public Action<LuaItem> onItemAdd;
        public Action<LuaItem> onItemRemove;
        protected override void OnItemAdd(LuaItem item)
        {
            base.OnItemAdd(item);
            if (onItemAdd != null)
            {
                onItemAdd(item);
            }
        }

        protected override void OnItemRemove(LuaItem item)
        {
            base.OnItemRemove(item);
            if (onItemRemove != null)
            {
                onItemRemove(item);
            }
        }
    }
}
