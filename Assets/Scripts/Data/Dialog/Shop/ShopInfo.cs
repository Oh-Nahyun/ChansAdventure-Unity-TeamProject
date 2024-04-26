using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopInfo : MonoBehaviour
{
    CanvasGroup canvasGroup;
    public CanvasGroup CanvasGroup => canvasGroup;
    TextBox textBox;

    /// <summary>
    /// 상점 창 이 사라지는 속도 
    /// </summary>
    public float alphaChangeSpeed = 5.0f;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        textBox = FindAnyObjectByType<TextBox>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!textBox.TalkingEnd)
        {
            StartCoroutine(setAlphaChange());
        }
    }

    IEnumerator setAlphaChange()
    {
        while (canvasGroup.alpha > 0.0f)
        {
            canvasGroup.alpha -= Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        gameObject.SetActive(false);
    }

}
