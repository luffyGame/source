using UnityEngine;

namespace Game
{
    [RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
    public class DlgBehavior : MonoBehaviour
    {
        #region Var
        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;
        private GameObject gameObj;
        
        public GameObject GameObj
        {
            get
            {
                return (this.gameObj ?? (this.gameObj = base.gameObject));
            }
        }
        public CanvasGroup CanvasGroup
        {
            get
            {
                return (this.canvasGroup ?? (this.canvasGroup = GetComponent<CanvasGroup>()));
            }
        }
        public RectTransform RectTransform
        {
            get
            {
                return (rectTransform ?? (rectTransform = GetComponent<RectTransform>()));
            }
        }
        public float Alpha
        {
            get
            {
                return this.CanvasGroup.alpha;
            }
            set
            {
                this.CanvasGroup.alpha = value;
            }
        }
        public bool Interactable
        {
            get
            {
                return this.CanvasGroup.interactable;
            }
            set
            {
                this.CanvasGroup.interactable = value;
            }
        }
        #endregion
    }
}