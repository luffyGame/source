using System;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.AI;

namespace FrameWork
{
    public class NavFollower : MonoBehaviour
    {
        private Transform self;
        public float speed = 5f;
        public float rotateSpeed = 10f;
        public bool useRandomDirection { get; set; }//是否使用随机位置，对于怪物，避免集中在一个点
        private Vector3[] path;
        private int nextWaypoint;
        private Transform transTarget;
        private Vector3 posTarget; //目标位置
        public float arriveRange { get; set; }//距离目标多远停下
        private double elapsedTime;//已经过去的时间
        private int navMask = -1;
        public int NavMask
        {
            set { navMask = value; }
        }

        public Action<float, float> mov;
        public Action onDirSet { get; set; }
        public Action onFollowCompleted { get; set; }

        private bool pathDone,rotateDone;
        private Vector3 targetOffset;
        
        private const float NAV_FIND_PERIOD = 0.2f;

        private bool debug;
        public void StartFollow(Vector3 target)
        {
            this.posTarget = target;
            this.Init();
            this.RandomOffset(target);
        }

        public void StartFollow(Transform target)
        {
            this.transTarget = target;
            this.Init();
            this.RandomOffset(target.position);
        }

        public void StartFollow(Transform target, Vector3 offset)
        {
            this.transTarget = target;
            this.Init();
            this.targetOffset = offset;
        }

        public void CancelFollow()
        {
            path = null;
            transTarget = null;
            this.enabled = false;
        }

        private void Complete()
        {
            CancelFollow();
            if (null != onFollowCompleted)
                onFollowCompleted();
        }

        void Update()
        {
            if(null == self)
                return;
            float deltaTime = Time.deltaTime;
            Vector3 dir = DoUpdatePos(deltaTime);
            this.RotateToward(dir);
        }

        private Vector3 GetTargetPos()
        {
            Vector3 pos = null == transTarget ? posTarget : transTarget.position;
            return pos + targetOffset;
        }
        
        private Vector3 DoUpdatePos(float deltaTime)
        {
            Vector3 curPos = self.position;
            Vector3 target = GetTargetPos();
            Vector3 dir = target - curPos;
            if (pathDone)
                return dir;
            elapsedTime += deltaTime;
            if (elapsedTime >= NAV_FIND_PERIOD)
            {
                path = FindPath(curPos, target);
                nextWaypoint = 1;
                if (null != path && nextWaypoint < path.Length)
                {
                    elapsedTime = 0f;
                }
                else
                {
                    pathDone = true;
                    return dir;
                }
            }

            Vector3 nextPos = path[nextWaypoint];
            Vector3 newPos = Vector3.MoveTowards(curPos,nextPos,speed*deltaTime);
            dir = newPos - curPos;
            if (null != mov)
                mov(dir.x/deltaTime,dir.z/deltaTime);
            if (IsInRange(newPos, target))
                pathDone = true;
            else if ((newPos - nextPos).sqrMagnitude <= 0.001f)
            {
                if(++nextWaypoint==path.Length)
                    pathDone = true;
            }

            return dir;
        }

        private void RotateToward(Vector3 dir)
        {
            dir.y = 0;
            dir.Normalize();
            if (dir != Vector3.zero)
            {
                self.rotation = Quaternion.Slerp(self.rotation, Quaternion.LookRotation(dir),
                    rotateSpeed * Time.deltaTime);
                if (null != onDirSet)
                    onDirSet();
            }

            if (pathDone)
            {
                if(IsInDir())
                    Complete();
            }
        }

        private bool IsInRange(Vector3 src, Vector3 tar)
        {
            Vector3 delta = src - tar;
            delta.y = 0;
            return delta.magnitude <= arriveRange;
        }

        private bool IsInDir()
        {
            Vector3 curPos = self.position;
            Vector3 dir = GetTargetPos() - curPos;
            dir.y = 0;
            dir.Normalize();
            if (dir.sqrMagnitude <= 0.001f)
                return true;
            Vector3 selfDir = self.forward;
            if ((selfDir - dir).sqrMagnitude <= 0.1f)
            {
                return true;
            }

            return false;
        }
        private Vector3[] FindPath(Vector3 src, Vector3 tar)
        {
            if (IsInRange(src,tar))
            {
                return null;
            }
            NavMeshPath navMeshPath = new NavMeshPath();
            if (NavMesh.CalculatePath(src, tar, navMask, navMeshPath))
            {
                Vector3 lastVec = navMeshPath.corners[navMeshPath.corners.Length - 1];
                Vector3 delta = lastVec-tar;
                delta.y = 0f;
                if (delta.magnitude <= 0.5f)
                    return navMeshPath.corners;
            }

            return null;
        }
        private void Init()
        {
            if (null == self)
                self = transform;
            pathDone = false;
            elapsedTime = NAV_FIND_PERIOD;
            this.enabled = true;
        }

        private void RandomOffset(Vector3 target)
        {
            if (useRandomDirection)
            {
                targetOffset = target - self.position;
                if (targetOffset.magnitude > arriveRange)
                {
                    targetOffset.y = 0f;
                    targetOffset.Normalize();
                    targetOffset = Quaternion.Euler(0f, UnityEngine.Random.Range(11, 60) * 5f, 0f) * targetOffset;
                    targetOffset *= arriveRange * 0.9f;
                }
                else
                {
                    targetOffset = Vector3.zero;;
                }
            }
        }

        private void OnDrawGizmos()
        {
            if(!debug)
                return;
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(GetTargetPos(),0.25f);
            if(null == path)
                return;
            for (int i = 0; i < path.Length - 1; ++i)
            {
                Gizmos.DrawLine(path[i],path[i+1]);
            }
        }
        [ContextMenu("ShowDebug")]
        private void ShowDebug()
        {
            debug = true;
        }
        [ContextMenu("HideDebug")]
        private void HideDebug()
        {
            debug = false;
        }
    }
}