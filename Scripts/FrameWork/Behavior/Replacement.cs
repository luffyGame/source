using System.Collections.Generic;
using Game;
using UnityEngine;

namespace FrameWork
{
    public enum EReplacePart : int
    {
        HEAD = 0,//头
        CLOTH = 1,//上衣
        PANTS = 2,//裤子
        FOOT = 3,//脚
        HAIR = 4,//头发
        BAG = 5,//背包
        WEAPON = 6,//武器
    }
    //武器为非身体的一部分，采用挂点的方案去实现
    public class Replacement : MonoBehaviour
    {
        
        private class ReplacePart
        {
            public int idx;
            public GameObject origin;
            
            public GameObject replace;
            private SkinnedMeshRenderer rep_render;
            private Transform rep_root;
            private Transform[] rep_bones;
            public void Do(Dictionary<string,Transform> bones)
            {
                rep_render = replace.GetComponentInChildren<SkinnedMeshRenderer>();
                rep_root = rep_render.rootBone;
                rep_bones = rep_render.bones;
                Transform newRoot = null;
                int boneCount = rep_render.bones != null ? rep_render.bones.Length : 0;
                if (boneCount > 0)
                {
                    Transform[] partBones = new Transform[boneCount];
                    for (int i = 0; i < boneCount; ++i)
                    {
                        Transform bone = rep_render.bones[i];
                        if (bones.ContainsKey(bone.name))
                        {
                            partBones[i] = bones[bone.name];
                            if (null == newRoot && partBones[i].name == rep_root.name)
                                newRoot = partBones[i];
                        }
                    }

                    rep_render.bones = partBones;
                }
                else
                {
                    rep_render.bones = null;
                }
                rep_render.rootBone = newRoot;
                if(null!=origin)
                    origin.SetActive(false);
                replace.transform.SetParentIndentical(newRoot);
                //replace.SetActive(true);
            }
            public void Undo()
            {
                rep_render.bones = rep_bones;
                rep_render.rootBone = rep_root;
                if(null!=origin)
                    origin.SetActive(true);
                //replace.SetActive(false);
            }
        }
        public Transform boneRoot;
        public GameObject[] originals;
        private Dictionary<string,Transform> bones;
        //不要用枚举做key，除非实现key的hash
        private Dictionary<int, ReplacePart> parts = new Dictionary<int, ReplacePart>();
        private Transform trans;
        public Transform Trans { get { if (null == trans) trans = transform; return trans; } }
        private void CheckGetBones()
        {
            if (null == bones)
            {
                bones = new Dictionary<string, Transform>();
                Transform[] alls = boneRoot.GetComponentsInChildren<Transform>();
                foreach (var bone in alls)
                {
                    bones.Add(bone.name,bone);
                }
            }
        }

        public void ClearBones()
        {
            bones = null;
        }
        public GameObject ReplaceToDefault(int partId)
        {
            if (parts.ContainsKey(partId))
            {
                ReplacePart rp = parts[partId];
                rp.Undo();
                parts.Remove(partId);
                return rp.replace;
            }

            return null;
        }
        public void Replace(int partId,GameObject partGo)
        {
            if (null == partGo)
            {
                Debug.LogError("use null gameobj to replace");
                return;
            }
            CheckGetBones();
            GameObject origin = null;
            if(partId<originals.Length)
                origin = originals[partId];
            ReplacePart rp = new ReplacePart();
            rp.idx = partId;
            rp.origin = origin;
            rp.replace = partGo;
            //partGo.transform.SetParentIndentical(Trans);
            rp.Do(bones);
            parts[partId] = rp;
        }

        public void ReplaceBones(SkinnedMeshRenderer ski)
        {
            if(null == ski)
                return;
            CheckGetBones();
            Transform rep_root = ski.rootBone;
            Transform[] rep_bones = ski.bones;
            Transform newRoot = null;
            int boneCount = rep_bones != null ? rep_bones.Length : 0;
            if (boneCount > 0)
            {
                Transform[] partBones = new Transform[boneCount];
                for (int i = 0; i < boneCount; ++i)
                {
                    Transform bone = rep_bones[i];
                    if (bones.ContainsKey(bone.name))
                    {
                        partBones[i] = bones[bone.name];
                        if (null == newRoot && partBones[i].name == rep_root.name)
                            newRoot = partBones[i];
                    }
                }

                ski.bones = partBones;
            }
            else
            {
                ski.bones = null;
            }
            ski.rootBone = newRoot;
        }
    }
}
