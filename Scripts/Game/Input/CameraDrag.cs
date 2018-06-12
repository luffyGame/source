using DG.Tweening;
using Lean.Touch;
using UnityEngine;

namespace Game
{
    public class CameraDrag : MonoBehaviour
    {
        public Camera camera;
        public Transform target;
        public Vector2 clampX = Vector2.zero;
        public Vector2 clampZ = Vector2.zero;
        public float distance = 1.0f;
        public float dampening = 1.0f;
        private Vector3 velocity;
        private bool canDrag;
        private bool isFocus;
        private Vector3 focusTarget;
        public Vector2 offset = Vector2.zero;

        public void Focus(Transform trans)
        {
            Stop();
            focusTarget = trans.position;
            focusTarget.y = target.position.y;
            focusTarget.x -= offset.x;
            focusTarget.z -= offset.y;
            isFocus = true;
        }

        public void SetPos(float x, float y, float z)
        {
            Stop();
            var tempPos = new Vector3(x - offset.x, target.position.y, z - offset.y);
            target.position = tempPos;
        }
        public void Focus(float x, float y, float z)
        {
            Stop();
            focusTarget.y = target.position.y;
            focusTarget.x = x - offset.x;
            focusTarget.z = z - offset.y;
            isFocus = true;
        }

        public void Loc(Transform trans)
        {
            Stop();
            focusTarget = trans.position;
            focusTarget.y = target.position.y;
            target.position = focusTarget;
        }

        public void Stop()
        {
            velocity = Vector3.zero;
        }

        private void LateUpdate()
        {
            if(isFocus)
                UpdateFocus();
            else
                UpdateDrag();
        }

        private void UpdateFocus()
        {
            Vector3 newPos = Vector3.MoveTowards(target.position, focusTarget, Time.deltaTime * 100f);
            target.position = newPos;
            if ((newPos - focusTarget).sqrMagnitude<0.01f)
            {
                isFocus = false;
            }
        }

        private void UpdateDrag()
        {
            var fingers = LeanTouch.GetFingers(true);
            bool isDrag = fingers != null && fingers.Count > 0;
            if (isDrag)
            {
                var finger = fingers[0];
                if (finger.Down)
                {
                    velocity = Vector3.zero;
                }
                else if(finger.Set)
                {
                    var worldDelta = finger.GetWorldDelta(distance, camera);
                    if (worldDelta.sqrMagnitude > 0.01f)
                    {
                        Vector3 oldPos = target.position;
                        Vector3 newPos = ClampTargetToPos(oldPos - worldDelta);
                        velocity = newPos - oldPos;
                    }
                    else
                    {
                        velocity = Vector3.zero;
                    }
                }
                
            }
            else if (velocity.sqrMagnitude>0.01f)
            {
                ClampTargetToPos(target.position + velocity);
                velocity = Vector3.Lerp(velocity, Vector3.zero, Time.deltaTime*dampening);
            }
        }

        private Vector3 ClampTargetToPos(Vector3 newPos)
        {
            newPos.x = Mathf.Clamp(newPos.x, clampX.x, clampX.y);
            newPos.z = Mathf.Clamp(newPos.z, clampZ.x, clampZ.y);
            newPos.y = target.position.y;
            target.position = newPos;
            return newPos;
        }
    }
}