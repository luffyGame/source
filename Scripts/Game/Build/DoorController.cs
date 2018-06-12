using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Game
{
    public class DoorController : MonoBehaviour
    {
        public Vector3 FrontRotate;
        public Vector3 BackRotate;
        public Vector3 CloseRotate;
        public Collider doorCollider;
        public Transform doorTrans;
        public float rotateTime = 2.0f;
        private bool isOpen = false;

        private void Start()
        {
            //SetColliderActive(false);
            isOpen = false;
        }

        public void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player" && !isOpen)
            {
                //doorCollider.enabled = false;
                OpenDoor(other.transform);
                isOpen = true;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.tag == "Player" && isOpen)
            {
                CloseDoor();
                isOpen = false;
            }

        }
        public void OpenDoor(Transform trans)
        {
            var angle = Vector3.Angle(transform.forward, transform.position - trans.position);
            var openDir = angle <= 90 ? FrontRotate : BackRotate;
            doorTrans.DOLocalRotate(openDir, rotateTime);
        }

        public void CloseDoor()
        {
            doorTrans.DOLocalRotate(CloseRotate, rotateTime);
        }

        public void SetColliderActive(bool active)
        {
            doorCollider.enabled = active;
        }
    }
}
