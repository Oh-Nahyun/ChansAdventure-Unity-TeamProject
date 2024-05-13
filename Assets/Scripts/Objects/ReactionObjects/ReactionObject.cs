using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

/// <summary>
/// 반응 오브젝트의 종류를 정하는 비트플래그
/// </summary>
[Flags]
public enum ReactionType
{
    Move = 1,           // 충격에 반응해서 이동하는 오브젝트
    Throw = 2,          // 들 수 있고 던질 수 있는 오브젝트
    Magnetic = 4,       // 마그넷캐치에 연결될 수 있는 오브젝트
    Destroy = 8,        // 파괴 가능한 오브젝트
    Explosion = 16,     // 폭발하는 오브젝트
    Hit = 32,           // 타격가능한 오브젝트
    Skill = 64,          // 스킬일 경우 확인용
}

[RequireComponent(typeof(Rigidbody))]
public class ReactionObject : RecycleObject, IBattler
{
    /// <summary>
    /// 폭발 관련 데이터 저장용 클래스
    /// </summary>
    [Serializable]
    public class ExplosiveObject
    {
        /// <summary>
        /// 폭발했을 때 다른 오브젝트가 밀려나는 충격량
        /// </summary>
        public float force = 4.0f;
        /// <summary>
        /// 폭발했을 때 다른 오브젝트가 위로 밀려나는 충격량 
        /// </summary>
        public float forceY = 5.0f;
        /// <summary>
        /// 폭발 범위
        /// </summary>
        public float boomRange = 3.0f;
        /// <summary>
        /// 폭발 데미지
        /// </summary>
        public float damage = 3.0f;
    }

    [Header("반응형오브젝트 데이터")]

    /// <summary>
    /// 폭발 관련 데이터
    /// </summary>
    public ExplosiveObject explosiveInfo;

    /// <summary>
    /// 무게 (이동, 던질 때 영향)
    /// </summary>
    public float weight = 1.0f;

    /// <summary>
    /// 최대 체력 (오브젝트가 파괴,폭발할 때 필요한 데미지량)
    /// </summary>
    public float objectMaxHp = 1.0f;

    /// <summary>
    /// 던진 이후 이 오브젝트가 충격받을 데미지
    /// </summary>
    public float takeDamageAfterThrow = 3.0f;

    /// <summary>
    /// 현재 체력
    /// </summary>
    float objectHP;

    /// <summary>
    /// 현재 체력 프로퍼티
    /// </summary>
    protected float ObjectHP
    {
        get => objectHP;
        set
        {
            objectHP = value;
            if (objectHP <= 0.0f && (reactionType & ReactionType.Destroy) != 0)
            {
                // 현재 상태가 파괴가 아니고 HP가 0 이하로 떨어지면 파괴, 폭발

                TryDestroy();
            }
        }
    }
    /// <summary>
    /// 타임록 지속시간
    /// </summary>
    float timeLockDuration = 0;

    /// <summary>
    /// 현재 지속 시간 축적용 (타임록, )
    /// </summary>
    float elapsedTime = 0;

    /// <summary>
    /// 현재 지속 시간 측정용 프로퍼티 (타임록, )
    /// </summary>
    float ElapsedTime
    {
        get => elapsedTime;
        set
        {
            elapsedTime = value;
            if (timeLockDuration < elapsedTime)
            {
                FinishTimeLock();
            }
            else if (timeLockDuration * intervalChange2 < elapsedTime)
            {
                timeLockInterval = new WaitForSeconds(interval2);
            }
            else if (timeLockDuration * intervalChange1 < elapsedTime)
            {
                timeLockInterval = new WaitForSeconds(interval1);
            }
        }
    }
    /// <summary>
    /// 타임록 걸렸을 때 깜빡이는 속도 (지속시간이 끝나갈수록 빨라짐)
    /// </summary>
    WaitForSeconds timeLockInterval;

    /// <summary>
    /// 타임록 깜빡이는 속도 변경되는 시간 비율 1 (조금 빠름, 50% 지났을 때)
    /// </summary>
    const float intervalChange1 = 0.5f;
    /// <summary>
    /// 타임록 깜빡이는 속도 변경되는 시간 비율 2 (매우 빠름, 80% 지났을 때)
    /// </summary>
    const float intervalChange2 = 0.8f;
    /// <summary>
    /// 타임록 깜빡이는 기본 속도
    /// </summary>
    const float baseInterval = 0.5f;
    /// <summary>
    /// 조금 빠른 깜빡이는 속도
    /// </summary>
    const float interval1 = 0.2f;
    /// <summary>
    /// 매우 빠른 깜빡이는 속도
    /// </summary>
    const float interval2 = 0.05f;
    /// <summary>
    /// 깜빡이는 코루틴용 변수
    /// </summary>
    IEnumerator blinkCoroutine;
    /// <summary>
    /// 지속시간 체크 코루틴용 변수
    /// </summary>
    IEnumerator durationCoroutine;

    /// <summary>
    /// 방어력 (0)
    /// </summary>
    public float defencePower = 0.0f;

    /// <summary>
    /// 반응하는 타입
    /// </summary>
    public ReactionType reactionType;

    /// <summary>
    /// 이동 가능 확인용
    /// </summary>
    public bool IsMoveable => (reactionType & ReactionType.Move) != 0;
    /// <summary>
    /// 던질 수 있는지 확인용
    /// </summary>
    public bool IsThrowable => (reactionType & ReactionType.Throw) != 0;
    /// <summary>
    /// 자석에 붙는지 확인용
    /// </summary>
    public bool IsMagnetic => (reactionType & ReactionType.Magnetic) != 0;
    /// <summary>
    /// 파괴 가능한지 확인용
    /// </summary>
    public bool IsDestructible => (reactionType & ReactionType.Destroy) != 0;
    /// <summary>
    /// 폭발 가능한지 확인용
    /// </summary>
    public bool IsExplosive => (reactionType & ReactionType.Explosion) != 0;
    /// <summary>
    /// 때릴 수 있는지 확인용
    /// </summary>
    public bool IsHitable => (reactionType & ReactionType.Hit) != 0;
    /// <summary>
    /// 스킬 인지 확인용
    /// </summary>
    public bool IsSkill => (reactionType & ReactionType.Skill) != 0;

    /// <summary>
    /// 현재 상태를 나타내는 enum
    /// </summary>
    public enum StateType
    {
        None = 0,   // 아무것도 아님(원래상태)
        PickUp,     // 들려있음
        Drop,       // 떨어트림
        Throw,      // 던짐
        Move,       // 이동중
        Destroy,    // 파괴중
        Boom,       // 터지는중
        Enable,     // 생성 직후 (활성화 직후에만 사용)
        TimeLock    // 타임록 걸린 상태
    }
    /// <summary>
    /// 현재 상태(기본, 들림, 던져짐, 이동, 파괴, 폭발)
    /// </summary>
    protected StateType currentState = StateType.Enable;

    /// <summary>
    /// 현재 상태 확인용 프로퍼티 (get)
    /// </summary>
    public StateType State => currentState;

    /// <summary>
    /// 폭발 시 데미지
    /// </summary>
    public float AttackPower
    {
        get
        {
            if (IsExplosive)
            {
                return explosiveInfo.damage;
            }
            else
            {
                return 0;
            }
        }
    }

    /// <summary>
    /// 피해 감소량
    /// </summary>
    public float DefencePower
    {
        get
        {
            if (IsExplosive || IsDestructible)
            {
                return defencePower;
            }
            else
            {
                return 0;
            }
        }
    }

    public Action<int> onHit { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    /// <summary>
    /// 원래 부모 (들렸을 때 변경된 부모를 되돌리기 위함)
    /// </summary>
    protected Transform originParent;

    /// <summary>
    /// 물체를 드는 트랜스폼 (들기, 놓기, 던지기에 사용)
    /// </summary>
    protected Transform pickUpUser;

    /// <summary>
    /// 원래 회전 마찰력 (마그넷캐치 시 변경내용 원래대로 되돌리기 위함)
    /// </summary>
    protected float originAngularDrag;
    /// <summary>
    /// 원래 마찰력 (마그넷캐치 시 변경내용 원래대로 되돌리기 위함)
    /// </summary>
    protected float originDrag;

    /// <summary>
    /// 자석에 붙었을 때 이동속도 (자석스킬에서 받아옴)
    /// </summary>
    float attachMoveSpeed;

    /// <summary>
    /// 자석에 붙어있는지 확인 (자석 목적지가 있는 경우 true)
    /// </summary>
    bool isAttachMagnet = false;

    /// <summary>
    /// 기본 머티리얼 들
    /// </summary>
    Material[] originMaterials;
    /// <summary>
    /// 스킬을 사용했을 때 변하는 머티리얼
    /// </summary>
    Material skillMaterial;

    // 컴포넌트
    protected Rigidbody rigid;

    [SerializeField]
    protected Renderer mainRenderer;

    /// <summary>
    /// 자석에 붙었을 때 회전을 할지 말지 정하는 변수 (true: 회전을 함), 충돌시 강제 회전 방지하기 위해 사용
    /// </summary>
    bool isFollowMagnetRotate = true;

    ParticleSystem particle;

    Transform objectShape;

    /// <summary>
    /// 재활용 여부를 결정하는 변수 (true: 재활용)
    /// </summary>
    protected bool isRecycle = false;

    // TODO: 플레이어 배틀 머지 완료되면 타격 데미지 저장되는 메서드 구현
    #region 유니티 이벤트 함수
    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        if (rigid == null)
        {
            rigid = transform.AddComponent<Rigidbody>();
        }

        originParent = transform.parent;    // 원래 부모 설정
        rigid.mass = weight;
        //reducePower = 1.0f / Weight;        // TODO: 감소량 결정 (리지드바디의 mess로 정할지 결정)

        if (mainRenderer == null)
        {
            mainRenderer = GetComponentInChildren<Renderer>();
        }
        
        originMaterials = mainRenderer.materials;
        

        durationCoroutine = DurationCoroutine();
        blinkCoroutine = BlinkCoroutine();
        if (transform.childCount != 0)
        {
            objectShape = transform.GetChild(0);
        }
        particle = GetComponentInChildren<ParticleSystem>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (objectShape != null)
        {
            objectShape.gameObject.SetActive(true);
        }
        currentState = StateType.Enable;    // 활성화 상태로 설정
        ObjectHP = objectMaxHp;             // TODO: hp 설정 (인터페이스 있으면 그걸로 사용)
    }

    protected void OnCollisionEnter(Collision collision)
    {
        if (currentState == StateType.Throw && collision.transform != pickUpUser)       // 던져진 상태일 때 부딪친 물체가 던진오브젝트가 아니면 (던져지자마자 바로 터짐 방지)
        {
            CollisionActionAfterThrow();        // 던진 후 충돌 동작
        }
        else if (currentState == StateType.Drop && collision.transform != pickUpUser)   // 떨어트린 상태일 때 부딪친 물체가 던진오브젝트가 아니면 (던져지자마자 바로 동작 방지)
        {
            CollisionActionAfterDrop();         // 떨어트린 후 충돌 동작
        }
        if (isAttachMagnet)                     // 자석에 붙어있을 때
        {
            isFollowMagnetRotate = false;       // 충돌 중이면 사용자의 회전 따라가지 않음
        }
    }

    protected void OnCollisionExit(Collision collision)
    {
        if (isAttachMagnet)
        {
            // 현재 자석에 붙어있다면
            //rigid.velocity = Vector3.zero;          // 물체에서 부딪친 후 밀리는 힘 제거
            isFollowMagnetRotate = true;
            rigid.angularVelocity = Vector3.zero;   // 물체에서 부딪친 후 회전하는 힘 제거
        }
    }
    #endregion

    #region 스킬관련 함수
    /// <summary>
    /// 자석에 붙어있을 때 움직이는 동작을 하는 메서드
    /// </summary>
    public void AttachMagnetMove(Vector3 position)
    {
        if (isAttachMagnet)
        {
            Vector3 dir = position - transform.position;
            //Vector3 dir = Vector3.Slerp(transform.position, position, Time.fixedDeltaTime * attachMoveSpeed);
            if (dir.sqrMagnitude > StopDistance * attachMoveSpeed)                  // 적당한 거리까지 붙으면 정지
            {
                // 모서리부분, 끝부분 뚫리는 현상 방지를 위해 velocity 사용
                rigid.velocity = dir * attachMoveSpeed;
            }
            else
            {
                // 도착하면 velocity를 없애 떨림 방지 (안하면 지나가고 되돌아오고 반복하면서 떨림)
                rigid.velocity = Vector3.zero;
            }
        }
    }

    const float StopDistance = 0.001f;

    /// <summary>
    /// 자석에 붙어있을 때 카메라 회전을 해도 자석 사용자를 그대로 바라보기 위한 메서드
    /// </summary>
    /// <param name="euler">사용자의 회전 각도</param>
    public virtual void AttachRotate(Vector3 euler)
    {
        if (isFollowMagnetRotate)
        {
            rigid.rotation = Quaternion.Euler(rigid.rotation.eulerAngles + euler);
        }
    }

    /// <summary>
    /// 자석에 붙을 때 동작하는 메서드
    /// </summary>
    /// <param name="destination">자석으로 옮기고자하는 목적지</param>
    /// <param name="moveSpeed">자석에 붙어서 이동할 때 속도</param>
    public void AttachMagnet(float moveSpeed)
    {
        if (IsMagnetic)
        {
            // 중력 사용 x, 마찰력 없앰, 자석에 붙음(true), 이동속도 설정
            rigid.useGravity = false;
            rigid.drag = 0;
            rigid.angularDrag = 0;
            isAttachMagnet = true;
            attachMoveSpeed = moveSpeed;
            isFollowMagnetRotate = true;
        }
    }

    /// <summary>
    /// 자석에서 떨어질 때 동작하는 메서드
    /// </summary>
    public void DettachMagnet()
    {
        if (IsMagnetic)
        {
            // 중력 원래대로, 마찰력 원래대로, 자석에서 떨어짐(false)
            rigid.useGravity = true;
            rigid.drag = originDrag;
            rigid.angularDrag = originAngularDrag;
            isAttachMagnet = false;
            isFollowMagnetRotate = false;
        }
    }

    RigidbodyConstraints originConstraints = RigidbodyConstraints.None;
    /// <summary>
    /// 타임록 중 쌓일 수 있는 최대 힘
    /// </summary>
    float maxTimeLockDamage = 50.0f;
    /// <summary>
    /// 축적된 힘의 방향 (마지막 방향)
    /// </summary>
    Vector3 accumulateDirection = Vector3.zero;
    /// <summary>
    /// 축적된 힘
    /// </summary>
    float accumulateDamage = 0;

    float AccumulateDamage
    {
        get => accumulateDamage;
        set
        {
            accumulateDamage = Mathf.Min(maxTimeLockDamage, value);
        }
    }

    public void OnTimeLock(Material timeLockColor, float duration, float maxTimeLockDamage)
    {
        if (IsMoveable && currentState != StateType.TimeLock)
        {
            originConstraints = rigid.constraints;
            rigid.constraints = RigidbodyConstraints.FreezeAll;
            currentState = StateType.TimeLock;

            originMaterials = mainRenderer.materials;
            skillMaterial = timeLockColor;

            timeLockDuration = duration;
            timeLockInterval = new WaitForSeconds(baseInterval);
            ElapsedTime = 0;

            this.maxTimeLockDamage = maxTimeLockDamage;

            StartCoroutine(durationCoroutine);
            StartCoroutine(blinkCoroutine);
        }
    }

    public void FinishTimeLock()
    {
        onTimeLockFinish?.Invoke();

        rigid.constraints = originConstraints;
        currentState = StateType.None;

        mainRenderer.materials = originMaterials;
        skillMaterial = null;

        StopCoroutine(durationCoroutine);
        StopCoroutine(blinkCoroutine);

        rigid.AddForce(accumulateDirection * AccumulateDamage, ForceMode.Impulse);
        currentState = StateType.Throw;

        accumulateDirection = Vector3.zero;
        AccumulateDamage = 0;
    }

    public void SetSkillColor(Material material)
    {
        // TODO: 나중에 셰이더로 바꾸기
        if (material == null)
        {
            mainRenderer.materials = originMaterials;
        }
        else
        {
            Material[] materials = new Material[mainRenderer.materials.Length + 1];
            for (int i = 0; i < mainRenderer.materials.Length; i++)
            {
                materials[i] = mainRenderer.materials[i];
            }
            materials[materials.Length - 1] = material;

            mainRenderer.materials = materials;
        }
    }

    public Action<Vector3, float> onTimeLockDamageChange;
    public Action onTimeLockFinish;

    void SetTimeLockPower(Vector3 power)
    {
        accumulateDirection = power.normalized; // 방향 설정
        AccumulateDamage += power.magnitude;    // 실제 수치가 필요해서 sqr 사용 x

        onTimeLockDamageChange?.Invoke(accumulateDirection, AccumulateDamage);
    }

    /// <summary>
    /// 피격 당할 때 움직이는 힘 값 - 05.13
    /// </summary>
    public float hitMovementPower = 2.0f;

    /// <summary>
    /// 외부에서 오브젝트를 상호작용 할 때 호출되는 함수
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="attacker"></param>
    public void ReactionToExternalObj(float damage, Transform attacker) // - 05.13
    {
        Vector3 correction = Vector3.up * 0.3f;
        Vector3 direction = (transform.position - attacker.position + correction).normalized;
        Vector3 power = direction * hitMovementPower;

        TryHit(damage, power);
    }
    #endregion

    #region 유니티 이벤트 관련 함수
    /// <summary>
    /// 던져 졌을 때 추가 동작을 위한 메서드
    /// </summary>
    protected virtual void CollisionActionAfterThrow()
    {
        pickUpUser = null;              // 부딪치면 들고 있는 사용자 없애기
        currentState = StateType.None;  // 현재 상태를 기본 상태로
        ObjectHP -= takeDamageAfterThrow;
    }
    protected virtual void CollisionActionAfterDrop()
    {
        pickUpUser = null;              // 부딪치면 들고 있는 사용자 없애기
        currentState = StateType.None;  // 현재 상태를 기본 상태로
    }
    #endregion

    #region 오브젝트 반응 함수
    /// <summary>
    /// 피격시 동작하는 메서드 (무기, 폭발 등)
    /// </summary>
    /// <param name="isExplosion">폭발 피해 확인용 (true: 폭발)</param>
    public void TryHit(bool isExplosion = false)
    {
        if (isExplosion && currentState != StateType.TimeLock)
        {
            // 폭발하는 오브젝트면 터짐
            if (IsExplosive)
            {
                TryBoom();
            }
            else
            {
                TryDestroy();
            }
        }
        else
        {
            TryHit(1.0f);
        }
    }

    /// <summary>
    /// 피격시 동작하는 메서드 (무기, 폭발 등)
    /// </summary>
    /// <param name="damage">피격 데미지</param>
    public void TryHit(float damage)
    {
        if (IsHitable && currentState != StateType.TimeLock)
        {
            ObjectHP -= damage;
        }
    }

    /// <summary>
    /// 피격시 동작하는 메서드 - 05.13
    /// </summary>
    /// <param name="damage">피격데미지</param>
    /// <param name="power"></param>
    public void TryHit(float damage, Vector3 power)
    {
        TryHit(damage);

        if(IsMoveable)
        {
            if (currentState == StateType.TimeLock)
            {
                SetTimeLockPower(power);
            }
            else
            {
                rigid.AddForce(power, ForceMode.Impulse);
            }
        }  
    }

    /// <summary>
    /// 파괴하는 메서드
    /// </summary>
    protected void TryDestroy()
    {
        if (IsDestructible)
        {
            currentState = StateType.Destroy;   // 현재 상태 파괴중으로 설정
            if (IsExplosive)
            {
                TryBoom();
            }
            else
            {
                // -- 파괴 동작 코루틴 추가해야됨
                ReturnToPool();                     // 비활성화
            }
        }
    }
    /// <summary>
    /// 폭발 처리하는 메서드
    /// </summary>
    protected void TryBoom()
    {
        // 폭발가능한 오브젝트이고 현재 폭발중이 아닐 때(폭발물 끼리 폭발 범위가 겹쳤을 때 무한 호출 방지)
        if (IsExplosive && currentState != StateType.Boom)
        {
            Debug.Log("터짐");
            currentState = StateType.Boom;  // 현재 상태 폭발중으로 설정
            // -- 폭발 동작 코루틴 추가해야됨
            Collider[] objects = Physics.OverlapSphere(transform.position, explosiveInfo.boomRange);    // 범위 내 모든 물체 검사
            foreach (Collider obj in objects)
            {
                IBattler battler = obj.GetComponent<IBattler>();
                ReactionObject reactionObj = battler as ReactionObject;

                if (reactionObj != null)                // 반응 오브젝트라면
                {
                    Vector3 dir = obj.transform.position - transform.position;  // 날아갈 방향벡터 구하기
                    Vector3 power = dir.normalized * explosiveInfo.force + Vector3.up * explosiveInfo.forceY; // 방향벡터에 파워 지정해주기
                    reactionObj.ExplosionShock(power);  // 폭발시 충격(이동) 가함
                    reactionObj.TryHit(true);      // 폭발 타격(데미지) 가함
                }
                else if (battler != null)
                {
                    Attack(battler);
                }
            }
            BoomAction();
        }
    }

    protected virtual void BoomAction()
    {
        if (objectShape != null && IsExplosive)
        {
            objectShape.gameObject.SetActive(false);
        }
        if (particle != null)
        {
            particle.Play();
            StartCoroutine(ReturnActionWait(particle.main.duration));
        }
        else
        {
            StartCoroutine(ReturnActionWait(0));
        }
    }

    /// <summary>
    /// 폭발시 다른 오브젝트에 충격을 주는 메서드
    /// </summary>
    /// <param name="power">충격량</param>
    public void ExplosionShock(Vector3 power)
    {
        if (IsMoveable)
        {
            if (currentState == StateType.TimeLock)
            {
                SetTimeLockPower(power);
            }
            else
            {
                //rigid.AddForce(power * reducePower, ForceMode.Impulse);   // 무게Ver2
                rigid.AddForce(power, ForceMode.Impulse);
            }
        }
    }

    /// <summary>
    /// 이 물체를 들기 시도하는 메서드
    /// </summary>
    /// <param name="user">드는 사용자</param>
    public virtual void TryPickUp(Transform user)
    {
        if ((IsSkill || IsThrowable) && (currentState == StateType.None || currentState == StateType.Enable))
        {
            ILifter lifter = user.GetComponent<ILifter>();

            if (lifter != null)
            {
                pickUpUser = user;
                transform.parent = lifter.Hand;
                Vector3 destPos = lifter.Hand.position;

                transform.position = destPos;
                transform.forward = lifter.Hand.forward;

                currentState = StateType.PickUp;
                rigid.isKinematic = true;

                lifter.PickUp(this);
            }
        }
    }

    /// <summary>
    /// 이 물체를 던지기 시도하는 메서드
    /// </summary>
    /// <param name="throwPower">던지는 힘</param>
    /// <param name="user">들고있는 사용자</param>
    public void TryThrow(float throwPower, Transform user)
    {
        if (IsThrowable && currentState == StateType.PickUp)
        {
            rigid.isKinematic = false;
            currentState = StateType.Throw;
            //rigid.AddForce((user.forward + user.up) * throwPower * reducePower, ForceMode.Impulse);   // 무게 Ver2
            rigid.AddForce((user.forward + user.up) * throwPower, ForceMode.Impulse);
            transform.parent = originParent;
        }
    }

    /// <summary>
    /// 이 물체를 버리기 시도하는 메서드
    /// </summary>
    public void TryDrop()
    {
        if (IsThrowable)
        {
            transform.parent = originParent;
            currentState = StateType.Drop;
            rigid.isKinematic = false;
            pickUpUser = null;
        }
    }
    #endregion

    /// <summary>
    /// 풀로 돌아감(비활성화)
    /// </summary>
    protected void ReturnToPool()
    {
        pickUpUser = null;
        transform.SetParent(originParent);
        ReturnAction();
    }


    /// <summary>
    /// 풀로 돌아갈 때 동작 (오버라이드용)
    /// </summary>
    protected virtual void ReturnAction()
    {
        gameObject.SetActive(false);
    }

    IEnumerator ReturnActionWait(float time)
    {
        WaitForSeconds wait = new WaitForSeconds(time);
        yield return wait;
        if (isRecycle)
        {
            ReturnToPool();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator BlinkCoroutine()
    {
        while (true)
        {
            SetSkillColor(skillMaterial);
            yield return timeLockInterval;
            SetSkillColor(null);
            yield return timeLockInterval;
        }
    }
    IEnumerator DurationCoroutine()
    {
        while (true)
        {
            ElapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    public void Attack(IBattler target, bool isWeakPoint = false)
    {
        target.Defence(AttackPower);
    }


    public void Defence(float damage)
    {
        TryHit(damage);
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if ((reactionType & ReactionType.Explosion) != 0)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, explosiveInfo.boomRange);
        }
    }


    public void TestBoom()
    {
        TryBoom();
    }


    //private void OnValidate()
    //{
    //    if(Type == ReactionType.Explosion)
    //    {
    //        Type |= ReactionType.Destroy;
    //    }
    //}
#endif

}

