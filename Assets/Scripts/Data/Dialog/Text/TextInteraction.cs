using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextInteraction : MonoBehaviour
{
    CanvasGroup canvasGroup;

    TextMeshProUGUI tagText;
    public TextMeshProUGUI TagText => tagText;
    TextBox textBox;
    TextBoxItem textBoxItem;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        tagText = GetComponentInChildren<TextMeshProUGUI>();
        textBox = FindAnyObjectByType<TextBox>();
        textBoxItem = FindAnyObjectByType<TextBoxItem>();
    }

    private void Start()
    {
        canvasGroup.alpha = 1; 
    }

    /// <summary>
    /// 가장 가까운 오브젝트의 상호작용 텍스트를 출력하는 함수
    /// </summary>
    /// <param name="obj">가장 가까운 오브젝트</param>
    public void SetTagText(GameObject obj)
    {
        switch (obj.tag)
        {
            case "NPC":
                TagText.SetText("말하기");
                break;
            case "Item":
                TagText.SetText("줍기");
                break;
            case "Chest":
                TagText.SetText("열기");
                break;
            case "Warp":
                TagText.SetText("이동하기");
                break;
            case "DoorOpen":
                TagText.SetText("닫기");
                break;
            case "DoorClose":
                TagText.SetText("열기");
                break;
            case "Lever":
                TagText.SetText("당기기");
                break;
            default:
                TagText.SetText("확인하기");
                break;
        }
    }

    /// <summary>
    /// TextInteraction오브젝트를 OnOff관리를 하기 위한
    /// </summary>
    /// <param name="OnOff">ture = On</param>
    public void TextActive(bool OnOff)
    {
        if (textBox.TalkingEnd || textBoxItem.Talking)
        {
            gameObject.SetActive(false);
        }else if (OnOff)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

}
