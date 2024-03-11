using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 적 스폰 포인트 제어할 스크립트
/// </summary>
public class EnemySpawnPoint : MonoBehaviour
{
    /// <summary>
    /// 적이 나타날 위치에 깜빡거리는 표시 해주는 속도
    /// </summary>
    [SerializeField]
    float fadeSpeed = 4;

    /// <summary>
    /// 매쉬랜더러 변수
    /// </summary>
    MeshRenderer meshRenderer;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>(); // 매쉬랜더러 컴포넌트 찾기
    }

    private void OnEnable()
    {
        StartCoroutine("OnFadeEffect"); // 깜빡거리는 효과 코루틴 시작
    }

    private void OnDisable()
    {
        StopCoroutine("OnFadeEffect"); // 깜빡거리는 효과 코루틴 정지
    }

    /// <summary>
    /// 깜빡 거리는 효과를 나타나게 할 코루틴
    /// </summary>
    /// <returns></returns>
    IEnumerator OnFadeEffect()
    {
        while (true)
        {
            Color color = meshRenderer.material.color;  // 랜더러에 머테리얼 색을 저장할 컬러 변수
            // Mathf.PingPong(Time.deltaTime * fadeSpeed, 1) : Time.deltaTime * fadeSpeed 값에 따라 0부터 1 사이의 값이 반환
            // Time.deltaTime * fadeSpeed 증가할 때 1까지 Time.deltaTime * fadeSpeed 값을 반환하고 
            // Time.deltaTime * fadeSpeed 값이 1보다 커졌을 때 순차적으로 0 까지 뺴준다, 1까지 더하기 반복
            // 머테리얼의 색의 알파값을 변경 시켜주기
            color.a = Mathf.Lerp(1, 0, Mathf.PingPong(Time.time * fadeSpeed, 1));
            // 색 변경
            meshRenderer.material.color = color;

            yield return null;
        }
    }
}
