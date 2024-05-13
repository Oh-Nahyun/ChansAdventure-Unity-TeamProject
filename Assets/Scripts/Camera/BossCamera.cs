using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    /// <summary>
    /// 플레이어 vcam
    /// </summary>
    public CinemachineVirtualCamera virtualCamera;
    public float delay = 1.5f; // 지연 시간, 2초로 설정

    /// <summary>
    /// 바라볼 대상을 저장하는 트랜스폼
    /// </summary>
    Transform target;

    /// <summary>
    /// 스테이지 진입했을 때 실행하는 델리게이트
    /// </summary>
    public Action OnEntryBossStage;

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    /// <summary>
    /// 카메라 연출을 시작하는 함수
    /// </summary>
    /// <param name="transform">바라볼 대상 트랜스폼</param>
    public void StartBossCameraCoroutine(Transform transform)
    {
        if(!SetLookAt(transform))
        {
            Debug.Log($"카메라가 바라볼 대상이 존재하지 않습니다.");
        }

        OnEntryBossStage?.Invoke();
        StartCoroutine(LowerPriorityAfterDelay());
    }

    /// <summary>
    /// 바라볼 대상을 지정하는 함수
    /// </summary>
    /// <param name="transform"></param>
    bool SetLookAt(Transform targetTransform)
    {
        bool result = true;

        target = targetTransform;
        virtualCamera.LookAt = targetTransform;

        if (target == null) result = false;

        return result;
    }

    IEnumerator LowerPriorityAfterDelay()
    {
        // 지정된 지연 시간 동안 대기
        virtualCamera.Priority = 100;
        yield return new WaitForSeconds(delay);

        // Priority를 0으로 설정
        virtualCamera.Priority = 0;
    }
}
