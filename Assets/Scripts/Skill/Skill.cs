using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SkillName
{
    RemoteBomb = 1,
    RemoteBomb_Cube = 2,
    MagnetCatch = 3,
    TimeLock = 4,
    IceMaker = 5
}

public class Skill : ReactionObject
{
    public SkillName skillName = SkillName.RemoteBomb;
    public float coolTime = 1.0f;
    public float CoolTime => coolTime;
    protected float currCoolTime = 0.0f;
    public float CurrentCoolTime => currCoolTime;
    public bool canUse = false;

    protected Player owner;

    protected override void OnEnable()
    {
        base.OnEnable();
        if(owner == null)
        {
            owner = GameManager.Instance.Player;
        }




    }

    protected override void OnDisable()
    {
        base.OnDisable();

        owner.SkillController.onSKillAction = null;
        owner.SkillController.useSkillAction = null;
        owner.SkillController.offSkillAction = null;

    }

    public virtual void OnSkillAction()
    {
        owner.SkillController.onSKillAction = OnSkillAction;
        owner.SkillController.useSkillAction = UseSkillAction;
        owner.SkillController.offSkillAction = OffSkillAction;

        transform.parent = owner.SkillController.HandRoot;
        transform.position = owner.SkillController.HandRoot.position;
        transform.forward = owner.transform.forward;
    }

    public virtual void UseSkillAction()
    {

    }

    public virtual void OffSkillAction()
    {

    }
}
