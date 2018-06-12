using FrameWork;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Item跟随移动界面
/// </summary>
public class DragItemView : MonoBehaviour
{
    public RectTransform root;
    [SerializeField]
    RectTransform rectTransform;
    [SerializeField]
    Image itemImage;

    Vector2 screenPos = Vector2.zero;

    // Update is called once per frame
    void Update()
    {
        if(itemImage.enabled){
            SetRectPos();    
        }
    }

    void SetRectPos()
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(root, Input.mousePosition, Global.Instance.UiCamera, out screenPos))
        {
            rectTransform.anchoredPosition = screenPos;
        }
    }
}
