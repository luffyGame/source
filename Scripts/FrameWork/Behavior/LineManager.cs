using UnityEngine;
using UnityEngine.AI;

namespace FrameWork
{
    public class LineManager : MonoBehaviour
    {
        #region Variables
        public LineRenderer[] lines;
        private int navMask = -1;
        public int NavMask
        {
            set { navMask = value; }
        }
        #endregion

        void Awake()
        {
            for (int i = 0; i < lines.Length; i++)
            {
                if (lines[i] != null)
                {
                    if (lines[i].material == null)
                        lines[i].material.shader = Shader.Find("Legacy Shaders/Diffuse");
                }
            }
        }

        public void DrawLine(Transform start, Transform end, int index)
        {
            lines[index].enabled = true;
            lines[index].SetPosition(0, start.position);
            lines[index].SetPosition(1, end.position);
        }

        public void DrawLine(Vector3[] path, int index)    
        {
            lines[index].enabled = true;
            lines[index].positionCount = path.Length;
            lines[index].SetPositions(path);
        }

        private Vector3[] FindPath(Vector3 src, Vector3 tar)
        {
            NavMeshPath navMeshPath = new NavMeshPath();
            if (NavMesh.CalculatePath(src, tar, navMask, navMeshPath))
            {
                return navMeshPath.corners;
            }

            return null;
        }

        //draw path line
        public void DrawPathLine(Transform form, Transform to, int index)
        {
            Vector3[] tempPath = FindPath(form.position, to.position);
            if (null != tempPath)
            {
                DrawLine(tempPath, index);
            }
        }

        public void ActiveLine(bool enabled, int index)
        {
            lines[index].enabled = enabled;
        }

    }
}
