using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public class ListViewSizeKeeper : MonoBehaviour
    {
        static readonly List<Vector2> groupPositions = new List<Vector2>{
			new Vector2(0.0f, 1.0f),//Anchors.UpperLeft
			new Vector2(0.0f, 0.0f),//Anchors.LowerLeft
		};
        //上下左右各个方向的延伸
        public float PaddingLeft { get; set; }
        public float PaddingRight { get; set; }
        public float PaddingTop { get; set; }
        public float PaddingBottom { get; set; }

        public int GroupPosition = 0;//UpperLeft = 0,LowerLeft = 1,
        public bool isHorizontal = false;
        public Vector2 spacing;//间距

        private RectTransform rectT;
        public RectTransform RectT { get { if (null == rectT)rectT = transform as RectTransform; return rectT; } }
        #region Behavior Method
        void Awake()
        {
        }
        #endregion
        #region Public Method
        public void SetPadding(float first, float last)
        {
            if(isHorizontal)
            {
                PaddingLeft = first;
                PaddingRight = last;
            }
            else
            {
                PaddingTop = first;
                PaddingBottom = last;
            }
        }
        public void UpdateSizeAndElements()
        {
            var elements = GetElements();
            Vector2 size = CalculateSize(elements);
            RectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x);
            RectT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y);

            var anchor_position = groupPositions[(int)GroupPosition];
            var start_position = new Vector2(
                RectT.rect.width * (anchor_position.x - RectT.pivot.x),
                RectT.rect.height * (anchor_position.y - RectT.pivot.y)
            );

            start_position.x -= anchor_position.x * size.x;
            start_position.y += (1 - anchor_position.y) * size.y;

            start_position.x += PaddingLeft;
            start_position.y -= PaddingTop;

            SetPositions(elements, start_position);
        }
        public float GetSpacing()
        {
            return isHorizontal ? spacing.x : spacing.y;
        }
        #endregion
        #region Private Method
        private List<ListViewItem> GetElements()
        {
            var elements = new List<ListViewItem>();
            for(int i=0,imax =RectT.childCount;i<imax;++i)
            {
                Transform e = RectT.GetChild(i);
                if (e.gameObject.activeSelf)
                {
                    ListViewItem item = e.GetComponent<ListViewItem>();
                    if (null != item)
                        elements.Add(item);
                }
            }
            return elements;
        }

        private Vector2 CalculateSize(List<ListViewItem> elements)
        {
            float width = 0f;
            float height = 0f;
            int count = elements.Count;
            for(int i=0;i<count;++i)
            {
                ListViewItem e = elements[i];
                Vector2 eSize = e.Size;
                if(isHorizontal)
                {
                    width += eSize.x;//保证本身不使用缩放
                    height = Mathf.Max(eSize.y, height);
                }
                else
                {
                    width = Mathf.Max(eSize.x, width);
                    height += eSize.y;
                }
            }
            if (isHorizontal)
                width += spacing.x * (count - 1);
            else
                height += spacing.y * (count - 1);
            width += (PaddingLeft + PaddingRight);
            height += (PaddingTop + PaddingBottom);
            return new Vector2(width, height);
        }
        private void SetPositions(List<ListViewItem> elements,Vector2 startPos)
        {
            Vector2 pos = startPos;
            for (int i = 0; i < elements.Count;++i )
            {
                ListViewItem e = elements[i];
                e.RectTransform.localPosition = pos;
                if (isHorizontal)
                    pos.x += e.Size.x + spacing.x;
                else
                    pos.y -= e.Size.y+ spacing.y;
            }
        }
        #endregion
    }
}
