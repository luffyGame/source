using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using LitJson;
using System.Collections;
using System.IO;

namespace FrameWork.BaseUtil
{
    public static class FuncUtility
    {
        public static string GetMD5(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] retBytes = md5.ComputeHash(bytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < retBytes.Length; i++)
            {
                sb.AppendFormat("{0:x2}", retBytes[i]);
            }
            return sb.ToString();
        }

        public static string GetBase64(string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(bytes);
        }

        #region Json Help
        public static string GetJsonStrField(this JsonData data, string field)
        {
            IJsonWrapper strData = (IJsonWrapper)data[field];
            if (strData == null)
                return null;
            return strData.GetString();
        }
        public static string GetJsonString(this JsonData data)
        {
            return ((IJsonWrapper)data).GetString();
        }
        public static int GetJsonInt(this JsonData data)
        {
            return ((IJsonWrapper)data).GetInt();
        }
        public static void SetJsonInt(this JsonData data, int val)
        {
            ((IJsonWrapper)data).SetInt(val);
        }
        public static bool GetJsonBool(this JsonData data)
        {
            return ((IJsonWrapper)data).GetBoolean();
        }
        public static void SetJsonBool(this JsonData data, bool val)
        {
            ((IJsonWrapper)data).SetBoolean(val);
        }
        public static long GetJsonLong(this JsonData data)
        {
            return ((IJsonWrapper)data).GetLong();
        }
        public static double GetJsonDouble(this JsonData data)
        {
            IJsonWrapper id = data;
            if (id.IsInt)
                return id.GetInt();
            else if (id.IsLong)
                return id.GetLong();
            return id.GetDouble();
        }
        public static float GetJsonFloat(this JsonData data)
        {
            return Convert.ToSingle(GetJsonDouble(data));
        }
        public static void SetJsonFloat(this JsonData data, float val)
        {
            ((IJsonWrapper)data).SetDouble(val);
        }
        public static bool HasField(this JsonData data, string key)
        {
            return ((IDictionary)data).Contains(key);
        }
        public static void AddField(this JsonData data, string key, object val)
        {
            ((IDictionary)data).Add(key, val);
        }
        public static string GetStrFromFile(string fileName, bool checkInside = true)
        {
            var path = FileUtility.GetFileReadFullPath(fileName, checkInside);
            if (null == path)
                return null;
            try
            {
                using (var stream = FileUtility.OpenFile(path))
                {
                    if (null != stream)
                    {
                        using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                Debugger.LogError("GetStrFromFile {0} err:{1}", path, ex.Message);
                return null;
            }
        }
        public static JsonData GetJsonFromFile(string fileName, bool checkInside = true)
        {
            var path = FileUtility.GetFileReadFullPath(fileName, checkInside);
            if (null == path)
                return null;
            try
            {
                using (var stream = FileUtility.OpenFile(path))
                {
                    if (null != stream)
                    {
                        using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                        {
                            return JsonMapper.ToObject<JsonData>(sr);
                        }
                    }
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                Debugger.LogError("Parse json file {0} err:{1}", path, ex.Message);
                return null;
            }
        }
        public static bool SaveStrToFile(String str, string fileName)
        {
            var path = FileUtility.GetFileWriteFullPath(fileName);
            if (null == path)
                return false;
            try
            {
                using (var stream = File.CreateText(path))
                {
                    stream.Write(str);
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debugger.LogError("SaveStrToFile {0} err:{1}", path, ex.Message);
                return false;
            }
        }
        public static bool SaveJsonToFile(JsonData data, string fileName)
        {
            return SaveStrToFile(data.ToJson(),fileName);
        }
        #endregion
    }
}
