using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork.FSM
{
    public abstract class FsmState<T,U>
    {
        public U self { get; set; }
        //状态
        public abstract T state { get; }
        //是否能被打断
        public virtual bool Interruptable { get { return false; } }
        //是否结束，大多状态都会结束
        public bool IsOver { get; set; }
        public virtual void OnEnter() { }

        public virtual void OnUpdate(){}
        public virtual void OnExit() {}
    }

    public abstract class StateMachine<T, U>
    {
        public U self;
        public FsmState<T,U> CurrentState { get; private set; }
        public FsmState<T,U> LastState { get; private set; }
        private Dictionary<T, FsmState<T,U> > stateLookup = new Dictionary<T, FsmState<T,U>>();
        public abstract FsmState<T,U> CreateState(T state);
        public abstract bool Equal(T a, T b);
        public bool ChangeState(T newState,bool forced = false)
        {
            if (null != CurrentState)
            {
                if (!forced&&Equal(CurrentState.state,newState))
                    return false;
                CurrentState.OnExit();
            }
            LastState = CurrentState;
            CurrentState = GetState(newState);
            if (null != CurrentState)
            {
                CurrentState.OnEnter();
                return true;
            }
            return false;
        }
        public void OnUpdate()
        {
            if (null != CurrentState)
                CurrentState.OnUpdate();
        }

        private FsmState<T,U> GetState(T state)
        {
            FsmState<T, U> fsmstate = null;
            if (!stateLookup.ContainsKey(state))
            {
                fsmstate = CreateState(state);
                fsmstate.self = self;
                stateLookup.Add(state, fsmstate);
            }
            else
                fsmstate = stateLookup[state];
            return fsmstate;
        }
    }
}
