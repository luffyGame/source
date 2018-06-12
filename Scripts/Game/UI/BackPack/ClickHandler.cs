using UnityEngine;
using UnityEngine.EventSystems;
using System;
namespace Game
{
    public class ClickHandler : MonoBehaviour, IPointerClickHandler
    {
        public Action onClick;
        public void OnPointerClick(PointerEventData eventData)
        {
            if (onClick != null)
            {
                onClick();
            }
        }
    }
}

