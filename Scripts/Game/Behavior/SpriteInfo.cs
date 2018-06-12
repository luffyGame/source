using FrameWork;
using UnityEngine;

namespace Game
{
    public class SpriteInfo : ObjInfo
    {
        #region Vars

        public int boneCount;
        public int trisCount;
        public Animation animation;
        public CharacterController cct;
        public NavFollower follower;
        public EquipWear equipWear;
        public Dismemberment dismemberment;
        public RagdollManagerHum ragdollManager;
        public UseChecker useChecker;
        public ObjCollider spriteCollider;
        public UseableCollider useableCollider;

        public override ObjCollider objCollider
        {
            get { return spriteCollider; }
        }

        public Transform[] mounts;//锚点

        #endregion
        public void SetUseEnable(bool bUsable)
        {
            if (null != useableCollider)
            {
                useableCollider.useable = bUsable;
                useableCollider.SetActive(bUsable);
            }
        }

        public override void Release()
        {
            if(null!=useableCollider)
                useableCollider.Release();
            Alive(true);
            SetUseEnable(false);
            base.Release();
        }

        public void Alive(bool alivable)
        {
            if (null != cct)
                cct.enabled = alivable;
            if(null!=spriteCollider)
                spriteCollider.SetActive(alivable);
        }
    }
}