using System;
using UnityEngine;
using UnityEngine.AI;

namespace FrameWork
{
    public class NavTransManager : MonoBehaviour
    {
        #region Variables
        private float calculateTime = 0.0f;

        private Transform moveTransRoot = null;
        private Animation[] animations = null;

        public float speed = 10f;
        public float rotateSpeed = 10f;
        public float departTime;
        private float startTime;
        private int nextWaypoint;
        private Vector3[] path;
        private Vector3 target;
        public float arriveRange;
        private bool pathDone = true;

        private int navMask = -1;
        public int NavMask
        {
            set { navMask = value; }
        }

        #endregion

        #region 事件回调
        public Action<float, float, float> notifyPosSet;
        public  Action notifyMoveCompleted;
        private Action tempCompleteEvent = null;

        public void BindOnCompleteCallBack(Action completeAction)
        {
            notifyMoveCompleted = completeAction;
        }

        public void BindMoveCallback(Action<float, float, float> notifyPosSet)
        {
            this.notifyPosSet = notifyPosSet;
        }

        public void UnBindNotify()
        {
            this.notifyPosSet = null;
            this.notifyMoveCompleted = null;
        }

        #endregion
        #region UnityEvent

        //private void Start()
        //{
        //    pathDone = true;
        //    nextWaypoint = 0;
        //}

        private void Update()
        {
            if(null == moveTransRoot || true == pathDone)
            {
                return;
            }

            float deltaTime = Time.deltaTime;
            startTime += deltaTime;
            if(startTime < departTime)
            {
                return;
            }

            calculateTime += Time.deltaTime;
            Vector3 dir = UpdatePosition(deltaTime);
            UpdateRotate(dir);
        }

        private Vector3 UpdatePosition(float deltaTime)
        {
            Vector3 curPos = moveTransRoot.position;
            Vector3 dir = target - curPos;

            Vector3 nextPos = path[nextWaypoint];
            Vector3 newPos = Vector3.MoveTowards(curPos, nextPos, speed * deltaTime);
            dir = newPos - curPos;
            moveTransRoot.position = newPos;
            //nofity
            if (notifyPosSet != null)
            {
                notifyPosSet(newPos.x, newPos.y, newPos.z);
            }

            if (IsInRange(newPos, target))
            {
                pathDone = true;
            }
            else if ((newPos - nextPos).sqrMagnitude <= 0.001f)
            {
                if (++nextWaypoint == path.Length)
                {
                    pathDone = true;
                }
            }

            return dir;
        }

        private void UpdateRotate(Vector3 dir)
        {
            dir.Normalize();
            if (dir != Vector3.zero)
            {
                moveTransRoot.rotation = Quaternion.Slerp(moveTransRoot.rotation, Quaternion.LookRotation(dir),
                    rotateSpeed * Time.deltaTime);
            }

            if (pathDone)
            {
                Complete();
            }
        }

        private void Complete()
        {
            calculateTime = 0.0f;
            EnableTrans(false);

            //anim.Play("idle");
            path = null;
            nextWaypoint = 0;
            startTime = 0;

            if (null != notifyMoveCompleted)
            {
                notifyMoveCompleted();
            }

            notifyMoveCompleted = null;
        }

        private void OnDestroy()
        {
            notifyMoveCompleted = null;
        }

        #endregion

        #region Init
        public void EnableTrans(bool enable)
        {
            if (moveTransRoot != null)
            {
                moveTransRoot.gameObject.SetActive(enable);
            }
        }

        public void Init(Transform target)
        {
            if (target != null)
            {
                moveTransRoot = target;
                animations = target.GetComponentsInChildren<Animation>();

                pathDone = true;
                nextWaypoint = 0;
            }
        }

        public void SetSpeed(float speed)
        {
            this.speed = speed;
        }
        #endregion


        #region Method
        public float GetPathLength(Transform from, Transform to)
        {
            float length = 0.0f;
            if (IsInRange(from.position, to.position))
            {
                return length;
            }

            NavMeshPath navMeshPath = new NavMeshPath();
            if (NavMesh.CalculatePath(from.position, to.position, navMask, navMeshPath))
            {
                for (int i = 1; i < navMeshPath.corners.Length; i++)
                {
                    length += Vector3.Distance(navMeshPath.corners[i - 1], navMeshPath.corners[i]);
                }
            }

            return length;
        }

        //计算movetrans到target路径距离
        public float GetPathLength(Vector3 target)
        {
            float length = 0.0f;
            if (IsInRange(moveTransRoot.position, target))
            {
                return length;
            }

            NavMeshPath navMeshPath = new NavMeshPath();
            if (NavMesh.CalculatePath(moveTransRoot.position, target, navMask, navMeshPath))
            {
                for(int i = 1; i < navMeshPath.corners.Length; i++)
                {
                    length += Vector3.Distance(navMeshPath.corners[i - 1], navMeshPath.corners[i]);
                }
            }

            return length;
        }

        public bool SetTransPos(Transform to, float time, float speed)
        {
            path = FindPath(moveTransRoot.position, to.position);
            int index = 1;
            while (index < path.Length)
            {
                float dist = (path[index] - moveTransRoot.position).magnitude;
                float tempTime = dist / speed;
                if (tempTime < time)
                {
                    time -= tempTime;
                    moveTransRoot.position = path[index++];
                }
                else
                {
                    moveTransRoot.position = Vector3.Lerp(moveTransRoot.position, path[index], time / tempTime);
                    return false;
                }
            }

            return true;
        }
        private bool IsInDir()
        {
            Vector3 curPos = moveTransRoot.position;
            Vector3 dir = target - curPos;
            dir.Normalize();
            if (dir.sqrMagnitude <= 0.001f)
                return true;

            Vector3 selfDir = moveTransRoot.forward;
            if ((selfDir - dir).sqrMagnitude <= 0.1f)
            {
                return true;
            }

            return false;
        }

        private bool IsInRange(Vector3 src, Vector3 tar)
        {
            Vector3 delta = src - tar;
            return delta.magnitude <= arriveRange;
        }

        private Vector3[] FindPath(Vector3 src, Vector3 tar)
        {
            if (IsInRange(src, tar))
            {
                nextWaypoint = 0;
                return new Vector3[] { src };
            }

            NavMeshPath navMeshPath = new NavMeshPath();
            if (NavMesh.CalculatePath(src, tar, navMask, navMeshPath))
            {
                return navMeshPath.corners;
            }

            return null;
        }
        #endregion

        #region 寻路接口
        /// <summary>
        /// 计算固定时间移动到的位置
        /// </summary>
        /// <param name="from">起点</param>
        /// <param name="to">终点</param>
        /// <param name="time">已过去时间</param>
        /// <param name="totalTime">总时间</param>
        public void MoveToTargetByTime(Transform from, Transform to, float time, float totalTime)
        {
            float length = GetPathLength(from, to);
            speed = length / totalTime;
            moveTransRoot.position = from.position;
            SetTransPos(to, time, speed);
            MoveToTargetBySpeed(to.position, speed);
        }

        //todo: replace Vector3 -> x,y,z
        public void MoveToTargetBySpeed(Vector3 target, float speed)
        {
            if (null == moveTransRoot)
            {
                return;
            }
            EnableTrans(true);
            this.target = target;

            path = FindPath(moveTransRoot.position, target);
            if(null != path)
            {
                pathDone = false;
                if(null != animations)
                {
                    foreach (var oneAnim in animations)
                    {
                        if (oneAnim["run"])
                        {
                            var animLength = oneAnim["run"].length;
                            oneAnim["run"].time = Global.RandomRange(0.2f, animLength);
                            oneAnim.Play("run");
                        }
                    }
                }
                this.speed = speed;
            }
        }

 
        #endregion
    }
}
