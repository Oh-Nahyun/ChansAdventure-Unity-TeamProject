using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public class SellCheckUI : MonoBehaviour
{
    CanvasGroup canvasGroup;

    /// <summary>
    /// 확인 내용 텍스트 
    /// </summary>
    TextMeshProUGUI checkText;

    /// <summary>
    /// 확인 버튼 
    /// </summary>
    Button okButton;

    /// <summary>
    /// 취소 버튼 
    /// </summary>
    Button cancelButton;

    /// <summary>
    /// show CheckPanel delegate
    /// </summary>
    public Action<InventorySlot, int> onCheckSell;

    /// <summary>
    /// ConformSell item delegate
    /// </summary>
    public Action onConformSell;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        Transform child = transform.GetChild(0);
        checkText = child.GetChild(0).GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(1);
        okButton = child.GetChild(0).GetComponent<Button>();
        okButton.onClick.AddListener(() =>
        {
            onConformSell?.Invoke();
            ClosePanel();
        });

        cancelButton = child.GetChild(1).GetComponent<Button>();
        cancelButton.onClick.AddListener(() =>
        {
            ClosePanel();
        });

        onCheckSell += SetText;
    }

    /// <summary>
    /// 텍스트를 설정하는 함수
    /// </summary>
    /// <param name="slot">설정할 내용이 있는 slot</param>
    public void SetText(InventorySlot slot, int count)
    {
        ItemData itemData = slot.SlotItemData;
        string name = itemData.itemName;
        uint price = itemData.price;

        checkText.text = $"[{name}]을 [{count}]만큼 살께 \n" +
                         $"[{price * count}]을 받을 수 있을꺼야";
    }

    public void ShowCheckPanel()
    {
        canvasGroup.alpha = 1.0f;
    }

    void ClosePanel()
    {
        canvasGroup.alpha = 0.0f;
    }
}