using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// 현재 적의 상태를 나타내는 enum
/// </summary>
public enum EnemyState
{
    None = -1,      // 적 없음
    Idle = 0,       // 적 대기
    Wander,         // 적 배회
    Pursuit,        // 적 추적
    Attack          // 적 공격
}

/// <summary>
/// 적의 행동을 제어하는 스크립트
/// </summary>
public class EnemyStatus : MonoBehaviour
{

    [Header("Pursuit")]
    /// <summary>
    /// 인식 범위 (범위 안으로 들어오면 추적 상태로 변경)
    /// </summary>
    [SerializeField]
    float targetRecognition = 8;

    /// <summary>
    /// 추적 범위 (범위 밖으로 나가면 배회 상태로 변경)
    /// </summary>
    [SerializeField]
    float trackingRange = 10;

    [Header("Attack")]
    /// <summary>
    /// 발사체 오브젝트를 담을 곳
    /// </summary>
    [SerializeField]
    GameObject projectilePrefab;

    /// <summary>
    /// 발사체의 생성 위치
    /// </summary>
    [SerializeField]
    Transform projectileSpawnPoint;

    /// <summary>
    /// 공격 범위(범위 안에 들어오면 공격 상태로 변경)
    /// </summary>
    [SerializeField]
    float attackRange = 5.0f;

    /// <summary>
    /// 공격 속도
    /// </summary>
    [SerializeField]
    float attackRate = 2.0f;


    /// <summary>
    ///  현재 적의 행동
    /// </summary>
    EnemyState enemyState = EnemyState.None;
    
    /// <summary>
    /// 공격 주기 계산용 변수(마지막 발사 시간 저장)
    /// </summary>
    float lastAttackTime = 0.0f;

    /// <summary>
    /// 상태 정보
    /// </summary>
    Status status;

    /// <summary>
    /// 이동 제어를 위한 NavMeshAgent 변수
    /// </summary>
    NavMeshAgent navMeshAgent;

    /// <summary>
    /// 추적할 대상
    /// </summary>
    Transform target;

    /// <summary>
    /// 적 메모리 풀(적 오브젝트 비활성화)
    /// </summary>
    EnemyMeoryPool enemyMeoryPool;

    //private void Awake()
    public void Setup(Transform target, EnemyMeoryPool enemyMeoryPool)
    {
        status = GetComponent<Status>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        this.target = target; // 타겟 저장
        this.enemyMeoryPool = enemyMeoryPool; // 적 저장

        // NavMeshAgent 컴포넌트에서 회전을 업데이트 못하게 설정
        navMeshAgent.updateRotation = false;
        
    }

    private void OnEnable()
    {
        // 적이 활성화될 때 적의 상태를 대기 상태로 설정
        ChangeState(EnemyState.Idle);
    }

    private void OnDisable()
    {
        // 적이 비활성화될 때 현재 재생중인 상태를 종료
        StopCoroutine(enemyState.ToString());

        // 적의 상태를 None으로 설정
        enemyState = EnemyState.None;
    }

    /// <summary>
    /// 적의 상태를 변경하는 함수
    /// </summary>
    /// <param name="newState">변경할 상태</param>
    public void ChangeState(EnemyState newState)
    {
        // 현재 재생중인 상태와 바꾸려고 하는 상태가 같으면 return
        if(enemyState == newState) return;

        // 이전에 재생중인 상태 종료
        StopCoroutine(enemyState.ToString());

        // 현재 적의 상태를 바꿀 상태로 변경
        enemyState = newState;

        // 바꿀 상태 재생
        StartCoroutine(enemyState.ToString());
    }

    /// <summary>
    /// 대기 상태일 때 행동하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator Idle()
    {
        // 기다렸다가 배회 상태로 변경하는 코루틴 실행
        StartCoroutine("AutoChangeFromIdleToWande");

        while(true)
        {
            // 대기 상태일 때 하는 행동
            // 타겟과의 거리에 따라 행동 선택(배회, 추격, 원거리 공격)
            SelecteState();

            yield return null;
        }
    }

    /// <summary>
    /// 대기에서 배회 상태로 바뀌는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator AutoChangeFromIdleToWande()
    {
        // 1 ~ 4초 시간 기다리기 위한 변수
        int chageTime = Random.Range(1, 5);

        // 랜덤 시간동안 기다리고
        yield return new WaitForSeconds(chageTime);

        // 상태를 배회 상태로 변경
        ChangeState(EnemyState.Wander);
    }

    /// <summary>
    /// 배회 상태일 때 행동을 실행하는 코루틴
    /// </summary>
    /// <returns></returns>

    IEnumerator Wander()
    {
        float currentTime = 0.0f;   // 현재 배회 시간
        float maxTime = 10.0f;      // 최대 배회 시간

        // 이동속도 설정
        navMeshAgent.speed = status.WalkSpeed;
        
        // 목표 위치 설정
        navMeshAgent.SetDestination(CalculateWanderPosition());

        // 목표 위치로 회전하기
        Vector3 to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);    // 목표 위치
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);              // 현재 위치
        // 목표 위치 - 현재 위치 = 방향 벡터, 구한 방향 벡터로 회전하기
        transform.rotation = Quaternion.LookRotation(to - from);   
        
        while(true)
        {
            currentTime += Time.deltaTime;

            to = new Vector3(navMeshAgent.destination.x, 0, navMeshAgent.destination.z);    // 목표 위치
            from = new Vector3(transform.position.x, 0, transform.position.z);              // 현재 위치

            if((to - from).sqrMagnitude < 0.01f || currentTime >= maxTime)
            {
                // 목표 위치에 근접하거나 최대 시간까지 배회 중이면 
                ChangeState(EnemyState.Idle);   // 대기 상태로 변경 
            }

            // 타겟과의 거리에 따라 행동 선택(배회, 추격, 원거리 공격)
            SelecteState();

            yield return null;
        }
    }

    Vector3 CalculateWanderPosition()
    {
        float wanderRadius = 10;    // 현재  위치를 원점으로 하는 원의 반지름 변수
        int wanderAngle = 0;        // 선택된 각도
        int WanderAngleMin = 0;     // 최소 각도
        int WanderAngleMax = 360;     // 최대 각도

        // 현재 적 캐릭터가 있는 월드 중심의 위치
        Vector3 rangePosition = Vector3.zero;
        // 현재 적 캐릭터가 있는 월드 중심의 크기
        Vector3 rangeScale = Vector3.one * 100.0f;

        // 선택된 각도는 최소에서 최대까지 랜덤
        wanderAngle = Random.Range(WanderAngleMin, WanderAngleMax);
        // 자신의 위치 중심으로 반지름 거리, 선택된  각도에 위치한 좌표를 목표 지점으로 설정
        Vector3 targetPosition = transform.position + SetAngle(wanderRadius, wanderAngle);

        // 생성된 목표위치가 자신의 이동구역을 벗어나지 않게 설정
        // x좌표
        targetPosition.x = Mathf.Clamp(targetPosition.x,
            rangePosition.x - rangeScale.x * 0.5f,
            rangePosition.x + rangeScale.x * 0.5f);
        // y좌표
        targetPosition.y = 0.0f;
        // z 좌표
        targetPosition.z = Mathf.Clamp(targetPosition.x,
            rangePosition.z - rangeScale.z * 0.5f,
            rangePosition.z + rangeScale.z * 0.5f);

        return targetPosition;
    }

    /// <summary>
    /// 원 둘레이 위치를 구해주는 함수
    /// </summary>
    /// <param name="radius">반지름</param>
    /// <param name="angle">각도</param>
    /// <returns>원 둘레의 각도</returns>
    Vector3 SetAngle(float radius, float angle)
    {
        Vector3 position = Vector3.zero;

        position.x = Mathf.Cos(angle) * radius;
        position.z = Mathf.Sin(angle) * radius;

        return position;
    }

    /// <summary>
    /// 타겟을 추적하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator Pursuit()
    {
        while (true)
        {
            // 배회할 때는 걷는 속도, 추적할 때는 뛰는 이동 속도
            navMeshAgent.speed = status.RunSpeed;

            // 목표위치를 현재 플레이어 위치로 설정
            navMeshAgent.SetDestination(target.position);

            // 타겟 방향을 계속 보게 만들기
            LookRotateTarget();

            // 타겟과의 거리에 따라 행동 선택(배회, 추격, 원거리 공격)
            SelecteState();

            yield return null;
        }
    }

    /// <summary>
    /// 공격을 실행하는 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator Attack()
    {
        // 공격할 때는 이동을 멈추도록 설정
        navMeshAgent.ResetPath();

        while (true)
        {
            // 공격할 때는 타겟을 바라보기
            LookRotateTarget();

            // 타겟과의 거리에 따라 행동 변경(배회, 추격, 공격)
            SelecteState();

            if(Time.time - lastAttackTime > attackRate) // 공격 주기가 지나면
            {
                // 현재 시간 저장
                lastAttackTime = Time.time;

                // 발사체 생성 (projectilePrefab 오브젝트를 projectileSpawnPoint의 위치와 각도로 생성)
                GameObject clone = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
                // clone 에는 EnemyProjectile 컴포넌트에 Setup에 타겟위치 추가
                clone.GetComponent<EnemyProjectile>().Setup(target.position);
            }

            yield return null;
        }
    }

    /// <summary>
    /// 타겟을 계속 보게 만드는 함수
    /// </summary>
    private void LookRotateTarget()
    {
        // 목표 위치 설정
        Vector3 to = new Vector3(target.position.x, 0, target.position.z);
        // 내 현재 위치
        Vector3 from = new Vector3(transform.position.x, 0, transform.position.z);

        // 바로 돌기
        transform.rotation = Quaternion.LookRotation(to - from);
        // 천천히 돌기
        //Quaternion rotation = Quaternion.LookRotation(to - from);
        //transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 0.01f);
    }

    /// <summary>
    /// 타겟과의 거리에 따라 행동을 하는 함수(배회, 추격, 원거리 공격)
    /// </summary>
    private void SelecteState()
    {
        if(target == null) return;  // 타겟이 없으면 리턴

        // 타겟과 적의 거리 계산
        float distance = Vector3.Distance(target.position, transform.position);

        if(distance <= attackRange) // 타겟과 거리가 공격 범위보다 작으면
        {
            ChangeState(EnemyState.Attack); // 공격 상태로 변경
        }
        else if(distance <= targetRecognition) // 타겟과 거리가 인식 범위 보다 작으면
        {
            ChangeState(EnemyState.Pursuit);    // 추적 상태로 변경
        }
        else if(distance >= trackingRange)  // 타겟과 거리가 추적 범위 보다 크면
        {
            ChangeState(EnemyState.Wander);     // 배회 상태로 변경
        }
    }

    /// <summary>
    /// 적이 공격 받으면 실행할 함수
    /// </summary>
    /// <param name="damage">받은 데미지</param>
    public void TakeDamage(int damage)
    {
        // 적이 죽었는지 확인용 변수
        bool isDie = status.DecreaseHP(damage);

        if(isDie == true) // 적이 죽으면
        {
            enemyMeoryPool.DeactivateEnemy(gameObject); // 오브젝트 비활성화

        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        // 배회 상태 일때 이동할 경로 표시
        Gizmos.DrawRay(transform.position, navMeshAgent.destination - transform.position);

        Gizmos.color = Color.red;
        // 목표 인식 범위 표시
        Gizmos.DrawWireSphere(transform.position, targetRecognition);

        Gizmos.color = Color.green;
        // 추적 범위
        Gizmos.DrawWireSphere(transform.position, trackingRange);

        Gizmos.color = new Color(0.39f, 0.04f, 0.04f);
        // 공격 가능 범위
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
