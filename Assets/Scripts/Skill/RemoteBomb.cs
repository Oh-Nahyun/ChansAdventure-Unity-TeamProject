using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RemoteBomb : Skill
{

    protected override void Awake()
    {
        base.Awake();
        skillName = SkillName.RemoteBomb;

    }

    protected override void OnSKillAction()
    {
        //if (currentState == StateType.Throw || currentState == StateType.Drop)
        if (currentState == StateType.None || currentState == StateType.Throw)
        {
            cooltime = 0;
            TryBoom();
        }
    }

    protected override void UseSkillAction()
    {
        // 아무행동도 안하기
    }

    protected override void OffSKillAction()
    {
        if(currentState == StateType.PickUp)
        {
            ReturnToPool();
        }
    }

    protected override void CollisionActionAfterThrow()
    {
        currentState = StateType.None;  // 현재 상태를 기본 상태로
        //currentState = StateType.Throw;
    }

    protected override void CollisionActionAfterDrop()
    {
        currentState = StateType.None;  // 현재 상태를 기본 상태로
        //currentState = StateType.Drop;
    }

    protected override void BoomAction()
    {
        cooltimeReset?.Invoke(skillName);
        base.BoomAction();
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosiveInfo.boomRange);

    }

#endif
}
