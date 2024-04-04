using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PlayerCursorOnOff : TestBase
{
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
