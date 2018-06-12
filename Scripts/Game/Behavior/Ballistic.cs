using System;
using UnityEngine;

namespace Game
{
    /// <summary>
    /// 弹道飞行的，
    /// </summary>
    public class Ballistic : MonoBehaviour
    {
        public float length = 0.2f;//自身长度
        private Vector3 dir;
        private float speed;
        private float dist;
        private Transform trans;
        private float flyDist;
        public Action onMoveToEnd;

        private Transform Trans
        {
            get
            {
                if (null == trans) trans = transform;
                return trans;
            }
        }

        public void Play(Vector3 toPos,float speed)
        {
            dir = toPos-Trans.position;
            dist = dir.magnitude;
            dir.Normalize();
            Trans.forward = dir;
            this.speed = speed;
            flyDist = 0f;
            enabled = true;
        }

        private void Update()
        {
            float mov = speed * Time.deltaTime;
            flyDist += mov;
            Trans.Translate(dir*mov,Space.World);
            CheckMoveToEnd();
        }

        private void CheckMoveToEnd()
        {
            if (flyDist >= dist)
            {
                enabled = false;
                if (null != onMoveToEnd)
                    onMoveToEnd();
            }
        }
    }
}