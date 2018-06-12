using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Game
{
    public class EquipInfo : MonoBehaviour
    {
        public Transform firePoint;
        public bool IsUseBone
        {
            get { return skis != null && skis.Length>0; }
        }

        private Transform trans;
        public Transform Trans {get
        {
            if (null == trans) trans = transform;
            return trans;
        }}
        #region 装备骨骼方案数据
        //冲突方案
        public enum ConflitOp:int
        {
            NONE = 0,
            REMOVE = 1,//删除
            HIDE = 2,//隐藏
            SHOW = 3,//显示
        }
        [Serializable]
        public class EquipSki
        {
            public GameObject skiGo;
            public SkinnedMeshRenderer render;
            public ConflitOp op = ConflitOp.NONE;
            public EquipMask conflit = EquipMask.NONE;
            private Transform[] bones;
            private Transform boneRoot;
            private Transform[] onBones;
            private Transform onBoneRoot;

            public void PutOn(Dictionary<string, Transform> allBones)
            {
                if(null == bones)
                    bones = render.bones;
                if(null == boneRoot)
                    boneRoot = render.rootBone;
                int count = bones != null ? bones.Length : 0;
                if (count > 0)
                {
                    onBones = new Transform[count];
                    for (int i = 0; i < count; ++i)
                    {
                        Transform bone = bones[i];
                        if (allBones.ContainsKey(bone.name))
                        {
                            onBones[i] = allBones[bone.name];
                            if (null == onBoneRoot && onBones[i].name == boneRoot.name)
                                onBoneRoot = onBones[i];
                        }
                    }
                }

                render.bones = onBones;
                render.rootBone = onBoneRoot;
            }

            public void PutOff()
            {
                skiGo.SetActive(true);
                render.bones = bones;
                render.rootBone = boneRoot;
                onBoneRoot = null;
                onBones = null;
            }

            public bool OnEquipPartsChanged(EquipWear equipWear,EquipPart equipPart,int suitTag)
            {
                bool hasConflt = equipWear.HasConflictEquip((int)conflit, (int) equipPart, suitTag);
                switch (op)
                {
                    case ConflitOp.REMOVE:
                        return hasConflt;
                    case ConflitOp.HIDE:
                        if(skiGo.activeSelf==hasConflt)
                            skiGo.SetActive(!hasConflt);
                        break;
                    case ConflitOp.SHOW:
                        if(skiGo.activeSelf != hasConflt)
                            skiGo.SetActive(hasConflt);
                        break;
                }
                return false;
            }
        }
        public EquipSki[] skis;
        public int suitTag;//套装标记
        private void BonePutOn(Dictionary<string,Transform> allBones,Transform root)
        {
            if(null == skis)
                return;
            foreach (var ski in skis)
            {
                ski.PutOn(allBones);
            }
            Trans.SetParentIndentical(root);
        }

        private void BonePutOff()
        {
            if(null == skis)
                return;
            foreach (var ski in skis)
            {
                ski.PutOff();
            }
        }
        #endregion

        #region 装备锚点方案数据
        [Serializable]
        public class MountPt
        {
            public string mountBone;
            public Vector3 mountPos, mountScale;
            public Quaternion mountRotate;
        }
        public MountPt mountPt;
        public void Mount(Dictionary<string,Transform> allBones)
        {
            if (allBones.ContainsKey(mountPt.mountBone))
            {
                Transform mountTo = allBones[mountPt.mountBone];
                Trans.parent = mountTo;
                Trans.localPosition = mountPt.mountPos;
                Trans.localRotation = mountPt.mountRotate;
                Trans.localScale = mountPt.mountScale;
            }
        }
        [ContextMenu("MountPtSet")]
        private void SetCfg()
        {
            if(null == mountPt)
                mountPt = new MountPt();
            if (null != Trans.parent)
            {
                mountPt.mountBone = Trans.parent.name;
                mountPt.mountPos = Trans.localPosition;
                mountPt.mountScale = Trans.localScale;
                mountPt.mountRotate = Trans.localRotation;
            }
        }
        #endregion
        public void PutOn(Dictionary<string,Transform> allBones,Transform root)
        {
            if(IsUseBone)
                BonePutOn(allBones,root);
            else
                Mount(allBones);
        }

        public void PutOff()
        {
            if(IsUseBone)
                BonePutOff();
        }

        public bool OnEquipPartsChanged(EquipWear equipWear,EquipPart equipPart)
        {
            if(null == skis)
                return false;
            bool ret = false;
            foreach (var ski in skis)
            {
                if (ski.OnEquipPartsChanged(equipWear,equipPart,suitTag))
                    ret = true;
            }
            return ret;
        }
    }
}