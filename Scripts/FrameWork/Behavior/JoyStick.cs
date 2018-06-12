using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FrameWork
{
    [System.Serializable]
    public class JoyStick : MonoBehaviour,IDragHandler,IEndDragHandler,IPointerDownHandler,IPointerUpHandler
    {
        public RectTransform stickAreaRectT;
        public RectTransform stickTouchRectT;
        public float stickAreaRadius = 60f;
        public bool isMovable;
        public Action<float,float,float> move;
        public Action begin;
        public Action end;
        private bool isUse;
        private Vector2 mStickPos = Vector2.zero;//摇杆位置
        private Vector2 mStickStartPos;
        private Vector3 movDir;
        private RectTransform rectT;
        public RectTransform RectT { get { if (null == rectT) rectT = transform as RectTransform; return rectT; } }
        private static readonly KeyCode[] movKeys = { KeyCode.W, KeyCode.S, KeyCode.A, KeyCode.D };
        private static readonly Vector3[] movDirs = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        void Awake()
        {
            mStickStartPos = stickAreaRectT.anchoredPosition;
        }
        void Update()
        {
            if (isUse&&null!=move)
                DoMove(movDir);
//#if UNITY_EDITOR
            else
            {
                Vector3 dir = Vector3.zero;
                bool isMov = false;
                for (int i = 0; i < movKeys.Length; ++i)
                {
                    if (Input.GetKey(movKeys[i]))
                    {
                        isMov = true;
                        dir += movDirs[i];
                    }
                }
                if (isMov)
                {
                    if (dir != Vector3.zero)
                    {
                        dir = dir.normalized;
                        DoMove(dir);
                    }
                }
                else
                {
                    bool stopMove = false;
                    for (int i = 0; i < movKeys.Length; ++i)
                    {
                        if (Input.GetKeyUp(movKeys[i]))
                        {
                            stopMove = true;
                            break;
                        }
                    }
                    if (stopMove && null != end)
                    {
                        end();
                    }
                }
            }
//#endif
        }

        private void DoMove(Vector3 dir)
        {
            float cameraYRot = Global.Instance.CameraYRot;
            dir = Quaternion.Euler(0f, cameraYRot, 0f) * dir;
            if(null!=move)
                move(dir.x,dir.z,Time.deltaTime);
        }
        private void OnStickBegin(Vector2 pos)
        {
            SetStickPos(pos);
            StickBegin();
        }
        private void StickBegin()
        {
            isUse = true;
            movDir = Vector3.zero;
            if (null != begin)
                begin();
        }

        private Vector3 DragStickTo(Vector2 stickTo)
        {
            Vector2 delta = stickTo - mStickPos;
            Vector2 touchPos;
            if (delta.magnitude <= stickAreaRadius)
                touchPos = stickTo;
            else
                touchPos = mStickPos + delta.normalized * stickAreaRadius;
            stickTouchRectT.anchoredPosition = touchPos;
            return new Vector3(delta.x, 0, delta.y).normalized;
        }
        private void SetStickPos(Vector2 pos)
        {
            mStickPos = pos;
            stickAreaRectT.anchoredPosition = pos;
            stickTouchRectT.anchoredPosition = pos;
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector2 ret;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(RectT, eventData.position, Global.Instance.UiCamera, out ret))
            {
                movDir = DragStickTo(ret);
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (isUse)
            {
                Init();
                if (null != end)
                    end();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (isMovable)
            {
                Vector2 ret;
                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(RectT, eventData.position, Global.Instance.UiCamera, out ret))
                {
                    //float x = (RectT.sizeDelta.x - stickAreaRectT.sizeDelta.x) / 2;
                    //float y = (RectT.sizeDelta.y - stickAreaRectT.sizeDelta.y) / 2;
                    //ret.x = Mathf.Clamp(ret.x, -x, x);
                    //ret.y = Mathf.Clamp(ret.y, -y, y);
                    OnStickBegin(ret);
                }
            }
            else
            {
                StickBegin();
            }
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            OnEndDrag(eventData);
        }

        public void Init()
        {
            isUse = false;
            SetStickPos(mStickStartPos);
        }
    }
}
