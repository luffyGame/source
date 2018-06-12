using UnityEngine;
using UnityEditor;
//use GUILayoutEx for custom button Color
using LevelEditor;
using System.Text;
using System;
using System.Collections;
using System.Collections.Generic;
using FrameWork;

public class DismemberConfig : EditorWindow
{
    public bool useDismember = false;
    //dismember 方案总数
    private const int dismemberParamNum = 10;
    //每个方案，特效、力、ignoreRagdoll总数
    private const int particleParamNum = 100;
    private GUIStyle normalStyle;
    private GUIStyle particleStyle;
    private GUIStyle ragdollStyle;

    private Vector2 scrollPos;
    public GameObject targetObj;
    private GameObject orignObj;

    //copy target
    public GameObject copyTarget;
    private bool isCopying = false;
    private bool copyParam = false;

    //Dismember
    private Dismemberment dismember;
    //Dismember Params
    private int dismemberNum;
    private Transform limbTransRoot;
    private Dismemberment.BodyPartHint[] bodyPart;
    private GameObject[] dismemberTarget;
    private Collider[] dismemberColliders;
    private Rigidbody[] dismemberRigidbodys;

    //particle
    private int[] particleNum;
    private string[] particleNames;
    private Transform[] particleRoot;
    private Vector3[] initPos;
    private Vector3[] initEurler;
    private Vector3[] initScale;

    //forceParams
    private int[] forceNum;
    private float[] drag;
    private float[] angleDrag;
    private Vector3[] force;
    private Vector3[] torque;
    private int forceMethod = 1;

    //ignore ragdoll effect
    private int[] ignoreRagdollNum;
    private Transform[] ignoreRagdollTrans;

    //ragdoll
    private RagdollManagerHum ragdollManager;
    private int ragdollId;
    private Vector3 ragdollForce;
    private int method = 1;

    // Add menu named "My Window" to the Window menu
    [MenuItem("游戏拓展/肢解编辑器")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        DismemberConfig window = (DismemberConfig)EditorWindow.GetWindow(typeof(DismemberConfig));
        window.Show();
    }

    private void initParams(GameObject targetObj)
    {
        ragdollManager = targetObj.GetComponent<RagdollManagerHum>();
        if (useDismember)
        {
            dismember = targetObj.GetComponent<Dismemberment>();
            if (dismember == null) return;

            dismemberNum = dismember.BoomParamsNum;
            limbTransRoot = dismember.LimbRoot;
            var boomParam = dismember.boomParam;

            //init nums
            dismemberTarget = new GameObject[dismemberParamNum];
            bodyPart = new Dismemberment.BodyPartHint[dismemberParamNum];
            dismemberColliders = new Collider[dismemberParamNum];
            dismemberRigidbodys = new Rigidbody[dismemberParamNum];


            particleNum = new int[dismemberParamNum];
            particleNames = new string[particleParamNum];
            particleRoot = new Transform[particleParamNum];
            initPos = new Vector3[particleParamNum];
            initEurler = new Vector3[particleParamNum];
            initScale = new Vector3[particleParamNum];

            //forceParam
            forceNum = new int[particleParamNum];
            drag = new float[particleParamNum];
            angleDrag = new float[particleParamNum];
            force = new Vector3[particleParamNum];
            torque = new Vector3[particleParamNum];

            for (int i = 0; i < particleParamNum; i++)
            {
                initScale[i] = Vector3.one;
            }

            ignoreRagdollNum = new int[dismemberParamNum];
            ignoreRagdollTrans = new Transform[particleParamNum];

            for (int i = 0; i < dismemberNum; i++)
            {
                var temp = boomParam[i];
                dismemberTarget[i] = temp.targetObj;
                dismemberColliders[i] = temp.collider;
                dismemberRigidbodys[i] = temp.rigidbody;
                bodyPart[i] = temp.bodyPart;
                //particle
                particleNum[i] = temp.ParticleNum;
                for (int j = 0; j < particleNum[i]; j++)
                {
                    var particleParm = temp.particles[j];
                    particleNames[dismemberParamNum * i + j] = particleParm.ParticleName;
                    particleRoot[dismemberParamNum * i + j] = particleParm.particleRoot;
                    initPos[dismemberParamNum * i + j] = particleParm.initPos;
                    initScale[dismemberParamNum * i + j] = particleParm.initScale;
                    initEurler[dismemberParamNum * i + j] = particleParm.initEurler;
                }

                //force
                forceNum[i] = temp.ForceNum;
                for (int j = 0; j < forceNum[i]; j++)
                {
                    var forceParam = temp.forceParams[j];
                    drag[dismemberParamNum * i + j] = forceParam.drag;
                    angleDrag[dismemberParamNum * i + j] = forceParam.angleDrag;
                    force[dismemberParamNum * i + j] = forceParam.force;
                    torque[dismemberParamNum * i + j] = forceParam.torque;
                }

                //ignore ragdoll
                ignoreRagdollNum[i] = temp.IgnoreRagdollEffectNum;
                for (int k = 0; k < ignoreRagdollNum[i]; k++)
                {
                    ignoreRagdollTrans[dismemberParamNum * i + k] = temp.ignoreRagdollTrans[k];
                }
            }
        }
    }

    private void CopyParamFromTarget(GameObject target, GameObject copyTarget)
    {
        var copyParam = copyTarget.GetComponent<Dismemberment>().boomParam;
        var targetParam = target.GetComponent<Dismemberment>().boomParam;

        if (targetParam != null)
        {
            targetParam.Clear();
            targetParam = null;
        }

        targetParam = new List<Dismemberment.BoomParams>();
        for (int i = 0; i < copyParam.Count; i++)
        {
            Dismemberment.BoomParams bp = new Dismemberment.BoomParams();
            targetParam.Add(bp);
            var targetName = copyParam[i].targetObj.name;
            var tempTarget = target.transform.FindRecursive(targetName);
            var tempObj = targetParam[i];
            tempObj.bodyPart = copyParam[i].bodyPart;
            tempObj.targetObj = tempTarget.gameObject;
            if (tempTarget != null)
            {
                tempObj.collider = tempTarget.GetComponent<Collider>();
                tempObj.rigidbody = tempTarget.GetComponent<Rigidbody>();
            }
            //particle init
            if (tempObj.particles != null)
            {
                tempObj.particles.Clear();
                tempObj.particles = null;
            }

            tempObj.particles = new List<Dismemberment.ParticleParms>();
            if (copyParam[i].particles != null)
            {
                for (int j = 0; j < copyParam[i].particles.Count; j++)
                {
                    tempObj.particles.Add(new Dismemberment.ParticleParms());
                    tempObj.particles[j].ParticleName = copyParam[i].particles[j].ParticleName;
                    if (copyParam[i].particles[j].particleRoot != null)
                    {
                        var particleTransName = copyParam[i].particles[j].particleRoot.name;
                        var particleTrans = target.transform.FindRecursive(particleTransName);
                        tempObj.particles[j].particleRoot = particleTrans;
                    }
                    tempObj.particles[j].initPos = copyParam[i].particles[j].initPos;
                    tempObj.particles[j].initEurler = copyParam[i].particles[j].initEurler;
                    tempObj.particles[j].initScale = copyParam[i].particles[j].initScale;
                }
            }

            //ignore Ragdoll
            if (tempObj.ignoreRagdollTrans != null)
            {
                tempObj.ignoreRagdollTrans.Clear();
                tempObj.ignoreRagdollTrans = null;
            }

            tempObj.ignoreRagdollTrans = new List<Transform>();
            if (copyParam[i].ignoreRagdollTrans != null)
            {
                for (int j = 0; j < copyParam[i].ignoreRagdollTrans.Count; j++)
                {
                    if (copyParam[i].ignoreRagdollTrans[j] != null)
                    {
                        var ignoreName = copyParam[i].ignoreRagdollTrans[j].name;
                        var ignoreTrans = target.transform.FindRecursive(ignoreName);
                        tempObj.ignoreRagdollTrans.Add(ignoreTrans);
                    }
                }
            }

            //Force Params
            if (tempObj.forceParams != null)
            {
                tempObj.forceParams.Clear();
                tempObj.forceParams = null;
            }

            tempObj.forceParams = new List<Dismemberment.ForceParams>(copyParam[i].forceParams.Count);
            if (copyParam[i].forceParams != null)
            {
                for (int j = 0; j < copyParam[i].forceParams.Count; j++)
                {
                    tempObj.forceParams.Add(new Dismemberment.ForceParams());
                    tempObj.forceParams[j].drag = copyParam[i].forceParams[j].drag;
                    tempObj.forceParams[j].angleDrag = copyParam[i].forceParams[j].angleDrag;
                    tempObj.forceParams[j].torque = copyParam[i].forceParams[j].torque;
                    tempObj.forceParams[j].force = copyParam[i].forceParams[j].force;
                }
            }
        }

        target.GetComponent<Dismemberment>().boomParam = targetParam;
    }

    public void CreateWithDefaultParam()
    {
        if (useDismember)
        {
            dismember.InitDefaultParam();
        }
    }

    void OnGUI()
    {
        if (normalStyle == null)
        {
            normalStyle = new GUIStyle();
            normalStyle.normal.textColor = Color.red;
        }

        if (particleStyle == null)
        {
            particleStyle = new GUIStyle();
            particleStyle.normal.textColor = Color.green;
        }

        if (ragdollStyle == null)
        {
            ragdollStyle = new GUIStyle();
            ragdollStyle.normal.textColor = Color.white;
        }

        targetObj = (GameObject)EditorGUILayout.ObjectField("目标角色", targetObj, typeof(GameObject), true);
        if (targetObj != null)
        {
            ////init
            if (orignObj == null)
            {
                initParams(targetObj);
                orignObj = targetObj;
                return;
            }

            if (targetObj != orignObj)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayoutEx.Button("更换模型", Color.red))
                {
                    initParams(targetObj);
                    orignObj = targetObj;
                }

                if (GUILayoutEx.Button("恢复模型", Color.green))
                {
                    targetObj = orignObj;
                }
                EditorGUILayout.EndHorizontal();
                return;
            }

            useDismember = EditorGUILayout.Toggle("是否使用Dismember", useDismember);
            if (useDismember)
            {
                //////init components
                if (dismember == null)
                {
                    if (targetObj.GetComponent<Dismemberment>() == null)
                    {
                        dismember = targetObj.AddComponent<Dismemberment>();
                        CreateWithDefaultParam();
                    }
                    else
                    {
                        dismember = targetObj.GetComponent<Dismemberment>();
                    }
                    initParams(targetObj);
                }
            }

            if (ragdollManager == null)
            {
                if (targetObj.GetComponent<RagdollManagerHum>() == null)
                {
                    ragdollManager = targetObj.AddComponent<RagdollManagerHum>();
                    ragdollManager.onCreateCompeleted = CreateWithDefaultParam;
                    ragdollManager.DefaultInitialize();
                }
                else
                {
                    ragdollManager = targetObj.GetComponent<RagdollManagerHum>();
                }

                initParams(targetObj);
            }

            //Copy from copyTarget
            copyTarget = (GameObject)EditorGUILayout.ObjectField("拷贝角色", copyTarget, typeof(GameObject), true);

            if (copyTarget != null && copyTarget.GetComponent<Dismemberment>() != null
                && copyTarget.GetComponent<RagdollManagerHum>() != null && isCopying == false)
            {
                if (GUILayoutEx.Button("拷贝参数", Color.yellow))
                {
                    isCopying = true;
                }
            }
            if (isCopying == true)
            {
                EditorGUILayout.BeginHorizontal();
                if (GUILayoutEx.Button("确认拷贝", Color.red))
                {
                    CopyParamFromTarget(targetObj, copyTarget);
                    initParams(targetObj);
                    copyParam = true;
                    isCopying = false;
                }
                if (GUILayoutEx.Button("恢复", Color.green))
                {
                    isCopying = false;
                }
                EditorGUILayout.EndHorizontal();
                return;
            }

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            if (useDismember)
            {
                //////Dismember
                GUILayout.Label("肢解设置", EditorStyles.boldLabel);

                limbTransRoot = (Transform)EditorGUILayout.ObjectField("肢解根节点:", limbTransRoot, typeof(Transform), true);
                dismember.LimbRoot = limbTransRoot;

                dismemberNum = EditorGUILayout.IntField("肢解方案个数：", dismemberNum);
                if (dismemberNum != dismember.BoomParamsNum && !copyParam)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayoutEx.Button("确定更改方案数", Color.red)) dismember.BoomParamsNum = dismemberNum;
                    if (GUILayoutEx.Button("恢复方案数", Color.green)) dismemberNum = dismember.BoomParamsNum;
                    EditorGUILayout.EndHorizontal();
                }
                //Scroll Dismember
                else
                {
                    copyParam = false;
                    if (dismemberNum > 0)
                    {
                        var boomParam = dismember.boomParam;
                        for (int i = 0; i < dismemberNum; i++)
                        {
                            string normalText = string.Format("-------------------------方案{0}-------------------------", i + 1);
                            EditorGUILayout.LabelField(normalText, normalStyle);
                            var temp = boomParam[i];
                            dismemberTarget[i] = (GameObject)EditorGUILayout.ObjectField("被肢解目标：", dismemberTarget[i], typeof(GameObject), true);
                            temp.targetObj = dismemberTarget[i];
                            dismemberColliders[i] = (Collider)EditorGUILayout.ObjectField("肢解碰撞体：", dismemberColliders[i], typeof(Collider), true);
                            temp.collider = dismemberColliders[i];
                            dismemberRigidbodys[i] = (Rigidbody)EditorGUILayout.ObjectField("被肢解目标：", dismemberRigidbodys[i], typeof(Rigidbody), true);
                            temp.rigidbody = dismemberRigidbodys[i];
                            bodyPart[i] = (Dismemberment.BodyPartHint)EditorGUILayout.EnumPopup("肢解部位:", bodyPart[i]);
                            temp.bodyPart = (Dismemberment.BodyPartHint)bodyPart[i];

                            //ignore Ragdoll Effect
                            ignoreRagdollNum[i] = EditorGUILayout.IntField("忽略布娃娃效果个数：", ignoreRagdollNum[i]);

                            if (ignoreRagdollNum[i] != temp.IgnoreRagdollEffectNum)
                            {
                                EditorGUILayout.BeginHorizontal();
                                if (GUILayoutEx.Button("确定更改", Color.red)) temp.IgnoreRagdollEffectNum = ignoreRagdollNum[i];
                                if (GUILayoutEx.Button("恢复", Color.green)) ignoreRagdollNum[i] = temp.IgnoreRagdollEffectNum;
                                EditorGUILayout.EndHorizontal();
                            }
                            else
                            {
                                copyParam = false;
                                if (ignoreRagdollNum[i] > 0)
                                {
                                    for (int j = 0; j < ignoreRagdollNum[i]; j++)
                                    {
                                        string particleText = string.Format("-------------------------忽略{0}-------------------------", j + 1);
                                        var ignoreRagdollParam = temp.ignoreRagdollTrans[j];
                                        ignoreRagdollTrans[dismemberParamNum * i + j] = (Transform)EditorGUILayout.ObjectField("忽略布娃娃效果：", ignoreRagdollTrans[dismemberParamNum * i + j], typeof(Transform), true);
                                        ignoreRagdollParam = ignoreRagdollTrans[dismemberParamNum * i + j];
                                    }
                                }
                            }

                            //particle
                            particleNum[i] = EditorGUILayout.IntField("特效个数：", particleNum[i]);

                            if (particleNum[i] != temp.ParticleNum && !copyParam)
                            {
                                EditorGUILayout.BeginHorizontal();
                                if (GUILayoutEx.Button("确定更改特效数", Color.red)) temp.ParticleNum = particleNum[i];
                                if (GUILayoutEx.Button("恢复特效数", Color.green)) particleNum[i] = temp.ParticleNum;
                                EditorGUILayout.EndHorizontal();
                            }
                            else
                            {
                                copyParam = false;
                                if (particleNum[i] > 0)
                                {
                                    for (int j = 0; j < particleNum[i]; j++)
                                    {
                                        string particleText = string.Format("-------------------------特效{0}-------------------------", j + 1);
                                        EditorGUILayout.LabelField(particleText, particleStyle);
                                        var particleParm = temp.particles[j];
                                        particleNames[dismemberParamNum * i + j] = EditorGUILayout.TextField("特效名称", particleNames[dismemberParamNum * i + j]);
                                        particleParm.ParticleName = particleNames[dismemberParamNum * i + j];
                                        particleRoot[dismemberParamNum * i + j] = (Transform)EditorGUILayout.ObjectField("特效起始位置父节点：", particleRoot[dismemberParamNum * i + j], typeof(Transform), true);
                                        particleParm.particleRoot = particleRoot[dismemberParamNum * i + j];
                                        initPos[dismemberParamNum * i + j] = EditorGUILayout.Vector3Field("特效初始位置：", initPos[dismemberParamNum * i + j]);
                                        particleParm.initPos = initPos[dismemberParamNum * i + j];
                                        initEurler[dismemberParamNum * i + j] = EditorGUILayout.Vector3Field("特效初始旋转位置：", initEurler[dismemberParamNum * i + j]);
                                        particleParm.initEurler = initEurler[dismemberParamNum * i + j];
                                        initScale[dismemberParamNum * i + j] = EditorGUILayout.Vector3Field("特效初始缩放：", initScale[dismemberParamNum * i + j]);
                                        particleParm.initScale = initScale[dismemberParamNum * i + j];
                                    }
                                }
                            }

                            //forceParam
                            forceNum[i] = EditorGUILayout.IntField("力参数个数：", forceNum[i]);

                            if (forceNum[i] != temp.ForceNum && !copyParam)
                            {
                                EditorGUILayout.BeginHorizontal();
                                if (GUILayoutEx.Button("确定更改特效数", Color.red)) temp.ForceNum = forceNum[i];
                                if (GUILayoutEx.Button("恢复特效数", Color.green)) forceNum[i] = temp.ForceNum;
                                EditorGUILayout.EndHorizontal();
                            }
                            else
                            {
                                if (forceNum[i] > 0)
                                {
                                    for (int j = 0; j < forceNum[i]; j++)
                                    {
                                        string forceParamText = string.Format("-------------------------力参数{0}-------------------------", j + 1);
                                        EditorGUILayout.LabelField(forceParamText, particleStyle);
                                        var forceParam = temp.forceParams[j];
                                        drag[dismemberParamNum * i + j] = EditorGUILayout.FloatField("摩擦力：", drag[dismemberParamNum * i + j]);
                                        forceParam.drag = drag[dismemberParamNum * i + j];
                                        angleDrag[dismemberParamNum * i + j] = EditorGUILayout.FloatField("角动量摩擦力：", angleDrag[dismemberParamNum * i + j]);
                                        forceParam.angleDrag = angleDrag[dismemberParamNum * i + j];
                                        force[dismemberParamNum * i + j] = EditorGUILayout.Vector3Field("速度：", force[dismemberParamNum * i + j]);
                                        forceParam.force = force[dismemberParamNum * i + j];
                                        torque[dismemberParamNum * i + j] = EditorGUILayout.Vector3Field("角速度：", torque[dismemberParamNum * i + j]);
                                        forceParam.torque = torque[dismemberParamNum * i + j];
                                    }
                                }
                            }


                            EditorGUILayout.BeginHorizontal();
                            forceMethod = EditorGUILayout.IntField("肢解力方案选择：1~10:", forceMethod);
                            if (GUILayoutEx.Button("测试肢解", Color.yellow)) dismember.TestDismember(i * 10 + forceMethod - 1);
                            //if (GUILayoutEx.Button("恢复", Color.green)) dismember.ReCoverOne(i * 10 + forceMethod - 1);
                            EditorGUILayout.EndHorizontal();
                        }

                    }
                }
            }

            //////Ragdoll
            string ragdollText = string.Format("-------------------------布娃娃系统-------------------------");
            EditorGUILayout.LabelField(ragdollText, ragdollStyle);

            ragdollId = EditorGUILayout.IntField("布娃娃受力点：", ragdollId);
            ragdollForce = EditorGUILayout.Vector3Field("布娃娃受力方向：", ragdollForce);

            EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button("测试布娃娃"))
            {
                ragdollManager.DisableCustomRagdoll();
                var anim = targetObj.GetComponent<Animation>();
                if (anim != null)
                {
                    anim.enabled = false;
                }

                var characterControl = targetObj.GetComponent<CharacterController>();
                if (characterControl != null)
                {
                    characterControl.enabled = false;
                }

                ragdollManager.CustomRagdoll(ragdollId, ragdollForce);
            }

            if (GUILayout.Button("恢复布娃娃"))
            {
                ragdollManager.DisableCustomRagdoll();

                var anim = targetObj.GetComponent<Animation>();
                if (anim != null)
                {
                    anim.enabled = true;
                    anim.Play("idle");
                }

                var characterControl = targetObj.GetComponent<CharacterController>();
                if (characterControl != null)
                {
                    characterControl.enabled = true;
                }
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            //综合方案
            method = EditorGUILayout.IntField("方案选择 ： 11~90:", method);
            if (GUILayout.Button("走你"))
            {
                ragdollManager.DisableCustomRagdoll();
                var anim = targetObj.GetComponent<Animation>();
                if (anim != null)
                {
                    anim.enabled = false;
                }

                var characterControl = targetObj.GetComponent<CharacterController>();
                if (characterControl != null)
                {
                    characterControl.enabled = false;
                }

                dismember.TestDismember(Mathf.Clamp(method - 10 - 1, 0, 79));

                ragdollManager.CustomRagdoll(ragdollId, ragdollForce);
            }

            if (GUILayout.Button("恢复"))
            {
                ragdollManager.DisableCustomRagdoll();

                var anim = targetObj.GetComponent<Animation>();
                if (anim != null)
                {
                    anim.enabled = true;
                    anim.Play("idle");
                }

                var characterControl = targetObj.GetComponent<CharacterController>();
                if (characterControl != null)
                {
                    characterControl.enabled = true;
                }

            }

            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }
    }
}