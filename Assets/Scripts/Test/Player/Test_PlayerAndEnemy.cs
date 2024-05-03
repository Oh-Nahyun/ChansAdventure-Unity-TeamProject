using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PlayerAndEnemy : TestBase
{
    public float damage = 10;

    Player player;

    HeartCheckUI heartCheckUI;

    private void Start()
    {
        player = GameManager.Instance.Player;
        heartCheckUI = FindAnyObjectByType<HeartCheckUI>();
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

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        heartCheckUI.PlusHeart();
        Debug.Log("하트 이미지 배열 크기 증가 완료");
    }
}
