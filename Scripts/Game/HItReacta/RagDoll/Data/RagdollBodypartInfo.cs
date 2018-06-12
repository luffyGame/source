using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RagdollData/RagdollBodyPartInfo")]
[System.Serializable]
public class RagdollBodypartInfo : ScriptableObject
{
    [System.Serializable]
    public class bodyInfo
    {
        public int[] bodypart;  
    }
    public bodyInfo[] bodyPartInfos;
}


