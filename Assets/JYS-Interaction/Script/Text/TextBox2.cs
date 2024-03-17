using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TextBox2 : MonoBehaviour
{
    public float alphaChangeSpeed = 5.0f;

    TextBoxManager textBoxManager;
    TextMeshProUGUI talkText;
    TextMeshProUGUI nameText;
    CanvasGroup canvasGroup;
    public GameObject scanObject;

    public int talkIndex = 0;
    public float charPerSeconds = 0.05f;

    private string talkData;

    public NPCBase objData;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        Transform child = transform.GetChild(0);
        talkText = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(1);
        nameText = child.GetComponent<TextMeshProUGUI>();

        objData = scanObject.GetComponent<NPCBase>();
    }

    private void Start()
    {
        canvasGroup.alpha = 0.0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        talkData = textBoxManager.GetTalk(1000, talkIndex);
        Debug.Log($"{talkData}");
        GameManager.Instance.onTalk += () =>
        {
            Action();
        };

    }

    public void Action()
    {
        talkText.text = "";
        nameText.text = "";
        //scanObject = gameObject;

        StartCoroutine(TalkStart());
    }

    bool talking;

    IEnumerator TalkStart()
    {

        if (!talking)
        {
            while (canvasGroup.alpha < 1.0f)
            {
                canvasGroup.alpha += Time.deltaTime * alphaChangeSpeed;
                yield return null;
            }
            canvasGroup.interactable = true;
            canvasGroup.blocksRaycasts = true;

            objData = scanObject.GetComponent<NPCBase>();
            Talk(objData.id);
            talkText.text = $"{talkData[objData.id]}";
            nameText.text = $"{scanObject.name}";

            StartCoroutine(TypingText(talkText.text));
        }
        else
        {
            while (canvasGroup.alpha > 0.0f)
            {
                canvasGroup.alpha -= Time.deltaTime * alphaChangeSpeed;
                yield return null;
            }
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            talkText.text = "";
            nameText.text = "";
        }
        talking = !talking;

    }

    /// <summary>
    /// 텍스트 타이핑 효과
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    IEnumerator TypingText(string text)
    {
        talkText.text = null;

        for(int i = 0; i < text.Length; i++)
        {
            talkText.text += text[i];
            yield return new WaitForSeconds(charPerSeconds);
        }
    }

    void Talk(int id)
    {
        talkData = textBoxManager.GetTalk(id, talkIndex);

        talkText.text = talkData;
    }

}
