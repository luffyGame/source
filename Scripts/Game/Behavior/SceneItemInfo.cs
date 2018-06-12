using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class SceneItemInfo : ObjInfo
    {
        #region Vars
        public Animation animation;
        public GameObject obstacle;
        public ObjCollider itemCollider;
        public UseableCollider useableCollider;
        
        public Transform[] mounts;//锚点

        public override ObjCollider objCollider
        {
            get { return itemCollider; }
        }

        #endregion

        public override void Release()
        {
            if (null != useableCollider)
            {
                useableCollider.Release();
            }
            base.Release();
        }

        public void SetUseEnable(bool benable)
        {
            if(null!=useableCollider)
                useableCollider.useable = benable;
        }

        public void EnableObstacle(bool benable)
        {
            if(null!=obstacle)
                obstacle.SetActive(benable);
        }
    }
}