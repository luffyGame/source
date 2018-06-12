using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace Game
{
    [InitializeOnLoad]
    public class TagLayerInit
    {
        static TagLayerInit()
        {
            InitLayer();
        }
        [MenuItem("FrameWork/其他/层级设置",false,100001)]
        static void InitLayer()
        {
            SerializedObject tagManager = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset")[0]);
            SerializedProperty layers = tagManager.FindProperty("layers");
            CreateLayers(layers);
            IgnoreAllCollision(layers);
            SetCollision(layers);
            tagManager.ApplyModifiedProperties();
        }

        static void CreateLayers(SerializedProperty layers)
        {
            Dictionary<int, string> sets = Const.GetAllLayerSet();
            for (int i = 8; i < layers.arraySize; ++i)
            {
                SerializedProperty dataPoint = layers.GetArrayElementAtIndex(i);
                if (sets.ContainsKey(i))
                    dataPoint.stringValue = sets[i];
                else
                {
                    dataPoint.stringValue = string.Empty;
                }
            }
        }

        static void IgnoreAllCollision(SerializedProperty layers)
        {
            for (int i = 0; i < layers.arraySize; ++i)
            {
                for(int j=i;j<layers.arraySize;++j)
                    Physics.IgnoreLayerCollision(i,j,true);
            }
        }

        static void SetCollision(SerializedProperty layers)
        {
            for (int i = 0; i < layers.arraySize; ++i)
            {
                int[] collisions = Const.GetCollisionLayers(i);
                if (null != collisions)
                {
                    foreach (int layer in collisions)
                    {
                        Physics.IgnoreLayerCollision(i,layer,false);
                    }
                }
            }
        }
    }
}