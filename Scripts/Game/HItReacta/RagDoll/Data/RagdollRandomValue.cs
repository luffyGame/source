using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "RagdollData/RagdollRandomValue")]
[System.Serializable]
public class RagdollRandomValue : ScriptableObject
{
    public int friction;
    public int angleFriction;
    public Vector3 speed;
    public Vector3 angleSpeed;
    public Vector3 ragdollSpeed;
}
