using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : RecycleObject
{
    [Header("적 기본 데이터")]
    /// <summary>
    /// 이동 속도
    /// </summary>
    public float moveSpeed = 1.0f;

    public float damage = 20.0f;

    /// <summary>
    /// 적의 HP
    /// </summary>
    float hp = 1;

    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            if (hp <= 0) // HP가 0 이하가 되면 죽는다.
            {
                hp = 0;
                OnDie();
            }
        }
    }

    /// <summary>
    /// 최대 HP
    /// </summary>
    public float maxHP = 100.0f;

    /// <summary>
    /// 적이 죽을 때 실행될 델리게이트
    /// </summary>
    Action onDie;

    /// <summary>
    /// 점수를 줄 플레이어
    /// </summary>
    Player player;


    protected override void OnEnable()
    {
        base.OnEnable();
        OnInitialize();     // 적 초기화 작업
    }

    protected override void OnDisable()
    {
        if (player != null)
        {
            onDie = null;               // 확실하게 정리한다고 표시
            player = null;
        }

        base.OnDisable();
    }

    /// <summary>
    /// EnemyWave 계열의 초기화 함수
    /// </summary>
    protected virtual void OnInitialize()
    {
        if (player == null)
        {
            player = GameManager.Instance.Player;   // 플레이어 찾기
        }

        HP = maxHP; // HP 최대로 설정
    }

    /// <summary>
    /// 사망 처리용 함수
    /// </summary>
    protected virtual void OnDie()
    {
        onDie?.Invoke();                // 죽었다는 신호보내기

        gameObject.SetActive(false);    // 자기 자신 비활성화
    }

}
