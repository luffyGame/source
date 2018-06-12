using System;
using UnityEngine;
using FrameWork;

namespace Game
{
    public class Config : SingleBehavior<Config>
    {
        public RagdollBodypartInfo ragdollBodyInfo;
        public RagdollRandomValue ragdollRandomValue;
        public RagdollDir ragdollDir;
        public RagdollPartId ragdollPartId;

        #region RagDoll  

        public int GetBodyPartId(int boomId)
        {
            int[] temp = ragdollBodyInfo.bodyPartInfos[boomId].bodypart;
            int id = Global.RandomRange(0, temp.Length);
            return temp[id];
        }

        public float GetFrictionRandom()
        {
            return ragdollRandomValue.friction;
        }

        public float GetAngleFrictionRandom()
        {
            return ragdollRandomValue.angleFriction;
        }

        public Vector3 GetSpeedRandom()
        {
            return ragdollRandomValue.speed;
        }

        public Vector3 GetAngleSpeedRandom()
        {
            return ragdollRandomValue.angleSpeed;
        }

        public Vector3 GetRagdollSpeedRandom()
        {
            return ragdollRandomValue.ragdollSpeed;
        }

        public Vector3 GetRagdollDir(int bodyPartInfo, int powerType)
        {
            if(bodyPartInfo < 0 || bodyPartInfo >= ragdollDir.BodyPartInfo.Length)
            {
                return Vector3.zero;
            }

            powerType += 1;

            var power = ragdollDir.BodyPartInfo[bodyPartInfo].Power_Dir;
            foreach(var dir in power)
            {
               if(powerType == dir.powerType)
                {
                    return dir.direction;
                }
            }

            return Vector3.zero;
        }

        public int GetRagdollPartId(int partId)
        {
            if(partId < 1 || partId > 3) { return 0;}
            var power = ragdollPartId.BodyPartInfo[partId - 1].id;
            var tempId = Global.RandomRange(0, power.Length);
            return power[tempId];
        }

        #endregion
    }
}
