using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceMaker_Ice : ReactionObject
{
    [Header("아이스메이커용 얼음 데이터")]
    /// <summary>
    /// 얼음의 크기
    /// </summary>
    public Vector3 size = new Vector3(1.7f, 2f, 1.7f);

    /// <summary>
    /// 얼음의 크기 프로퍼티 (get)
    /// </summary>
    public Vector3 Size => size;

    /// <summary>
    /// 생성시 걸리는 시간 변수
    /// </summary>
    float generateTime = 1.0f;

    /// <summary>
    /// 사용자와 멀어졌을 때 파괴되는 거리
    /// </summary>
    float sqrDestroyDistance = 20.0f;

    /// <summary>
    /// 생성시 걸리는 시간 프로퍼티
    /// </summary>
    public float GenerateTime
    {
        set
        {
            if (generateTime != value)
            {
                generateTime = value;
                animator.SetFloat(Hash_GenerateSpeed, generateTime);    // 걸리는 시간을 통해 생성 애니메이션 속도 조절 (기본적으로 1초여서 걸리는 시간을 넣으면 맞음)
            }
        }
    }

    // 애니메이터 해시값
    readonly int Hash_Generate = Animator.StringToHash("Generate");
    readonly int Hash_Destroy = Animator.StringToHash("Destroy");
    readonly int Hash_GenerateSpeed = Animator.StringToHash("GenerateSpeed");

    // 컴포넌트
    Animator animator;
    Transform user;

    public Action<IceMaker_Ice> onDestroy;

    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        animator.SetFloat(Hash_GenerateSpeed, generateTime);

        Transform ice = transform.GetChild(0);
        ice.localScale = size;
        BoxCollider collider = transform.GetComponent<BoxCollider>();
        collider.size = size;
    }

    protected override void OnEnable()
    {
        StopAllCoroutines();
        base.OnEnable();
        animator.SetTrigger(Hash_Generate);
    }

    protected override void OnDisable()
    {
        user = null;
        onDestroy = null;
        base.OnDisable();
    }

    /// <summary>
    /// 얼음 생성시 초기화 메서드
    /// </summary>
    /// <param name="user">얼음을 사용한 유저</param>
    /// <param name="destroyDistance">유저와 멀어졌을 때 파괴되는 거리</param>
    /// <param name="rotation">유저의 회전각 (정면으로 소환되도록)</param>
    public void Initialize(Transform user, float destroyDistance, Vector3 rotation)
    {
        transform.Rotate(rotation);
        this.user = user;
        sqrDestroyDistance = destroyDistance * destroyDistance;
    }

    private void Update()
    {
        Vector3 direction = user.position - transform.position;
        if(direction.sqrMagnitude > sqrDestroyDistance)
        {
            onDestroy?.Invoke(this);
        }
    }

    protected override void ReturnAction()
    {
        onDestroy?.Invoke(this);
    }

    /// <summary>
    /// 애니메이션을 파괴로 변경하기 위한 메서드
    /// </summary>
    public void SetDestroy()
    {
        animator.SetTrigger(Hash_Destroy);
        StartCoroutine(DestroyCoroutine());
    }

    IEnumerator DestroyCoroutine()
    {
        yield return null;
        gameObject.SetActive(false);
    }
}
