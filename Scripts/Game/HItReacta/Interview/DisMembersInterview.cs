#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif

/// <summary>
/// 能让字段在inspect面板显示中文字符
/// </summary>
[AttributeUsage(AttributeTargets.Field)]
public class FieldLabelAttribute : PropertyAttribute
{
    public string label;//要显示的字符
    public FieldLabelAttribute(string label)
    {
        this.label = label;
        //获取你想要绘制的字段（比如"技能"）
    }

}

//绑定特性描述类
[CustomPropertyDrawer(typeof(FieldLabelAttribute))]
public class FieldLabelDrawer : PropertyDrawer
{
    private FieldLabelAttribute FLAttribute
    {
        get { return (FieldLabelAttribute)attribute; }
        ////获取你想要绘制的字段
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //在这里重新绘制
        EditorGUI.PropertyField(position, property, new GUIContent(FLAttribute.label), true);

    }
}

public class DisMembersInterview : MonoBehaviour
{

    [FieldLabel("肢解脚本")]
    public Dismemberment dismember;
    public RagdollManagerHum ragdollManager;

    [FieldLabel("")]
    public int boomId;

    //disable Animation
    public Transform unusedTrans;
    private Animation anim;
    private CharacterController charactorControl;

    private RagdollManagerHum ragdoll;
    public int RagdollId;
    public Vector3 ragdollForce;


    public void Start()
    {
        //anim = GetComponent<Animation>();
        //charactorControl = GetComponent<CharacterController>();
        //anim.Play("idle");
        ////if (anim != null) anim.enabled = false;
        //if (charactorControl != null) charactorControl.enabled = false;

        //ragdoll = GetComponent<RagdollManagerHum>();
        //if (ragdoll == null)
        //{
        //    ragdoll = gameObject.AddComponent<RagdollManagerHum>();
        //}

        //dismember = GetComponent<Dismemberment>();
        //if (dismember == null)
        //{
        //    dismember = gameObject.AddComponent<Dismemberment>();
        //}

        StartCoroutine("BeginTest");
    }

    public IEnumerator BeginTest()
    {
        yield return new WaitForSeconds(2.0f);

        yield return new WaitForSeconds(3.0f);
        ragdollManager.DefaultInitialize();
        yield return new WaitForSeconds(2.0f);
        //anim.enabled = true;
        //anim.Play("run");
        //anim.Stop();
        //anim.enabled = false;
        //dismember.DismemberOne(0);
        //yield return new WaitForFixedUpdate();
        //yield return new WaitForFixedUpdate();
        //yield return new WaitForFixedUpdate();
        //ragdoll.CustomRagdoll(new int[] { 0 }, new Vector3(-10, 0, -10));
        //yield return new WaitForSeconds(3.0f);


        //anim.enabled = true;
        //dismember.ReCoverDefault();
        //ragdoll.DisableCustomRagdoll();
        ////anim.Play("run");
        ////ragdoll.transform.SetParent(unusedTrans);

        ////ragdoll.DisableCustomRagdoll();

        //yield return new WaitForFixedUpdate();
        //yield return new WaitForFixedUpdate();
        //yield return new WaitForFixedUpdate();
        ////yield return new WaitForFixedUpdate();
        ////dismember.disableGame.SetActive(true);

        ////        yield return new WaitForSeconds(0.4f);
        ////dismember.disableGame.SetActive(true);
    }



    private void OnGUI()
    {

        if (GUI.Button(new Rect(0, 0, 50, 50), "走"))
        {
            anim.Play("run");
        }

        if (GUI.Button(new Rect(100, 0, 50, 50), "肢解"))
        {
            dismember.DismemberOne(boomId, 0);
        }

        if (GUI.Button(new Rect(200, 0, 50, 50), "粉粹"))
        {
            dismember.Crush(0);
        }

        if (GUI.Button(new Rect(300, 0, 50, 50), "Stand"))
        {
            anim.Play("idle");
        }

        if (GUI.Button(new Rect(0, 100, 50, 50), "恢复单一"))
        {
            dismember.ReCoverOne(boomId);
        }

        if (GUI.Button(new Rect(100, 100, 50, 50), "恢复所有"))
        {
            dismember.ReCoverDefault();
        }

        if (GUI.Button(new Rect(200, 100, 50, 50), "Ragdoll"))
        {
            anim.Stop();
            ragdoll.CustomRagdoll(RagdollId, ragdollForce);
        }

        if (GUI.Button(new Rect(300, 100, 50, 50), "解绑Ragdoll"))
        {
            ragdoll.DisableCustomRagdoll();
        }

        if (GUI.Button(new Rect(400, 0, 50, 50), "方案1"))
        {
            anim.Stop();
            dismember.DismemberOne(boomId, 0);
            ragdoll.CustomRagdoll(RagdollId, ragdollForce);
            //StartCoroutine("RunCustomRag");
        }

    }

    public IEnumerator RunCustomRag()
    {
        yield return new WaitForSeconds(0.1f);
        dismember.DismemberOne(boomId, 0);
        //        ragdoll.CustomRagdoll(RagdollId, ragdollForce);
    }
}
#endif
