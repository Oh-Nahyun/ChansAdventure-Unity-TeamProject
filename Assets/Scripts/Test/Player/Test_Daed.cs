using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_Daed : TestBase
{
    public Player player;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        player = FindAnyObjectByType<Player>();
        player.onDie?.Invoke();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        player = FindAnyObjectByType<Player>();
        player.HP = 0f;
    }
}
