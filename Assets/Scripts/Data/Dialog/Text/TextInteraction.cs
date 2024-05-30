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
                TagText.SetText("Talk");
                break;
            case "DropItem":
                TagText.SetText("Pick Up");
                break;
            case "Chest":
                TagText.SetText("Open");
                break;
            case "Warp":
                TagText.SetText("Move");
                break;
            case "DoorOpen":
                TagText.SetText("Close");
                break;
            case "DoorClose":
                TagText.SetText("Open");
                break;
            case "Lever":
                TagText.SetText("Pull");
                break;
            default:
                TagText.SetText("Check");
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
