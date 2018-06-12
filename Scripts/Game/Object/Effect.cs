using System;
using FrameWork;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 特效分为：
    /// 0.武器和动作特效，表现上和武器、动作直接关联，但设计上采用逻辑关联，需要的时候加载的方案
    ///   它的加载由需要时刻加载，认为一旦加载上就会有需求，释放则跟着武器或动作持有者进行释放
    /// 1.飞行特效
    /// 2.被击特效，一定时间后销毁，lua端完全不关心
    /// </summary>
    public class Effect : ObjBase
    {
        private enum EType
        {
            NONE = 0,//手动管理删除
            BALLISTIC = 1,//手动管理删除
            TIMED_POS = 2,//限时位置特效
            TIMED_ATT = 3,//挂载限时特效，手动管理删除
        }
        private string res;
        private EType etype;
        public Action<Effect> onOver;

        public override ObjType objType
        {
            get { return ObjType.O_EFFECT; }
        }

        private ParticleCtrl ctrl;
        private Ballistic ballistic;
        private OnTime onTime;

        #region StaticMethod

        public static Effect PlayTimedAtPos(string res, float x,float y,float z)
        {
            Effect effect = new Effect(res,(int)EType.TIMED_POS);
            effect.Load(() =>
            {
                effect.SetPos(x,y,z);
                effect.Play();
            });
            return effect;
        }

        public static Effect PlayTimedAttach(string res, Transform trans,Action<Effect> onLoaded,Action<Effect> onOver)
        {
            Effect effect = new Effect(res,(int)EType.TIMED_ATT);
            effect.SetOnOver(onOver);
            effect.Load(() =>
            {
                effect.SetParent(trans);
                if (null != onLoaded)
                    onLoaded(effect);
                effect.Play();
            });
            return effect;
        }

        #endregion
        #region Public Method
        public Effect(string res,int etype)
        {
            this.res = res;
            this.etype = (EType) etype;
        }
        public override void Load(Action done = null)
        {
            if (this.IsLoaded())
                return;
            rootModel = AssetModelFactory.CreateModel(AssetType.MODEL_EFFECT,res);
            rootModel.Load(OnLoaded + done);
        }
        public override void Release()
        {
            if(null!=ctrl)
                ctrl.Stop();
            if (null != ballistic)
                ballistic.enabled = false;
            if (null != onTime)
                onTime.enabled = false;
            base.Release();
        }
        
        public void Play()
        {
            if(null!=ctrl)
                ctrl.Play();
            if(null!=onTime)
                onTime.Play();
        }

        public void SetOnOver(Action<Effect> onOver)
        {
            this.onOver = onOver;
        }

        public void PlayBallistic(float tox,float toy,float toz, float speed)
        {
            if(null!=ballistic)
                ballistic.Play(new Vector3(tox,toy,toz),speed);
        }
        #endregion
        #region Private Method

        private void OnLoaded()
        {
            ctrl = rootModel.GetComponent<ParticleCtrl>();
            switch (etype)
            {
                case EType.BALLISTIC:
                    ballistic = rootModel.GetComponent<Ballistic>();
                    if (null != ballistic)
                    {
                        ballistic.onMoveToEnd = this.OnBallisticToEnd;
                    }
                    SetParent(ObjLocator.Instance.effectRoot,true);
                    break;
                case EType.TIMED_POS:
                    {
                        onTime = rootModel.GetComponent<OnTime>();
                        if(null==onTime)
                            Debug.Log(string.Format("effect:{0} is not timed",res));
                        onTime.onTime = this.OnTimeOver;
                        ObjLocator.Instance.AddManagedEffect(this);
                    }
                    break;
                case EType.TIMED_ATT:
                    {
                        onTime = rootModel.GetComponent<OnTime>();
                        if(null==onTime)
                            Debug.Log(string.Format("effect:{0} is not timed",res));
                        onTime.onTime = this.OnTimeOver;
                    }
                    break;
            }
        }

        private void OnBallisticToEnd()
        {
            if (null != onOver)
                onOver(this);
        }

        private void OnTimeOver()
        {
            if (null != onOver)
                onOver(this);
            if(etype == EType.TIMED_POS)
                ObjLocator.Instance.RemoveManagedEffect(this);
        }
        #endregion
    }
}