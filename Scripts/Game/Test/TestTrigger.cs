using UnityEngine;

namespace Game.Test
{
    public class TestTrigger : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log(other.name + " enter");
        }

        private void OnTriggerExit(Collider other)
        {
            Debug.Log(other.name + " exit");
        }
    }
}