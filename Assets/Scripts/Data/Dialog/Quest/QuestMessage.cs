using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;

public class QuestMessage : MonoBehaviour
{
    private TextMeshProUGUI questName;
    private CanvasGroup questMessage;
    private CanvasGroup completeMessage;
    private float alphaChangeSpeed = 2.0f;
    public float delayTime = 10.0f;

    private void Awake()
    {
        questName = GetComponentInChildren<TextMeshProUGUI>();
        questMessage = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(2);
        completeMessage = child.GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        questMessage.alpha = 0;
        completeMessage.alpha = 0;
        //completeMessage.gameObject.SetActive(false);
    }

    public void OnQuestMessage(string text, bool complete)
    {
        StopAllCoroutines();
        StartCoroutine(SetOnAlphaChange());
        questName.text = text;
        completeMessage.gameObject.SetActive(complete);
        StartCoroutine(DelayTime(delayTime));
    }

    IEnumerator SetOnAlphaChange()
    {
        while (questMessage.alpha < 1.0f)
        {
            questMessage.alpha += Time.deltaTime * alphaChangeSpeed;
            completeMessage.alpha += Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
    }

    IEnumerator SetOffAlphaChange()
    {
        while (questMessage.alpha > 0.0f)
        {
            questMessage.alpha -= Time.deltaTime * alphaChangeSpeed;
            completeMessage.alpha -= Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        //gameObject.SetActive(false);
    }

    IEnumerator DelayTime(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        StartCoroutine(SetOffAlphaChange());
    }
}

