// © 2016 Mario Lelas

#define CALC_CUSTOM_VELOCITY_KINEMATIC

using UnityEngine;
using FrameWork;
using System.Collections.Generic;

/// <summary>
/// body parts enum
/// </summary>
public enum BodyParts : int
{
    Spine = 0,
    Chest,
    Head,
    LeftShoulder,
    RightShoulder,
    LeftElbow,
    RightElbow,
    LeftHip,
    RightHip,
    LeftKnee,
    RightKnee,
    BODY_PART_COUNT,
    None,
}

/// <summary>
/// ragdoll and hit reaction manager
/// </summary>
[System.Serializable]
public class RagdollManagerHum : RagdollManager
{
    //画线：
    private bool DrawDir = false;
    public Vector3 OrignPos = Vector3.zero;
    public Vector3 TestDir = Vector3.zero;
    public Vector3 OrignDir = Vector3.zero;


    /// <summary>
    /// gets number of bodyparts
    /// </summary>
    public override int BodypartCount
    {
        get { return (int) BodyParts.BODY_PART_COUNT; }
    }

    public void DefaultInitialize()
    {
        RagdollBuilder builder = new RagdollBuilder();
        builder.InitParams(transform);
        builder.SynchronousRagdoll();

        builder.OnRagdollCreated = () =>
        {
            RagdollBones = new Transform[(int) BodyParts.BODY_PART_COUNT];
            RagdollBones[(int) BodyParts.Spine] = builder.pelvis;
            RagdollBones[(int) BodyParts.Chest] = builder.middleSpine;
            RagdollBones[(int) BodyParts.Head] = builder.head;
            RagdollBones[(int) BodyParts.LeftShoulder] = builder.leftArm;
            RagdollBones[(int) BodyParts.LeftElbow] = builder.leftElbow;
            RagdollBones[(int) BodyParts.RightShoulder] = builder.rightArm;
            RagdollBones[(int) BodyParts.RightElbow] = builder.rightElbow;
            RagdollBones[(int) BodyParts.LeftHip] = builder.leftHips;
            RagdollBones[(int) BodyParts.LeftKnee] = builder.leftKnee;
            RagdollBones[(int) BodyParts.RightHip] = builder.rightHips;
            RagdollBones[(int) BodyParts.RightKnee] = builder.rightKnee;

            //EditorUtility.SetDirty(transform);
            //serializedObject.ApplyModifiedProperties();

            builder.OnRagdollCreated = null;
            Initialize();
            if (onCreateCompeleted != null)
            {
                onCreateCompeleted();
            }
        };

        builder.OnCreated();
    }

    /// <summary>
    /// initialize class instance
    /// </summary>
    public override void Initialize()
    {
        if (m_Initialized) return;

        base.Initialize();

        // keep track of colliders and rigid bodies
        m_BodyParts = new BodyPartInfo[(int) BodyParts.BODY_PART_COUNT];

        bool ragdollComplete = true;
        for (int i = 0; i < (int) BodyParts.BODY_PART_COUNT; ++i)
        {
            Rigidbody rb = RagdollBones[i].GetComponent<Rigidbody>();
            Collider col = RagdollBones[i].GetComponent<Collider>();
            if (rb == null || col == null)
            {
                ragdollComplete = false;
#if DEBUG_INFO
                    Debug.LogError("missing ragdoll part: " + ((BodyParts)i).ToString());
#endif
            }

            m_BodyParts[i] = new BodyPartInfo();
            m_BodyParts[i].transform = RagdollBones[i];
            m_BodyParts[i].rigidBody = rb;
            m_BodyParts[i].collider = col;
            m_BodyParts[i].bodyPart = (BodyParts) i;
            m_BodyParts[i].index = i;
            m_BodyParts[i].orig_parent = RagdollBones[i].parent;
            CharacterJoint cj = RagdollBones[i].GetComponent<CharacterJoint>();
            if (cj != null)
            {
                m_BodyParts[i].constraintJoint = cj;
                m_BodyParts[i].jointConnectBody = cj.connectedBody;
            }
        }

        if (!ragdollComplete)
        {
            Debug.LogError("ragdoll is incomplete or missing");
            return;
        }

        m_RootTransform = m_BodyParts[(int) BodyParts.Spine].transform;

        m_Initialized = true;
        disableRagdoll();
    }


    #region Unity Event

    public void Awake()
    {
        m_RootTransform = m_BodyParts[(int) BodyParts.Chest].transform;
        m_RagdollEnabled = true;
        disableRagdoll();
    }

    public void Update()
    {
        if (m_InRagdoll)
        {
            if (currentRagdollTime >= totalRagdollTime)
            {
                currentRagdollTime = 0.0f;
                m_InRagdoll = false;
                if (onCompelted != null)
                {
                    onCompelted(RootPosition);
                }
            }

            currentRagdollTime += Time.deltaTime;
        }
    }

    #endregion

    public void DisableCustomRagdoll()
    {
        DrawDir = false;

        disableRagdoll();

        for (int i = 1; i < BodypartCount; i++)
        {
            if (m_BodyParts[i].ignoreRagdoll == true) continue;
            BodyPartInfo b = m_BodyParts[i];
            b.transform.SetParent(b.orig_parent);
        }

        foreach (BodyPartInfo b in m_BodyParts)
        {
            b.transitionRotation = b.transform.rotation;
            b.transitionPosition = b.transform.position;
        }
    }

    private void OnDrawGizmos()
    {
        if (DrawDir)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(OrignPos, OrignPos + new Vector3(TestDir.x, 0, TestDir.z));
            Gizmos.color = Color.red;
            Gizmos.DrawLine(OrignPos, OrignPos + 10 * OrignDir);
        }
    }

    public void CustomRagdoll
    (
        int? hit_parts = null,
        Vector3? hitForce = null,
        Vector3? overallHitForce = null
    )
    {
        Vector3 ragdollSpeed = Game.Config.Instance.GetRagdollSpeedRandom();
        var dir = hitForce.Value;
        //画线: 
        DrawDir = true;
        OrignPos = transform.position;
        TestDir = dir;


        base.StartRagdoll(hit_parts, new Vector3(
                Global.RandomRange(dir.x * (1 - ragdollSpeed.x), dir.x * (1 + ragdollSpeed.x)),
                Global.RandomRange(dir.y * (1 - ragdollSpeed.y), dir.y * (1 + ragdollSpeed.y)),
                Global.RandomRange(dir.z * (1 - ragdollSpeed.z), dir.z * (1 + ragdollSpeed.z)))
            , overallHitForce);

        RunRagdoll();
    }

    // start ragdoll method
    private void RunRagdoll()
    {
#if DEBUG_INFO
            if (m_BodyParts == null) { Debug.LogError("object cannot be null."); return; }
#endif
        enableRagdoll(true);

#if SAVE_ANIMATOR_STATES
            saveAnimatorStates();
#endif

        for (int i = 1; i < BodypartCount; i++)
        {
            if (m_BodyParts[i].ignoreRagdoll == true) continue;
            BodyPartInfo b = m_BodyParts[i];
            b.transform.SetParent(transform);
        }

        if (m_ForceVelocityOveral.HasValue)
        {
            for (int i = 0; i < m_BodyParts.Length; i++)
            {
                m_BodyParts[i].rigidBody.velocity = m_ForceVelocityOveral.Value;
            }
        }
#if CALC_CUSTOM_VELOCITY_KINEMATIC
        else
        {
            for (int i = 0; i < m_BodyParts.Length; i++)
            {
                BodyPartInfo b = m_BodyParts[i];
                b.rigidBody.velocity = b.customVelocity;
            }
        }
#endif

        if (m_HitParts != null)
        {
            if (m_ForceVel.HasValue)
            {
                BodyPartInfo b = m_BodyParts[m_HitParts.Value];
                b.rigidBody.velocity = m_ForceVel.Value;
            }
        }

        m_ForceVel = null;
        m_ForceVelocityOveral = null;
        m_HitParts = null;
    }
}