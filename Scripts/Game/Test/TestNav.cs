using UnityEngine;
using UnityEngine.AI;

namespace Game.Test
{
    public class TestNav : MonoBehaviour
    {
        public Transform target;
        private NavMeshPath path;
        public bool arrivable;
        private Vector3[] waypoints;
        [ContextMenu("Execute")]
        private void Execute()
        {
            path = new NavMeshPath();
            arrivable = NavMesh.CalculatePath(transform.position, target.position, -1, path);
            if (arrivable)
            {
                waypoints = path.corners;
            }
            else
            {
                waypoints = null;
            }
        }

        private void OnDrawGizmos()
        {
            if (null != waypoints)
            {
                Gizmos.color = Color.blue;
                for(int i=0;i<waypoints.Length-1;++i)
                    Gizmos.DrawLine(waypoints[i],waypoints[i+1]);
            }
        }
    }
}