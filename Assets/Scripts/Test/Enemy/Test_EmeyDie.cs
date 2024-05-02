using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_EmeyDie : TestBase
{
    SwordSkeleton enemy;

    private void Start()
    {
        enemy = FindAnyObjectByType<SwordSkeleton>();
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetEnemy();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        enemy.Defence(100000.0f);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        Factory.Instance.GetNightmareDragonEnemy();
    }
}
