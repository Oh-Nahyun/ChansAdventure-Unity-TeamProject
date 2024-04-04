using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Arrow : TestBase
{
    public float arrowSpeed = 7.0f;

    Arrow arrow;

    private void Awake()
    {
        arrow = GetComponent<Arrow>();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        transform.Translate(Time.deltaTime * arrowSpeed * Vector3.up); // 화살 발사
    }
}
