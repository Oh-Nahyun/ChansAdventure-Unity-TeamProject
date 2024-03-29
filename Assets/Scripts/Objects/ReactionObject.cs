using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UIElements.UxmlAttributeDescription;

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
    Skill = 64          // 스킬일 경우 확인용
}

[RequireComponent(typeof(Rigidbody))]
public class ReactionObject : RecycleObject
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
    public float Weight = 1.0f;
    /// <summary>
    /// 무게로 감소되는 이동량 계산용
    /// </summary>
    protected float reducePower = 1.0f;
    /// <summary>
    /// 최대 체력 (오브젝트가 파괴,폭발할 때 필요한 데미지량)
    /// </summary>
    public float objectMaxHp = 1.0f;

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
                // 현재 상태가 파괴가 아니고 HP가 0 이하로 떨어지면 파괴
                DestroyReaction();
            }
        }
    }

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

    public bool IsHitable => (reactionType & ReactionType.Hit) != 0;
    /// <summary>
    /// 스킬 인지 확인용
    /// </summary>
    public bool IsSkill => (reactionType & ReactionType.Skill) != 0;

    /// <summary>
    /// 현재 상태를 나타내는 enum
    /// </summary>
    protected enum StateType
    {
        None = 0,   // 아무것도 아님(원래상태)
        PickUp,     // 들려있음
        Throw,      // 던져짐
        Move,       // 이동중
        Destroy,    // 파괴중
        Boom,       // 터지는중
    }
    /// <summary>
    /// 현재 상태(기본, 들림, 던져짐, 이동, 파괴, 폭발)
    /// </summary>
    protected StateType currentState = StateType.None;

    //bool isCarried = false;
    //bool isThrow = false;

    /// <summary>
    /// 원래 부모 (들렸을 때 변경된 부모를 되돌리기 위함)
    /// </summary>
    protected Transform originParent;

    /// <summary>
    /// 자석에 붙었을 때 이동속도 (자석스킬에서 받아옴)
    /// </summary>
    float attachMoveSpeed;

    /// <summary>
    /// 자석에 붙었을 때 목적지
    /// </summary>
    Transform magentDestination;


    /// <summary>
    /// 자석에 붙어있는지 확인 (자석 목적지가 있는 경우 true)
    /// </summary>
    bool IsAttachMagnet => magentDestination != null;

    // 컴포넌트
    protected Rigidbody rigid;


    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        if(rigid == null)
        {
            rigid = transform.AddComponent<Rigidbody>();
        }
        originParent = transform.parent;
        reducePower = 1.0f / Weight;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        currentState = StateType.None;
        ObjectHP = objectMaxHp;
    }

    private void FixedUpdate()
    {
        if (IsAttachMagnet)
        {
            AttachMagnetMove();
        }
    }
    protected void OnCollisionEnter(Collision collision)
    {
        if (currentState == StateType.Throw) // 현재 오브젝트가 던져진 상태일 때
        {
            CollisionAfterThrow();
        }
    }

    protected void OnCollisionExit(Collision collision)
    {
        if (IsAttachMagnet)
        {
            // 현재 자석에 붙어있다면
            rigid.velocity = Vector3.zero;          // 물체에서 부딪친 후 밀리는 힘 제거
            rigid.angularVelocity = Vector3.zero;   // 물체에서 부딪친 후 회전하는 힘 제거
        }
    }

    /// <summary>
    /// 자석에 붙어있을 때 움직이는 동작을 하는 메서드
    /// </summary>
    public void AttachMagnetMove()
    {
        Vector3 dir = magentDestination.position - rigid.position;
        if (dir.sqrMagnitude > 0.001f * attachMoveSpeed)            // 이동속도에 따라 적당한 정지거리 지정(떨림 방지)
        {
            rigid.MovePosition(rigid.position + Time.fixedDeltaTime * attachMoveSpeed * dir.normalized);    // 이동
        }
        else
        {
            rigid.MovePosition(magentDestination.position);         // 정지거리 도달 시 정확한 목적지로 옮기기
        }
    }

    /// <summary>
    /// 자석에 붙을 때 동작하는 메서드
    /// </summary>
    /// <param name="destination">자석으로 옮기고자하는 목적지</param>
    /// <param name="moveSpeed">자석에 붙어서 이동할 때 속도</param>
    public void AttachMagnet(Transform destination, float moveSpeed)
    {
        if(IsMagnetic)
        {
            // 중력 사용 x, 자석 목적지 설정, 이동속도 설정
            rigid.useGravity = false;
            magentDestination = destination;
            attachMoveSpeed = moveSpeed;
        }
    }

    /// <summary>
    /// 자석에서 떨어질 때 동작하는 메서드
    /// </summary>
    public void DettachMagnet()
    {
        if (IsMagnetic)
        {
            // 중력 원래대로, 목적지 없애기
            rigid.useGravity = true;
            magentDestination = null;
        }
    }

    /// <summary>
    /// 자석에 붙어있을 때 카메라 회전을 해도 자석 사용자를 그대로 바라보기 위한 메서드
    /// </summary>
    /// <param name="euler">사용자의 회전 각도</param>
    public void AttachRotate(Vector3 euler)
    {
        rigid.MoveRotation(Quaternion.Euler(rigid.rotation.eulerAngles + euler));
    }


    /// <summary>
    /// 던져 졌을 때 추가 동작을 위한 메서드
    /// </summary>
    protected virtual void CollisionAfterThrow()
    {
        currentState = StateType.None;  // 현재 상태를 기본 상태로
        DestroyReaction();              // 파괴 동작 (내부에서 파괴 가능한지 판단)
    }


    /// <summary>
    /// 피격시 동작하는 메서드 (무기, 폭발 등)
    /// </summary>
    /// <param name="isExplosion">폭발 피해 확인용 (true: 폭발)</param>
    public void HitReaction(bool isExplosion = false)
    {
        if (isExplosion)
        {
            // 폭발하는 오브젝트면 터짐
            Boom();
        }
        else
        {
            HitReaction(1.0f);
        }
    }
    /// <summary>
    /// 피격시 동작하는 메서드 (무기, 폭발 등)
    /// </summary>
    /// <param name="damage">피격 데미지</param>
    public void HitReaction(float damage)
    {
        if (IsHitable)
        {
            ObjectHP -= damage;
        }
    }
    /// <summary>
    /// 파괴하는 메서드
    /// </summary>
    protected void DestroyReaction()
    {
        if (IsDestructible) 
        {
            currentState = StateType.Destroy;   // 현재 상태 파괴중으로 설정
            // -- 파괴 동작 코루틴 추가해야됨
            ReturnToPool();                     // 비활성화
        }
    }
    /// <summary>
    /// 폭발 처리하는 메서드
    /// </summary>
    protected void Boom()
    {
        // 폭발가능한 오브젝트이고 현재 폭발중이 아닐 때(폭발물 끼리 폭발 범위가 겹쳤을 때 무한 호출 방지)
        if (IsExplosive && currentState != StateType.Boom)
        {
            currentState = StateType.Boom;  // 현재 상태 폭발중으로 설정
            // -- 폭발 동작 코루틴 추가해야됨
            Collider[] objects = Physics.OverlapSphere(transform.position, explosiveInfo.boomRange);    // 범위 내 모든 물체 검사
            foreach (Collider obj in objects)
            {
                ReactionObject reactionObj = obj.GetComponent<ReactionObject>();

                if (reactionObj != null)                // 반응 오브젝트라면
                {
                    Vector3 dir = obj.transform.position - transform.position;  // 날아갈 방향벡터 구하기
                    Vector3 power = dir.normalized * explosiveInfo.force + obj.transform.up * explosiveInfo.forceY; // 방향벡터에 파워 지정해주기
                    reactionObj.ExplosionShock(power);  // 폭발시 충격(이동) 가함
                    reactionObj.HitReaction(true);      // 폭발 타격(데미지) 가함
                }
            }
            DestroyReaction();
        }
    }

    public void ExplosionShock(Vector3 power)
    {
        if (IsMoveable)
        {
            rigid.AddForce(power * reducePower, ForceMode.Impulse);
        }
    }

    public void PickUp(Transform root)
    {
        if ((IsSkill || IsThrowable) && currentState == StateType.None)
        {
            transform.parent = root;
            Vector3 destPos = root.position;

            transform.position = destPos;
            transform.forward = root.forward;

            currentState = StateType.PickUp;
            rigid.isKinematic = true;
        }
    }

    public void PickUp()
    {
        if (IsThrowable && currentState == StateType.None)
        {
            currentState = StateType.PickUp;
            rigid.isKinematic = true;
        }
    }

    public void Throw(float throwPower, Transform user)
    {
        if (IsThrowable && currentState == StateType.PickUp)
        {
            rigid.isKinematic = false;
            //isCarried = false;
            //isThrow = true;
            currentState = StateType.Throw;

            rigid.AddForce((user.forward + user.up) * throwPower * reducePower, ForceMode.Impulse);
            //rigid.AddRelativeForce((transform.forward + transform.up) * throwPower, ForceMode.Impulse);
            transform.parent = originParent;
        }
    }

    protected void ReturnToPool()
    {
        ReturnAction();
        transform.SetParent(originParent);
        gameObject.SetActive(false);
    }

    protected virtual void ReturnAction()
    {

    }

    public void Drop()
    {
        if (IsThrowable)
        {
            transform.parent = originParent;
            //isCarried = true;
            currentState = StateType.None;
            rigid.isKinematic = false;
        }
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

    //private void OnValidate()
    //{
    //    if(Type == ReactionType.Explosion)
    //    {
    //        Type |= ReactionType.Destroy;
    //    }
    //}
#endif

}

