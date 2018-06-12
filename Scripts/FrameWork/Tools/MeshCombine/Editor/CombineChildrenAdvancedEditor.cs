using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Text;
using System.IO;


namespace MeshCombine
{
	[CustomEditor(typeof(CombineChildrenAdvanced))]
	public class CombineChildrenAdvancedEditor : Editor {
		CombineChildrenAdvanced _target;
		void OnEnable() {
			_target = (CombineChildrenAdvanced)target;
		}
		
		
		public override void OnInspectorGUI() 
		{
			//EditorGUIUtility.labelWidth = 0;
			_target.generateLightmappingUVs = EditorGUILayout.Toggle ("Generate Lightmapping UVs", _target.generateLightmappingUVs);
			//_target.destroyChilds = EditorGUILayout.Toggle("Destroy old childs",_target.destroyChilds);暂时不用
			//_target.exportMesh = EditorGUILayout.Toggle("Export mesh",_target.exportMesh);暂时不用
			//_target.exportRootPath = EditorGUILayout.TextField("Export mesh Path", _target.exportRootPath);暂时不用
			if (!_target.IsCombined) 
			{
				if (GUILayout.Button ("Combine now (May take a while!)")) 
				{
					_target.Combine();
					if (_target.generateLightmappingUVs) 
					{
						GenerateLightmappingUVs();
					}
					if (_target.exportMesh)
					{
						ExportMeshes();
					}
					DestroyImmediate(_target.GetComponent<CombineChildrenAdvanced>());
					_target.GenerateMeshes.Clear();
				}
			}
		}
		
		
		private void GenerateLightmappingUVs() 
		{
			foreach(MeshFilter mf in _target.GenerateMeshes)
			{
				Unwrapping.GenerateSecondaryUVSet(mf.sharedMesh);
			}
			/*
			MeshFilter mf = _target.GetComponent<MeshFilter>();
			if (mf != null) 
			{
				Unwrapping.GenerateSecondaryUVSet(mf.sharedMesh);
			}
			*/
		}

		private string GetSavePath(GameObject go)
		{
			StringBuilder sb = new StringBuilder("");
			Transform t = go.transform.parent;
			while(null != t)
			{
				sb.Insert(0,string.Format("{0}/",t.name));
				t = t.parent;
			}
			sb.Insert(0,string.Format("Assets/{0}/",_target.exportRootPath));
			return sb.ToString();
		}
		
		private void CheckAndCreateFolder(string path)
		{
			if(!File.Exists(path))
			{
				Directory.CreateDirectory(path);
			}
		}
		private void SaveMeshFile(MeshFilter filter)
		{
			if(null == filter)
				return;
			GameObject go = filter.gameObject;
			string path = GetSavePath(go);
			CheckAndCreateFolder(path);
			string fileName = path+go.name+".asset";
			if(!File.Exists(fileName))
				AssetDatabase.CreateAsset(filter.sharedMesh,path+go.name+".asset");
			else
				Debug.LogWarning(string.Format("{0} exist,please reset its mesh",fileName));
		}

		private void ExportMeshes()
		{
			foreach(MeshFilter mf in _target.GenerateMeshes)
			{
				SaveMeshFile(mf);
			}
		}
	}
}