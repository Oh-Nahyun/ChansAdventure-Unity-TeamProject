using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    /// <summary>
    /// 검의 Collider
    /// </summary>
    Collider swordCollider;
    Player player;
    private void Awake()
    {
        player = GameManager.Instance.Player;   // 플레이어 찾기
    }

    void Start()
    {
        swordCollider = GetComponent<Collider>();
    }

    // 플레이어가 화살로 적을 공격했을 때 ---------------------------------------------------------------------------
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"name : {other.gameObject.name} , layer : {other.gameObject.layer}");
        if(other.gameObject.layer == 11) // 11 : hitpoint layer
        {
            // 닿은 대상이 Enemy인지 체크
            if (other.CompareTag("BodyPoint"))
            {
                // 몸에 칼을 맞췄을 경우
                IBattler target = other.GetComponentInParent<IBattler>();
                if (target != null)
                {
                    player.Attack(target, false);
                }
            }
            else if (other.CompareTag("WeakPoint"))
            {
                // 적에게 칼을 맞췄을 경우
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

        if(other.CompareTag("ReactObject")) // 05.13
        {
            ReactionObject obj = other.GetComponent<ReactionObject>();

            if(obj != null)
            {
                obj.ReactionToExternalObj(player.ReactPower, player.transform);
            }
        }
    }
    // --------------------------------------------------------------------------------------------------------------

    /// <summary>
    /// 검의 Collider를 켜는 함수 (Animation 설정용)
    /// </summary>
    public void SwordColliderEnable()
    {
        swordCollider.enabled = true;
    }

    /// <summary>
    /// 검의 Collider를 끄는 함수 (Animation 설정용)
    /// </summary>
    public void SwordColliderDisable()
    {
        swordCollider.enabled = false;
    }

    /// <summary>
    /// 검을 꺼내는 함수 (Animation 설정용)
    /// </summary>
    public void OpenSwordWeapon()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 검을 넣는 함수 (Animation 설정용)
    /// </summary>
    public void CloseSwordWeapon()
    {
        gameObject.SetActive(false);
    }
}
