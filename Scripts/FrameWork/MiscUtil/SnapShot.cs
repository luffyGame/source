using System.IO;
using UnityEngine;

namespace FrameWork
{
    public class SnapShot : MonoBehaviour
    {
        public string snapFile;
        private string saveDir;
        private Camera camera;

        private string SavePath
        {
            get { return string.Format("{0}/{1}.png",saveDir,snapFile); }
        }

        private void Start()
        {
            camera = GetComponent<Camera>();
            saveDir = Application.dataPath + "/ScreenShot";
            if (!Directory.Exists(saveDir))
                Directory.CreateDirectory(saveDir);
        }

        void OnGUI()  
        {  
            if(GUILayout.Button("截图方式1",GUILayout.Height(30))){  
                ScreenSnap.ScreenShot(SavePath);  
            }  
            if(GUILayout.Button("截图方式2",GUILayout.Height(30))){  
                StartCoroutine(ScreenSnap.CaptureByRect(new Rect(0,0,1024,768),SavePath));  
            }  
            if(GUILayout.Button("截图方式3",GUILayout.Height(30))&&null!=camera){  
                camera.enabled=true;  
                StartCoroutine(ScreenSnap.CaptureByCamera(camera,new Rect(0,0,512,256),SavePath));  
            }  
        }  
    }
}