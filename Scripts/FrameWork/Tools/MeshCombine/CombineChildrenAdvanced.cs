/// <summary>
/// 在编辑器里整合mesh
/// </summary>
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace MeshCombine
{
	[AddComponentMenu("Mesh/Advanced Combine Children")]
	public class CombineChildrenAdvanced : MonoBehaviour {
		public bool generateLightmappingUVs = true;/*是否生成uv*/
		public bool destroyChilds = true;/*是否删除孩子*/
		public bool exportMesh = true;/*是否导出网格*/
		public string exportRootPath = "MeshExport";/*导出网格路径*/

		private bool isCombined = false; /*是否已合并成功，这个是个辅助量*/

		private IList<MeshFilter> generateMeshes = new List<MeshFilter>();/*生成的网格，这个是辅助记录*/

		public bool IsCombined
		{
			get{return isCombined;}
		}
		public IList<MeshFilter> GenerateMeshes
		{
			get{return generateMeshes;}
		}

		public void Combine() {
			generateMeshes.Clear();
			Transform cachedTransform = transform;
			MeshFilter[] filters  = GetComponentsInChildren<MeshFilter>();
			if (filters.Length <= 1) {
				Debug.LogWarning ("Not enough meshes to combine!");
				return;
				
			}
			
			Matrix4x4 myTransform = cachedTransform.worldToLocalMatrix;
			Hashtable materialToMesh= new Hashtable();
			for (int i=0;i<filters.Length;i++) {
				MeshFilter filter = filters[i];
				Renderer curRenderer  = filters[i].GetComponent<Renderer>();
				MeshCombineUtility.MeshInstance instance = new MeshCombineUtility.MeshInstance ();
				instance.mesh = filter.sharedMesh;
				if (curRenderer != null && curRenderer.enabled && instance.mesh != null) {
					instance.transform = myTransform * filter.transform.localToWorldMatrix;
					Material[] materials = curRenderer.sharedMaterials;
					for (int m=0;m<materials.Length;m++) {
						instance.subMeshIndex = System.Math.Min(m, instance.mesh.subMeshCount - 1);
						ArrayList objects = (ArrayList)materialToMesh[materials[m]];
						if (objects != null) {
							objects.Add(instance);
						}
						else
						{
							objects = new ArrayList ();
							objects.Add(instance);
							materialToMesh.Add(materials[m], objects);
						}
					}
					curRenderer.enabled = false;
				}
			}

			if(destroyChilds)
			{
				for(int i=cachedTransform.childCount-1;i>=0;--i)
					DestroyImmediate(cachedTransform.GetChild(i).gameObject);
			}

			int meshIndex = 0;
			foreach (DictionaryEntry de  in materialToMesh) {
				ArrayList elements = (ArrayList)de.Value;
				MeshCombineUtility.MeshInstance[] instances = (MeshCombineUtility.MeshInstance[])elements.ToArray(typeof(MeshCombineUtility.MeshInstance));
				// We have a maximum of one material, so just attach the mesh to our own game object
				if (materialToMesh.Count == 1)
				{
					// Make sure we have a mesh filter & renderer
					if (GetComponent<MeshFilter>() == null)
						gameObject.AddComponent<MeshFilter>();
					if (!GetComponent<MeshRenderer>())
						gameObject.AddComponent<MeshRenderer>();
					MeshFilter filter = GetComponent<MeshFilter>();
					filter.mesh = MeshCombineUtility.Combine(instances);
					GetComponent<Renderer>().material = (Material)de.Key;
					GetComponent<Renderer>().enabled = true;

					generateMeshes.Add(filter);
				}
				// We have multiple materials to take care of, build one mesh / gameobject for each material
				// and parent it to this object
				else
				{
					string name = string.Format("Combined_mesh_{0}",++meshIndex);
					GameObject go = new GameObject(name);
					go.transform.parent = cachedTransform;
					go.transform.localScale = Vector3.one;
					go.transform.localRotation = Quaternion.identity;
					go.transform.localPosition = Vector3.zero;
					go.AddComponent<MeshFilter>();
					go.AddComponent<MeshRenderer>();
					go.GetComponent<Renderer>().material = (Material)de.Key;
					MeshFilter filter = go.GetComponent<MeshFilter>();
					filter.mesh = MeshCombineUtility.Combine(instances);

					generateMeshes.Add(filter);
				}
			}
			isCombined = true;
		}
	}
}
