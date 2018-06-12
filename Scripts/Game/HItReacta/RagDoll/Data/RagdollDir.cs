using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RagdollData/RagdollDir")]
[System.Serializable]
public class RagdollDir : ScriptableObject
{
    [System.Serializable]
    public class Power
    {
        public Dir[] Power_Dir;
    }

    [System.Serializable]
    public class Dir
    {
        public int powerType;
        public Vector3 direction;
    }

    public Power[] BodyPartInfo;
}
