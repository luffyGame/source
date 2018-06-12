using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using UnityEngine;

namespace FrameWork
{
    //Warning,In order to work though, you still have to derive a non generic class from this one:
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> _keys = new List<TKey>();
        [SerializeField]
        private List<TValue> _values = new List<TValue>();
        public List<TKey> _Keys
        {
            get { return _keys; }
        }

        public List<TValue> _Values
        {
            get { return _values; }
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

        public void OnAfterDeserialize()
        {
            Clear();
            int count = Math.Min(_keys.Count, _values.Count);
            for(int i = 0; i < count; ++i)
            {
                this.Add(_keys[i], _values[i]);
            }
        }

        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();
            _keys.Capacity = Count;
            _values.Capacity = Count;
            foreach(var kvp in this)
            {
                _keys.Add(kvp.Key);
                _values.Add(kvp.Value);
            }
        }

        public override void OnDeserialization(object sender)
        {
            base.OnDeserialization(sender);
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("count:{0}\n", Count);
            foreach(var kvp in this)
            {
                sb.AppendFormat("{0}:{1}\n", kvp.Key, kvp.Value);
            }
            return sb.ToString();
        }
    }
}
