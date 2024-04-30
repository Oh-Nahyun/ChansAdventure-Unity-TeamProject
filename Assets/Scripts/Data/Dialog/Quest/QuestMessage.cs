using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestMessage : MonoBehaviour
{
    /// <summary>
    /// 퀘스트 이름
    /// </summary>
    TextMeshProUGUI questName;

    TextMeshProUGUI QuestName => questName;

    CanvasGroup questMessage;

    CanvasGroup completeMessage;

    public float alphaChangeSpeed = 5.0f;
    /// <summary>
    /// 퀘스트 메시지가 남아있는 시간
    /// </summary>
    float delayTime = 5.0f;


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
    }



    /// <summary>
    /// 퀘스트가 시작, 클리어시 표시
    /// </summary>
    /// <param name="text">퀘스트 이름</param>
    /// <param name="Complete">true면 퀘스트 클리어</param>
    public void onQuestMessage(string text, bool Complete)
    {
        StartCoroutine(SetOnAlphaChange());
        questName.text = text;
        if (Complete)
        {
            completeMessage.alpha = 1;
        }
        else
        {
            completeMessage.alpha = 0;
        }
        StartCoroutine(DelayTime(delayTime));
        StartCoroutine(SetOffAlphaChange());
    }

    IEnumerator SetOnAlphaChange()
    {
        while (questMessage.alpha > 0.0f)
        {
            questMessage.alpha -= Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    IEnumerator SetOffAlphaChange()
    {
        while (questMessage.alpha > 0.0f)
        {
            questMessage.alpha -= Time.deltaTime * alphaChangeSpeed;
            yield return null;
        }
        gameObject.SetActive(false);
    }

    IEnumerator DelayTime(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
    }

}
