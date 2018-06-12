using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleSetDecal : MonoBehaviour
    {
        private ParticleSystem pss;

        public ParticleSystem Pss
        {
            get
            {
                if (null == pss) pss = GetComponent<ParticleSystem>();return pss; 
            }
        }

        public void SetCollision(Transform trans)
        {
             Pss.collision.SetPlane(0, trans);
        }
    }
}
