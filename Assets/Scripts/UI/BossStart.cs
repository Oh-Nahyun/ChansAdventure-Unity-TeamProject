using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossStart : MonoBehaviour
{
    private CanvasGroup bossTextCanvasGroup;
    private CanvasGroup miniMapPanelCanvasGroup;
    private CanvasGroup bossHpTextCanvasGroup;
    private CanvasGroup sliderCanvasGroup;

    void Start()
    {
        // Canvas의 자식 오브젝트에서 CanvasGroup 컴포넌트를 찾습니다.
        bossTextCanvasGroup = GameObject.Find("BossText").GetComponent<CanvasGroup>();

        // Canvas의 자식 오브젝트인 MiniMapPanel에서 CanvasGroup 컴포넌트를 찾습니다.
        miniMapPanelCanvasGroup = GameObject.Find("MiniMapPanel").GetComponent<CanvasGroup>();

        bossHpTextCanvasGroup = GameObject.Find("BossHpText").GetComponent<CanvasGroup>();

        sliderCanvasGroup = GameObject.Find("Slider").GetComponent<CanvasGroup>();

        // 코루틴을 시작하여 텍스트를 페이드 아웃합니다.
        StartCoroutine(FadeOutText());
        StartCoroutine(FadeInMiniMapPanel());
        StartCoroutine(FadeInBossHpText());
        StartCoroutine(FadeInSlider());
    }

    IEnumerator FadeOutText()
    {
        // 1.5초 동안 대기
        yield return new WaitForSeconds(2.1f);

        // Alpha 값을 0으로 설정하여 텍스트를 페이드 아웃
        bossTextCanvasGroup.alpha = 0;
    }
    IEnumerator FadeInMiniMapPanel()
    {
        // 2.5초 동안 대기
        yield return new WaitForSeconds(2.5f);

        // MiniMapPanel의 Alpha 값을 1로 설정하여 페이드 인
        miniMapPanelCanvasGroup.alpha = 1;
    }

    IEnumerator FadeInBossHpText()
    {
        yield return new WaitForSeconds(2.5f);

        bossHpTextCanvasGroup.alpha = 1;
    }

    IEnumerator FadeInSlider()
    {
        yield return new WaitForSeconds(2.5f);

        sliderCanvasGroup.alpha = 1;
    }
}