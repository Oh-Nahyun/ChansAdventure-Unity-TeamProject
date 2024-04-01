using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    protected bool isActivate = false;

    protected Skill_Player owner;
    protected SkillVCam skillVcam;

    protected Action camOn;
    protected Action camOff;

    protected override void Awake()
    {
        base.Awake();
        rigid.isKinematic = true;
    }

    protected virtual void Start()
    {
        if (owner == null)
        {
            owner = GameManager.Instance.Player;
        }
        if (skillVcam == null)
        {
            skillVcam = GameManager.Instance.Cam.SkillCam;
        }
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if(owner == null)
        {
            owner = GameManager.Instance.Player;
        }
        if (skillVcam != null)
        {
            camOn = skillVcam.OnSkillCamera;
            camOff = skillVcam.OffSkillCamera;
        }
        else
        {
            skillVcam = GameManager.Instance.Cam.SkillCam;
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        isActivate = false;
    }


    public void OnSkill()
    {
        if (!isActivate)
        {
            OnSKillAction();
        }
    }

    protected virtual void OnSKillAction()
    {
        camOn?.Invoke();

    }

    public void UseSkill()
    {
        if (!isActivate)
        {
            UseSkillAction();
        }
    }

    protected virtual void UseSkillAction()
    {
        camOff?.Invoke();
        isActivate = true;

    }

    public void OffSkill()
    {
        OffSKillAction();
    }

    protected virtual void OffSKillAction()
    {
        camOff?.Invoke();
        isActivate = false;
        ReturnToPool();

    }
}
