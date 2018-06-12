using System;
using UnityEngine;
using UnityEngine.AI;

namespace FrameWork
{
    public class mapPlaceManager : MonoBehaviour
    {
        public Transform randomPosRoot;

        /// <summary>
        /// 放置模型
        /// </summary>
        /// <param name="modelRes">模型</param>
        /// <param name="posIndex">随机位置</param>
        public void PlaceDropItem(Transform modelRes, int posIndex)
        {
            Transform model = modelRes.parent;
            if (randomPosRoot.childCount > posIndex)
            {
                model.SetParent(randomPosRoot.GetChild(posIndex));
                model.localPosition = Vector3.zero;
                model.localEulerAngles = Vector3.zero;
                model.localScale = Vector3.one;
                model.gameObject.SetActive(true);
            }
        }

        public void RemoveDropItem(Transform modelRes)
        {
            modelRes.gameObject.SetActive(false);
        }

    }
}
