// © 2016 Mario Lelas

using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// base ragdoll and hit reaction manager
/// </summary>
public abstract class RagdollManager : MonoBehaviour
{
    /// <summary>
    ///  Class that holds useful information for each body part
    /// </summary>
    [System.Serializable]
    public class BodyPartInfo
    {
        public BodyParts bodyPart = BodyParts.None;                 // current body part
        public int index = -1;                                      // index of body part
        public Transform transform = null;                          // transform of body part
        public Transform orig_parent = null;                        // original parent of body part
        public Collider collider = null;                            // collider of body part
        public Rigidbody rigidBody = null;                          // rigidbody of body part
        public Vector3 transitionPosition = Vector3.zero;           // transition position used for blending
        public Quaternion transitionRotation = Quaternion.identity; // transition rotation used for blending
        public Vector3 extraForce = Vector3.zero;                   // extra force used for adding to body part in ragdoll mode
        public CharacterJoint constraintJoint = null;            // constraint to add body parts like legs
        public Vector3 previusPosition = Vector3.zero;              // previus position to help calculate velocity
        public Vector3 customVelocity = Vector3.zero;               // custom velocity calculated on kinematic bodies
        public bool ignoreRagdoll = false;
        public Rigidbody jointConnectBody = null;
    }

    #region Fields
        
    /// <summary>
    /// ragdoll transforms with colliders and rigid bodies 
    /// </summary>
    public Transform[] RagdollBones;

    /// <summary>
    /// create joints on constrained bodyparts / legs
    /// </summary>
    //public bool useJoints = true;

    // array of body parts
    [SerializeField]
    protected BodyPartInfo[] m_BodyParts;

    // is ragdoll physics on
    protected bool m_RagdollEnabled = true;

    // does have root motion
    protected Vector3? m_ForceVel = null;                     // hit force velocity
    protected Vector3? m_ForceVelocityOveral = null;          // force velocity on non hits parts
    protected bool m_Initialized = false;                     // is class initialized

    protected List<int> m_ConstraintIndices;    // list of constrained indices ( legs usualy )

    protected Transform m_RootTransform;

    protected int? m_HitParts = null;                      // body parts hit array

    protected bool m_InRagdoll = false;
    protected float currentRagdollTime = 0.0f;
    public float totalRagdollTime = 4.0f;
    public Action<Vector3> onCompelted = null;
    public Action onCreateCompeleted = null;
    #endregion


    #region Properties

    /// <summary>
    /// return true if component is initialized
    /// </summary>
    public bool Initialized { get { return m_Initialized; } }

    /// <summary>
    /// gets spine bone transform
    /// </summary>
    public Transform RootTransform
    {
        get
        {
            return m_RootTransform;
        }
    }

    public Vector3 RootPosition
    {
        get
        {
            return m_RootTransform.position;
        }
    }

    /// <summary>
    /// gets number of bodyparts
    /// </summary>
    public abstract int BodypartCount { get; }

    #endregion

    /// <summary>
    /// initialize component
    /// </summary>
    public virtual void Initialize()
    {

    }

    /// <summary>
    /// setup colliders and rigid bodies for ragdoll start
    /// set colliders to be triggers and set rigidbodies to be kinematic
    /// </summary>
    protected virtual void enableRagdoll(bool gravity = true)
    {
#if DEBUG_INFO
            if (m_BodyParts == null) { Debug.LogError("object cannot be null."); return; }
#endif

        if (m_RagdollEnabled)
        {
            return;
        }

        m_InRagdoll = true;

        for (int i = 0; i < m_BodyParts.Length; ++i)
        {
#if DEBUG_INFO
                if (m_BodyParts[i] == null) { Debug.LogError("object cannot be null."); continue; }
#else
            if (m_BodyParts[i] == null || m_BodyParts[i].ignoreRagdoll == true) continue;
#endif
            if (m_BodyParts[i].collider != null)
            {
                m_BodyParts[i].collider.enabled = true;
                m_BodyParts[i].collider.isTrigger = false;
            }
#if DEBUG_INFO
                else Debug.LogWarning("body part collider is null.");
#endif


            if (m_BodyParts[i].rigidBody)
            {
                m_BodyParts[i].rigidBody.useGravity = gravity;
                m_BodyParts[i].rigidBody.isKinematic = false;
            }
#if DEBUG_INFO
                else Debug.LogWarning("body part rigid body is null.");
#endif

        }
        m_RagdollEnabled = true;

    }

    /// <summary>
    /// disable ragdoll. setup colliders and rigid bodies for normal use
    /// set colliders to not be triggers and set rigidbodies to not be kinematic
    /// </summary>
    protected virtual void disableRagdoll()
    {
#if DEBUG_INFO
            if (m_BodyParts == null) { Debug.LogError("object cannot be null."); return; }
#endif

        if (!m_RagdollEnabled) return;

        m_InRagdoll = false;

        for (int i = 0; i < m_BodyParts.Length; ++i)
        {
#if DEBUG_INFO
                if (m_BodyParts[i] == null) { Debug.LogError("object cannot be null."); continue; }
#else
            if (m_BodyParts[i] == null) continue;
#endif
            if (m_BodyParts[i].collider != null)
            {
                m_BodyParts[i].collider.enabled = false;
                //m_BodyParts[i].collider.isTrigger = true;
            }
#if DEBUG_INFO
                else Debug.LogWarning("body part collider is null.");
#endif

            if (m_BodyParts[i].rigidBody)
            {
                m_BodyParts[i].rigidBody.useGravity = false;
                m_BodyParts[i].rigidBody.isKinematic = true;
            }
#if DEBUG_INFO
                else Debug.LogWarning("body part rigid body is null.");
#endif
            m_RagdollEnabled = false;
        }
    }

    /// <summary>
    /// starts ragdoll flag by adding velocity to chosen body part index and overall velocity to all parts
    /// </summary>
    /// <param name="part">hit body part indices</param>
    /// <param name="velocityHit">force on hit body part</param>
    /// <param name="velocityOverall">overall force applied on rest of bodyparts</param>
    public void StartRagdoll
        (
        int? hit_parts = null,
        Vector3? hitForce = null,
        Vector3? overallHitForce = null
        )
    {
        m_HitParts = hit_parts;
        m_ForceVel = hitForce;
        m_ForceVelocityOveral = overallHitForce;
    }

    public void SetIgnoreRagdoll(Transform ragdollTrans, bool ignoreRagdollEffect = false)
    {
        for (int i = 0; i < BodypartCount; i++)
        {
            if (m_BodyParts[i].transform == ragdollTrans)
            {
                m_BodyParts[i].ignoreRagdoll = ignoreRagdollEffect;
                if(ignoreRagdollEffect)
                {
                    m_BodyParts[i].constraintJoint.connectedBody = null;
                }
                else
                {
                    m_BodyParts[i].constraintJoint.connectedBody = m_BodyParts[i].jointConnectBody;
                }
            }
        }
    }
}
