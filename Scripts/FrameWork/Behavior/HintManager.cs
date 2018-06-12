using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork
{
    public class HintManager : MonoBehaviour
    {
        #region Variables
        public GameObject hintTemp = null;
        public float rotateDegree = 0;
        public Vector4 hintOffset = Vector4.zero;
        private Dictionary<int, GameObject> hintItems = new Dictionary<int, GameObject>();
        private List<GameObject> unUsed = new List<GameObject>();

        private int itemIdx = 0;
        #endregion

        void Awake()
        {
            if (null != hintTemp)
                hintTemp.SetActive(false);
            itemIdx = 0;
        }

        public Transform AddHintItem(int ownerId, Transform hintRoot)
        {
            GameObject hintItem = CreateHintItem(hintRoot);
            hintItems.Add(ownerId, hintItem);
            return hintItem.transform;
        }

        public void RemoveHintItem(int ownerId)
        {
            if (hintItems.ContainsKey(ownerId))
            {
                GameObject hintItem = hintItems[ownerId];
                hintItem.SetActive(false);
                unUsed.Add(hintItem);
                hintItems.Remove(ownerId);
            }
        }

        private GameObject CreateHintItem(Transform hintRoot)
        {
            if (unUsed.Count > 0)
            {
                GameObject hintItem = unUsed[unUsed.Count - 1];
                unUsed.RemoveAt(unUsed.Count - 1);
                hintItem.SetActive(true);
                return hintItem;
            }

            GameObject go = GameObject.Instantiate(hintTemp) as GameObject;
            go.name = string.Format("HintItem_{0}", ++itemIdx);
            Transform hintItemTrans = go.transform;
            hintItemTrans.SetParent(hintRoot);
            hintItemTrans.localPosition = Vector3.zero;
            hintItemTrans.localScale = Vector3.one;
            Image image = go.GetComponent<Image>();
            if (image != null)
                image.enabled = false;
            go.SetActive(true);

            return go;
        }

        public void PositionHint(Image image, Transform targetPos)
        {
            image.enabled = false;
            Vector3 v3Pos = Global.Instance.MainCamera.WorldToViewportPoint(targetPos.transform.position);
            if (v3Pos.z < 0.01f)
            {
                v3Pos = -v3Pos;
                //return;  
            }
            if (v3Pos.x >= 0.0f && v3Pos.x <= 1.0f && v3Pos.y >= 0.0f && v3Pos.y <= 1.0f)
                return;

            image.enabled = true;
            v3Pos.x -= 0.5f;
            v3Pos.y -= 0.5f;
            v3Pos.z = 0;

            float fAngle = Mathf.Atan2(v3Pos.x, v3Pos.y);
            image.transform.localEulerAngles = new Vector3(0.0f, 0.0f, (rotateDegree - fAngle) * Mathf.Rad2Deg);

            //v3Pos.x = 0.5f * Mathf.Sin(fAngle) + 0.5f;
            //v3Pos.y = 0.5f * Mathf.Cos(fAngle) + 0.5f;
            v3Pos.x = Mathf.Lerp(hintOffset.x, hintOffset.y, Mathf.InverseLerp(-1, 1, Mathf.Sin(fAngle)));
            v3Pos.y = Mathf.Lerp(hintOffset.z, hintOffset.w, Mathf.InverseLerp(-1, 1, Mathf.Cos(fAngle)));

            v3Pos.z = Global.Instance.MainCamera.nearClipPlane + 0.01f;
            var tempPos11 = Global.Instance.MainCamera.ViewportToScreenPoint(v3Pos);
            var tempos2 = Global.Instance.UiCamera.ScreenToWorldPoint(tempPos11);

            image.transform.position = tempos2;
        }
    }
}
