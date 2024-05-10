using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class Boss : MonoBehaviour, IBattler, IHealth
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

    /// <summary>
    /// HP 확인 및 설정용 프로퍼티
    /// </summary>
    protected float hp = 50.0f;
    public float HP
    {
        get => hp;
        set
        {
            hp = value;
            if(State != BossState.Dead)
            {
                Die();
            }
            hp = Mathf.Clamp(hp, 0, MaxHP);
            onHealthChange?.Invoke(hp / MaxHP);
        }
    }

    /// <summary>
    /// 최대 HP확인용 프로퍼티
    /// </summary>
    public float maxHP = 100.0f;
    public float MaxHP => maxHP;

    /// <summary>
    /// HP가 변경될 때마다 실행될 델리게이트(float:비율)용 프로퍼티
    /// </summary>
    public Action<float> onHealthChange { get; set; }

    /// <summary>
    /// 생존을 확인하기 위한 프로퍼티
    /// </summary>
    public bool IsAlive => hp > 0;

    /// <summary>
    /// 사망 처리용 함수(메서드 method)
    /// </summary>
    public void Die()
    {
        Debug.Log($"{gameObject.name} 사망");
    }

    /// <summary>
    /// 사망을 알리기 위한 델리게이트용 프로퍼티
    /// </summary>
    public Action onDie { get; set; }

    /// <summary>
    /// 체력을 지속적으로 증가시켜 주는 함수. 초당 totalRegen/duration 만큼 회복
    /// </summary>
    /// <param name="totalRegen">전체 회복량</param>
    /// <param name="duration">전체 회복되는데 걸리는 시간</param>
    public void HealthRegenerate(float totalRegen, float durationm)
    {

    }

    /// <summary>
    /// 체력을 틱단위로 회복시켜 주는 함수. 
    /// 전체 회복량 = tickRegen * totalTickCount. 전체 회복 시간 = tickInterval * totalTickCount
    /// </summary>
    /// <param name="tickRegen">틱 당 회복량</param>
    /// <param name="tickInterval">틱 간의 시간 간격</param>
    /// <param name="totalTickCount">전체 틱 수</param>
    public void HealthRegenerateByTick(float tickRegen, float tickInterval, uint totalTickCount)
    {

    }

    /// <summary>
    /// 공격력 확인용 프로퍼티
    /// </summary>
    public float attackPower = 10.0f;
    public float AttackPower => attackPower;

    /// <summary>
    /// 방어력 확인용 프로퍼티
    /// </summary>
    public float defencePower => 3.0f;
    public float DefencePower => defencePower;

    /// <summary>
    /// 맞았을 때 실행될 델리게이트(int:실제로 입은 데미지)
    /// </summary>
    public Action<int> onHit { get; set; }

    /// <summary>
    /// 기본 공격 함수
    /// </summary>
    /// <param name="target">내가 공격할 대상</param>
    /// <param name="isWeakPoint">약점인지 아닌지 확인용(true이면 약점, false이면 약점아님</param>
    public void Attack(IBattler target, bool isWeakPoint = false)
    {

    }

    /// <summary>
    /// 기본 방어 함수
    /// </summary>
    /// <param name="damage">내가 받은 데미지</param>
    public void Defence(float damage)
    {

    }

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
        //Instantiate(fireBallPrefab, fireBallSpawnPosition, transform.rotation);
        Factory.Instance.GetFireBall(fireBallSpawnPosition);
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

    public void OnDodge()
    {
        animator.SetTrigger("GroundDodge");
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
