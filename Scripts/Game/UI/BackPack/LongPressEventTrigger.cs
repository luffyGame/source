using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game
{
    public class LongPressEventTrigger : UIBehaviour, IPointerDownHandler, IPointerUpHandler,IBeginDragHandler
    {

        //触发事件的时间
        public float durationOnPress = 0.5f;
        private float timePress;

        private bool timeChecked = false;
        private bool longPressed = false;

        public UnityEvent onLongPress = new UnityEvent();
        public UnityEvent onLongPressCancel = new UnityEvent();
        void Update()
        {
            if (timeChecked)
            {
                timePress += Time.deltaTime;
                if (timePress >= durationOnPress)
                {
                    longPressed = true;
                    onLongPress.Invoke();
                    timeChecked = false;
                }
            }
        }



        public void OnPointerDown(PointerEventData eventData)
        {
            timePress = 0f;
            timeChecked = true;
            longPressed = false;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            timeChecked = false;
            if (longPressed)
            {
                longPressed = false;
                onLongPressCancel.Invoke();
            }
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            timeChecked = false;
            if (longPressed)
            {
                longPressed = false;
                onLongPressCancel.Invoke();
            }
        }
    }
}
