using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : RecycleObject
{
    [Header("적 기본 데이터")]
    // 최대 체력
    public float maxHealth;

    // 현재 체력 프로퍼티
    private float _currentHealth;
    public float CurrentHealth
    {
        get { return _currentHealth; }
        set
        {
            // 현재 체력이 최대 체력을 넘지 않도록 보정
            _currentHealth = Mathf.Clamp(value, 0f, maxHealth);

            // 체력이 0 이하로 떨어졌을 때 죽음 처리
            if (_currentHealth <= 0f)
            {
                Die();
            }
        }
    }

    // 데미지
    public float damage;

    // 배회 상태일 때 이동 속도
    public float patrollingSpeed;

    // 추격 상태일 때 이동 속도
    public float chasingSpeed;

    // 공격 가능 쿨타임
    public float attackCooldown;

    // 추격 사거리
    public float chaseRange;

    // 공격 사거리
    public float attackRange;

    // 죽었음을 알리는 델리게이트
    public Action onDeath;

    private void Awake()
    {
        _currentHealth = maxHealth;
    }

    // 죽음 처리 메서드
    private void Die()
    {
        // 죽었음을 알리는 델리게이트 호출
        onDeath?.Invoke();
        // 적 캐릭터 게임 오브젝트 비활성화 또는 제거 등의 적절한 처리
    }
}
