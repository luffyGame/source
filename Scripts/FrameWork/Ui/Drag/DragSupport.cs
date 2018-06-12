using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FrameWork
{
    public class DragSupport : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public UnityEvent onDropOff;
        public GameObject dragIgnore;
        public bool CanDrag { get; set; }
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (DragIn.Instance.isDragging||!CanDrag)
                return;
            DragIn.Instance.StartDrag(gameObject, eventData);
        }
        public void OnDrag(PointerEventData eventData)
        {
            if (DragIn.Instance.DragOriginal == gameObject)
                DragIn.Instance.SetDraggedPosition(eventData);
        }
        public void OnEndDrag(PointerEventData eventData)
        {
            if (DragIn.Instance.DragOriginal == gameObject)
            {
                if(!DragIn.Instance.droped)//没有drop
                {
                    bool ignore = false;
                    if (dragIgnore == null)
                        ignore = eventData.hovered.Contains(gameObject);
                    else
                        ignore = eventData.hovered.Contains(dragIgnore);

                    if (!ignore){
                        if (null != onDropOff)
                            onDropOff.Invoke();
                    }
                }
                DragIn.Instance.StopDrag();
            }
        }
    }
}
