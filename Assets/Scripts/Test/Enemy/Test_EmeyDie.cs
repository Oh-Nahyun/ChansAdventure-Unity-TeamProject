using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_EmeyDie : TestBase
{
    SwordSkeleton enemy;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetEnemy();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        enemy.Die();
    }
}
