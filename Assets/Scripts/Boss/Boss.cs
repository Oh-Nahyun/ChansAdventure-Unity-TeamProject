using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Boss : MonoBehaviour
{
    protected enum BossState
    {
        Wait = 0,
        Bite,
        Breath,
        ClowLR,
        ClowL,
        ClowR,
        FireBall,
        Move,
        GroundDodge,
        GroundDash,
        Dead
    }

    BossState state = BossState.Wait;

    protected BossState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                state = value;
                switch (state)
                {
                    case BossState.Wait:
                        isActive = false;
                        agent.isStopped = true;
                        agent.velocity = Vector3.zero;
                        animator.SetTrigger("Idle");
                        break;
                    case BossState.Bite:
                        isActive = false;
                        agent.isStopped = true;
                        transform.LookAt(player.transform.position);
                        animator.SetTrigger("Bite");
                        break;
                    case BossState.Breath:
                        isActive = false;
                        agent.isStopped = true;
                        transform.LookAt(player.transform.position);
                        animator.SetTrigger("Breath");
                        break;
                    case BossState.ClowLR:
                        isActive = false;
                        agent.isStopped = true;
                        transform.LookAt(player.transform.position);
                        animator.SetTrigger("ClowLR");
                        break;
                    case BossState.ClowL:
                        isActive = false;
                        agent.isStopped = true;
                        transform.LookAt(player.transform.position);
                        animator.SetTrigger("ClowL");
                        break;
                    case BossState.ClowR:
                        isActive = false;
                        agent.isStopped = true;
                        transform.LookAt(player.transform.position);
                        animator.SetTrigger("ClowR");
                        break;
                    case BossState.FireBall:
                        isActive = false;
                        agent.isStopped = true;
                        transform.LookAt(player.transform.position);
                        animator.SetTrigger("FireBall");
                        break;
                    case BossState.Move:
                        isActive = false;
                        agent.isStopped = false;
                        StartCoroutine(MoveRandomDirection());
                        break;
                    case BossState.GroundDodge:
                        isActive = false;
                        agent.isStopped = false;
                        transform.LookAt(player.transform.position);
                        animator.SetTrigger("GroundDodge");
                        break;
                    case BossState.GroundDash:
                        isActive = false;
                        agent.isStopped = false;
                        transform.LookAt(player.transform.position);
                        animator.SetTrigger("GroundDash");
                        OnDash();
                        break;
                    case BossState.Dead:
                        isActive = false;
                        agent.isStopped = true;
                        break;
                }
            }
        }

    }

    public GameObject fireBallPrefab;
    BossInputActions inputActions;
    Rigidbody rb;
    ClowAttackArea clowAttackArea;
    BiteAttackArea biteAttackArea;
    Animator animator;
    ParticleSystem breathParticle;
    Player player;
    NavMeshAgent agent;
    Rigidbody rigid;
    Action onStateUpdate;
   
    Vector3 fireBallSpawnPosition;
    Vector3 difference;
    float sqrDistance;

    float moveDuration = 1.1f;
    public float speed = 2.0f;
    public float approachDuration = 1.2f;
    public float stopDistance = 1.5f;
    bool isActive = false;

    IEnumerator MoveRandomDirection()
    {
        agent.isStopped = false;
        float endTime = Time.time + moveDuration;

        // 랜덤 방향 선택
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.left, Vector3.right };
        Vector3 direction = directions[UnityEngine.Random.Range(0, directions.Length)];

        if (direction == Vector3.forward)
        {
            animator.SetTrigger("WalkF");
        }
        else if (direction == Vector3.back)
        {
            animator.SetTrigger("WalkB");
        }
        else if (direction == Vector3.left)
        {
            animator.SetTrigger("WalkL");
        }
        else
        {
            animator.SetTrigger("WalkR");
        }

        while (Time.time < endTime)
        {
            transform.LookAt(player.transform.position);
            agent.Move(direction * Time.deltaTime * agent.speed);
            yield return null;
        }
        agent.isStopped = true;
    }
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        clowAttackArea = GetComponentInChildren<ClowAttackArea>(true);
        biteAttackArea = GetComponentInChildren<BiteAttackArea>(true);
        breathParticle = GetComponentInChildren<ParticleSystem>(true);
    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>();
        animator.SetTrigger("Roar");
    }

    public void OnFireBall()
    {
        Instantiate(fireBallPrefab, fireBallSpawnPosition, transform.rotation);
    }

    public void OnClowArea()
    {
        if (clowAttackArea != null)
        {
            clowAttackArea.Activate();
        }
    }

    public void OffClowArea()
    {
        if (clowAttackArea != null)
        {
            clowAttackArea.Deactivate();
        }
    }

    public void OnBiteArea()
    {
        if (biteAttackArea != null)
        {
            biteAttackArea.Activate();
        }
    }

    public void OffBiteArea()
    {
        if (biteAttackArea != null)
        {
            biteAttackArea.Deactivate();
        }
    }

    public void OnBreathArea()
    {
        breathParticle.Play();
    }

    public void OffBreathArea()
    {
        breathParticle.Stop();
    }

    public void OnDash()
    {
        StartCoroutine(MoveTowardsPlayer());
    }

    public void OnActive()
    {
        isActive = true;
    }

    IEnumerator MoveTowardsPlayer()
    {
        float startTime = Time.time;
        Vector3 startPosition = transform.position;

        // 플레이어 방향으로의 벡터 계산
        Vector3 directionToPlayer = (player.transform.position - transform.position).normalized;

        // 최종 목표 위치는 플레이어 위치에서 stopDistance만큼 떨어진 지점
        Vector3 targetPosition = player.transform.position - directionToPlayer * stopDistance;

        while (Time.time < startTime + approachDuration)
        {
            float t = (Time.time - startTime) / approachDuration;
            transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            yield return null;
        }

        transform.position = targetPosition; // 정확히 목표 위치에 정지
    }

    private void Update()
    {
        fireBallSpawnPosition = transform.TransformPoint(new Vector3(-0.1f, 1.35f, 1.6f));
        difference = player.transform.position - transform.position;
        sqrDistance = difference.sqrMagnitude;

        if(isActive == true)
        {
            DecideStateBasedOnDistance();
        }
    }
    void DecideStateBasedOnDistance()
    {
        if (sqrDistance > 100) // 거리가 10 이상 (10^2 = 100)
        {
            ChooseNextState(BossState.Move, BossState.GroundDash, BossState.FireBall);
        }
        else if (sqrDistance > 16 && sqrDistance <= 100) // 거리가 4에서 10 사이 (4^2 = 16, 10^2 = 100)
        {
            ChooseNextState(BossState.Move, BossState.GroundDash, BossState.Breath, BossState.FireBall, BossState.GroundDodge);
        }
        else // 거리가 4 미만 (4^2 = 16)
        {
            ChooseNextState(BossState.ClowLR, BossState.ClowL, BossState.ClowR, BossState.Bite, BossState.GroundDodge);
        }
    }

    void ChooseNextState(params BossState[] states)
    {
        State = states[UnityEngine.Random.Range(0, states.Length)];
    }
}
