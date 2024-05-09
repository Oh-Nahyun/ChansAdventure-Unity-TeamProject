using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ArrowAimUI : MonoBehaviour
{
    /// <summary>
    /// UI 이동 속도
    /// </summary>
    float moveSpeed = 2.0f;

    /// <summary>
    /// aim01 원래 위치
    /// </summary>
    Vector3 originalPosition01 = new Vector3(-0.09f, 58.85f, 0);

    /// <summary>
    /// aim02 원래 위치
    /// </summary>
    Vector3 originalPosition02 = new Vector3(53.28f, -26.49f, 0);

    /// <summary>
    /// aim03 원래 위치
    /// </summary>
    Vector3 originalPosition03 = new Vector3(-52.11f, -25.56f, 0);

    /// <summary>
    /// aim 위치에서 떨어지는 정도
    /// </summary>
    Vector3 offset = new Vector3(0, -5, 0);

    TextMeshProUGUI arrowCount;

    Transform aim01;
    Transform aim02;
    Transform aim03;

    Weapon weapon;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        arrowCount = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(1);
        aim01 = child.GetChild(0);
        aim02 = child.GetChild(1);
        aim03 = child.GetChild(2);

        weapon = FindAnyObjectByType<Weapon>();
    }

    private void Update()
    {
        arrowCount.text = weapon.arrowCount.ToString(); // 남은 화살 개수 출력
        PrintArrowAim();
    }

    /// <summary>
    /// 화살 카메라 상태에 따른 ArrowAim UI 생성 함수
    /// </summary>
    void PrintArrowAim()
    {
        float timeElapsed = 0.0f;
        timeElapsed += moveSpeed * Time.deltaTime;

        if (weapon.IsZoomIn)
        {
            Vector3 targetPosition01 = aim01.position + new Vector3(0, -1.1f, 0);
            aim01.position = Vector3.Lerp(aim01.position, targetPosition01, timeElapsed);

            Vector3 targetPosition02 = aim02.position + new Vector3(-1, 0.5f, 0);
            aim02.position = Vector3.Lerp(aim02.position, targetPosition02, timeElapsed);

            Vector3 targetPosition03 = aim03.position + new Vector3(0.9f, 0.47f, 0);
            aim03.position = Vector3.Lerp(aim03.position, targetPosition03, timeElapsed);
        }
        else
        {
            //////////////////////////////////// 전체 시간 설정 및 원래 위치로 돌아가도록 구현할 것!!
            aim01.position = originalPosition01;
            aim02.position = originalPosition02;
            aim03.position = originalPosition03;
        }
    }
}

/*
/// <summary>
/// 화살 카메라 상태에 따른 ArrowAim UI 생성 함수
/// </summary>
void PrintArrowAim()
{
    if (weapon.IsZoomIn)
    {
        aim01.position = Vector3.Lerp(originalPosition01, originalPosition01 + new Vector3(0, 26.85f, 0), moveSpeed * Time.deltaTime);
        aim02.position = Vector3.Lerp(originalPosition02, originalPosition02 + offset, moveSpeed * Time.deltaTime);
        aim03.position = Vector3.Lerp(originalPosition03, originalPosition03 + offset, moveSpeed * Time.deltaTime);
    }
    else
    {
        aim01.position = Vector3.Lerp(aim01.position, originalPosition01, moveSpeed * Time.deltaTime);
        aim02.position = Vector3.Lerp(aim02.position, originalPosition02, moveSpeed * Time.deltaTime);
        aim03.position = Vector3.Lerp(aim03.position, originalPosition03, moveSpeed * Time.deltaTime);
    }
}
*/
