using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork.BehaviorTree
{
    // How to use:
    // 1. Initiate values in the database for the children to use.
    // 2. Initiate BT _root
    // 3. Some actions & preconditions that will be used later
    // 4. Add children nodes
    // 5. Activate the _root, including the children nodes' initialization
    public abstract class BTTree : MonoBehaviour
    {
        private static List<BTTree> trees = new List<BTTree>();
        private int frameCount;
        private int framePeriod;
        public static void RegisterTree(BTTree tree)
        {
            if (!trees.Contains(tree))
                trees.Add(tree);
        }
        public static void RemoveTree(BTTree tree)
        {
            int index = trees.IndexOf(tree);
            if (index >= 0)
            {
                trees.RemoveAt(index);
            }
        }
        public static void Update()
        {
            for (int i = 0; i < trees.Count;++i )
            {
                trees[i].DoUpdate();
            }
        }

        protected BTNode _root = null;

        [HideInInspector]
        public BTDatabase database;
        [HideInInspector]
        public bool isRunning;
        public const string RESET = "Rest";
        private static int _resetId;
        void Awake()
        {
            Init();
        }
        public void DoUpdate()
        {
            if (!isRunning) return;
            //if(++frameCount>=framePeriod)
            {
//                 frameCount = 0;
//                 if (database.GetData<bool>(RESET))
//                 {
//                     Reset();
//                     database.SetData<bool>(RESET, false);
//                 }

                // Iterate the BT tree now!
                if (_root.Evaluate())
                {
                    _root.Tick();
                }
            }
        }

        void OnDestroy()
        {
            if (_root != null)
            {
                _root.Clear();
            }
        }

        protected virtual void Init()
        {
            database = GetComponent<BTDatabase>();
            if (database == null)
            {
                database = gameObject.AddComponent<BTDatabase>();
            }
        }

        protected void Reset()
        {
            if (_root != null)
            {
                _root.Clear();
            }
        }
        public void Run()
        {
            _resetId = database.GetDataId(RESET);
            database.SetData<bool>(_resetId, false);

            _root.Activate(database);
            isRunning = true;
            frameCount = 0;
        }
        public void Stop()
        {
            isRunning = false;
            if (_root != null)
            {
                _root.Clear();
            }
            database.Clear();
            _root.Deactive();
        }
        public void BeLazy()
        {
            framePeriod = BTConfiguration.LAZY_UPDATE_FRAME;
            frameCount = 0;
        }
        public void BeSmart()
        {
            framePeriod = BTConfiguration.SMART_UPDATE_FRAME;
            frameCount = 0;
        }
    }
}
