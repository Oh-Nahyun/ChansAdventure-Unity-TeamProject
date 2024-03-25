using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RemoteBomb : Skill
{
    protected override void OnEnable()
    {
        base.OnEnable();
        PickUp();
    }

    public override void OnSkillAction()
    {
        if (currentState == StateType.None)
        {
            DestroyReaction();
        }
        else
        {
            base.OnSkillAction();
        }
    }

    public override void OffSkillAction()
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

    public void TestSkill()
    {
        OnSkillAction();
    }

    public void TestOnSkill()
    {
        UseSkillAction();
    }

    public void TestRemoteOn()
    {
        //RemoteOn();
    }



#endif
}
