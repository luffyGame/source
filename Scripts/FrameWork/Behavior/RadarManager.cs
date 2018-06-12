using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

namespace FrameWork
{
    public class RadarManager : MonoBehaviour
    {
        #region Variables
        public RadarItem template = null;

        private Dictionary<int, RadarItem> radars = new Dictionary<int, RadarItem>();//objId做key
        private List<RadarItem> unUsed = new List<RadarItem>();
        private int itemIdx = 0;

        public RectTransform miniMap = null;
        public Transform unUsedRoot;
        public float multiply;

        public float borderMultiply;
        public float borderOffset;

        public RectTransform playerTrans;
        public RectTransform borderRoot;
        public RectTransform borderImage;
        private float cameraYRot;

        private Transform trans;
        private Transform Trans { get { if (null == trans) trans = transform; return trans; } }
        #endregion

        private void Awake()
        {
            itemIdx = 0;
        }

        private void OnEnable()
        {
            cameraYRot = Global.Instance.CameraYRot;
        }

        #region Public Method
        public Transform AddRadar(int ownerId, float x, float z, string image)
        {
            RadarItem radar = CreateRadar();
            if (!radars.ContainsKey(ownerId))
            {
                radars.Add(ownerId, radar);
                MoveRadarPos(ownerId, x, z,0,-1);
            }
            radar.icon.sprite = CommonResMgr.Instance.GetSprite(image);
            radar.icon.SetNativeSize();
            return radar.transform;
        }

        private RadarItem CreateRadar()
        {
            RadarItem radarItem = null;
            if (unUsed.Count > 0)
            {
                radarItem = unUsed[unUsed.Count - 1];
                unUsed.RemoveAt(unUsed.Count - 1);
                radarItem.transform.SetParent(Trans);
            }
            else
            {
                radarItem = GameObject.Instantiate<RadarItem>(template, Trans);
                radarItem.name= string.Format("Radar_{0}", ++itemIdx);
                radarItem.gameObject.SetActive(true);
                radarItem.rectTrans.localPosition = Vector3.zero;
                radarItem.rectTrans.localScale = Vector3.one;
            }
            
            return radarItem;
        }

        public void MoveRadarPos(int radarID, float x, float z,float dirx,float diry)
        {
            RadarItem radar = null;
            radars.TryGetValue(radarID, out radar);
            if (radar != null){
                var newVec = new Vector2(x, z) * multiply;
                
                radar.SetPos(newVec);
                radar.SetDir(new Vector2(dirx, diry).normalized, cameraYRot, playerTrans);
            }
                
        }

        public void RotatePlayerIcon(float dirx, float diry)
        {
            Vector2 newDir = new Vector2(dirx, diry);
            playerTrans.localEulerAngles = new Vector3(0, 0, Vector2.SignedAngle(Vector2.up, newDir) + cameraYRot + 180);
        }

        public void RemoveRadar(int ownerId)
        {
            if (radars.ContainsKey(ownerId))
            {
                RadarItem radar = radars[ownerId];
                radar.transform.SetParent(unUsedRoot);
                unUsed.Add(radar);
                radars.Remove(ownerId);
            }
        }

        public void ChangeIcon(int radarID, string iconName)
        {
            RadarItem radar = null;
            radars.TryGetValue(radarID, out radar);
            if (radar != null)
            {
                radar.icon.sprite = CommonResMgr.Instance.GetSprite(iconName);
                radar.icon.SetNativeSize();
            }
        }

        public void InitBorderRot(float x, float z, float offsetX, float offsetZ)
        {
            borderImage.sizeDelta = new Vector3(x * borderMultiply + borderOffset, z * borderMultiply + borderOffset);
            borderImage.anchoredPosition = new Vector2(offsetX * borderMultiply, offsetZ * borderMultiply);
            borderRoot.localEulerAngles = new Vector3(0, 0, cameraYRot);
        }

        public void OutBorder(float x, float z)
        {
            borderImage.anchoredPosition = new Vector2(x* borderMultiply, z * borderMultiply);
        }

        public void DestroyRadarItems()
        {
            foreach(var radar in radars)
            {
                RadarItem ri = radar.Value;
                ri.transform.SetParent(unUsedRoot);
                unUsed.Add(ri);
            }

            radars.Clear();
        }
        #endregion
    }
}
	
