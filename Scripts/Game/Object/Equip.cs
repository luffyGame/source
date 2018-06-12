using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Game
{
    public class Equip : ObjBase
    {
        public string res { get; private set; }
        public EquipInfo equipInfo { get; private set; }
        public EquipPart epart { get; private set; }
        public int SuitTag { get
        {
            if (null != equipInfo) return equipInfo.suitTag;
            return 0;
        } }
        
        private Dictionary<int,Effect> mountEffects;
        private Action<Effect> onEffectOver;//cached delegate
        
        public override ObjType objType
        {
            get { return ObjType.O_EQUIP; }
        }
        
        #region Public Method
        public Equip(string res,EquipPart epart)
        {
            this.res = res;
            this.epart = epart;
        }
        public override void Load(Action done = null)
        {
            if (this.IsLoaded())
                return;
            rootModel = AssetModelFactory.CreateModel(AssetType.MODEL_EQUIP,res);
            rootModel.Load(OnLoaded + done);
        }

        public override void Release()
        {
            ReleaseEffects();
            base.Release();
        }

        public void PutOn(Dictionary<string, Transform> allBones,Transform root)
        {
            if(null!=equipInfo)
                equipInfo.PutOn(allBones,root);
        }

        public void PutOff()
        {
            if(null!=equipInfo)
                equipInfo.PutOff();
        }

        public bool OnEquipPartsChanged(EquipWear equipWear)
        {
            if (null != equipInfo)
            {
                return equipInfo.OnEquipPartsChanged(equipWear,epart);
            }
            return false;
        }

        public Transform GetFireMount()
        {
            if (null != equipInfo)
                return equipInfo.firePoint;
            return GetRootTrans();
        }

        public bool IsWeapon()
        {
            return epart == EquipPart.WEAPON;
        }
        public void PlayTimedEffectAtMount(string effectRes,bool mounted)
        {
            Transform mountTrans = GetFireMount();
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
        #endregion
        #region Private Method

        private void OnLoaded()
        {
            equipInfo = rootModel.GetComponent<EquipInfo>();
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
    }
}