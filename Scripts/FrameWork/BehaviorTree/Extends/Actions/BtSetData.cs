using System;
using System.Collections.Generic;

namespace FrameWork.BehaviorTree
{
    public class BtSetData<T> : BTAction
    {
        protected string _dataToSet;
        protected int _dataIdToSet;
        private T _data;
        public BtSetData(string dataToSet, T data)
        {
            _dataToSet = dataToSet;
            _data = data;
        }

        public override void Activate(BTDatabase database)
        {
            base.Activate(database);
            _dataIdToSet = database.GetDataId(_dataToSet);
        }

        protected override BTResult Execute()
        {
            database.SetData<T>(_dataIdToSet, _data);
            return BTResult.Ended;
        }

    }

    public class BtSetAndClearData<T> : BtSetData<T>
    {
        private T _dataClear;
        public BtSetAndClearData(string dataToSet, T data, T dataClear)
            : base(dataToSet, data)
        {
            _dataClear = dataClear;
        }
        public override void Clear()
        {
            base.Clear();
            database.SetData<T>(_dataIdToSet, _dataClear);
        }
    }
}
