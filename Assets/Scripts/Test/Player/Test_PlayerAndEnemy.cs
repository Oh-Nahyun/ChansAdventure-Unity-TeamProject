using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_PlayerAndEnemy : TestBase
{
    /// <summary>
    /// 플레이어에게 가할 데미지
    /// </summary>
    public float damage = 10;

    // 컴포넌트들
    Player player;
    HeartCheckUI heartCheckUI;
    StaminaCheckUI staminaCheckUI;

    private void Start()
    {
        player = GameManager.Instance.Player;
        heartCheckUI = FindAnyObjectByType<HeartCheckUI>();
        staminaCheckUI = FindAnyObjectByType<StaminaCheckUI>();
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

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        staminaCheckUI.PlusStamina();
        Debug.Log("스태미나 총량 증가 완료");
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        //player.Defence(25 + 3); // DefencePower = 3.0f
        player.Defence(79 + 3);
        Debug.Log($"Player's HP : {player.HP}");
    }
}
