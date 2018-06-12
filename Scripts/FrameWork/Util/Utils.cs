using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FrameWork.BaseUtil;
using System.IO;
using System.Text;
using System;
using System.Xml.Serialization;
using ICSharpCode.SharpZipLib.Zip;
using UnityEngine.SceneManagement;

namespace FrameWork
{
    public static class Utils
    {
        #region Extend Method
        public static void SetScale(this Transform trans, Vector3 scale)
        {
            Transform parent = trans.parent;
            Vector3 parentScale = null == parent ? Vector3.one : parent.lossyScale;
            trans.localScale = new Vector3(scale.x / parentScale.x, scale.y / parentScale.y, scale.z / parentScale.z);
        }

        public static void SetParentIndentical(this Transform trans, Transform parent)
        {
            trans.SetParent(parent,false);
            trans.localPosition = Vector3.zero;
            trans.localRotation = Quaternion.identity;
            trans.localScale = Vector3.one;
        }

        public static Transform FindRecursive(this Transform trans, string name)
        {
            Transform find = trans.Find(name);
            if (null != find)
                return find;
            foreach (Transform child in trans)
            {
                find = child.FindRecursive(name);
                if (null != find)
                    return find;
            }
            return null;
        }
        #endregion

        #region Bound Method
        /// <summary>  
        /// 计算content相对父级的bounds  
        /// </summary>  
        /// <param name="content"></param>  
        /// <returns></returns>  
        public static Bounds CalculateRelativeBounds(RectTransform content)
        {
            return CalculateRelativeBounds(content.parent, content);
        }
        /// <summary>  
        /// 计算相对于relativeTo的bounds  
        /// </summary>  
        /// <param name="relativeTo"></param>  
        /// <param name="content"></param>  
        /// <returns></returns>  
        public static Bounds CalculateRelativeBounds(Transform relativeTo, RectTransform content)
        {
            Bounds bounds = CalculateWorldBounds(content);
            Debug.Log(bounds);

            if (relativeTo != null)
            {
                Vector3 size = relativeTo.InverseTransformVector(bounds.size);
                size.z = 0;
                Vector3 center = relativeTo.InverseTransformPoint(bounds.center);
                center.z = 0;
                return new Bounds(center, size);
            }

            return bounds;
        }  
        /// <summary>  
        /// 计算content相对世界坐标的bounds  
        /// </summary>  
        /// <param name="content"></param>  
        /// <returns></returns>  
        public static Bounds CalculateWorldBounds(RectTransform content)
        {
            Vector2 min = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 max = new Vector2(float.MinValue, float.MinValue);
            GetRect(content,false,ref min, ref max);
            return new Bounds((max + min) / 2f, (max - min));
        } 
        //获取包含content在内的边框
        public static void GetRect(RectTransform content,bool selfIn,ref Vector2 min, ref Vector2 max)
        {
            if(selfIn)
            {
                Vector3[] corners = new Vector3[4];
                content.GetWorldCorners(corners);
                min.x = Mathf.Min(min.x, corners[0].x, corners[2].x);
                min.y = Mathf.Min(min.y, corners[0].y, corners[2].y);
                max.x = Mathf.Max(max.x, corners[0].x, corners[2].x);
                max.y = Mathf.Max(max.y, corners[0].y, corners[2].y);
            }
            
            for (int i = 0, imax = content.childCount; i < imax; i++)
            {
                GetRect(content.GetChild(i) as RectTransform,true,ref min, ref max);
            }
        }  
        #endregion
        
        public static int GetRandomIdx(int curIdx,int count)
        {
            if (count <= 0) return -1;
            if (count == 1) return 0;
            if (curIdx < 0)
                return Global.RandomRange(0, count);
            else
            {
                int i = Global.RandomRange(0, count - 1);
                if (i < curIdx)
                    return i;
                else
                    return i + 1;
            }
        }

        public static void GetComponent<T>(GameObject go, ref List<T> components)
        {
            if (null == components)
                components = new List<T>();
            T[] comps = go.GetComponents<T>();
            if (null != comps)
            {
                foreach (T comp in comps)
                    components.Add(comp);
            }
            foreach (Transform child in go.transform)
            {
                GetComponent<T>(child.gameObject, ref components);
            }
        }

        public static IEnumerator DelayAction(float delay, System.Action action, bool ignoreTimeScale = false)
        {
            if (ignoreTimeScale)
            {
                bool isWaiting = true;
                float startTime = Time.realtimeSinceStartup;
                while (isWaiting)
                {
                    if (Time.realtimeSinceStartup - startTime >= delay)
                        isWaiting = false;
                    else
                        yield return new WaitForEndOfFrame();
                }
                action();
            }
            else
            {
                yield return new WaitForSeconds(delay);
                action();
            }
        }
        
        public static IEnumerator DelayAction(System.Action action,int delayFrames = 1)
        {
            for(int i=0;i<delayFrames;++i)
                yield return new WaitForEndOfFrame();
            action();
        }

        public static int GetTextLength(string txt,Text text)
        {
            int length = 0;
            Font font = text.font;
            CharacterInfo characterInfo = new CharacterInfo();
            font.RequestCharactersInTexture(txt, text.fontSize, text.fontStyle);
            for(int i=0;i<txt.Length;++i)
            {
                font.GetCharacterInfo(txt[i], out characterInfo,text.fontSize);
                length += characterInfo.advance;
            }
            return length;
        }

        public static string GetHttpHead(params string[] headParams)
        {
            int count = headParams.Length;
            if(count < 2 || count%2 == 1)
                return null;
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < count; i += 2)
            {
                if (i > 0)
                    sb.Append('&');
                sb.AppendFormat("{0}={1}", headParams[i], headParams[i + 1]);
            }
            return sb.ToString();
        }

        public static WWWForm GetWWWPostData(params string[] dataParams)
        {
            int count = dataParams.Length;
            if (count < 2 || count % 2 == 1)
                return null;
            WWWForm ret = new WWWForm();
            for (int i = 0; i < count; i += 2)
            {
                ret.AddField(dataParams[i], dataParams[i + 1]);
            }
            return ret;
        }
        public static void FileWrite(string file, string str)
        {
            StreamWriter sw = null;
            try
            {
                if (file == null)
                    return;
                sw = GetStreamWriter(file);
                sw.WriteLine(str);
            }
            finally
            {
                if (sw != null)
                    sw.Close();
            }
        }
        public static MemoryStream ZipFile(string entry, string file)
        {
            try
            {
                MemoryStream output = new MemoryStream();
                using (ZipOutputStream zipStream = new ZipOutputStream(output))
                {
                    ZipEntry zipentry = new ZipEntry(entry);
                    zipStream.PutNextEntry(zipentry);
                    using (FileStream fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                    {
                        FileUtility.CopyStream(fs, zipStream);
                    }
                }
                output.Position = 0;
                return output;
            }
            catch (Exception ex)
            {
                Debugger.LogError(ex);
                return null;
            }
        }

        public static GameObject[] GetCurSceneRootObjs()
        {
            Scene curScene = SceneManager.GetActiveScene();
            return curScene.GetRootGameObjects();
        }

        public static void SetByJsonFile<T>(T data,string fileName,bool checkInside = true)
        {
            var path = FileUtility.GetFileReadFullPath(fileName, checkInside);
            if (null == path)
                return;
            try
            {
                using (var stream = FileUtility.OpenFile(path))
                {
                    if (null != stream)
                    {
                        using (StreamReader sr = new StreamReader(stream, Encoding.UTF8))
                        {
                            JsonUtility.FromJsonOverwrite(sr.ReadToEnd(),data);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debugger.LogError("SetByJsonFile {0} err:{1}", path, ex.Message);
            }
        }
        public static bool SaveToJsonFile<T>(T data, string fileName)
        {
            var path = FileUtility.GetFileWriteFullPath(fileName);
            if (null == path)
                return false;
            try
            {
                using (var stream = File.CreateText(path))
                {
                    stream.Write(JsonUtility.ToJson(data));
                    return true;
                }
            }
            catch (Exception ex)
            {
                Debugger.LogError("SaveToJsonFile {0} err:{1}", path, ex.Message);
                return false;
            }
        }
        private static float fInver = 1f/255f;
        public static Color Parse_RGBA_Color (uint color)
        {
            uint r = color >> 24;
            uint g = (color >> 16) & 0xFF;
            uint b = (color >> 8) & 0xFF;
            uint a = color & 0xFF;
            return new Color(r*fInver,g*fInver,b*fInver,a*fInver);
        }

        public static uint Color_2_RGBA_Val(Color color)
        {
            uint r = (uint)(color.r * 255) << 24;
            uint g = (uint)(color.g * 255) << 16;
            uint b = (uint)(color.b * 255) << 8;
            uint a = (uint)(color.a * 255);
            return r|g|b|a;
        }

        #region Statiicstic Method
        //依据前缀来计算骨骼数,这是一个基于约定的估计
        public static int CalcBoneCount(GameObject go, string bonePre)
        {
            Transform[] trans = go.GetComponentsInChildren<Transform>();
            string[] pres = bonePre.Split(new char[] {','});
            int count = 0;
            foreach (var tran in trans)
            {
                string name = tran.name;
                foreach (var pre in pres)
                {
                    if (name.StartsWith(pre))
                    {
                        ++count;
                        break;
                    }
                }
            }
            return count;
        }
        //获取模型面数
        public static int CalcTriCount(GameObject go)
        {
            int count = 0;
            MeshFilter[] meshFilters = go.GetComponentsInChildren<MeshFilter>();
            foreach (var meshFilter in meshFilters)
            {
                if(null != meshFilter&&null!=meshFilter.sharedMesh)
                    count += meshFilter.sharedMesh.triangles.Length / 3;
            }

            SkinnedMeshRenderer[] skinnedMeshRenderers = go.GetComponentsInChildren<SkinnedMeshRenderer>();
            foreach (var ski in skinnedMeshRenderers)
            {
                if(null!=ski&&null!=ski.sharedMesh)
                    count += ski.sharedMesh.triangles.Length / 3;
            }

            return count;
        }
        #endregion
        #region Private Method
        private static StreamWriter GetStreamWriter(string file)
        {
            if (File.Exists(file))
                return File.AppendText(file);//using utf-8
            else
                return File.CreateText(file);//using utf-8
        }
        #endregion
    }
}
