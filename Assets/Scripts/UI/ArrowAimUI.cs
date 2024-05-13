using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArrowAimUI : MonoBehaviour
{
    /// <summary>
    /// 타이머
    /// </summary>
    float timer = 0.0f;

    /// <summary>
    /// UI 이동 속도
    /// </summary>
    float moveSpeed = 5.0f;

    /// <summary>
    /// 원래 위치
    /// </summary>
    Vector3 originalPosition01;
    Vector3 originalPosition02;
    Vector3 originalPosition03;

    /// <summary>
    /// 타겟 위치까지의 offset 변수
    /// </summary>
    Vector3 targetOffset01 = new Vector3(0, -1.1f, 0);
    Vector3 targetOffset02 = new Vector3(-1, 0.5f, 0);
    Vector3 targetOffset03 = new Vector3(0.9f, 0.47f, 0);

    /// <summary>
    /// 보유중인 화살 개수
    /// </summary>
    TextMeshProUGUI arrowCountUI;

    // 트랜스폼
    Transform aim01;
    Transform aim02;
    Transform aim03;

    // 컴포넌트
    Weapon weapon;

    Weapon PlayerWeapon
    {
        get => weapon;
        set
        {
            if (weapon != value)
            {
                if(weapon == null)
                {
                    weapon = FindAnyObjectByType<Weapon>();
                }

                weapon = value;
            }
        }
    }

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        arrowCountUI = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(1);
        aim01 = child.GetChild(0);
        originalPosition01 = aim01.position;
        aim02 = child.GetChild(1);
        originalPosition02 = aim02.position;
        aim03 = child.GetChild(2);
        originalPosition03 = aim03.position;

        weapon = FindAnyObjectByType<Weapon>();
    }

    private void Update()
    {
        if (PlayerWeapon != null)
        {
            arrowCountUI.text = PlayerWeapon.ArrowCount.ToString(); // 남은 화살 개수 출력
            timer += Time.deltaTime;                        // 타이머 갱신
            PrintArrowAim();
        }
    }

    /// <summary>
    /// 화살 카메라 상태에 따른 ArrowAim UI 생성 함수
    /// </summary>
    void PrintArrowAim()
    {
        if (weapon.IsZoomIn)
        {
            StopAllCoroutines();
            StartCoroutine(ZoomInArrowAim());
        }
    }

    /// <summary>
    /// 화살 카메라 줌인 상태일 때, 변하는 ArrowAim UI 코루틴
    /// </summary>
    IEnumerator ZoomInArrowAim()
    {
        // timeElapsed 초기화
        float timeElapsed = 0.0f;

        // 일정 시간(3초)이 지나면 더 이상 이동하지 않음
        while (timer < 3.0f)
        {
            // timeElapsed 갱신
            timeElapsed += moveSpeed * Time.deltaTime;

            // 줌인 상태이면 목표 위치를 향해 이동함
            aim01.position = Vector3.Lerp(aim01.position, aim01.position + targetOffset01, timeElapsed);
            aim02.position = Vector3.Lerp(aim02.position, aim02.position + targetOffset02, timeElapsed);
            aim03.position = Vector3.Lerp(aim03.position, aim03.position + targetOffset03, timeElapsed);

            yield return null;
        }
    }

    /// <summary>
    /// 화살 카메라 줌아웃 상태일 때, 원래 위치로 돌아가는 ArrowAim UI 함수
    /// </summary>
    public void ZoomOutArrowAim()
    {
        // 타이머 초기화
        timer = 0.0f;

        // ArrowAim UI 위치 초기화
        aim01.position = originalPosition01;
        aim02.position = originalPosition02;
        aim03.position = originalPosition03;
    }
}
