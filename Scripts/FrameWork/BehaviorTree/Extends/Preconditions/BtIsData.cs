using System;
using System.Collections.Generic;

namespace FrameWork.BehaviorTree
{
    public class BtIsData<T> : BTPrecondition
    {
        private string _dataToCheck;
        private int _dataIdToCheck;
        private T _defaultValue;
        private Func<T,bool> checkFunc;

        public BtIsData(string dataToCheck, T defaultValue, Func<T, bool> checkFunc)
        {
            _dataToCheck = dataToCheck;
            _defaultValue = defaultValue;
            this.checkFunc = checkFunc;
        }
        public override void Activate(BTDatabase database)
        {
            base.Activate(database);
            _dataIdToCheck = database.GetDataId(_dataToCheck);
            database.SetData<T>(_dataIdToCheck, _defaultValue);
        }

        public override bool Check()
        {
            return checkFunc(database.GetData<T>(_dataIdToCheck));
        }
    }
}
