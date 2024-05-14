using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_AllSkillAcquisition : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.RemoteBomb_Cube);
        GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.RemoteBomb);
        Debug.Log("리모컨폭탄 등록");
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.MagnetCatch);
        Debug.Log("마그넷캐치 등록");
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.IceMaker);
        Debug.Log("아이스메이커 등록");
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.TimeLock);
        Debug.Log("타임록 등록");
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        GameManager.Instance.Player.MaxHP += 100;
        GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.RemoteBomb);
        GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.RemoteBomb_Cube);
        GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.MagnetCatch);
        GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.IceMaker);
        GameManager.Instance.Skill.PlayerSkill.SkillAcquisition(SkillName.TimeLock);
    }

}
