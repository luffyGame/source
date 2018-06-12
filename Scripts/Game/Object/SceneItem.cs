using System;
using FrameWork;
using UnityEngine;
using UnityEngine.AI;

namespace Game
{
    public class SceneItem : ObjBase
    {
        #region Var
        private string res;
        private Animation animation
        {
            get
            {
                if (theInfo == null)
                {
                    return null;
                }
                return theInfo.animation;
            }
        }

        private AniClipLen clipLen;
        private SceneItemInfo theInfo;
        public override ObjType objType
        {
            get { return ObjType.O_SCENE_ITEM; }
        }

        public override ObjInfo info
        {
            get { return theInfo; }
        }

        #endregion
        #region Public Method
        public SceneItem(string res)
        {
            this.res = res;
        }
        public override void Load(Action done = null)
        {
            if (this.IsLoaded())
                return;
            rootModel = AssetModelFactory.CreateModel(AssetType.MODEL_SCENEITEM,res);
            rootModel.Load(OnLoaded + done);
        }

        public override void Release()
        {
            if(null!=theInfo)
                theInfo.Release();
            base.Release();
        }

        public float PlayAni(string act)
        {
            if (null != animation)
            {
                animation.Play(act);
            }
            if (null != clipLen && clipLen.ContainsKey(act))
                return clipLen[act];
            return 0f;
        }


        public void SetUsable(bool bUsable)
        {
            if (null != theInfo)
            {
                theInfo.SetUseEnable(bUsable);
            }
        }

        public void EnableObstacle(bool bEnable)
        {
            if(null!=theInfo)
                theInfo.EnableObstacle(bEnable);
        }

        /// <summary>
        /// 得到锚点
        /// </summary>
        /// <param name="mountTag"></param>
        /// <returns></returns>
        public Transform GetMount(int mountTag)
        {
            if (null != info)
                return theInfo.mounts[mountTag];
            return null;
        }
        
        public float PlayDefaultAnim()
        {
            if (null == clipLen || clipLen.Count == 0)
                return 0f;
            return PlayAni(clipLen.UniqueAni);
        }

        #endregion
        #region Private Method

        private void OnLoaded()
        {
            theInfo = rootModel.GetComponent<SceneItemInfo>();
            this.InfoRef();
            CheckGetActionSet();
        }

        private void CheckGetActionSet()
        {
            if(null == animation)
                return;
            clipLen = AniClipLenCfg.Instance.GetClipLen(res);
            if (null == clipLen)
            {
                clipLen = new AniClipLen(animation);
                AniClipLenCfg.Instance.SetClipLen(res,clipLen);
            }
        }
        #endregion
    }
}