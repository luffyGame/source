using Game;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public abstract class SimpleListViewItem<TData> : MonoBehaviour
    {
        public int Index { get; set; }
        [HideInInspector]
        [SerializeField]
        private TData data;
        public TData Data {
            get { return data; }
            set { data = value; UpdateShow(); }
        }
        protected virtual void UpdateShow() { }
        public virtual void OnClick() { }
    }
    public class SimpleListView<TItem, TData> : MonoBehaviour where TItem : SimpleListViewItem<TData>
    {
        #region Vars
        public int count;
        public TItem template;//显示项模板
        protected List<TItem> items = new List<TItem>();
        #endregion
        #region Public Method
        public void Init()
        {
            this.UpdateItemsCount();
        }
        public void SetData(int index, TData item)
        {
            if (index < items.Count)
                items[index].Data = item;
        }
		public TData GetData(int index)
		{
			if (index >= 0 && index < items.Count)
				return items [index].Data;
			return default(TData);
		}
		public TItem GetItem(int index)
		{
			if (index >= 0 && index < items.Count)
				return items [index];
			return default(TItem);
		}
        public TItem DynamicAddItem()
        {
            this.count += 1;
            return this.AddItem();
        }
        public void DynamicRemoveItem(TItem item)
        {

            for (int i = item.Index; i < this.count; ++i)
                this.items[i].Index -= 1;
            this.count -= 1;
            this.RemoveItem(item);
        }
        public void ClearItems()
        {
            count = 0;
            UpdateItemsCount();
        }
        #endregion
        #region Private Method
        private TItem AddItem()
        {
            TItem item = CachedClone.Clone(template.gameObject).GetComponent<TItem>();
            item.Index = items.Count;
            item.transform.SetAsLastSibling();
            items.Add(item);
            OnItemAdd(item);
            return item;
        }
        private void RemoveItem(TItem item)
        {
            item.Index = -1;
            CachedClone.RemoveClone(item.gameObject);
            items.Remove(item);
            OnItemRemove(item);
        }
        private void UpdateItemsCount()
        {
            if (items.Count == count)
            {
                return;
            }
            if (items.Count < count)
            {
                for (int i = items.Count; i < count; ++i)
                    AddItem();
            }
            else
            {
                for (int i = items.Count-1; i >=count ; --i)
                    RemoveItem(items[i]);
            }
        }
        protected virtual void OnItemAdd(TItem item){}
        protected virtual void OnItemRemove(TItem item) { }
        #endregion
    }
}
