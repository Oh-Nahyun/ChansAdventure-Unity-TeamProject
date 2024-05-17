using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_TimeLock : TestBase
{
#if UNITY_EDITOR

    public ReactionObject target;
    public ReactionObject bomb;
    Vector3 targetPos;

    void Start()
    {
        targetPos = target.transform.position;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Instantiate(bomb);
        Vector3 pos = targetPos;
        pos.x -= 0.5f;
        bomb.transform.position = pos;
        bomb.TestBoom();
    }
    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Instantiate(bomb);
        Vector3 pos = targetPos;
        pos.x += 0.5f;
        bomb.transform.position = pos;
        bomb.TestBoom();
    }
    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Instantiate(bomb);
        Vector3 pos = targetPos;
        pos.z += 0.5f;
        bomb.transform.position = pos;
        bomb.TestBoom();
    }
    protected override void OnTest4(InputAction.CallbackContext context)
    {
        Instantiate(bomb);
        Vector3 pos = targetPos;
        pos.z -= 0.5f;
        bomb.transform.position = pos;
        bomb.TestBoom();
    }
    protected override void OnTest5(InputAction.CallbackContext context)
    {
        Instantiate(bomb);
        Vector3 pos = targetPos;
        pos.y -= 0.5f;
        bomb.transform.position = pos;
        bomb.TestBoom();
    }

#endif
}
