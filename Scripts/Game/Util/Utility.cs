using System;
using FrameWork;
using UnityEngine;

namespace Game
{
    public class Utility
    {
        
        #region Collider Method
        /// <summary>
        /// 技能是否能到达指定位置，或触碰到指定碰撞体
        /// </summary>
        /// <param name="from"></param>
        /// <param name="toPos">返回由于碰撞或技能距离返回的终点位置</param>
        /// <param name="skillRange">技能距离</param>
        /// <param name="target">为空的情况是单指定位置</param>
        /// <returns></returns>
        public static bool CheckSkillReach(Vector3 from,ref Vector3 toPos, float skillRange, Collider target=null)
        {
            bool canReach = true;
            RaycastHit hit;
            Vector3 dir = toPos - from;
            dir.y = 0f;
            float reachDist = dir.magnitude;
            dir.Normalize();
            float dist = reachDist;
            if (reachDist > skillRange)
            {
                dist = skillRange;
                toPos = from + dir * dist;
                if(target==null)
                    canReach = false;
            }
            
            if (Physics.Raycast(from, dir, out hit, dist, Const.SKILL_CHECK_LAYER_MASK))
            {
                toPos = hit.point;
                if(canReach)
                    canReach = (hit.collider == target);
            }

            return canReach;
        }
        #endregion
    }
}