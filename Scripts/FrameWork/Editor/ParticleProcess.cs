using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

namespace FrameWork.Editor
{
	public class ParticleProcess
	{
		[MenuItem ("Assets/FrameWork/Particle/Process")]
		static void Process()
		{
			Object[] os = Selection.GetFiltered(typeof(Object),SelectionMode.DeepAssets);
			foreach(Object o in os)
			{
				if(o is GameObject)
				{
					GameObject go = o as GameObject;
					ProcessSingle(go);
				}
			}
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			Debug.Log("ParticleProcess done");
		}
        [MenuItem("Assets/FrameWork/Particle/Single")]
		static void ProcessOne()
		{
			GameObject go = Selection.activeGameObject;
			if(null == go)
				return;
			ProcessSingle(go);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			Debug.Log(string.Format ("{0} done",go.name));
		}

		private static void ProcessSingle(GameObject go)
		{
			ParticleCtrl.Attach(go);
			EditorUtility.SetDirty(go);
		}
	}
}