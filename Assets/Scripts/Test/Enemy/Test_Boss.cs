using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Boss : TestBase
{
    public Boss boss;

    public float hpValue;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        boss.HP -= hpValue;
        Debug.Log(boss.HP);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
    }
}
