using UnityEngine;
using UnityEngine.EventSystems;
using System;
namespace Game
{
    public class EventHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IDropHandler
    {
        public Action onBeginDrag;
        public Action<float, float> onDrag;
        public Action<float,float> onEndDrag;
        public Action onClick;
        public Action onDrop;
        public Action onDoubleClick;

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (onBeginDrag != null)
            {
                onBeginDrag();
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (onDrag != null)
            {
                
                onDrag(eventData.delta.x, eventData.delta.y);
            }
        }

        public void OnDrop(PointerEventData eventData)
        {
            if (onDrop != null)
            {
                onDrop();
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (onEndDrag != null)
            {
                onEndDrag(eventData.delta.x,eventData.delta.y);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (onClick != null)
            {
                onClick();
            }

            if (onDoubleClick != null && eventData.clickCount == 2)
            {
                onDoubleClick();
            }
        }

		private void OnDisable()
		{
            onBeginDrag = null;
            onDrag = null;
            onEndDrag = null;
            onClick = null;
            onDrop = null;
		    onDoubleClick = null;
		}

	}
}

