/* 
 * @Descript 背包基类 
 * @Author SunShubin
 * @Time 2018-03-17
 */
using FrameWork;
using System;
namespace Game
{
    public class LuaSimpleList : SimpleListView<LuaItem, LuaInjector>
    {
        public Action<LuaItem> onItemAdd;
        public Action<LuaItem> onItemRemove;
		protected override void OnItemAdd(LuaItem item)
		{
            base.OnItemAdd(item);
            if(onItemAdd != null){
                onItemAdd(item);
            }
		}
		protected override void OnItemRemove(LuaItem item)
		{
            base.OnItemRemove(item);
            if(onItemRemove != null){
                onItemRemove(item);
            }
		}
	}
}
