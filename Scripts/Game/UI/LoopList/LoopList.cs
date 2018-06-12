using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Events;
using FrameWork;

namespace Game{
    [RequireComponent(typeof(ScrollRect))]
    public class LoopList : MonoBehaviour
    {
        #region Evenets
        public Action<int, LuaItem> onSetValue;
        public Action<int> onClick;
        #endregion

        #region Var
        public int dataCount;
        int startIndex = 0;

        [SerializeField]
        private RectTransform viewRect;
        [SerializeField]
        private RectTransform contentRect;
        private ScrollRect scroll;

        [SerializeField]
        private LuaItem template;
        protected List<LuaItem> items = new List<LuaItem>();
        LayoutGroup layoutGroup;
        RectOffset originPadding;
        //只能是水平或者垂直滚动
        bool isHorizontal;

        bool firstEnter = true;
        #endregion

        #region Public
        public void Init(int count)
        {
            this.dataCount = count;
            if(!firstEnter){
                this.UpdateView();    
            }
        }
        public void SetItem(int index, object item)
        {

        }
        public object GetItem(int index)
        {
            return default(object);
        }
        #endregion

        #region Private
        //每个Item的空间
        private int itemSpace;
        //可视区域内Item的数量
        private int visableItemCount;

        /// <summary>
        /// 只有grid layout时候才会用到，其他layout都是1
        /// </summary>
        int ConstraintCount
        {
            get
            {
                if (layoutGroup is GridLayoutGroup)
                {
                    return (layoutGroup as GridLayoutGroup).constraintCount;
                }
                return 1;
            }
        }
        //视口大小
        float ViewSize
        {
            get
            {
                return isHorizontal ? viewRect.rect.width : viewRect.rect.height;
            }
        }
        //内容大小
        float ContentSize
        {
            get
            {
                return isHorizontal ? contentRect.rect.width : contentRect.rect.height;
            }
        }
        //缓存数量
        int CacheCount
        {
            get
            {
                return ConstraintCount + dataCount % ConstraintCount;
            }
        }

        int CacheUnitCount
        {
            get
            {
                return Mathf.CeilToInt((float)CacheCount / ConstraintCount);
            }
        }

        int DataUnitCount
        {
            get
            {
                return Mathf.CeilToInt((float)dataCount / ConstraintCount);
            }
        }


        #endregion



        #region UnityEvents
        void Awake()
        {
            scroll = this.GetComponent<ScrollRect>();
            if (scroll.horizontal && scroll.vertical)
            {
                Debug.LogError("can not horizontal and vertical");
                return;
            }
            layoutGroup = layoutGroup ?? this.GetComponentInChildren<LayoutGroup>();
            originPadding = layoutGroup.padding;
            if (viewRect == null)
            {
                viewRect = this.transform as RectTransform;
            }
            if (contentRect == null)
            {
                contentRect = layoutGroup.transform as RectTransform;
            }
            this.isHorizontal = scroll.horizontal;

            var gridLayout = layoutGroup as GridLayoutGroup;
            if (gridLayout != null)
            {
                itemSpace = (int)(isHorizontal ? gridLayout.cellSize.x + gridLayout.spacing.x : gridLayout.cellSize.y + gridLayout.spacing.y);
            }
            else
            {
                var layoutElement = template.GetComponent<LayoutElement>();
                itemSpace = (int)(isHorizontal ? layoutElement.preferredWidth : layoutElement.preferredHeight + (layoutGroup as HorizontalOrVerticalLayoutGroup).spacing);
            }

        }

		private void Start()
		{
            visableItemCount = Mathf.CeilToInt(ViewSize / itemSpace);
            if(firstEnter){
                UpdateView();
                firstEnter = false;
            }
		}

		void OnEnable()
        {
            scroll.onValueChanged.AddListener(OnValueChanged);
        }

        void OnDisable()
        {
            scroll.onValueChanged.RemoveListener(OnValueChanged);
        }

        LuaItem AddItem()
        {
            var item = CachedClone.Clone(template.gameObject).GetComponent<LuaItem>();
            item.Index = items.Count;
            item.transform.SetAsLastSibling();
            items.Add(item);
            var clickHandler = item.GetComponent<ClickHandler>();
            if(clickHandler != null){
                clickHandler.onClick = () => { onClick(item.Index); };
            }

            OnItemAdd(items.Count, item);
            return item;
        }
        void RemoveItem(int index)
        {
            Destroy(items[index].gameObject);
            items.RemoveAt(index);
            OnItemRemove(index);
        }

        public void ResetList(int dataCount)
        {
            Debug.Log(items.Count);
            scroll.content.anchoredPosition = Vector3.zero;
            for (int i = 0; i < items.Count; i++)
            {
                Destroy(items[i].gameObject);
                OnItemRemove(i);
            }
            items.Clear();

            this.dataCount = dataCount;
            startIndex = 0;
            UpdateView();
        }
        
        void UpdateView()
        {
            startIndex = Mathf.Max(0, Mathf.Min(startIndex / ConstraintCount, DataUnitCount - visableItemCount - CacheUnitCount)) * ConstraintCount;
            var frontSpace = startIndex / ConstraintCount * itemSpace;
            var behindSpace = Mathf.Max(0, itemSpace * (DataUnitCount - CacheUnitCount) - frontSpace - (itemSpace * visableItemCount));
            if (isHorizontal)
            {
                layoutGroup.padding = new RectOffset(frontSpace, behindSpace, originPadding.top, originPadding.bottom);
            }
            else
            {
                layoutGroup.padding = new RectOffset(originPadding.left, originPadding.right, frontSpace, behindSpace);
            }

            int viewCount = Mathf.Min(dataCount, visableItemCount * ConstraintCount + CacheCount);
            if (items.Count < viewCount)
            {
                for (int i = items.Count; i < viewCount; i++)
                {
                    AddItem();
                }
            }
            else if (items.Count > viewCount)
            {
                for (int i = items.Count - 1; i >= viewCount; i--)
                {
                    RemoveItem(i);
                }
            }
            for (int i = 0; i < items.Count; i++)
            {
                if(onSetValue != null){
                    items[i].Index = startIndex + i;
                    onSetValue(startIndex + i, items[i]);
                }
            }
        }

        void OnValueChanged(Vector2 vector)
        {

            var value = (ContentSize - ViewSize) * (isHorizontal ? 1 - vector.x : vector.y);
            var start = ContentSize - value - ViewSize;
            var newStartIndex = Mathf.FloorToInt(start / itemSpace) * ConstraintCount;
            newStartIndex = Mathf.Max(0, newStartIndex);
            if (startIndex != newStartIndex)
            {
                startIndex = newStartIndex;
                UpdateView();
            }

        }
        #endregion

        protected virtual void OnItemAdd(int index, object j) { }
        protected virtual void OnItemRemove(int index) { }

    }
}

