using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 텍스트 UI를 페이드 인 아웃 하는 클래스
/// </summary>
public class FadeInOutTextUI : MonoBehaviour
{
    /// <summary>
    /// 페이드 인 아웃 할 텍스트 
    /// </summary>
    public TextMeshProUGUI text;

    public float fadeIntime = 1f;
    public float fadeOutTime = 1f;

    float alpha = 0f;

    float Alpha
    {
        get => alpha;
        set
        {
            alpha = Mathf.Clamp(value, 0f, 1f);
        }
    }

    void Start()
    {
        text.color = new Color(1f, 1f, 1f, 0f);
    }

    /// <summary>
    /// 페이드 인 할 때 실행하는 함수
    /// </summary>
    public void StartFadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float timeElapsed = 0f;
        while (timeElapsed < fadeIntime)
        {
            timeElapsed += Time.deltaTime;
            Alpha += timeElapsed;
            text.color = new Color(1f, 1f, 1f, Alpha);

            yield return null;
        }
    }

    /// <summary>
    /// 페이드 아웃할 때 실행하는 함수
    /// </summary>
    public void StartFadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        float timeElapsed = 0f;
        while (timeElapsed < fadeOutTime)
        {
            timeElapsed += Time.deltaTime;
            Alpha -= timeElapsed;
            text.color = new Color(1f, 1f, 1f, Alpha);

            yield return null;
        }
    }
}
