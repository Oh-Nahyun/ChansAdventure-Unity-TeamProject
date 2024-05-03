using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Test_SwordSkeleton : EnemyBase
{
    public Waypoints waypoints;

    // NavMeshAgent ������Ʈ
    private NavMeshAgent navMeshAgent;

    // Animator ������Ʈ
    private Animator animator;

    // �߰��� ���
    private Transform target;

    // ���� �������� ����
    private bool canAttack = true;

    // Idle ���� ���� �ð�
    public float idleDuration = 5f;

    // �ִϸ��̼� ���� �ؽ��ڵ�
    private readonly int Idle_Hash = Animator.StringToHash("Idle");
    private readonly int Patrol_Hash = Animator.StringToHash("Patrol");
    private readonly int Chase_Hash = Animator.StringToHash("Chase");
    private readonly int Attack_Hash = Animator.StringToHash("Attack");
    private readonly int Gethit_Hash = Animator.StringToHash("Gethit");
    private readonly int Die_Hash = Animator.StringToHash("Die");

    // SwordPoint ������Ʈ
    private GameObject swordPoint;

    // SwordPoint ������Ʈ�� �ݶ��̴�
    private Collider swordCollider;

    // onWeaponBladeEnabe ��������Ʈ ����
    public Action<bool> onWeaponBladeEnabe;

    // Waypoints�� ������ ����Ʈ
    private List<Transform> waypointsList = new List<Transform>();

    // �߰� ��Ÿ� ������ üũ�ϴ� �޼���
    private bool IsPlayerInRange()
    {
        if (target != null)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            return distance <= chaseRange;
        }
        return false;
    }

    // ���� �������� üũ�ϴ� �޼���
    private bool CanAttack()
    {
        if (target != null && target.CompareTag("Player"))
        {
            float distance = Vector3.Distance(transform.position, target.position);
            return distance <= attackRange;
        }
        return false;
    }

    // ���� ���� �޼���
    private void Attack()
    {
        if (canAttack && CanAttack())
        {
            // ���� �ִϸ��̼� ����
            animator.SetTrigger(Attack_Hash);

            // ���� ����̵� Ȱ��ȭ
            //WeaponBladeEnable_Old();

            // ���� ��ٿ� ����
            canAttack = false;
            Invoke("ResetAttack", attackCooldown);

            // TODO: ���� ó��
        }
    }

    // ���� ��ٿ� ���� �޼���
    private void ResetAttack()
    {
        canAttack = true;
    }

    // �÷��̾� ���� �޼���
    private void ChasePlayer()
    {
        if (target != null)
        {
            // �÷��̾� ���� �� ����
            navMeshAgent.SetDestination(target.position);
            if (IsPlayerInRange())
            {
                // ���� ���·� ����
                animator.SetTrigger(Chase_Hash);

                // ���� ���¿��� ���� ������ ��� ���� ����
                Attack();
            }
        }
    }

    // Awake �޼��带 ����Ͽ� �ʱ�ȭ
    void Awake()
    {
        // NavMeshAgent ������Ʈ ��������
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Animator ������Ʈ ��������
        animator = GetComponent<Animator>();

        // ü�� �ʱ�ȭ
        CurrentHealth = maxHealth;

        // ���� ��� ����
        target = GameObject.FindGameObjectWithTag("Player").transform;

        // SwordPoint ������Ʈ ã��
        swordPoint = GameObject.Find("SwordPoint").gameObject;
        // SwordPoint ������Ʈ�� �ݶ��̴� ã��
        swordCollider = swordPoint.GetComponent<Collider>();

        // Waypoints�� ã�� ����Ʈ�� ����
        Transform waypointsParent = transform.GetChild(3); // 3��° �ڽ�
        foreach (Transform waypoint in waypointsParent)
        {
            waypointsList.Add(waypoint);
        }

        // Idle ���� ���� �� Walk �ִϸ��̼����� ����
        Invoke("StartPatrolling", idleDuration);
    }

    // Walk �ִϸ��̼����� �����Ͽ� ��ȸ ����
    private void StartPatrolling()
    {
        // Walk �ִϸ��̼� ����
        animator.SetTrigger(Patrol_Hash);
        navMeshAgent.speed = patrollingSpeed;

        // Waypoints ��ġ�� �̵�
        if (waypointsList.Count > 0)
        {
            Transform randomWaypoint = waypointsList[UnityEngine.Random.Range(0, waypointsList.Count)];
            navMeshAgent.SetDestination(randomWaypoint.position);
        }
    }

    // Update �޼��带 ����Ͽ� �����Ӹ��� ����
    void Update()
    {
        // �÷��̾� ���� �� ����
        if (IsPlayerInRange())
        {
            // ���� ���·� ����
            ChasePlayer();
        }
        else
        {
            // �߰� ��Ÿ��� ����� Walk �ִϸ��̼����� �����Ͽ� ��ȸ ����
            animator.SetTrigger(Idle_Hash);
            navMeshAgent.speed = patrollingSpeed;

            // Waypoints ��ġ�� �̵�
            if (!navMeshAgent.pathPending && navMeshAgent.remainingDistance < 0.5f)
            {
                if (waypointsList.Count > 0)
                {
                    Transform randomWaypoint = waypointsList[UnityEngine.Random.Range(0, waypointsList.Count)];
                    navMeshAgent.SetDestination(randomWaypoint.position);
                }
            }
        }
    }

    // �÷��̾��� ������ �޾��� �� ȣ��Ǵ� �޼���
    public void TakeDamage(float damageAmount)
    {
        // ������ �޾��� �� ������ ����
        CurrentHealth -= damageAmount;
        if (CurrentHealth <= 0)
        {
            // ü���� 0 ������ �� Die �ִϸ��̼� ���� �� ������Ʈ ��Ȱ��ȭ
            Die();
        }
        else
        {
            // Damage �ִϸ��̼� ����
            animator.SetTrigger(Gethit_Hash);
            // TODO: Damage �ִϸ��̼� ����
        }
    }

    // ���� ó�� �޼���
    private void Die()
    {
        // Die �ִϸ��̼� ���� �� ������Ʈ ��Ȱ��ȭ
        animator.SetTrigger("Die");
        // TODO: Die �ִϸ��̼� ���� �� ������Ʈ ��Ȱ��ȭ
    }

    // ���� ����̵� Ȱ��ȭ �޼���
    //private void WeaponBladeEnable_Old()
    //{
    //    if (swordCollider != null)
    //    {
    //        swordCollider.enabled = true;
    //    }

    //    // onWeaponBladeEnabe ��������Ʈ ȣ��
    //    onWeaponBladeEnabe?.Invoke(true);
    //}

    //// ���� ����̵� ��Ȱ��ȭ �޼���
    //private void WeaponBladeDisable_Old()
    //{
    //    if (swordCollider != null)
    //    {
    //        swordCollider.enabled = false;
    //    }

    //    // onWeaponBladeEnabe ��������Ʈ ȣ��
    //    onWeaponBladeEnabe?.Invoke(false);
    //}
}
