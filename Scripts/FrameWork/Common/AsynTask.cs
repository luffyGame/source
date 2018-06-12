using System;
using System.Collections.Generic;
using System.Text;

namespace FrameWork
{
    public delegate void AsynTaskDelegate(AsynTask asynTask);
    public abstract class AsynTask
    {
        #region Variables
        private AsynTaskDelegate m_callbackHandler = null;
		public AsynTaskDelegate CallBackHandler { set {m_callbackHandler = value;}}
        #endregion
        public virtual void Execute() { Callback(); }
        private void Callback()
        {
            if (null != m_callbackHandler)
                m_callbackHandler(this);
        }
    }
	public abstract class UnityCbTask : AsynTask
	{
		public abstract void UpdateExec();
	}
}
