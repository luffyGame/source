/* 
 * @Descript Item基类 
 * @Author SunShubin
 * @Time 2018-03-17
 */

using System.Runtime.CompilerServices;
using FrameWork;
using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(LuaInjector))]
    public class LuaItem : SimpleListViewItem<LuaInjector>
    {
        private void Awake()
        {
            this.Data = this.GetComponent<LuaInjector>();
        }
    }
}