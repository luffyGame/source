/* @Description 触发区组件
 * @Auhtor SunShubin
 * @2018-05-14
 */

using UnityEngine;
using System;
using Game;

[RequireComponent(typeof(Rigidbody))]
public class EventTriggerComponent : MonoBehaviour
{
    public Action<int, int> enterEvent;
    public Action<int, int> exitEvent;
#if UNITY_EDITOR
    private Rigidbody _rigidbody;

    private void Reset()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.isKinematic = true;
    }

    public static EventTriggerComponent CreateEventTrigger(TriggerInfo triggerInfo, ColliderType colliderType,
        Transform root)
    {
        var go = new GameObject(triggerInfo.ID.ToString());
        go.transform.SetParent(root);
        go.transform.position = triggerInfo.pos;
        var eventTriggerComponent = go.AddComponent<EventTriggerComponent>();
        eventTriggerComponent.triggerID = triggerInfo.ID;
        eventTriggerComponent.mType = colliderType;
        switch (colliderType)
        {
            case ColliderType.Player:
                go.layer = Const.LAYER_AREA_TRIGGER_PLAYER;
                break;
            case ColliderType.Monster:
                go.layer = Const.LAYER_AREA_TRIGGER_MONSTER;
                break;
        }

        Collider collider = null;
        switch (triggerInfo.mTriggerType)
        {
            case TriggerInfo.TriggerType.Cube:
                collider = go.AddComponent<BoxCollider>();
                ((BoxCollider) collider).size = triggerInfo.scale;
                break;
            case TriggerInfo.TriggerType.Capsule:
                collider = go.AddComponent<CapsuleCollider>();
                ((CapsuleCollider) collider).radius = triggerInfo.scale.x * 0.5f;
                ((CapsuleCollider) collider).height = triggerInfo.scale.y;
                break;
        }

        if (collider != null)
        {
            collider.isTrigger = true;
        }

        return eventTriggerComponent;
    }
#endif

    public enum ColliderType
    {
        Player = 0,
        Monster = 1,
    }

    public int triggerID = 0;

    public ColliderType mType = ColliderType.Player;

    public void RegisterEvent(Action<int, int> enterEvent, Action<int, int> exitEvent)
    {
        this.enterEvent = enterEvent;
        this.exitEvent = exitEvent;
    }


    private void OnTriggerEnter(Collider other)
    {
        var objCollider = other.GetComponent<ObjCollider>();
        if (objCollider == null)
        {
            return;
        }

        if (enterEvent != null)
        {
            enterEvent(triggerID, objCollider.GetId());
        }
    }
    

    private void OnTriggerExit(Collider other)
    {
        var objCollider = other.GetComponent<ObjCollider>();
        if (objCollider == null)
        {
            return;
        }

        if (exitEvent != null)
        {
            exitEvent(triggerID, objCollider.GetId());
        }
    }

    private void OnDisable()
    {
        enterEvent = null;
        exitEvent = null;
    }
}