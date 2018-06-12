using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FrameWork
{
    public class DragIn : SingleBehavior<DragIn>
    {
        private RectTransform rTrans;
        public GameObject DragOriginal { get; set; }
        private RectTransform dragTrans;
        public bool isDragging { get; set; }
        public bool droped { get; set; }
        public RectTransform RTrans
        {
            get
            {
                if (null == rTrans)
                    rTrans = transform as RectTransform;
                return rTrans;
            }
        }
        public override void OnInit()
        {
            CanvasGroup group = gameObject.GetComponent<CanvasGroup>();
            if (null == group)
                group = gameObject.AddComponent<CanvasGroup>();
            group.blocksRaycasts = false;
        }
        public void StartDrag(GameObject go, PointerEventData eventData)
        {
            isDragging = true;
            droped = false;
            DragOriginal = go;
            GameObject dragObj = GameObject.Instantiate<GameObject>(go);
            dragTrans = dragObj.transform as RectTransform;
            dragTrans.sizeDelta = (go.transform as RectTransform).sizeDelta;
            dragTrans.SetParent(RTrans, false);
            dragTrans.SetAsLastSibling();
            SetDraggedPosition(eventData);
        }

        public void SetDraggedPosition(PointerEventData eventData)
        {
            if (null == dragTrans)
                return;
            Vector3 globalMousePos;
            if (RectTransformUtility.ScreenPointToWorldPointInRectangle(RTrans, eventData.position, eventData.pressEventCamera, out globalMousePos))
            {
                dragTrans.position = globalMousePos;
            }
        }
        public void StopDrag()
        {
            Debugger.Log("stopDrag");
            DragOriginal = null;
            Destroy(dragTrans.gameObject);
            dragTrans = null;
            isDragging = false;
        }
    }
}
