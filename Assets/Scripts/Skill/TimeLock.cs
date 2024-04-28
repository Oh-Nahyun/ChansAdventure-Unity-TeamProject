using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeLock : Skill
{
    [Header("타임록 데이터")]
    /// <summary>
    /// 타임록에 걸린 오브젝트 색변화용 머티리얼
    /// </summary>
    public Material timeLockColor;
    /// <summary>
    /// 타임록 지속시간
    /// </summary>
    public float duration = 10.0f;

    ReactionObject target;

    bool IsValidTarget => target != null;

    IEnumerator detectObject;

    protected override void Awake()
    {
        base.Awake();
        detectObject = DetectObject();
    }

    protected override void OnEnable()
    {
        StopAllCoroutines();
        base.OnEnable();
    }

    protected override void OnSKillAction()
    {
        base.OnSKillAction();
        StartCoroutine(detectObject);
    }

    protected override void UseSkillAction()
    {
        if (IsValidTarget)
        {
            base.UseSkillAction();
            StopCoroutine(detectObject);
            target.OnTimeLock(timeLockColor, duration);

            OffSkill();
        }
    }

    IEnumerator DetectObject()
    {
        while (true)
        {
            Ray ray = Camera.main.ViewportPointToRay(Center);
            if(Physics.Raycast(ray, out RaycastHit hitInfo, skillDistance))
            {
                target = hitInfo.transform.GetComponent<ReactionObject>();
                // TODO: 나중에 쉐이더 씌우기
            }
            yield return null;
        }
    }



    
}
