using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FrameWork
{
    public class PressScale : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
    {
        public Vector3 scale=new Vector3(0.8f,0.8f,0.8f);
        private Transform trans;

        public Transform Trans
        {
            get
            {
                if (null == trans) trans = transform;
                    return trans;
            }
        }

        private Vector3 initScale;
        private Vector3 desScale;
        private void Start()
        {
            initScale = Trans.localScale;
            var tempScale = Trans.localScale;
            tempScale.x *= scale.x;
            tempScale.y *= scale.y;
            tempScale.z *= scale.z;
            desScale = tempScale;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Trans.DOScale(desScale, 0.1f).Play();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Trans.DOScale(initScale, 0.1f).Play();
        }
    }
}