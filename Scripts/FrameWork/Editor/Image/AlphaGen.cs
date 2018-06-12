using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Diagnostics;
using System;

namespace FrameWork.Editor
{
	public class AlphaGen
	{
		[MenuItem ("Assets/FrameWork/AlphaGen")]
		static void Process()
		{
			UnityEngine.Object obj = Selection.activeObject;
			Texture textureObj = obj as Texture;
			if(null!=textureObj)
			{
				string path = Util.GetObjectAbsolutPath(textureObj);
				int index = path.LastIndexOf('.');
				string genPath = string.Format("{0}Alpha{1}",path.Substring(0,index),path.Substring(index));
				string cmdStr = string.Format("\"{0}\" --sheet \"{1}\" --format unity --data dummy.bytes --opt ALPHA --allow-free-size --border-padding 0 --padding 0 --trim-mode None",
				                              path,genPath);
				if(Util.RunCmd("TexturePacker",cmdStr))
				{
					UnityEngine.Debug.Log("AlphaGen Success");
					AssetDatabase.Refresh();
				}
				//cmd = 'TexturePacker %s%s --sheet %s%s ' % (srcPath, filename, srcPath, filename[:-ext_len] + dst_name+extname)
			}
		}
	}
}

