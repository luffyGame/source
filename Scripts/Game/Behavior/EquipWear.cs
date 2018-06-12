using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace Game
{
    public class EquipWear : MonoBehaviour
    {
        public GameObject[] origins;
        public Transform boneRoot;
        private Dictionary<string,Transform> allBones;
        private Dictionary<int,Equip> putOns = new Dictionary<int, Equip>();//部位->装备
        private List<EquipPart> dels = new List<EquipPart>();
        public int equipMask { get; private set; }
        private Transform trans;
        private Transform Trans{get
        {
            if (null == trans) trans = transform;
            return trans;
        }}
        public Equip GetEquip(EquipPart epart)
        {
            int epartId = (int) epart;
            if (putOns.ContainsKey(epartId))
                return putOns[epartId];
            return null;
        }
        public void PutOn(EquipPart epart, string equipRes,System.Action<ObjBase> cb)
        {
            Equip equip = GetEquip(epart);
            if (null != equip)
            {
                if(equip.res == equipRes)
                    return;
                PutOff(epart);
            }
            equip = new Equip(equipRes,epart);
            int epartInt = (int) epart;
            putOns.Add(epartInt,equip);
            equip.Load(() =>
            {
                Equip loadedeEquip = GetEquip(epart);
                if(loadedeEquip == equip)
                    PutOn(equip,epartInt);
                if(cb != null)
                {
                    cb(loadedeEquip);
                }
            });
        }

        public void PutOff(EquipPart epart)
        {
            if(null == allBones)
                return;
            int epartInt = (int) epart;
            if (putOns.ContainsKey(epartInt))
            {
                Equip equip = putOns[epartInt];
                equip.PutOff();
                equip.Release();
                EnableOriginPart(epartInt,true);
                putOns.Remove(epartInt);
                OnEquipPartsChanged(epartInt,false);
            }
        }

        public void ReleaseAllEquips()
        {
            foreach (var kvp in putOns)
            {
                Equip equip = kvp.Value;
                equip.PutOff();
                equip.Release();
                EnableOriginPart(kvp.Key,true);
            }
            putOns.Clear();
            equipMask = 0;
        }

        public bool HasConflictEquip(int conflictMask,int epartInt, int suitTag)
        {
            int conflicts = conflictMask & equipMask;
            if (conflicts == 0)
                return false;
            foreach (var kvp in putOns)
            {
                if(kvp.Key == epartInt)
                    continue;
                if ((conflictMask & (1 << kvp.Key)) > 0)
                {
                    if (suitTag > 0 && kvp.Value.SuitTag != suitTag)
                        return true;
                }
            }

            return false;
        }

        private void OnEquipPartsChanged(int epartInt,bool onOff)
        {
            if(onOff)
                equipMask = equipMask | (1 << epartInt);
            else
                equipMask = equipMask & ~(1 << epartInt);
            dels.Clear();
            foreach (var kvp in putOns)
            {
                Equip equip = kvp.Value;
                bool needDel = equip.OnEquipPartsChanged(this);
                if(needDel)
                    dels.Add(equip.epart);
            }

            if (dels.Count > 0)
            {
                foreach (var del in dels)
                {
                    PutOff(del);
                }
            }
        }

        private void Awake()
        {
            if (null == allBones)
            {
                allBones = new Dictionary<string,Transform>();
                Transform[] alls = boneRoot.GetComponentsInChildren<Transform>();
                foreach (var bone in alls)
                {
                    allBones.Add(bone.name,bone);
                }
            }
        }
        
        private void PutOn(Equip equip,int epartInt)
        {
            if(null == allBones)
                return;
            equip.PutOn(allBones,Trans);
            EnableOriginPart(epartInt, false);
            OnEquipPartsChanged(epartInt,true);
        }

        private void EnableOriginPart(int epartId,bool bEnable)
        {
            GameObject origin = null;
            if (epartId < origins.Length)
                 origin = origins[epartId];
            if(null!=origin)
                origin.SetActive(bEnable);
        }
    }
}