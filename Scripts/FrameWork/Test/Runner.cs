using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace FrameWork
{
    public class Runner : MonoBehaviour
    {
        private Transform cachedTrans;
        public Terrain terrain;
        public float speed;
        public UnityEvent init;
        private CameraFollow follow;
        void Awake()
        {
            cachedTrans = transform;
        }
        void Start()
        {
            GameObject cameraGo = Camera.main.gameObject;
            follow = cameraGo.GetComponent<CameraFollow>();
            if (null == follow)
                follow = cameraGo.AddComponent<CameraFollow>();
            follow.followTarget = cachedTrans;
            if (null != init)
                init.Invoke();
        }
        public void Move(Vector3 dir)
        {
            float cameraYRot = follow.yRot;
            Vector3 finalDir = Quaternion.Euler(0, cameraYRot, 0) * dir;
            Vector3 newPos = cachedTrans.position + finalDir * speed * Time.deltaTime;
            newPos.y = terrain.SampleHeight(newPos) + terrain.GetPosition().y;
            cachedTrans.position = newPos;
        }
    }
}
