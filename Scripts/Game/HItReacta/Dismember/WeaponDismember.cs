using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FrameWork;

public class WeaponDismember : MonoBehaviour {
    public Transform LimbRoot;
    public CapsuleCollider collider;
    public Rigidbody rigidbody;
    public Transform parentTrans;
    public Vector3 pos;
    public Quaternion rot;
    public Vector3 scale;

    private void Start()
    {
        collider.enabled = false;
        rigidbody.isKinematic = true;
        rigidbody.useGravity = true;
        parentTrans = transform.parent;
        pos = transform.localPosition;
        rot = transform.localRotation;
        scale = transform.localScale;
    }

    public void Do()
    {
        StartCoroutine("DelayWeaponDismember");
    }

    public void UnDo()
    {
        collider.enabled = false;
        rigidbody.isKinematic = true;
        transform.SetParent(parentTrans);
        transform.localPosition = pos;
        transform.localRotation = rot;
        transform.localScale = scale;
    }

    private IEnumerator DelayWeaponDismember()
    {
        yield return new WaitForFixedUpdate();
        yield return new WaitForSeconds(Global.RandomRange(0, 0.8f));
        transform.SetParent(LimbRoot);
        rigidbody.isKinematic = false;
        collider.enabled = true;
    }

    [ContextMenu("WeaponDismemberSet")]
    protected void WeaponSet()
    {
        collider = transform.GetComponent<CapsuleCollider>();
        if(collider == null)
        {
            collider = gameObject.AddComponent<CapsuleCollider>();
            collider.center = new Vector3(-0.4f, 0, 0);
            collider.radius = 0.05f;
            collider.height = 1.0f;
            collider.direction = 0;
        }

        rigidbody = transform.GetComponent<Rigidbody>();
        if(rigidbody == null)
        {
            rigidbody = gameObject.AddComponent<Rigidbody>();
        }

        var temp = transform.parent;
        while(temp != null)
        {
            if (temp.name == "Bip001")
            {
                break;
            }
            temp = temp.parent;
        }

        if(temp != null && temp.name == "Bip001")
        {
            LimbRoot = temp.parent.FindRecursive("Limb");
        }

        parentTrans = transform.parent;
        pos = transform.localPosition;
        rot = transform.localRotation;
        scale = transform.localScale;

    }

}
