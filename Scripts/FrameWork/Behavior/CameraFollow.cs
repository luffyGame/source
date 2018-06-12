using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public class CameraFollow : MonoBehaviour
    {
        public Transform followTarget = null;
        public Vector3 followOffset = new Vector3(0f,2f,0f);
        public float xRot = 45f;
        public float yRot = 0f;
        //public Vector3 followDir = new Vector3(0f, 1f, -1f);
        public float followDist = 5.5f;
        public float minDist = 0f;
        public float maxDist = 15f;
        public Vector3 buildmodelOffset = new Vector3(0f, 2f, 0f);
        public float buildmodelXRot = 0f;
        private Vector3 cachedOffset;
        private float cachedXRot;
        public Transform cachedTrans { get; private set; }
        public Vector3 targetPos
        {
            get { return followTarget.position + followOffset; }
        }
        public bool IsValidated
        {
            get { return followTarget != null; }
        }
        void Awake()
        {
            cachedTrans = transform;
            cachedOffset = followOffset;
            cachedXRot = xRot;
            //followDir.Normalize();
        }

        void LateUpdate()
        {
            Loc();
        }

        public void Loc()
        {
            if (!IsValidated)
                return;
            Vector3 pos = targetPos;
            cachedTrans.position = pos - Quaternion.Euler(xRot,yRot,0f) * Vector3.forward  * followDist;
            cachedTrans.LookAt(pos);
        }
        public void ScaleFollowDist(float scale)
        {
            followDist = Mathf.Clamp(followDist / scale,minDist,maxDist);
        }
        public void XRotate(float xRotDelta)
        {
            xRot = ClampAngle(xRot - xRotDelta, -80, 80);
        }
        public void YRotate(float yRotDelta)
        {
            yRot = ClampAngle(yRot + yRotDelta, -360, 360);
        }

        public void SetBuildModelView()
        {
            followOffset = buildmodelOffset;
            xRot = buildmodelXRot;
        }

        public void RecoverBuildModelView()
        {
            followOffset = cachedOffset;
            xRot = cachedXRot;
        }

        static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360)
                angle += 360;
            if (angle > 360)
                angle -= 360;
            return Mathf.Clamp(angle, min, max);
        }  
    }
}
