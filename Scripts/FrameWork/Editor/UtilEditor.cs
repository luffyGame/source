using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UObj = UnityEngine.Object;
using FrameWork;

namespace FrameWork.Editor
{
    public class UtilEditor
    {
        [MenuItem("Assets/FrameWork/GenBoxClider")]
        static void GenBoxCliderProcess()
        {
            UObj[] os = Selection.GetFiltered(typeof(UObj), SelectionMode.DeepAssets);
            foreach (UObj o in os)
            {
                if (o is GameObject)
                {
                    GameObject go = o as GameObject;
                    GenBoxClider(go);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("GenBoxClider done");
        }
        private static void GenBoxClider(GameObject go)
        {
            int[] x = {0,1,4,5};
            int[] y = {0,1,2,3};
            int[] z = {0,3,4,7};
            GameObject instance = GameObject.Instantiate<GameObject>(go);
            MeshFilter[] filters = instance.GetComponentsInChildren<MeshFilter>();
            if (null != filters && filters.Length > 0)
            {
                bool isFirst = true;
                Bounds bs = new Bounds();
                Vector3 vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                Vector3 vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);
                for (int i = 0; i < filters.Length; ++i)
                {
                    Bounds bounds = filters[i].sharedMesh.bounds;
                    Matrix4x4 matrix = filters[i].transform.localToWorldMatrix;
                    for(int j=0;j<8;++j)
                    {
                        bool xL = Array.IndexOf<int>(x, j) == -1;
                        float xValue = xL ? bounds.max.x : bounds.min.x;
                        bool yL = Array.IndexOf<int>(y, j) == -1;
                        float yValue = yL ? bounds.max.y : bounds.min.y;
                        bool zL = Array.IndexOf<int>(z, j) == -1;
                        float zValue = zL ? bounds.max.z : bounds.min.z;
                        Vector3 v = matrix.MultiplyPoint3x4(new Vector3(xValue,yValue,zValue));
                        vMax = Vector3.Max(v, vMax);
                        vMin = Vector3.Min(v, vMin);
                    }
                    if (isFirst)
                        bs = new Bounds(vMin, Vector3.zero);
                    else
                        bs.Encapsulate(vMin);
                    bs.Encapsulate(vMax);
                }
                BoxCollider bc = go.GetComponent<BoxCollider>();
                if (null == bc)
                    bc = go.AddComponent<BoxCollider>();
                bc.center = bs.center;
                bc.size = bs.size;
                EditorUtility.SetDirty(go);
            }
            GameObject.DestroyImmediate(instance);
        }
        [MenuItem("Assets/FrameWork/SetLayer")]
        static void SetLayer()
        {
            UObj[] os = Selection.GetFiltered(typeof(UObj), SelectionMode.DeepAssets);
            foreach (UObj o in os)
            {
                if (o is GameObject)
                {
                    GameObject go = o as GameObject;
                    SetLayer(go, 22);
                }
            }
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("SetLayer done");
        }

        static public void SetLayer(GameObject go, int layer)
        {
            go.layer = layer;

            Transform t = go.transform;

            for (int i = 0, imax = t.childCount; i < imax; ++i)
            {
                Transform child = t.GetChild(i);
                SetLayer(child.gameObject, layer);
            }
        }
        [MenuItem("GameObject/Animator/Remove",false,0)]
        static void RemoveAnimator()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null) return;
            Animator[] animtors = go.GetComponentsInChildren<Animator>();
            for (int i = animtors.Length - 1; i >= 0; --i)
                UnityEngine.Object.DestroyImmediate(animtors[i]);
        }
        [MenuItem("GameObject/MeshRender/Simple", false, 0)]
        static void MeshRenderSimple()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null) return;
            MeshRenderer[] renders = go.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < renders.Length; ++i)
            {
                MeshRenderer r = renders[i];
                r.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                r.receiveShadows = false;
                r.motionVectors = false;
                r.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
                r.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;
            }
        }
        [MenuItem("GameObject/MeshRender/MeterialMissNoDraw", false, 0)]
        static void MaterialDefault()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null) return;
            MeshRenderer[] renders = go.GetComponentsInChildren<MeshRenderer>();
            for (int i = 0; i < renders.Length; ++i)
            {
                MeshRenderer r = renders[i];
                if (r.sharedMaterials.Length > 0 && r.sharedMaterials[0] == null)
                    r.enabled = false;
            }
        }
        [MenuItem("GameObject/GameObject/ChildNum", false, 0)]
        static void GameObjectChildNum()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null) return;
            Debug.Log(go.transform.childCount);
        }
        [MenuItem("GameObject/GameObject/SameNum", false, 0)]
        static void GameObjectSameNum()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null) return;
            PrintGameObjectSameNum(go, 0);
        }
        [MenuItem("GameObject/GameObject/EnableSlider", false, 0)]
        static void GameObjectEnableSlider()
        {
            GameObject go = Selection.activeGameObject;
            if (go == null) return;
            List<Slider> sliders = new List<Slider>();
            Utils.GetComponent(go,ref sliders);
            foreach (var slider in sliders)
            {
                slider.enabled = true;
            }
        }
        static void PrintGameObjectSameNum(GameObject go, int max)
        {
            Dictionary<string, int> counts = new Dictionary<string, int>();
            CountGameObjectByName(go.transform, counts);
            List<KeyValuePair<string,int>> ret = HelpFunc.SortDicitionary(counts, (a1, a2) =>
            {
                return a2.Value.CompareTo(a1.Value);
            });
            string filePath = Application.dataPath + "/" + go.name + ".txt";
            if (File.Exists(filePath))
                File.Delete(filePath);
            int count = 0;
            foreach (KeyValuePair<string, int> element in ret)
            {
                if (element.Value > max)
                {
                    count += element.Value;
                    Utils.FileWrite(filePath, string.Format("{0}:{1}", element.Key, element.Value));
                }
            }
            Utils.FileWrite(filePath, "total:" + count.ToString());
        }

        static void CountGameObjectByName(Transform t, Dictionary<string, int> counts)
        {
            string name = t.name;
            if (name[name.Length-1] == ')')
            {
                int last = name.LastIndexOf('(');
                if(last>0)
                {
                    string copy = name.Substring(last + 1, name.Length - last - 2);
                    int copyId;
                    if (int.TryParse(copy, out copyId))
                        name = name.Substring(0, last);
                }
            }
            if (counts.ContainsKey(name))
                counts[name] = counts[name] + 1;
            else
                counts.Add(name, 1);
            for (int i = 0; i < t.childCount; ++i)
                CountGameObjectByName(t.GetChild(i), counts);
        }
    }
}
