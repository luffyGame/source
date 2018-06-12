using System;
using UnityEngine.EventSystems;

namespace FrameWork
{
    public class UiEventHandler : UIBehaviour,IPointerDownHandler,IPointerUpHandler
    {
        public Action onPointerDown;
        public Action onPointerUp;
        public void OnPointerDown(PointerEventData eventData)
        {
            if (null != onPointerDown)
                onPointerDown();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (null != onPointerUp)
                onPointerUp();
        }
    }
}