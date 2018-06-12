using System;
using System.Collections.Generic;
using System.ComponentModel;
using FrameWork;

namespace Game
{
    public class Const
    {
        public const float CELL_SIZE = 2f;//建筑格子尺寸
        //场景中静态环境
        public const int LAYER_GROUND = 16;//环境地表
        public const int LAYER_AIR_WALL = 20; //空气墙
        //场景中的对象本体
        public const int LAYER_PLAYER_COLLIDER = 10;//代表玩家
        public const int LAYER_MONSTER_COLLIDER = 11;//代表怪物
        public const int LAYER_ITEM_COLLIDER = 15;//代表场景物体
        public const int LAYER_FURNITURE_COLLIDER = 21;//工作台
        public const int LAYER_BUILDING_COLLIDER = 22;//基础建筑:地板、墙
        public const int LAYER_FURNITURE_COLLIDER_MASK = 1 << LAYER_FURNITURE_COLLIDER;
        public const int LAYER_BUILDING_COLLIDER_MASK = 1 << LAYER_BUILDING_COLLIDER;

        public const int LAYER_PLAYER_COLLIDER_MASK = 1 << LAYER_PLAYER_COLLIDER;
        public const int LAYER_MONSTER_COLLIDER_MASK = 1 << LAYER_MONSTER_COLLIDER;
        public const int LAYER_ITEM_COLLIDER_MASK = 1 << LAYER_ITEM_COLLIDER;

        public const int LAYER_GROUND_COLLIDER_MASK = 1 << LAYER_GROUND;

        public const int LAYER_AIR_WALL_COLLIDER_MASK = 1 << LAYER_AIR_WALL;
        //触发区
        public const int LAYER_AREA_TRIGGER_PLAYER = 12;//玩家出发去
        public const int LAYER_AREA_TRIGGER_MONSTER = 13;//怪物触发区
        
        //==============================
        //功能使用
        public const int LAYER_CHARACTER_MOVER = 8; //角色移动
        public const int LAYER_NO_COLLIDER = 9;//用于肢解死亡纯表现碰撞
        public const int LAYER_BUILD_GROUND = 14;//建筑时的地表检测
        public const int LAYER_BUILD_GROUND_MASK = 1 << LAYER_BUILD_GROUND;

        public const int LAYER_OBSTACLE = 17;//障碍

        public const int LAYER_USE_CHECK = 18;//使用检查

        public const int LAYER_USABLE = 19;//单独的可使用层级

        public const int LAYER_TEST = 31;//仅用于测试，不得在游戏中使用
        //
        public const int SKILL_CHECK_LAYER_MASK = LAYER_PLAYER_COLLIDER_MASK | LAYER_MONSTER_COLLIDER_MASK | LAYER_GROUND_COLLIDER_MASK;//技能检测

        public static Dictionary<int, string> GetAllLayerSet()
        {
            Dictionary<int,string> layers = new Dictionary<int, string>();
            
            layers.Add(LAYER_PLAYER_COLLIDER,"player_collider");
            layers.Add(LAYER_MONSTER_COLLIDER,"monster_collider");
            layers.Add(LAYER_ITEM_COLLIDER,"item_collider");
            
            layers.Add(LAYER_AREA_TRIGGER_PLAYER,"area_player_trigger");
            layers.Add(LAYER_AREA_TRIGGER_MONSTER,"area_monster_trigger");
            
            layers.Add(LAYER_CHARACTER_MOVER,"charater_mover");
            layers.Add(LAYER_NO_COLLIDER,"no_collider");
            layers.Add(LAYER_BUILD_GROUND,"build_ground");
            
            layers.Add(LAYER_GROUND,"ground");
            layers.Add(LAYER_OBSTACLE,"obstacle");
            layers.Add(LAYER_USE_CHECK,"use_check");
            layers.Add(LAYER_USABLE,"usable");
            layers.Add(LAYER_FURNITURE_COLLIDER,"furniture_collider");
            layers.Add(LAYER_BUILDING_COLLIDER, "building_collider");
            layers.Add(LAYER_AIR_WALL,"airwall");
            
            //=================================
            layers.Add(LAYER_TEST,"test");
            return layers;
        }

        public static int[] GetCollisionLayers(int layer)
        {
            switch (layer)
            {
                case LAYER_NO_COLLIDER:
                    return new[] {LAYER_GROUND,LAYER_OBSTACLE};
                case LAYER_AREA_TRIGGER_MONSTER:
                    return new[] {LAYER_MONSTER_COLLIDER};
                case LAYER_AREA_TRIGGER_PLAYER:
                    return new[] {LAYER_PLAYER_COLLIDER};
                case LAYER_CHARACTER_MOVER:
                    return new[] {LAYER_GROUND,LAYER_CHARACTER_MOVER,LAYER_OBSTACLE,LAYER_FURNITURE_COLLIDER,LAYER_BUILDING_COLLIDER,LAYER_AIR_WALL};
                case LAYER_USE_CHECK:
                    return new[] {LAYER_USABLE};
                case LAYER_FURNITURE_COLLIDER:
                    return new[] {LAYER_FURNITURE_COLLIDER,LAYER_CHARACTER_MOVER, LAYER_MONSTER_COLLIDER,LAYER_BUILDING_COLLIDER};

                case LAYER_TEST:
                    return new[] {LAYER_TEST};

                case LAYER_BUILDING_COLLIDER:
                    return new[] { LAYER_BUILDING_COLLIDER,LAYER_CHARACTER_MOVER, LAYER_MONSTER_COLLIDER,LAYER_FURNITURE_COLLIDER};

                default:
                    return null;
            }
        }
    }
    
    public enum EquipPart : int
    {
        NONE = -1,//未定义
        HEAD = 0,//头
        CLOTH = 1,//上衣
        PANTS = 2,//裤子
        FOOT = 3,//脚
        HAIR = 4,//头发
        BAG = 5,//背包
        WEAPON = 6,//武器
    }

    public enum EquipMask : int
    {
        NONE = 0,
        HEAD = 1 << 0,
        CLOTH = 1 << 1,
        PANTS = 1 << 2,
        FOOT = 1 << 3,
        HAIR = 1 << 4,
        BAG = 1 << 5,
        WEAPON = 1 << 6,
    }
}