using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCamera : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float delay = 1.5f; // 지연 시간, 2초로 설정

    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        // 코루틴 시작
        StartCoroutine(LowerPriorityAfterDelay());
    }

    IEnumerator LowerPriorityAfterDelay()
    {
        // 지정된 지연 시간 동안 대기
        yield return new WaitForSeconds(delay);

        // Priority를 0으로 설정
        virtualCamera.Priority = 0;
    }
}
