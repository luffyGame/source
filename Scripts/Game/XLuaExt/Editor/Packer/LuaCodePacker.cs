using System.Collections.Generic;
using System.IO;
using FrameWork.BaseUtil;
using FrameWork.Editor;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine;
using UnityEditor;

namespace Game
{
    public class LuaCodePacker
    {
        private static readonly string exportMainFile = Application.streamingAssetsPath+"/gmain";
        private static readonly string insideSrcPath = Application.dataPath+"/LuaRoot";
        
        #region Private Method
		private static void PackZip(string srcPath,bool isPatcher = false)
		{
			if(!Directory.Exists(srcPath))
				Debug.Log(srcPath + " not exist!");
			else
			{
				using(ZipOutputStream zipStream = new ZipOutputStream(File.Create(exportMainFile)))
				{
					zipStream.UseZip64 = UseZip64.Off;
					byte[] buffer = new byte[4096]; //缓冲区大小
					List<FileInfo> files = Util.GetFiles(srcPath,new string[]{".lua"});
					foreach(FileInfo f in files)
					{
						string fileEntryName = f.FullName.Substring(srcPath.Length+1);
						bool isPatcherFile = fileEntryName.StartsWith("patcher");
						if(isPatcherFile != isPatcher)
							continue;
						fileEntryName = fileEntryName.Replace('\\','/');//文件名中不能用#
						ZipEntry entry = new ZipEntry(fileEntryName);
						zipStream.PutNextEntry(entry);
						using (FileStream fs = File.OpenRead(f.FullName))
						{
							int sourceBytes;
							do
							{
								sourceBytes = fs.Read(buffer, 0, buffer.Length);
								if(sourceBytes>0)
									zipStream.Write(buffer, 0, sourceBytes);
							} 
							while (sourceBytes > 0);
						}
					}
					zipStream.Close();
				}
				//BaseUtil.FileUtility.FileTranslate(tempFile,isPatcher?exportPatcherFile:exportMainFile);
				//File.Delete(tempFile);
			}
		}

		//[MenuItem("GJ/B3.脚本标记")]
		/*--packer-->--
		 * ScriptInfo = {
		 * 		ver = x,
		 * 		code = "{0:yyyyMMddHHmm}",
		 * }
		 * debug.traceback(ScriptInfo.code)
		 * --<--packer-- auto gen,don't edit above
		 */
		private static void PatchLuaScriptInfo(string srcPath)
		{
			string file = srcPath+"/app/init.lua";
			int ver = 0;
			string code = string.Format("CODE:{0:yyyyMMddHHmm}",System.DateTime.Now);
			string[] lines = File.ReadAllLines(file);
			lines[0] = "--packer auto gen start-->----------------------";
			lines[1] = "ScriptInfo = {";
			string verline = lines[2].Trim().Replace(" ","");
			if(int.TryParse(verline.Substring(4,verline.Length-5),out ver))
				++ver;
			lines[2] = string.Format("\tver = {0},",ver);
			lines[3] = string.Format("\tcode = \"{0}\",",code);
			lines[4] = "}";
			lines[5] = "debug.traceback(ScriptInfo.code)";
			lines[6] = "--<--packer auto gen end,don't edit above------------";
			File.WriteAllLines(file,lines);
			Debug.Log(string.Format("ver={0},{1}",ver,code));
		}
		#endregion

		[MenuItem("Game/脚本/打包",false,101)]
		static void Pack ()
		{
			//PatchLuaScriptInfo(insideSrcPath);
			PackZip(insideSrcPath);
			AssetDatabase.Refresh();
			Debug.Log("打包脚本 完成");
		}
		[MenuItem("Game/脚本/移除",false,102)]
		static void RemovePack()
		{
			if(File.Exists(exportMainFile))
				File.Delete(exportMainFile);
			AssetDatabase.Refresh();
			Debug.Log("移除脚本 完成");
		}
		[MenuItem("Game/脚本/_.打包指定目录",false,103)]
		static void PackSpec()
		{
			var path = EditorUtility.OpenFolderPanel("Select LuaRoot Directory", "", "");
			if(!string.IsNullOrEmpty(path))
			{
				PatchLuaScriptInfo(path);
				PackZip(path);
				Debug.Log(string.Format("打包脚本完成：{0}",path));
			}
		}
		[MenuItem ("Assets/Game/LuaInterpret")]
		static void InterpretLuaZip()
		{
			UnityEngine.Object obj = Selection.activeObject;
			string path = Util.GetObjectAbsolutPath(obj);
		}
    }
}