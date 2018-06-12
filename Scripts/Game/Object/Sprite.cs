using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Game
{
    public class Sprite : ObjBase
    {
        #region Var
        private string bodyRes;
        //动作
        private AniClipLen clipLen;
        private Animation animation
        {
            get { return theInfo.animation; }
        }
        //
        private CharacterController cct
        {
            get { return theInfo.cct; }
        }
        //寻路
        private NavFollower follower
        {
            get { return theInfo.follower; }
        }
        //换装
        private EquipWear equipWear
        {
            get { return theInfo.equipWear; }
        }
        //肢解
        private Dismemberment dismemberment
        {
            get { return theInfo.dismemberment; }
        }
        //布娃娃
        private RagdollManagerHum ragdollManager
        {
            get { return theInfo.ragdollManager; }
        }

        private UseChecker useChecker
        {
            get { return theInfo.useChecker; }
        }
        private bool isDeathShow;
        
        private Action<float, float, float> notifyPosSet;
        private Action<float, float, float> notifyDirSet;
        private Action notifyFollowCompleted;

        private Dictionary<int,Effect> mountEffects;
        private Action<Effect> onEffectOver;//cached delegate

        private Action<int, int> onUseChecker;
        
        public override ObjType objType
        {
            get { return ObjType.O_SPRITE; }
        }
        
        private SpriteInfo theInfo;
        public override ObjInfo info
        {
            get { return theInfo; }
        }

        #endregion

        public Sprite(string bodyRes)
        {
            this.bodyRes = bodyRes;
        }
        #region Resource
        public override void Load(Action done = null)
        {
            if (this.IsLoaded())
                return;
            rootModel = AssetModelFactory.CreateModel(AssetType.MODEL_AVATAR,bodyRes);
            rootModel.Load(OnLoaded + done);
        }

        public override void Release()
        {
            this.ReleaseEffects();
            this.RecoverFromDeathShow();
            base.Release();
        }

        private void OnLoaded()
        {
            theInfo = rootModel.GetComponent<SpriteInfo>();
            if(null == theInfo)
                Debug.Log(string.Format("{0} has not set info",bodyRes));
            this.InfoRef();
            CheckGetActionSet();
            if (null != follower)
            {
                follower.mov = SimpleMove;
                follower.onDirSet = OnDirSet;
                follower.onFollowCompleted = OnFollowCompleted;
            }
            if(null!=ragdollManager)
                ragdollManager.onCompelted = this.OnRagdollComplete;
            if (null != useChecker)
            {
                useChecker.onTargetSet = this.OnUseCheck;
            }
        }

        #endregion
        #region Action
        public bool HasAni(string act)
        {
            if (null != clipLen && clipLen.ContainsKey(act))
                return true;
            return false;
        }

        public float PlayAni(string act)
        {
            if (null == act)
                return 0f;
            float actTime = 0f;
            if (null != clipLen && clipLen.ContainsKey(act))
                actTime = clipLen[act];
            else
            {
                Debug.Log(String.Format("PlayAni : {0} has no {1}",bodyRes,act));
                return actTime;
            }

            if (null != animation)
            {
                animation.Stop();
                animation.Play(act);
            }

            return actTime;
        }

        public float PlayAni(string act, float fade)
        {
            if (null == act)
                return 0f;
            float actTime = 0f;
            if (null != clipLen && clipLen.ContainsKey(act))
                actTime = clipLen[act];
            else
            {
                Debug.Log(String.Format("PlayAni : {0} has no {1}",bodyRes,act));
                return actTime;
            }

            if (null != animation)
            {
                if (animation.IsPlaying(act))
                {
                    float paseTime = animation[act].normalizedTime;
                    animation.Stop();
                    if(animation[act].clip.isLooping)
                        animation[act].normalizedTime = paseTime;
                }
                //Debug.Log("cross: " + act + " : " + Time.realtimeSinceStartup);
                animation.CrossFade(act,fade);
            }
            return actTime;
        }
        private void CheckGetActionSet()
        {
            if(null == animation)
                return;
            clipLen = AniClipLenCfg.Instance.GetClipLen(bodyRes);
            if (null == clipLen)
            {
                clipLen = new AniClipLen(animation);
                AniClipLenCfg.Instance.SetClipLen(bodyRes,clipLen);
            }
        }
        #endregion
        #region Move
        public void SimpleMove(float x,float z)
        {
            Vector3 mov = new Vector3(x,0f,z);
            if (null != cct)
            {
                if (cct.SimpleMove(mov))
                {
                    OnPosSet();
                }
            }
            else
            {
                Translate(mov*Time.deltaTime);
                OnPosSet();
            }
        }
        public void BindFollowCallback(Action<float, float, float> notifyPosSet,
            Action<float, float, float> notifyDirSet, Action notifyMoverComp)
        {
            this.notifyPosSet = notifyPosSet;
            this.notifyDirSet = notifyDirSet;
            this.notifyFollowCompleted = notifyMoverComp;
        }
        public void FollowInRandomDirection(bool use)
        {
            if (null != follower)
            {
                follower.useRandomDirection = use;
            }
        }

        public void SetSpeed(float speed)
        {
            if(null!=follower)
                follower.speed = speed;
        }

        public void StartFollow(ObjBase obj, float arriveRange)
        {
            if (null != follower)
            {
                follower.arriveRange = arriveRange;
                follower.StartFollow(obj.GetRootTrans());
            }
        }

        public void StartFollowOffset(ObjBase obj, float x, float y, float z)
        {
            if (null != follower)
            {
                follower.arriveRange = 0;
                follower.StartFollow(obj.GetRootTrans(),new Vector3(x,y,z));
            }
        }

        public void StartFollow(float x,float y,float z,float arriveRange)
        {
            if (null != follower)
            {
                follower.arriveRange = arriveRange;
                follower.StartFollow(new Vector3(x,y,z));
            }
        }

        public void CancelFollow()
        {
            if(null!=follower)
                follower.CancelFollow();
        }
        
        private void OnPosSet()
        {
            if (null != notifyPosSet)
            {
                Vector3 pos = rootModel.GetPos();
                notifyPosSet(pos.x, pos.y, pos.z);
            }
        }

        private void OnDirSet()
        {
            if (null != notifyDirSet)
            {
                Vector3 dir = rootModel.GetForward();
                notifyDirSet(dir.x, dir.y, dir.z);
            }
        }

        private void OnFollowCompleted()
        {
            if (null != notifyFollowCompleted)
                notifyFollowCompleted();
        }
        #endregion
        #region Wear//换装
        public void WearOn(int partId, string equipRes,Action<ObjBase> cb)
        {
            if (null != equipWear)
            {
                equipWear.PutOn((EquipPart)partId,equipRes,cb);
            }
        }

        public void WearOff(int partId)
        {
            if (null != equipWear)
                equipWear.PutOff((EquipPart)partId);
        }

        public void WearOff(int partId1, int partId2)
        {
            WearOff(partId1);
            WearOff(partId2);
        }

        public void WearAllRemove()
        {
            if (null != equipWear)
                equipWear.ReleaseAllEquips();
        }
        #endregion
        #region 肢解布娃娃
        /// <summary>
        /// 
        /// </summary>
        /// <param name="boomPart">肢解部位</param>
        /// <param name="power">Power : 力参数</param>
        /// <param name="otherboomNum">其余肢解数量</param>
        /// <param name="power">布娃娃肢解大小</param>
        /// <param name="x">布娃娃方向X</param>
        /// <param name="y">布娃娃方向Y</param>
        /// <param name="z">布娃娃方向Z</param>
        public void Dismember(int boomPart, int power, int otherboomNum, float x, float y, float z)
        {
            if(isDeathShow)
                return;
            ReadyForDeathShow();
            if(dismemberment == null)
            {
                return;
            }

            List<int> otherBoomId;
            int boomId = dismemberment.FindBoomPart(boomPart);
            dismemberment.FindOtherDismemberId(boomId, otherboomNum, out otherBoomId);
            otherBoomId.Add(boomId);
            if(null!=dismemberment)
                dismemberment.Dismember(otherBoomId.ToArray(), power);

            if (null!=ragdollManager)
            {
                ragdollManager.OrignDir = new Vector3(x, 0, z);
                //Debug.LogError("原始方向:" + ragdollManager.OrignDir);
                int ragdollId = Config.Instance.GetBodyPartId(boomId);
                if (x == 0 && z == 0)
                {
                    ragdollManager.OrignDir = ragdollManager.transform.forward;
                    ragdollManager.CustomRagdoll(ragdollId, ragdollManager.transform.forward);
                    return;
                }
                var dir = Config.Instance.GetRagdollDir(ragdollId, power);
               // Debug.LogError("力方向参数:" + dir);
                //var dirLength = dir.x * dir.x + dir.z * dir.z;
                //var dirSin = z / Mathf.Sqrt(x * x + z * z);
                float dirDegree = Mathf.Acos(Mathf.Clamp(Vector2.Dot(new Vector2(0, 1), new Vector2(x, z).normalized), -1, 1)) * Mathf.Rad2Deg;
                var dirLength = Mathf.Sqrt(dir.x * dir.x + dir.z * dir.z);
                var dirSin = z / Mathf.Sqrt(x * x + z * z);
                var dirCos = x / Mathf.Sqrt(x * x + z * z);
                ragdollManager.CustomRagdoll(ragdollId, new Vector3(dirLength * dirCos, dir.y, dirLength * dirSin));

               // Debug.LogError("肢解方向:" + new Vector3(Mathf.Sqrt(dirLength * (1 - dirSin * dirSin)), dir.y, dirSin * Mathf.Sqrt(dirLength)));
               // ragdollManager.CustomRagdoll(ragdollId, new Vector3(Mathf.Sqrt(dirLength * (1 - dirSin * dirSin)), dir.y, dirSin * Mathf.Sqrt(dirLength)));
            }
        }
        //布娃娃
        public void Ragdoll(int boomPart,int power, float x, float y, float z)
        {
            if (isDeathShow)
                return;
            ReadyForDeathShow();
            if(null!=ragdollManager)
            {
                ragdollManager.OrignDir = new Vector3(x, 0, z);
                //Debug.LogError("原始方向:" + ragdollManager.OrignDir);
                int ragdollId = Config.Instance.GetRagdollPartId(boomPart);
                if (x == 0 && z == 0)
                {
                    ragdollManager.OrignDir = ragdollManager.transform.forward;
                    ragdollManager.CustomRagdoll(ragdollId, ragdollManager.transform.forward);
                    return;
                }
                var dir = Config.Instance.GetRagdollDir(ragdollId, power);
                //Debug.LogError("力方向参数:" + dir);
                float dirDegree = Mathf.Acos(Mathf.Clamp(Vector2.Dot(new Vector2(0, 1), new Vector2(x, z).normalized), -1, 1)) * Mathf.Rad2Deg;
                var dirLength = Mathf.Sqrt(dir.x * dir.x + dir.z * dir.z);
                var dirSin = z / Mathf.Sqrt(x * x + z * z);
                var dirCos = x / Mathf.Sqrt(x * x + z * z);
                ragdollManager.CustomRagdoll(ragdollId, new Vector3(dirLength * dirCos, dir.y, dirLength * dirSin));
            }
        }

        public void RecoverFromDeathShow()
        {
            if(!isDeathShow)
                return;
            isDeathShow = false;
            if (dismemberment != null)
            {
                dismemberment.Recover();
            }

            if (ragdollManager != null)
            {
                ragdollManager.DisableCustomRagdoll();
            }
            if (null != animation)
                animation.enabled = true;
            if (null != cct)
                cct.enabled = true;
        }
        
        private void OnRagdollComplete(Vector3 rootPos)
        {
            if (null != notifyPosSet)
            {
                notifyPosSet(rootPos.x, rootPos.y, rootPos.z);
            }
        }
        private void ReadyForDeathShow()
        {
            isDeathShow = true;
            if(animation != null)
            {
                animation.Stop();
                animation.enabled = false;
            }
            if(cct != null)
            {
                cct.enabled = false;
            }
        }
        #endregion
        #region UseChecker

        public void BindUseChecker(Action<int, int> onCheck)
        {
            this.onUseChecker = onCheck;
            this.EnableUseChecker(onCheck!=null);
        }

        private void EnableUseChecker(bool benable)
        {
            if(null!=useChecker)
                useChecker.EnableCheck(benable);
        }
        private void OnUseCheck(int id, int useTag)
        {
            if (null != onUseChecker)
                onUseChecker(id, useTag);
        }
        public void SetUsable(bool bUsable)
        {
            if (null != theInfo)
            {
                theInfo.SetUseEnable(bUsable);
            }
        }
        #endregion
        #region Skill
        public Transform GetMount(int mountTag)
        {
            if (null != info)
                return theInfo.mounts[mountTag];
            return null;
        }
        public bool CanSkillReach(ObjBase target, float skillRange, out float x,out float y,out float z)
        {
            Vector3 toPos = target.GetBodyCenterPos();
            bool ret = CanSkillReach(ref toPos, skillRange, target);
            x = toPos.x;
            y = toPos.y;
            z = toPos.z;
            return ret;
        }
        public bool CanSkillReach(float tox,float toy,float toz, float skillRange, out float x,out float y,out float z)
        {
            Vector3 toPos = new Vector3(tox,toy,toz);
            bool ret = CanSkillReach(ref toPos, skillRange);
            x = toPos.x;
            y = toPos.y;
            z = toPos.z;
            return ret;
        }
        public void GetSkillReach(float dirx,float diry,float dirz, float skillRange, out float x,out float y,out float z)
        {
            Vector3 origin = GetBodyCenterPos();
            Vector3 toPos = origin + new Vector3(dirx,diry,dirz)*skillRange;
            bool ret = CanSkillReach(ref toPos, skillRange);
            x = toPos.x;
            y = toPos.y;
            z = toPos.z;
        }
        private bool CanSkillReach(ref Vector3 toPos,float skillRange,ObjBase target=null)
        {
            Vector3 origin = GetBodyCenterPos();
            Collider targetCollider = target == null ? null : target.GetCollider();
            return Utility.CheckSkillReach(origin, ref toPos, skillRange,targetCollider);
        }
        
        #endregion
        #region Effect
        public void PlayTimedEffectAtPoint(string effectRes)
        {
            Vector3 pos = GetPos();
            Effect.PlayTimedAtPos(effectRes,pos.x,pos.y,pos.z);
        }
        public void PlayTimedEffectAtMount(string effectRes, int mount,bool mounted)
        {
            Transform mountTrans = GetMount(mount);
            if (null == mountTrans)
                mountTrans = GetRootTrans();
            if (!mounted)
            {
                Vector3 pos = mountTrans.position;
                Effect.PlayTimedAtPos(effectRes, pos.x, pos.y, pos.z);
                return;
            }
            if(null == onEffectOver)
                onEffectOver = OnEffectOver;
            Effect effect = Effect.PlayTimedAttach(effectRes,mountTrans,null,onEffectOver);
            if (null == mountEffects)
            {
                mountEffects = new Dictionary<int, Effect>();
            }
            mountEffects.Add(effect.oid,effect);
        }

        public void PlayWeaponEffect(string effectRes, int mount, bool mounted)
        {
            if(null == equipWear)
                return;
            Equip weapon = equipWear.GetEquip(EquipPart.WEAPON);
            if(null!=weapon)
                weapon.PlayTimedEffectAtMount(effectRes,mounted);
        }

        public Transform GetWeaponFireMount()
        {
            if (null != equipWear)
            {
                Equip weapon = equipWear.GetEquip(EquipPart.WEAPON);
                if (null != weapon)
                    return weapon.GetFireMount();
            }

            return GetRootTrans();
        }
        private void OnEffectOver(Effect effect)
        {
            if (mountEffects.ContainsKey(effect.oid))
            {
                mountEffects.Remove(effect.oid);
            }
            effect.Release();
        }
        private void ReleaseEffects()
        {
            if (null != mountEffects)
            {
                foreach (var kvp in mountEffects)
                {
                    kvp.Value.Release();
                }
                mountEffects.Clear();
            }
        }
        #endregion
        #region Other

        public void Alive(bool alivable)
        {
            if(theInfo!=null)
                theInfo.Alive(alivable);
        }
        #endregion
    }
}