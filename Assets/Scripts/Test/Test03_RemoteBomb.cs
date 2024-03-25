using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test03_RemoteBomb : TestBase
{
#if UNITY_EDITOR
    RemoteBomb bomb;

    private void Start()
    {
    }
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        bomb = SkillFactory.Instance.GetRemoteBomb();
        bomb.TestSkill();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        bomb.TestOnSkill();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        bomb.TestRemoteOn();
    }
#endif
}
