using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PlayerAndEnemy : TestBase
{
    public float damage = 10;
    Player player;

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        //Factory.Instance.GetEnemy(new Vector3(0.0f, 0.0f, 0.0f), 0.0f);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        player.Defence(damage);
        Debug.Log($"Player's HP : {player.HP}");
    }
}
