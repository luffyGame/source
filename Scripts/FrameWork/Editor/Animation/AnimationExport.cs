using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace FrameWork.Editor
{
	public class AnimationExport
	{
        private static readonly char[] pathsep = new char[] { '\\', '/' };
        [MenuItem("Assets/FrameWork/动画/导出")]
		static void ExportAni()
		{
			GameObject go = Selection.activeGameObject;
			if(null == go)
				return;
			string objName = go.name;
			Animation anim = go.GetComponent<Animation>();
			if (anim != null) 
			{
				IDictionary<string,AnimationClip> clips = new Dictionary<string, AnimationClip>();
				foreach (AnimationState animClip in anim) 
				{
					clips[animClip.name] = animClip.clip;
				}
				foreach(KeyValuePair<string,AnimationClip> element in clips)
				{
					AnimationClip clip = element.Value;
					if(null == clip)
					{
						Debug.Log(string.Format("{0} not exist",element.Key));
						continue;
					}
                    string filepath = AssetDatabase.GetAssetPath(clip);
                    string folder = filepath.Substring(0,filepath.LastIndexOfAny(pathsep));
 					anim.RemoveClip(clip);
 					AnimationClip cloneClip = Object.Instantiate(clip) as AnimationClip;
 					cloneClip.name = clip.name;
 					AssetDatabase.CreateAsset(cloneClip, 
 					    string.Format("{0}/{1}.anim", folder,cloneClip.name));
 					anim.AddClip(cloneClip,cloneClip.name);
				}
				AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
				Debug.Log("Export ani success");
			}
			else
				Debug.LogError("animation not find");
		}
	}
}