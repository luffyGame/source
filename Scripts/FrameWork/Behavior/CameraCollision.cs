using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public class CameraCollision : MonoBehaviour
    {
        private static RaycastHit[] buffers = new RaycastHit[10];
        public float pushSpeed = 4f;
        public float normalDist = 6f;
        public int hitLayerMask;
        public CameraFollow follow;
        private Transform trans;
        public float targetDist;
        public Transform Trans { get { if (null == trans) trans = transform; return trans; } }
        void LateUpdate()
        {
            if (null == follow||!follow.IsValidated)
                return;
            CheckHit();
            follow.followDist = Mathf.Lerp(follow.followDist, targetDist, Time.deltaTime * pushSpeed);
        }

        private void CheckHit()
        {
            Vector3 origin = follow.targetPos;
            int hitNum = Physics.RaycastNonAlloc(origin, -Trans.forward, buffers, normalDist, hitLayerMask);
            if (hitNum > 0)
            {
                float minDist = normalDist;
                for (int i = 0; i < hitNum; ++i)
                {
                    float dist = Vector3.Distance(origin, buffers[i].point);
                    if (minDist > dist)
                        minDist = dist;
                }
                targetDist = minDist;
            }
            else
                targetDist = normalDist;
            Vector3 cameraPos = origin - Trans.forward * targetDist;
            Debug.DrawLine(origin, cameraPos, Color.red);
        }
    }
}
