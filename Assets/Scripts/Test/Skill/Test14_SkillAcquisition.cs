using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test14_SkillAcquisition : TestBase
{
    public SkillName skillName;
    public SkillWindowUI skillWindowUI;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.RemoteBomb);
        GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.RemoteBomb_Cube);
        Debug.Log("리모컨폭탄 획득");
    }
    protected override void OnTest2(InputAction.CallbackContext context)
    {
        GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.MagnetCatch);
        Debug.Log("마그넷캐치 획득");
    }
    protected override void OnTest3(InputAction.CallbackContext context)
    {
        GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.IceMaker);
        Debug.Log("아이스메이커 획득");
    }
    protected override void OnTest4(InputAction.CallbackContext context)
    {
        GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.TimeLock);
        Debug.Log("타임록획득");
    }


}
