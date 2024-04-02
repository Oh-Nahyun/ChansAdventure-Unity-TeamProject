using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestNPC : TestBase
{

    ChestBase chestBase;
#if UNITY_EDITOR

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        GameManager_JYS.Instance.StartTalk();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        GameManager_JYS.Instance.NextTalk();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        GameManager_JYS.Instance.OpenChest();
        //GameManager_JYS.Instance.IsNPCObj();
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {

    }

#endif
}
