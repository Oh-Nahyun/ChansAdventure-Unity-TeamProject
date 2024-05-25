using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class InventoryDetailUI : MonoBehaviour
{
    Image itemImage;

    TextMeshProUGUI itemName;
    TextMeshProUGUI itemDesc;
    TextMeshProUGUI itemPrice;

    CanvasGroup canvasGroup;

    /// <summary>
    /// 임시 슬롯 UI창이 열렸는지 확인하는 프로퍼티 ( true : 열려있음 , false : 닫혀있음 )
    /// </summary>
    bool IsOpen => transform.localScale == Vector3.one;

    float fadeInSpeed = 3f;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(1);
        itemImage = child.GetComponent<Image>();
        child = transform.GetChild(2);
        itemName = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(3);
        itemDesc = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(4);
        itemPrice = child.GetComponent<TextMeshProUGUI>();
        if(itemPrice == null) // 판매 패널이 아님
        {
            itemPrice = child.GetChild(1).GetComponent<TextMeshProUGUI>();  
        }

        itemName.text = $"아이템 명";
        itemDesc.text = $"아이템 설명";
        itemPrice.text = $"999999999";

        CloseItemDetail();
    }

    void Update()
    {
    }

    public void MovePosition(Vector2 mousePosition)
    {
        RectTransform rect = (RectTransform)transform;

        rect.position = mousePosition;
        // 현재 포지션 + width > maxwidth 넘으면 = maxwidth
        int overWidth = (int)(rect.position.x + rect.sizeDelta.x);

        overWidth = Mathf.Max(0, overWidth);

        if(overWidth > Screen.width)
        {
            rect.position = new Vector3(Screen.width - rect.sizeDelta.x, rect.position.y);
        }
    }

    /// <summary>
    /// 아이템 설명 텍스트를 설정하는 함수
    /// </summary>
    /// <param name="name">아이템 이름</param>
    /// <param name="desc">아이템 설명</param>
    /// <param name="price">아이템 가격</param>
    public void SetDetailText(string name, string desc, uint price, Sprite icon)
    {
        itemImage.sprite = icon;
        itemName.text = $"{name}";
        itemDesc.text = $"{desc}";
        itemPrice.text = price.ToString("N0");
    }

    /// <summary>
    /// 아이템 설명 초기화 함수
    /// </summary>
    public void ClearText()
    {
        itemName.text = $"아이템 명";
        itemDesc.text = $"아이템 설명";
        itemPrice.text = $"999999999";
    }
    /// <summary>
    /// 임시 슬롯을 여는 함수
    /// </summary>
    public void ShowItemDetail()
    {
        //canvasGroup.alpha = 1;
        StartCoroutine(FadeInDetail());
    }

    /// <summary>
    /// 임시 슬롯을 닫는 함수
    /// </summary>
    public void CloseItemDetail()
    {
        StopAllCoroutines();
        canvasGroup.alpha = 0;
    }

    IEnumerator FadeInDetail()
    {
        float timeElpased = 0;

        while (timeElpased < 1f)
        {
            timeElpased += Time.deltaTime;
            canvasGroup.alpha = timeElpased * fadeInSpeed;
            yield return null;
        }
    }
}