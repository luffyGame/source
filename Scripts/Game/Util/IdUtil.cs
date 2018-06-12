using System;
using System.IO;
using FrameWork;
using FrameWork.BaseUtil;

namespace Game
{
    public static class IdUtil
    {
        private static readonly string path = "id";

        public static int Load()
        {
            try
            {
                using (var stream = FileUtility.OpenFile(path))
                {
                    if (stream == null)
                        return 0;
                    else
                    {
                        using (var reader = new BinaryReader(stream))
                        {
                            int curId = reader.ReadInt32();
                            reader.Close();
                            return curId;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debugger.LogError("IdGen read {0} err:{1}", path, ex.Message);
                return 0;
            }
        }

        public static void Save(int curId)
        {
            try
            {
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    using (var writer = new BinaryWriter(stream))
                    {
                        writer.Write(curId);
                        writer.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Debugger.LogError("IdGen write {0} err:{1}", path, ex.Message);
            }
        }
    }
}