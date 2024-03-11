using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 체력 정보가 바뀔 때마다 외부에 있는 메소드 자동 호출할 수 있게 이벤트 클래스 생성
/// </summary>
[System.Serializable]
public class HPEvent : UnityEngine.Events.UnityEvent<int, int> { }

/// <summary>
/// 캐릭터의 상태를 관리하는 스크립트
/// </summary>
public class Status : MonoBehaviour
{
    /// <summary>
    /// HP이벤트 클래스 인스턴스 선언
    /// </summary>
    [HideInInspector]
    public HPEvent onHPEvent = new HPEvent();

    [Header("Move Speed")]
    /// <summary>
    /// 걷는 속도
    /// </summary>
    [SerializeField]
    float walkSpeed;

    /// <summary>
    /// 뛰는 속도
    /// </summary>
    [SerializeField]
    float runSpeed;

    [Header("HP")]
    /// <summary>
    /// 최대 체력
    /// </summary>
    [SerializeField]
    int maxHP = 100;

    /// <summary>
    /// 현재 체력
    /// </summary>
    int currentHP;

    /// <summary>
    /// 외부에서 걷는 속도를 확인하기 위한 프로퍼티
    /// </summary>
    public float WalkSpeed => walkSpeed;

    /// <summary>
    /// 외부에서 뛰는 속도를 확인하기 위한 프로퍼티
    /// </summary>
    public float RunSpeed => runSpeed;

    /// <summary>
    /// 현재 체력 HP 확인을 위한 프로퍼티
    /// </summary>
    public int CurrentHP => currentHP;

    /// <summary>
    /// 최대 체력 HP 확인을 위하나 프로퍼티
    /// </summary>
    public int MaxHP => maxHP;

    private void Awake()
    {
        currentHP = maxHP; // 현재 체력을 최대 체력으로 설정
    }

    /// <summary>
    /// 데미지 감소 확인용 변수
    /// </summary>
    /// <param name="damage">데미지</param>
    /// <returns></returns>
    public bool DecreaseHP(int damage)
    {
        int preHP = currentHP; // 이전의 현재 체력 저장

        // 현재 체력에서 데미지를 뺐을 때 0보다 크면 현재 체력에 데미지 만큼 감소한 체력을 저장 0보다 작으면 현재 체력에 0을 저장
        currentHP = currentHP - damage > 0 ? currentHP - damage : 0;

        onHPEvent.Invoke(preHP, currentHP); // 체력이 바뀐것을 알리기

        if (currentHP == 0) // 현재 체력이 0이면 
        {
            return true; // 참
        }
        // 현재 체력이 0이 아니면
        return false;   // 거짓
    }
}
