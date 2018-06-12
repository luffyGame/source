using System.Collections.Generic;
using System.IO;
using FrameWork.BaseUtil;
using ICSharpCode.SharpZipLib.Zip;

namespace Game
{
    public class LuaZip
    {
        private static readonly string LURMAINZIPFILE = "gmain";
        private Dictionary<string,MemoryStream> zipMems = new Dictionary<string, MemoryStream>();

	    public void LoadMain()
	    {
		    SetZip(LURMAINZIPFILE);
	    }
        private void SetZip(string zipFile)
		{
			CloseZip();
			if(string.IsNullOrEmpty(zipFile))
				return;
			string zipPath = FileUtility.GetFileReadFullPath(zipFile);
            using (Stream zipStream = FileUtility.OpenFile(zipPath))
			{
                if (null != zipStream)
                {
	                using (ZipInputStream unZip = new ZipInputStream(zipStream))
	                {
		                ZipEntry theEntry;
		                byte[] buffer = new byte[4096]; //缓冲区大小
		                while ((theEntry = unZip.GetNextEntry()) != null)
		                {
			                if (!theEntry.IsDirectory)
			                {
				                MemoryStream stream = new MemoryStream();
				                while (true)
				                {
					                int size = unZip.Read(buffer, 0, 4096);
					                if (size > 0)
					                {
						                stream.Write(buffer, 0, size);
					                }
					                else
					                {
						                break;
					                }
				                }
				                zipMems.Add(theEntry.Name, stream);
			                }
		                }
	                }
                }
			}
		}
		public void CloseZip()
		{
			foreach(KeyValuePair<string,MemoryStream> element in zipMems)
			{
				element.Value.Close();
			}
			zipMems.Clear();
		}
	    
	    public byte[] Load(ref string filePath)
	    {
		    string path = string.Format("{0}.lua",filePath.Replace('.','/'));
		    if (zipMems.ContainsKey(path))
		    {
			    MemoryStream stream = zipMems[path];
			    zipMems.Remove(path);
			    byte[] datas = stream.ToArray();
			    stream.Close();
			    return datas;
		    }

		    return null;
	    }
    }
}