using System;
using System.Collections.Generic;

namespace FrameWork.MVVM
{
    public class BindableProperty<T>
    {
        public delegate void ValueChangeHandler(T lastValue, T newValue);
        public ValueChangeHandler onValueChanged;
        private T _value;
        public T Value
        {
            get { return _value; }
            set
            {
                if(!Equals(_value,value))
                {
                    T last = _value;
                    _value = value;
                    ValueChanged(last, _value);
                }
            }
        }

        private void ValueChanged(T lastValue,T newValue)
        {
            if (null != onValueChanged)
                onValueChanged(lastValue, newValue);
        }
        public override string ToString()
        {
            return (Value != null ? Value.ToString() : null);
        }
        public override int GetHashCode()
        {
            return Value != null ? Value.GetHashCode():0;
        }
    }
}
