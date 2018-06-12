using System;
using System.Collections.Generic;

namespace FrameWork.BehaviorTree
{
    public abstract class BTPrecondition : BTNode
    {
        public BTPrecondition() : base(null) { }
        public abstract bool Check();
        public override BTResult Tick()
        {
            bool success = Check();
            if (success)
            {
                return BTResult.Ended;
            }
            else
            {
                return BTResult.Running;
            }
        }
    }

    public abstract class BTPreconditionUseDB : BTPrecondition
    {
        protected string _dataToCheck;
        protected int _dataIdToCheck;


        public BTPreconditionUseDB(string dataToCheck)
        {
            this._dataToCheck = dataToCheck;
        }

        public override void Activate(BTDatabase database)
        {
            base.Activate(database);

            _dataIdToCheck = database.GetDataId(_dataToCheck);
        }
    }
}
