using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace FrameWork
{
    public class SingleToggle : MonoBehaviour
    {
        public Object active;
        public Object inactive;

        public void Turn(bool on)
        {
            Turn(active,on);
            Turn(inactive,!on);
        }

        private void Turn(Object o, bool on)
        {
            if (null != o)
            {
                GameObject go = o as GameObject;
                if(null!=go)
                    go.SetActive(on);
                else
                {
                    Behaviour beh = o as Behaviour;
                    if (null != beh)
                        beh.enabled = on;
                }
            }
        }
        [ContextMenu("on")]
        public void On()
        {
            Turn(true);
        }
        [ContextMenu("off")]
        public void Off()
        {
            Turn(false);
        }
    }
}