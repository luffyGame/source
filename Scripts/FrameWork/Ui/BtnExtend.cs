using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FrameWork.Ui
{
    [RequireComponent(typeof(Button))]
    public class BtnExtend : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private BtnTarget[] _btnTargets;
        [SerializeField] private Button _button;

        private bool isInteractable;

        public bool IsInteractable
        {
            set
            {
                isInteractable = value;
                _button.interactable = isInteractable;
                if (isInteractable == true)
                {
                    foreach (var v in _btnTargets)
                    {
                        v.OnNormal();
                    }   
                }
                else
                {
                    foreach (var v in _btnTargets)
                    {
                        v.OnDisable();
                    }   
                }
            }
            get { return isInteractable; }
        }



#if UNITY_EDITOR
        private void Reset()
        {
            _button = GetComponent<Button>();
            foreach (var v in _btnTargets)
            {
                v.Init();
            }
        }
#endif

        private void Start()
        {
            isInteractable = _button.IsInteractable();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isInteractable)
            {
                foreach (var v in _btnTargets)
                {
                    v.OnPress();
                }    
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (isInteractable)
            {
                foreach (var v in _btnTargets)
                {
                    v.OnNormal();
                }    
            }
        }
    }

    [System.Serializable]
    public class BtnTarget
    {
        public Graphic targetGraphic;

        public enum Transtion
        {
            None,
            ColorTint,
        }

        public Transtion transtion = Transtion.None;
        public Color initColor;
        public Color pressColor;
        public Color disableColor;

        public void Init()
        {
            initColor = targetGraphic.color;
        }

        public void OnPress()
        {
            targetGraphic.color = pressColor;
        }

        public void OnDisable()
        {
            targetGraphic.color = disableColor;
        }

        public void OnNormal()
        {
            targetGraphic.color = initColor;
        }
    }
}