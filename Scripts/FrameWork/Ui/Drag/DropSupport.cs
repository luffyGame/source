using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace FrameWork
{
    public class DropSupport : MonoBehaviour, IDropHandler
    {
        [Serializable]
        public class DropOn : UnityEvent<GameObject>{}
        public DropOn dropOn;
        public void OnDrop(PointerEventData data)
        {
            var originalObj = data.pointerDrag;
            if (originalObj == null)
                return;
            if (null == DragIn.Instance.DragOriginal)
                return;
            if (null != dropOn)
                dropOn.Invoke(DragIn.Instance.DragOriginal);
            DragIn.Instance.droped = true;
        }
    }
}
