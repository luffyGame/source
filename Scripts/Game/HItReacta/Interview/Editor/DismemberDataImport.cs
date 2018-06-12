using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using LitJson;

public class DismemberDataImport : MonoBehaviour
{

    [Serializable]
    public class dismemberItemInfo
    {
        public string prefab;
        public int bodyPartHint;
        public int dismemberType;
        public int powerType;
        public double mass;
        public double anglemass;
        public double[] speed;
        public double[] anglespeed;
    }

    [Serializable]
    public class DismemberItemsInfos
    {
        public List<dismemberItemInfo> dismemberItems;
        public DismemberItemsInfos()
        {
            dismemberItems = new List<dismemberItemInfo>();
        }

        public void AddItem(dismemberItemInfo testInfo)
        {
            dismemberItems.Add(testInfo);
        }
    }


    //肢解参数导入
    [MenuItem("游戏拓展/肢解数据导入")]
    static void ImportDismemberData()
    {
        string pathJson = Application.dataPath + "/LevelEditor/Input/cfg.json";
        var dataString = string.Empty;
        using (var sr = File.OpenText(pathJson))
        {
            dataString = sr.ReadToEnd();
        }
        if (string.IsNullOrEmpty(dataString))
        {
            Debug.LogError("json is null");
            return;
        }

        var jsonMap = JsonMapper.ToObject(dataString);
        var dismemberItems = jsonMap["dismemberItems"] as JsonData;
        var items = JsonMapper.ToObject<Dictionary<string, List<dismemberItemInfo>>>(dismemberItems.ToJson());

        foreach (var tempItem in items)
        {
            var name = tempItem.Value[0].prefab;
            var obj = GameObject.Find(name);
            if (obj != null)
            {
                var temp = obj.GetComponent<Dismemberment>();
                if (temp != null)
                {
                    var boomParam = temp.boomParam;
                    for (int i = 0; i < tempItem.Value.Count; i++)
                    {
                        var dismemberItem = tempItem.Value[i];
                        //JsonMapper.ToObject<dismemberItemInfo>(dismemberItems[i].ToJson());
                        boomParam[i / 4].bodyPart = (Dismemberment.BodyPartHint)dismemberItem.bodyPartHint;
                        boomParam[i / 4].forceParams[i % 4].drag = (float)dismemberItem.mass;
                        boomParam[i / 4].forceParams[i % 4].angleDrag = (float)dismemberItem.anglemass;
                        boomParam[i / 4].forceParams[i % 4].force = new Vector3((float)dismemberItem.speed[0], (float)dismemberItem.speed[1], (float)dismemberItem.speed[2]);
                        boomParam[i / 4].forceParams[i % 4].torque = new Vector3((float)dismemberItem.anglespeed[0], (float)dismemberItem.anglespeed[1], (float)dismemberItem.anglespeed[2]);
                    }

                    if (PrefabUtility.GetPrefabType(temp.gameObject) == PrefabType.PrefabInstance)
                    {
                        UnityEngine.Object parentObject = PrefabUtility.GetPrefabParent(temp.gameObject);
                        PrefabUtility.ReplacePrefab(temp.gameObject, parentObject, ReplacePrefabOptions.ConnectToPrefab);
                    }
                }
            }
        }
    }
}
