using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test02_MagnetCatch : Skill_TestBase
{
    public MagnetCatch magnet;
    Skill_Player player;

    private void Start()
    {
        player = GameManager.Instance.Player;
    }
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        //player.SkillController.startSkill?.Invoke();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        player.SkillController.useSkillAction?.Invoke();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        player.SkillController.offSkillAction?.Invoke();
    }

}
