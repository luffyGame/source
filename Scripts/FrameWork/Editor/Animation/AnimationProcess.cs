using UnityEngine;
using System.Collections;
using UnityEditor;

namespace FrameWork.Editor
{
	public class AnimationProcess
	{
		[MenuItem ("Assets/FrameWork/动画/D->Once")]
		static void Process()
		{
			Object[] aniclips = GetSelectedAnimationClips();
			Selection.objects = new Object[0];
			foreach (AnimationClip clip in aniclips)
			{
				string path = AssetDatabase.GetAssetPath(clip);
				if(clip.wrapMode == WrapMode.Default)
					clip.wrapMode = WrapMode.Once;
				AssetDatabase.ImportAsset(path);
			}
		}

        [MenuItem("Assets/FrameWork/动画/Once")]
		static void AniOnce()
		{
			ConvertTo(WrapMode.Once);
		}

		static void ConvertTo(WrapMode wm)
		{
			Object[] aniclips = GetSelectedAnimationClips();
			Selection.objects = new Object[0];
			foreach (AnimationClip clip in aniclips)
			{
				string path = AssetDatabase.GetAssetPath(clip);
				if(clip.wrapMode == WrapMode.ClampForever)
					clip.wrapMode = wm;
				AssetDatabase.ImportAsset(path);
			}
		}

		static Object[] GetSelectedAnimationClips()
		{
			return Selection.GetFiltered(typeof(AnimationClip),SelectionMode.DeepAssets);
		}

		[MenuItem ("Assets/FrameWork/动画/Loop")]
		static void AniLoop()
		{
			Object[] aniclips = GetSelectedAnimationClips();
			Selection.objects = new Object[0];
			foreach (AnimationClip clip in aniclips)
			{
				if(clip.name == "stand1")
				{
					if(clip.wrapMode != WrapMode.Loop)
					{
						string path = AssetDatabase.GetAssetPath(clip);
						clip.wrapMode = WrapMode.Loop;
						AssetDatabase.ImportAsset(path);
					}
				}
			}
			Debug.Log("AniLoop done");
		}
		
		[MenuItem("Assets/FrameWork/动画/RecClipLength")]
		static void RecClipLength()
		{
			ActionConfig cfg = ScriptableObject.CreateInstance<ActionConfig>();
			cfg.cfg = new ActionLens();
			Object[] os = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
			foreach (Object o in os)
			{
				if (o is GameObject)
				{
					GameObject go = o as GameObject;
					ActionLen alen = RecClipLength(go);
					if(null!=alen)
						cfg.cfg.Add(go.name, alen);
				}
			}
			string path = AssetDatabase.GetAssetPath(Selection.activeObject);
			string atlasPath = path + ".asset";
			AssetDatabase.CreateAsset(cfg, atlasPath);
			AssetDatabase.SaveAssets();
			AssetDatabase.Refresh();
			Debug.Log("Action len rec done");
		}
		
		static ActionLen RecClipLength(GameObject go)
		{
			Animation anim = go.GetComponent<Animation>();
			if (null != anim)
			{
				ActionLen alen = new ActionLen();
				foreach (AnimationState animState in anim)
				{
					string name = animState.name;
					AnimationClip clip = animState.clip;
					float length = clip.length;
					alen.Add(name,length);
				}

				return alen;
			}

			return null;
		}
	}
}