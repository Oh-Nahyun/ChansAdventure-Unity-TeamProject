using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SellCountUI : MonoBehaviour
{
    CanvasGroup canvasGroup;

    Image itemIcon;
    TMP_InputField inputField;
    Slider slider;
    Button decreaseBtn;
    Button increaseBtn;
    Button okBtn;
    Button cancelBtn;

    InventorySlot targetSlot = null; // 판매할 아이템 슬롯

    /// <summary>
    /// 나눌 아이템 
    /// </summary>
    int sellCount = 1;

    /// <summary>
    /// 나눌 아이템을 설정 및 접근을 하기 위한 프로퍼티
    /// </summary>
    int SellCount
    {
        get => sellCount;
        set
        {
            sellCount = value;
            sellCount = Mathf.Clamp(value, 1, (int)slider.maxValue);
        }
    }

    /// <summary>
    /// 판매할 때 실행하는 델리게이트
    /// </summary>
    public Action<InventorySlot, int> onSell;
    
    /// <summary>
    /// SellCountUI창을 닫았을 때 실행하는 델리게이트
    /// </summary>
    public Action onCloseSellCount;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();
        child = transform.GetChild(1);
        inputField = child.GetComponent<TMP_InputField>();
        child = transform.GetChild(2);
        slider = child.GetComponent<Slider>();
        slider.onValueChanged.AddListener((float count) =>
        {
            // 슬라이더 value 업데이트
            SellCount = (int)count;
            UpdateValue(SellCount);
        });

        child = transform.GetChild(3);
        decreaseBtn = child.GetComponent<Button>();
        decreaseBtn.onClick.AddListener(() =>
        {
            // 왼쪽 버튼 value 업데이트 ( 빼기 )
            SellCount--;
            UpdateValue(SellCount);
        });

        child = transform.GetChild(4);
        increaseBtn = child.GetComponent<Button>();
        increaseBtn.onClick.AddListener(() =>
        {
            // 오른쪽 버튼 value 업데이트 ( 추가하기 )
            SellCount++;
            UpdateValue(SellCount);
        });

        child = transform.GetChild(5);
        okBtn = child.GetComponent<Button>();
        okBtn.onClick.AddListener(() =>
        {
            // 확인 버튼 ( 아이템 나누기 )
            onSell?.Invoke(targetSlot, SellCount);
            SellCountUIClose();
        });

        child = transform.GetChild(6);
        cancelBtn = child.GetComponent<Button>();
        cancelBtn.onClick.AddListener(() =>
        {
            // 취소 버튼 ( 패널 닫기 )
            SellCountUIClose();
        });
    }

    void Start()
    {
        SellCountUIClose();
    }

    public void InitializeValue(InventorySlot slot, int minCount, int maxCount)
    {
        itemIcon.sprite = slot.SlotItemData.itemIcon;

        slider.minValue = minCount;
        slider.maxValue = maxCount;
        slider.value = SellCount;

        //DividCount = Mathf.Clamp(DividCount, minCount, maxCount);
        targetSlot = slot;
    }

    public void UpdateValue(int count)
    {
        inputField.text = count.ToString();
        slider.value = count;
    }

    /// <summary>
    /// SellCountUI 보이게 하는 함수
    /// </summary>
    public void SellCountUIOpen()
    {
        canvasGroup.alpha = 1;
    }

    /// <summary>
    /// SellCountUI 숨기는 함수 ( alpha = 0 )
    /// </summary>
    public void SellCountUIClose()
    {
        canvasGroup.alpha = 0;
        onCloseSellCount?.Invoke();
    }
}