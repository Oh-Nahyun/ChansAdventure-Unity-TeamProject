using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SwordSkeleton : EnemyBase
{
    // NavMeshAgent 컴포넌트
    private NavMeshAgent navMeshAgent;

    // Animator 컴포넌트
    private Animator animator;

    // 추격할 대상
    private Transform target;

    // 공격 가능한지 여부
    private bool canAttack = true;

    // Idle 상태 지속 시간
    public float idleDuration = 5f;

    // 애니메이션 상태 해시코드
    private readonly int isPatrolling_Hash = Animator.StringToHash("IsPatrolling");
    private readonly int isChasing_Hash = Animator.StringToHash("IsChasing");
    private readonly int isAttacking_Hash = Animator.StringToHash("IsAttacking");
    private readonly int isDamaged_Hash = Animator.StringToHash("IsDamaged");

    // 추격 사거리 내인지 체크하는 메서드
    private bool IsPlayerInRange()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            return distance <= chaseRange;
        }
        return false;
    }

    // 공격 가능한지 체크하는 메서드
    private bool CanAttack()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            return distance <= attackRange;
        }
        return false;
    }

    // 공격 실행 메서드
    private void Attack()
    {
        if (canAttack && CanAttack())
        {
            // 공격 애니메이션 실행
            animator.SetBool(isAttacking_Hash, true);

            // 공격 쿨다운 적용
            canAttack = false;
            Invoke("ResetAttack", attackCooldown);

            // TODO: 공격 처리
        }
    }

    // 공격 쿨다운 리셋 메서드
    private void ResetAttack()
    {
        canAttack = true;
    }

    // 플레이어 추적 메서드
    private void ChasePlayer()
    {
        if (target != null)
        {
            // 플레이어 추적 및 공격
            navMeshAgent.SetDestination(target.position);
            if (IsPlayerInRange())
            {
                // 추적 상태로 변경
                animator.SetBool(isChasing_Hash, true);

                // 추적 상태에서 공격 가능한 경우 공격 실행
                Attack();
            }
        }
    }

    // Start 메서드를 사용하여 초기화
    void Start()
    {
        // NavMeshAgent 컴포넌트 가져오기
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Animator 컴포넌트 가져오기
        animator = GetComponent<Animator>();

        // 체력 초기화
        CurrentHealth = maxHealth;

        // 추적 대상 설정
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // Idle 상태 지속 후 Walk 애니메이션으로 변경
        Invoke("StartPatrolling", idleDuration);
    }

    // Walk 애니메이션으로 변경하여 배회 시작
    private void StartPatrolling()
    {
        // Walk 애니메이션 실행
        animator.SetBool(isPatrolling_Hash, true);
        navMeshAgent.speed = patrollingSpeed;
        // TODO: Waypoints를 순회하며 이동
    }

    // Update 메서드를 사용하여 프레임마다 실행
    void Update()
    {
        // 플레이어 추적 및 공격
        if (IsPlayerInRange())
        {
            // 추적 상태로 변경
            ChasePlayer();
        }
        else
        {
            // 추격 사거리를 벗어나면 Walk 애니메이션으로 변경하여 배회 시작
            animator.SetBool(isChasing_Hash, false);
            animator.SetBool(isPatrolling_Hash, true);
            navMeshAgent.speed = patrollingSpeed;
            // TODO: Waypoints를 순회하며 이동
        }
    }

    // 플레이어의 공격을 받았을 때 호출되는 메서드
    public void TakeDamage(float damageAmount)
    {
        // 공격을 받았을 때 실행할 로직
        CurrentHealth -= damageAmount;
        if (CurrentHealth <= 0)
        {
            // 체력이 0 이하일 때 Die 애니메이션 실행 후 오브젝트 비활성화
            Die();
        }
        else
        {
            // Damage 애니메이션 실행
            animator.SetTrigger(isDamaged_Hash);
            // TODO: Damage 애니메이션 실행
        }
    }

    // 죽음 처리 메서드
    private void Die()
    {
        // Die 애니메이션 실행 후 오브젝트 비활성화
        animator.SetBool(isPatrolling_Hash, false);
        animator.SetBool(isChasing_Hash, false);
        animator.SetTrigger("Die");
        // TODO: Die 애니메이션 실행 후 오브젝트 비활성화
    }
}
