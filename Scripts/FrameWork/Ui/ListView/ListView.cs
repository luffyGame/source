using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork
{
    /// <summary>
    /// 注意:单个元素的宽高采用的是最小值，所以显示的数目肯定>=真正需要显示的数目的
    /// 这里的可见是针对最小尺寸的，所以生成的组件是足够渲染所有可见区域的元素的
    /// </summary>
    /// <typeparam name="TComponent"></typeparam>
    /// <typeparam name="TItem"></typeparam>
    public class ListView<TComponent,TItem> : MonoBehaviour where TComponent : ListViewItem
    {
        #region Vars
        public TComponent templateViewItem;//显示项模板
        protected List<TItem> data = new List<TItem>();
        //
        protected List<TComponent> components = new List<TComponent>();

        protected float scrollHeight;
        protected float scrollWidth;
        [Tooltip("Minimal height of item")]
        protected float itemHeight;
        [Tooltip("Minimal width of item")]
        protected float itemWidth;
        //最大可见数目
        protected int maxVisibleItems;
        //当前可见数目
        protected int visibleItems;
        //上方不可见数目
        protected int topHiddenItems;
        //下方不可见数目
        protected int bottomHiddenItems;
        [System.NonSerialized]
        private bool isInited = false;
        public ScrollRect scrollRect;
        //容器
        public RectTransform container;
        //
        private ListViewSizeKeeper sizeKeeper;
        public bool IsHorizontal { get { return sizeKeeper.isHorizontal; } }
        #endregion
        #region Behavior Method
        void Awake()
        {
            Init();
        }
        protected void Init()
        {
            if (isInited)
                return;
            isInited = true;
            if (templateViewItem == null)
            {
                throw new NullReferenceException(String.Format("templateViewItem is null. Set component of type {0} to templateViewItem.", typeof(TComponent).FullName));
            }
            if (null != scrollRect)
            {
                scrollRect.onValueChanged.AddListener(OnScrollRectUpdate);
            }
            sizeKeeper = container.GetComponent<ListViewSizeKeeper>();
            if (CanOptimize())
            {
                var scroll_rect_transform = scrollRect.transform as RectTransform;
                scrollHeight = scroll_rect_transform.rect.height;
                scrollWidth = scroll_rect_transform.rect.width;
                CalculateItemSize();
                CalculateMaxVisibleItems();
            }
            templateViewItem.gameObject.SetActive(false);
        }
        #endregion
        #region Public Method
        public void ForEachComponent(Action<ListViewItem> func)
        {
            for (int i = 0; i < components.Count; ++i)
                func(components[i]);
        }
        public void ScrollTo(int index)
        {
            if (!CanOptimize())
            {
                return;
            }
            var first_visible = GetFirstVisibleIndex(true);
            var last_visible = GetLastVisibleIndex(true);

            if (first_visible > index)
            {
                SetScrollValue(GetItemPosition(index));
            }
            else if (last_visible < index)
            {
                SetScrollValue(GetItemPositionBottom(index));
            }
        }
        public int GetNearestItemIndex()
        {
            return Mathf.Clamp(Mathf.RoundToInt(GetScrollValue() / GetItemSize()), 0, data.Count - 1);
        }
        public int Add(TItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item", "Item is null.");
            }

            data.Add(item);
            OnItemChanges();
            return data.Count-1;
        }
        public void Add(List<TItem> items)
        {
            for (int i = 0; i < items.Count; ++i)
                data.Add(items[i]);
            OnItemChanges();
        }
        public int Remove(TItem item)
        {
            var index = data.IndexOf(item);
            if (index == -1)
            {
                return index;
            }

            data.RemoveAt(index);
            OnItemChanges();
            return index;
        }
        public void Remove(int index)
        {
            data.RemoveAt(index);
            OnItemChanges();
        }
        #endregion
        #region Self Method
        private bool CanOptimize()
        {
            return scrollRect != null;
        }
        protected void OnScrollRectUpdate(Vector2 position)
        {
            ScrollUpdate();
        }
        protected virtual void SetData(TComponent component, TItem item)
        {
        }

        private Vector2 GetItemMinimalSize()
        {
            RectTransform rectT = templateViewItem.transform as RectTransform;
            return new Vector2(rectT.rect.width, rectT.rect.height);
        }
        private float GetItemSize()
        {
            return IsHorizontal? (itemWidth+sizeKeeper.spacing.x):(itemHeight+sizeKeeper.spacing.y);
        }
        private void CalculateItemSize()
        {
            var size = GetItemMinimalSize();
            if (itemHeight <= 0f)
            {
                itemHeight = size.y;
            }
            if (itemWidth <= 0f)
            {
                itemWidth = size.x;
            }
        }
        private void CalculateMaxVisibleItems()
        {
            if (IsHorizontal)
            {
                maxVisibleItems = Mathf.CeilToInt(scrollWidth / itemWidth);
            }
            else
            {
                maxVisibleItems = Mathf.CeilToInt(scrollHeight / itemHeight);
            }
            maxVisibleItems = Mathf.Max(maxVisibleItems, 1) + 1;
        }
        private void UpdateView()
        {
            if (CanOptimize() && data.Count > 0)
            {
                visibleItems = (maxVisibleItems < data.Count) ? maxVisibleItems : data.Count;
            }
            else
            {
                visibleItems = data.Count;
            }
            if (CanOptimize())
            {
                topHiddenItems = GetFirstVisibleIndex();
                if (topHiddenItems > data.Count - 1)
                    topHiddenItems = Mathf.Max(0, data.Count - 2);
                if ((topHiddenItems + visibleItems) > data.Count)
                {
                    visibleItems = data.Count - topHiddenItems;
                }
                bottomHiddenItems = Mathf.Max(0, data.Count - visibleItems - topHiddenItems);
            }
            else
            {
                topHiddenItems = 0;
                bottomHiddenItems = data.Count - visibleItems;
            }
            UpdateComponentsCount();
            for(int i=0;i<components.Count;++i)
            {
                TComponent component = components[i];
                component.Index = topHiddenItems + i;
                SetData(component, data[component.Index]);
            }
            KeepSize();
            if (scrollRect != null)
            {
                var item_ends = (data.Count == 0) ? 0f : Mathf.Max(0f, GetItemPositionBottom(data.Count - 1));

                if (GetScrollValue() > item_ends)
                {
                    SetScrollValue(item_ends);
                }
            }
        }
        private int GetFirstVisibleIndex(bool strict = false)
        {
            var first_visible_index = (strict)
                ? Mathf.CeilToInt(GetScrollValue() / GetItemSize())
                : Mathf.FloorToInt(GetScrollValue() / GetItemSize());
            first_visible_index = Mathf.Max(0, first_visible_index);
            if (strict)
            {
                return first_visible_index;
            }

            return Mathf.Min(first_visible_index, Mathf.Max(0, data.Count - visibleItems));
        }
        private int GetLastVisibleIndex(bool strict = false)
        {
            var window = GetScrollValue() + GetScrollSize();
            var last_visible_index = (strict)
                ? Mathf.FloorToInt(window / GetItemSize())
                : Mathf.CeilToInt(window / GetItemSize());

            return last_visible_index - 1;
        }
        public float GetItemPosition(int index)
        {
            return index * GetItemSize() - sizeKeeper.GetSpacing();
        }
        public virtual float GetItemPositionBottom(int index)
        {
            return GetItemPosition(index) + GetItemSize() - GetScrollSize();
        }
        private float GetScrollValue()
        {
            var pos = scrollRect.content.anchoredPosition;
            return Mathf.Max(0f, IsHorizontal ? -pos.x : pos.y);
        }
        private float GetScrollSize()
        {
            return IsHorizontal ? scrollWidth : scrollHeight;
        }
        private void UpdateComponentsCount()
        {
            if (components.Count == visibleItems)
            {
                return;
            }
            if (components.Count < visibleItems)
            {
                for (int i = components.Count; i < visibleItems ; ++i)
                    AddComponent();
            }
            else
            {
                for (int i = visibleItems; i < components.Count; ++i)
                    RemoveComponent(components[i]);
            }
        }
        private void AddComponent()
        {
            TComponent component = CachedClone.Clone(templateViewItem.gameObject).GetComponent<TComponent>();
            component.Index = -1;
            component.transform.SetAsLastSibling();
            components.Add(component);
        }
        private void RemoveComponent(TComponent component)
        {
            component.Index = -1;
            CachedClone.RemoveClone(component.gameObject);
        }
        //获取底部尺寸
        private float CalculateBottomFillerSize()
        {
            if (bottomHiddenItems == 0)
            {
                return 0f;
            }
            return Mathf.Max(0, bottomHiddenItems * GetItemSize());
        }
        private float CalculateTopFillerSize()
        {
            if (topHiddenItems == 0)
            {
                return 0f;
            }
            return Mathf.Max(0, topHiddenItems * GetItemSize());
        }
        private void KeepSize()
        {
            if (null != sizeKeeper)
            {
                float topFillerSize = CalculateTopFillerSize();
                float bottomFillerSize = CalculateBottomFillerSize();
                sizeKeeper.SetPadding(topFillerSize, bottomFillerSize);
                sizeKeeper.UpdateSizeAndElements();
            }
        }
        private void SetScrollValue(float value)
        {
            if (scrollRect.content == null)
                return;
            var current_position = scrollRect.content.anchoredPosition;
            var new_position = IsHorizontal
                ? new Vector2(value, current_position.y)
                : new Vector2(current_position.x, value);
            var diff_x = IsHorizontal ? Mathf.Abs(current_position.x - new_position.x) > 0.1f : false;
            var diff_y = IsHorizontal ? false : Mathf.Abs(current_position.y - new_position.y) > 0.1f;
            if (diff_x || diff_y)
            {
                scrollRect.content.anchoredPosition = new_position;
                ScrollUpdate();
            }
        }

        //不会改变真实显示的数目（未必能看到），仅仅是调整真实显示的项
        private void ScrollUpdate()
        {
            var oldTopHiddenItems = topHiddenItems;
            topHiddenItems = GetFirstVisibleIndex();
            bottomHiddenItems = Mathf.Max(0, data.Count - visibleItems - topHiddenItems);
            if (oldTopHiddenItems == topHiddenItems)
            {
                //do nothing
            }
            // optimization on +-1 item scroll
            else if (oldTopHiddenItems == (topHiddenItems + 1))
            {
                var bottomComponent = components[components.Count - 1];
                components.RemoveAt(components.Count - 1);
                components.Insert(0, bottomComponent);
                bottomComponent.transform.SetAsFirstSibling();

                bottomComponent.Index = topHiddenItems;
                SetData(bottomComponent, data[topHiddenItems]);
            }
            else if (oldTopHiddenItems == (topHiddenItems - 1))
            {
                var topComponent = components[0];
                components.RemoveAt(0);
                components.Add(topComponent);
                topComponent.transform.SetAsLastSibling();

                topComponent.Index = topHiddenItems + visibleItems - 1;
                SetData(topComponent, data[topHiddenItems + visibleItems - 1]);
            }
            else
            {
                List<int> newIdx = new List<int>();
                for (int i = topHiddenItems; i < topHiddenItems + visibleItems; ++i)
                {
                    if (i < oldTopHiddenItems || i >= oldTopHiddenItems + visibleItems)
                        newIdx.Add(i);
                }
                List<TComponent> newChanges = new List<TComponent>();
                int x = 0;
                for (int i = 0; i < components.Count; ++i)
                {
                    TComponent comp = components[i];
                    if(comp.Index<topHiddenItems||comp.Index>=topHiddenItems+visibleItems)//不在新可视区的重新分配
                    {
                        comp.Index = newIdx[x];
                        SetData(comp, data[comp.Index]);
                    }
                }
                for (int i = 0; i < components.Count; ++i)
                    components[i].transform.SetAsLastSibling();
            }
            KeepSize();
        }
        private void OnItemChanges()
        {
            UpdateView();
        }
        #endregion
    }
}
