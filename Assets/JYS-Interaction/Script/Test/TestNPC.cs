using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestNPC : TestBase
{
#if UNITY_EDITOR
    
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        GameManager.Instance.StartTalk();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        GameManager.Instance.NextTalk();
    }

#endif
}
