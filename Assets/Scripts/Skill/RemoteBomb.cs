using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RemoteBomb : Skill
{
    protected override void OnEnable()
    {
        base.OnEnable();
        //PickUp();
    }

    protected override void OnSKillAction()
    {
        if (currentState == StateType.None)
        {
            Boom();
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

    protected override void CollisionAfterThrow()
    {
        currentState = StateType.None;
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosiveInfo.boomRange);

    }

#endif
}
