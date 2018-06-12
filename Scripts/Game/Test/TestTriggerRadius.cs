using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Test
{
    public class TestTriggerRadius : MonoBehaviour
    {
        public List<Collider> targets = new List<Collider>();
        public SphereCollider collider;
        public Action onCheckDone;
        private bool isChecking;
        private int fixCount = 0;
        private float nextTime;
        private void OnTriggerEnter(Collider other)
        {
            //Debug.Log(other.name + " enter");
            targets.Add(other);
        }
        public void StartCheck(Vector3 pos,float radius)
        {
            enabled = true;
            isChecking = true;
            transform.position = pos;
            collider.radius = radius;
            collider.isTrigger = true;
            //gameObject.SetActive(true);
            targets.Clear();
            fixCount = 0;
            //StartCoroutine(CheckStart());
        }

        public bool CheckPeriod(float deltaTime)
        {
            if (isChecking)
                return false;
            nextTime -= deltaTime;
            if (nextTime <= 0)
            {
                nextTime = UnityEngine.Random.Range(0.1f, 0.3f);
                return true;
            }

            return false;
        }
        
        private void FixedUpdate()
        {
            if (isChecking)
            {
                fixCount++;
                if(fixCount == 2)
                    FinishCheck();
            }
        }
        private IEnumerator CheckStart()
        {
            yield return new WaitForFixedUpdate();
            FinishCheck();
        }

        private void FinishCheck()
        {
            Debug.Log("finish " + targets.Count);
            isChecking = false;
            collider.isTrigger = false;
            enabled = false;
            if (null != onCheckDone)
                onCheckDone();
            //gameObject.SetActive(false);
        }
    }
}