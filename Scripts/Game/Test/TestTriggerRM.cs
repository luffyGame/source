using System.Collections.Generic;
using UnityEngine;

namespace Game.Test
{
    public class TestTriggerRM : MonoBehaviour
    {
        public TestTriggerRadius[] triggers;
        public Transform[] poss;
        public float[] rs;
        public float period;
        private bool isplaying;

        private void Update()
        {
            if(!isplaying)
                return;
            for (int i = 0; i < triggers.Length; ++i)
            {
                if (triggers[i].CheckPeriod(Time.deltaTime))
                {
                    DoTrigger(triggers[i]);
                }
            }
        }
        private void Start()
        {
            isplaying = true;
            enabled = true;
        }

        private void DoTrigger(TestTriggerRadius trigger)
        {
            int index = Random.Range(0, poss.Length);
            Vector3 pos = poss[index].position;
            float r = rs[index];
            trigger.StartCheck(pos,r);
            //isplaying = false;
        }
    }
}