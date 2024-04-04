using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_CameraZoom : TestBase
{
    protected override void OnTestLClick(InputAction.CallbackContext context)
    {
        Camera.main.fieldOfView += 100.0f;
    }
}

