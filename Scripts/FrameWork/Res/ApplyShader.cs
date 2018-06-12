using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

namespace FrameWork
{
	public class ApplyShader : MonoBehaviour
	{
		// Use this for initialization
		public void Start ()
		{
			ResetShader(gameObject);
		}

		public static ApplyShader CheckShader(Object o)
		{
			GameObject go = o as GameObject;
			if(null!=go)
			{
                ApplyShader comp = go.GetComponent<ApplyShader>();
                if (comp == null)
                    comp = go.AddComponent<ApplyShader>();
                return comp;
			}
            return null;
		}

		private void ResetShader(GameObject go)
		{
			List<Renderer> renders = null;
            Utils.GetComponent(go, ref renders);
            Dictionary<Material, string> materialShaders = new Dictionary<Material, string>();
            for (int i = 0; i < renders.Count; ++i)
            {
                Material[] materials = renders[i].sharedMaterials;
                if (materials == null)
                    continue;
                for (int j = 0; j < materials.Length; ++j)
                {
                    Material m = materials[j];
                    if (null != m && !materialShaders.ContainsKey(m))
                        materialShaders.Add(m, m.shader.name);
                }
            }
            List<Image> images = null;
            Utils.GetComponent(go, ref images);
            for (int i = 0; i < images.Count; ++i)
            {
                Material m = images[i].material;
                if (null != m && !materialShaders.ContainsKey(m))
                    materialShaders.Add(m, m.shader.name);
            }
			
			foreach(KeyValuePair<Material,string> element in materialShaders)
			{
				element.Key.shader = Shader.Find(element.Value);
			}

		}
	}
}

