using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    public class ColliderHit : MonoBehaviour
    {
        void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            RaycastHit hitInfo;
            if (Physics.Raycast(transform.position, transform.forward, out hitInfo))
            {
                Gizmos.DrawLine(transform.position, hitInfo.point);
                Gizmos.DrawSphere(hitInfo.point, 0.05f);
            }
            else
            {
                Vector3 end = transform.position + transform.forward * 50f;
                Gizmos.DrawLine(transform.position, end);
            }
        }
    }
}
