using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RagdollData/RagdollPartId")]
[System.Serializable]
public class RagdollPartId : ScriptableObject
{
    [System.Serializable]
    public class Power
    {
        public int[] id;
    }

    public Power[] BodyPartInfo;
}
