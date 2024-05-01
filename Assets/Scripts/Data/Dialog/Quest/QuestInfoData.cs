using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestInfoData : MonoBehaviour
{
    CanvasGroup canvasGroup;

    TextMeshProUGUI qusetName;
    TextMeshProUGUI questContents;
    TextMeshProUGUI questObjectives;

    GameObject oldTarget = null;

    public float alphaChangeSpeed = 5.0f;
    bool onInfo = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(0);
        qusetName = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(1);
        questContents = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        questObjectives = child.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        canvasGroup.alpha = 0;
        //gameObject.SetActive(false);
    }

    public void OnQuestInfo(GameObject newTarget)
    {
        if (onInfo) {
            if (oldTarget != newTarget)
            {
                oldTarget = newTarget;
            }
            else
            {
                oldTarget = null;
                onInfo = false;
                StartCoroutine(setAlphaChange(false));
            }
        }
        else
        {
            oldTarget = newTarget;
            onInfo = true;
            StartCoroutine(setAlphaChange(true));
        }
    }

    public void setQuestList(string questName, string contents, string objectives)
    {
        qusetName.text = questName;
        questContents.text = contents;
        questObjectives.text = objectives;
    }

    IEnumerator setAlphaChange(bool onInfo)
    {
        if (!onInfo)
        {
            while (canvasGroup.alpha > 0.0f)
            {
                canvasGroup.alpha -= Time.deltaTime * alphaChangeSpeed;
                yield return null;
            }
            //gameObject.SetActive(false);
        }
        else
        {
            while (canvasGroup.alpha < 1.0f)
            {
                canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
                yield return null;
            }
        }
    }
}
