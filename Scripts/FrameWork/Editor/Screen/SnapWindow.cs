using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace FrameWork.Editor
{
    public class SnapWindow:EditorWindow
    {
        private string baseDir;
        private string saveFile = "screen";
        private int width = 1024;
        private int height = 1024;
        [MenuItem("Window/Snap %#p")]
        public static void ShowWindow()
        {
            var window = GetWindow<SnapWindow>();
            window.titleContent = new GUIContent("Snap");
            window.Show();
        }

        private void OnEnable()
        {
            baseDir = Application.dataPath + "/ScreenShot";
            CheckDir(baseDir);
        }

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("根目录:ScreenShot");
            saveFile = EditorGUILayout.TextField("文件名:", saveFile);
            width = Math.Max(256,EditorGUILayout.IntField("宽度", width));
            height = Math.Max(256, EditorGUILayout.IntField("高度", height));
            if (GUILayout.Button("快照"))
            {
                Snap();
            }
            EditorGUILayout.EndVertical();
        }

        private void CheckDir(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        private void Snap()
        {
            string path = string.Format("{0}/{1}.png", baseDir, saveFile);
            Rect rect = new Rect(0,0,width,height);
            CaptureByCamera(Camera.main, rect, path);
            AssetDatabase.Refresh();
        }

        private static void CaptureByCamera(Camera mCamera,Rect mRect,string mFileName)  
        {  
            RenderTexture mRender=new RenderTexture((int)mRect.width,(int)mRect.height,24); 
            mCamera.targetTexture=mRender;  
            mCamera.Render();  
            RenderTexture.active=mRender;  
            Texture2D mTexture=new Texture2D((int)mRect.width,(int)mRect.height,TextureFormat.RGB24,false);  
            mTexture.ReadPixels(mRect,0,0);  
            mTexture.Apply();  
            mCamera.targetTexture = null;     
            RenderTexture.active = null;   
            GameObject.DestroyImmediate(mRender);    
            byte[] bytes = mTexture.EncodeToPNG();    
            System.IO.File.WriteAllBytes(mFileName,bytes);  
        }
    }
}