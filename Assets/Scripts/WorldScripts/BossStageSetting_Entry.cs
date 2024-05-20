using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 보스 스테이지 입구 스크립트
/// </summary>
public class BossStageSetting_Entry : MonoBehaviour
{
    /// <summary>
    /// 스테이지 세팅
    /// </summary>
    BossStageSetting stageSetting;

    /// <summary>
    /// 보스 연출 카메라
    /// </summary>
    BossCamera bossCamera;

    /// <summary>
    /// 보스 이름
    /// </summary>
    FadeInOutTextUI bossNameUI;

    /// <summary>
    /// 보스 체력바
    /// </summary>
    BossHPSlider bossHPSlider;

    Collider collider;

    void Start()
    {
        bossCamera = FindAnyObjectByType<BossCamera>();
        stageSetting = GetComponentInParent<BossStageSetting>();
        bossNameUI = FindAnyObjectByType<FadeInOutTextUI>();
        bossHPSlider = FindAnyObjectByType<BossHPSlider>();

        collider = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            OnStageEnter();
        }
    }

    /// <summary>
    /// 스테이지 진입할 때 실행하는 함수
    /// </summary>
    void OnStageEnter()
    {
        Transform bossTransform = stageSetting.GetBoss().gameObject.transform;
        // Boss spawn
        bossNameUI.StartFadeInOut();
        bossCamera.StartBossCameraCoroutine(bossTransform);
        bossTransform.gameObject.SetActive(true);

        bossHPSlider.ShowPanel();

        collider.isTrigger = false; // 트리거 비활성화
        transform.localPosition += Vector3.left * 2f;
    }
}
