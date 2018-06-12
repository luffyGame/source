using UnityEngine;

namespace FrameWork
{
    public class UiRotateAni : MonoBehaviour
    {
        public float speed = 5f;
        private Transform trans;

        public Transform Trans
        {
            get
            {
                if (null == trans)
                    trans = transform;
                return trans;
            }
        }
        void Update()
        {
            Trans.Rotate(Vector3.forward, 90.0f * speed * Time.deltaTime);
        }
        [ContextMenu("reset")]
        public void Reset()
        {
            Trans.localRotation = Quaternion.identity;
        }
    }
}