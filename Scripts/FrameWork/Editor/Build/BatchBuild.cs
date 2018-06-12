using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;

namespace FrameWork.Editor
{
	public class BatchBuild : MonoBehaviour
	{
		#region Build Method
        [MenuItem("FrameWork/打包/Android", false, 1001)]
		public static void BuildAndroid()
		{
			//GenClientVer();
			Build(Application.productName,BuildTarget.Android);
		}
		#endregion
		#region Private Method
		//获取生效的场景
		private static List<string> GetEnabledEditorScenes()
		{
			List<string> scenes = new List<string>();
			foreach(EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
			{
				if(scene.enabled)
					scenes.Add(scene.path);
			}
			return scenes;
		}
		private static void ProcessBuild(string[] scenes,string targetDir,BuildTarget buildTarget,BuildOptions buildOps)
		{
			EditorUserBuildSettings.SwitchActiveBuildTarget(buildTarget);
			string res = BuildPipeline.BuildPlayer(scenes,targetDir,buildTarget,buildOps);
			if(res.Length>0)
				Debug.LogError("BuildPlayer failure: " + res);
		}
		private static void Build(string name,BuildTarget buildTarget)
		{
			List<string> scenes = GetEnabledEditorScenes();
			if(scenes.Count == 0)
				return;
			string dir = Application.dataPath.Replace("/Assets","");
			int index = dir.LastIndexOf('/');
			dir = dir.Substring(0,index) + "/app";
			if(!Directory.Exists(dir))
				Directory.CreateDirectory(dir);
			string appName = string.Format("{0}_{1:yyyyMMdd_HHmm}",name,System.DateTime.Now);
			switch(buildTarget)
			{
			case BuildTarget.Android:
				appName += ".apk";
				break;
			}
			string targetDir = dir + "/" + appName;
			ProcessBuild(scenes.ToArray(),targetDir,buildTarget,BuildOptions.None);
			string param = " /select, " + targetDir.Replace('/','\\');
			Util.RunCmd("explorer.exe",param);
		}

		#endregion
	}
}