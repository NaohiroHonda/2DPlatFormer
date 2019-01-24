using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EventTrigger : MonoBehaviour {

    /// <summary>
    /// 
    /// </summary>
    public enum EventType
    {
        Object,
        Tag,
    }

    /// <summary>
    /// 特定Objectかタグかの指定
    /// </summary>
    [SerializeField]
    private EventType type;

    /// <summary>
    /// ObjectModeで指定するObject
    /// </summary>
    [SerializeField]
    private GameObject targetObj;

    [SerializeField]
    private string tagName;

    private static GameObject player;

    private int delay;

    public GameObject CollidedObj { get; private set; }

    /// <summary>
    /// ジャッジ部で順番を考慮した特殊な処理をしているので順番変更禁止
    /// </summary>
    public enum TriggerStatus
    {
        Enter,
        Stay,
        Exit,
        None
    }

    public TriggerStatus Status { get; private set; }

	// Use this for initialization
	void Start () {
        Status = TriggerStatus.None;

        CollidedObj = null;
	}
	
	// Update is called once per frame
	void Update () {
        //Stay/None時に自発的なUpdateは無い
        if (((int)Status & 1) == 1) return;
        
        //確実に検知させるためのdelay判定
        if(delay != 0)
        {
            Status++;
        }
        else delay++;
	}

    private void OnTriggerEnter2D(Collider2D target)
    {
        if(IsTarget(target))
        {
            UpdateStatus(TriggerStatus.Enter);

            CollidedObj = target.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D target)
    {
        if (IsTarget(target))
        {
            UpdateStatus(TriggerStatus.Exit);

            CollidedObj = target.gameObject;
        }
    }

    private void UpdateStatus(TriggerStatus newStatus)
    {
        Status = newStatus;
        delay = 0;
    }

    private bool IsTarget(Collider2D target)
    {
        GameObject obj = target.gameObject;

        return (
            (type == EventType.Object && obj == targetObj) ||
            (type == EventType.Tag && obj.tag == tagName)
            );
    }
}
