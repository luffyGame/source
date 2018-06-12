using System;
using System.Collections.Generic;
using FrameWork;
using UnityEngine;
using XLua;
using Object = UnityEngine.Object;

namespace Game
{
    public class LuaInjector : MonoBehaviour
    {
        [Serializable]
        public class Injection
        {
            public string key;
            public Object value;
            public string typeName;
            public bool asComponent;

            public void Inject(LuaTable self)
            {
                if (key.Contains("."))
                {
                    if(asComponent)
                        self.SetInPath(key,(Component)value);
                    else
                        self.SetInPath(key, value);   // SetInPath必须保证Path不为nil
                }
                else if (key.Contains("#"))
                {
                    string[] info = key.Split('#');
                    string tableName = info[0];
                    int index = System.Convert.ToInt32(info[1]);
                    LuaTable table = self.Get<LuaTable>(tableName); // 与SetInPath规则保持一致，此处table不能为nil
                    if(asComponent)
                        table.Set(index,(Component)value);
                    else
                        table.Set(index, value);
                }
                else
                {
                    if(asComponent)
                        self.Set(key,(Component)value);
                    else
                        self.Set(key, value);
                }
            }
        }

        public Injection[] injections;

        public void Inject(LuaTable self)
        {
            for (int i = 0; i < injections.Length; i++)
            {
                Injection pair = injections[i];
                pair.Inject(self);
            }
        }
        [ContextMenu("Inject Scene")]
        public void InjectScene()
        {
            List<Injection> injects = new List<Injection>();
            Camera mainCamera = Camera.main;
            if (null != mainCamera)
            {
                GameObject cameraGo = mainCamera.gameObject;
                Injection cameraGoInj = new Injection()
                {
                    key = "env.mainCamera",
                    typeName = typeof(GameObject).FullName,
                    value = cameraGo,
                    asComponent = false,
                };
                injects.Add(cameraGoInj);
                Injection cameraInj = new Injection()
                {
                    key = "env.cameraFollow",
                    typeName = typeof(CameraFollow).FullName,
                    value = cameraGo.GetComponent<CameraFollow>(),
                    asComponent = true,
                };
                injects.Add(cameraInj);
            }
            injections = injects.ToArray();
        }
        
        [ContextMenu("Inject Map")]
        public void InjectMap()
        {
            List<Injection> injects = new List<Injection>();
            //相机
            GameObject cameraDrag = GameObject.Find("CameraControl");
            Injection inj = null;
            if (null != cameraDrag)
            {
                inj = new Injection()
                {
                    key = "env.cameraControl",
                    typeName = typeof(CameraDrag).FullName,
                    value = cameraDrag.GetComponent<CameraDrag>(),
                    asComponent = true,
                };
                injects.Add(inj);
            }
            Camera mainCamera = Camera.main;
            if (null != mainCamera)
            {
                GameObject cameraGo = mainCamera.gameObject;
                inj = new Injection()
                {
                    key = "env.mainCamera",
                    typeName = typeof(GameObject).FullName,
                    value = cameraGo,
                    asComponent = false,
                };
                injects.Add(inj);
            }
            GameObject dataGo = GameObject.Find("run");
            if (null != dataGo)
            {
                Transform dataTrans = dataGo.transform;
                foreach (Transform child in dataTrans)
                {
                    if (child.name == "moveTrans")
                    {
                        inj = new Injection()
                        {
                            key = "moveTrans",
                            typeName = typeof(Transform).FullName,
                            value = child,
                            asComponent = false,
                        };
                        injects.Add(inj);
                    }
                    else
                    {
                        Transform loc = child.Find("loc");
                        if (null != loc)
                        {
                            inj = new Injection()
                            {
                                key = "loc."+child.name,
                                typeName = typeof(Transform).FullName,
                                value = loc,
                                asComponent = false,
                            };
                            injects.Add(inj);
                        }
                        Transform marker = child.Find("marker");
                        if (null != marker)
                        {
                            inj = new Injection()
                            {
                                key = "marker."+child.name,
                                typeName = typeof(Transform).FullName,
                                value = marker,
                                asComponent = false,
                            };
                            injects.Add(inj);
                        }
                    }
                }
            }
            injections = injects.ToArray();
        }
    }
}