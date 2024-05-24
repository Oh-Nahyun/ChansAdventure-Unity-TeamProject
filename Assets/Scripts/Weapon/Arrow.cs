using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : RecycleObject
{
    /// <summary>
    /// 화살 속도
    /// </summary>
    float arrowSpeed = 7.0f;

    /// <summary>
    /// 화살 사거리
    /// </summary>
    float arrowRange = 5.0f;

    /// <summary>
    /// 화살 수명
    /// </summary>
    float lifeTime = 10.0f;

    // 컴포넌트들
    ArrowFirePoint arrowFirePoint;
    Collider arrowCollider;
    Rigidbody rigid;
    //ParticleSystem ps;
    Player player;

    private void Awake()
    {
        //ps = GetComponent<ParticleSystem>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        rigid = GetComponent<Rigidbody>();
        arrowCollider = GetComponent<Collider>();
        StartCoroutine(LifeOver(lifeTime));                         // 수명 설정
        rigid.angularVelocity = Vector3.zero;                       // 이전의 회전력 제거

        if (player == null)
        {
            player = GameManager.Instance.Player;   // 플레이어 찾기
        }
        else
        {
            arrowFirePoint = FindAnyObjectByType<ArrowFirePoint>();
            arrowRange = arrowFirePoint.arrowFireRange;
            //rigid.velocity = player.transform.forward * arrowSpeed * arrowRange;    // 발사 방향과 속도 설정 // transform.up        }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        StopAllCoroutines();
        StartCoroutine(LifeOver(5.0f)); // 충돌하고 5초 뒤에 사라짐
    }

    // 플레이어가 화살로 적을 공격했을 때 ---------------------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11) // 11 : hitpoint layer
        {
            if (other.CompareTag("BodyPoint"))
            {
                // 몸에 화살을 맞췄을 경우
                IBattler target = other.GetComponentInParent<IBattler>();
                if (target != null)
                {
                    player.Attack(target, false);
                }
            }
            else if (other.CompareTag("WeakPoint"))
            {
                // 적에게 화살을 맞췄을 경우
                IBattler target = other.GetComponentInParent<IBattler>();
                if (target != null)
                {
                    player.Attack(target, true);
                }
            }
            else
            {
                IBattler target = other.GetComponentInParent<IBattler>();
                if (target != null)
                {
                    player.Attack(target, false);
                }
            }
        }
    }
    // --------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 발사 되었을 때 실행되는 함수
    /// </summary>
    public void Fired(float pressedTime)
    {
        transform.rotation = Quaternion.Euler(90f, 0f, -player.transform.localEulerAngles.y); // 카메라 회전과 동일 시 하기 ( 기본 회전값 90,0,0 )
        rigid.AddForce( transform.up * arrowRange * arrowSpeed * pressedTime, ForceMode.Impulse);
    }

    public void SetArrowSpeed(float drawTime)
    {
        arrowSpeed *= drawTime;
    }

    /// <summary>
    /// 화살의 Collider를 켜는 함수 (Animation 설정용)
    /// </summary>
    public void ArrowColliderEnable()
    {
        arrowCollider.enabled = true;
    }

    /// <summary>
    /// 화살의 Collider를 끄는 함수 (Animation 설정용)
    /// </summary>
    public void ArrowColliderDisable()
    {
        arrowCollider.enabled = false;
    }

    /// <summary>
    /// 화살을 꺼내는 함수 (Animation 설정용)
    /// </summary>
    public void OpenArrow()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 화살을 넣는 함수 (Animation 설정용)
    /// </summary>
    public void CloseArrow()
    {
        gameObject.SetActive(false);
    }
}
