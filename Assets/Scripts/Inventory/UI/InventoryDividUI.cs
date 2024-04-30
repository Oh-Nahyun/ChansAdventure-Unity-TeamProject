using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryDividUI : MonoBehaviour
{
    CanvasGroup canvasGroup;

    Image itemIcon;
    TMP_InputField inputField;
    Slider slider;
    Button decreaseBtn;
    Button increaseBtn;
    Button okBtn;
    Button cancelBtn;

    InventorySlot targetSlot = null; // 나누기를 수행할 슬롯 

    /// <summary>
    /// 최소값
    /// </summary>
    const int minValue = 1;

    /// <summary>
    /// 나눌 아이템 
    /// </summary>
    int dividCount = 1;

    /// <summary>
    /// 나눌 아이템을 설정 및 접근을 하기 위한 프로퍼티
    /// </summary>
    int DividCount
    {
        get => dividCount;
        set
        {
            dividCount = value;
            dividCount = Mathf.Clamp(value, 1, (int)slider.maxValue);
        }
    }

    public Action<InventorySlot, int> onDivid; // 나눌 때 실행하는 델리게이트

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(0);
        itemIcon = child.GetComponent<Image>();
        child = transform.GetChild(1);
        inputField = child.GetComponent<TMP_InputField>();
        inputField.onValueChanged.AddListener((text) =>
        {
            if (int.TryParse(text, out int value))
            {
                DividCount = value;
            }
            else
            {
                // inputField가 정수만 받게 설정되어 있어서 -값을 넣는 것이 아니면 실행 안됨                
                DividCount = minValue;
            }

            UpdateValue(DividCount);
        });

        child = transform.GetChild(2);
        slider = child.GetComponent<Slider>();
        slider.onValueChanged.AddListener((float count) =>
        {
            // 슬라이더 value 업데이트
            DividCount = (int)count;
            UpdateValue(DividCount);
        });

        child = transform.GetChild(3);
        decreaseBtn = child.GetComponent<Button>();
        decreaseBtn.onClick.AddListener(() =>
        {
            // 왼쪽 버튼 value 업데이트 ( 빼기 )
            DividCount--;
            UpdateValue(DividCount);
        });

        child = transform.GetChild(4);
        increaseBtn = child.GetComponent<Button>();
        increaseBtn.onClick.AddListener(() =>
        {
            // 오른쪽 버튼 value 업데이트 ( 추가하기 )
            DividCount++;
            UpdateValue(DividCount);
        });

        child = transform.GetChild(5);
        okBtn = child.GetComponent<Button>();
        okBtn.onClick.AddListener(() =>
        {
            // 확인 버튼 ( 아이템 나누기 )
            onDivid?.Invoke(targetSlot, DividCount);
            DividUIClose();
        });

        child = transform.GetChild(6);
        cancelBtn = child.GetComponent<Button>();
        cancelBtn.onClick.AddListener(() =>
        {
            // 취소 버튼 ( 패널 닫기 )
            DividUIClose();
        });
    }

    void Start()
    {
        DividUIClose();
    }

    /// <summary>
    /// 패널의 값을 초기화하는 함수
    /// </summary>
    /// <param name="slot">나눌 아이템 슬롯</param>
    /// <param name="minCount">아이템 최소 개수</param>
    /// <param name="maxCount">아이템 최대 개수</param>
    public void InitializeValue(InventorySlot slot, int minCount, int maxCount)
    {       
        itemIcon.sprite = slot.SlotItemData.itemIcon;

        slider.minValue = minCount;
        slider.maxValue = maxCount;
        slider.value = DividCount;

        //DividCount = Mathf.Clamp(DividCount, minCount, maxCount);
        targetSlot = slot;
    }

    /// <summary>
    /// 값을 업데이트 하는 함수
    /// </summary>
    /// <param name="count"></param>
    public void UpdateValue(int count)
    {
        inputField.text = count.ToString();
        slider.value = count;
    }

    /// <summary>
    /// Divid UI 보이게 하는 함수
    /// </summary>
    public void DividUIOpen()
    {
        canvasGroup.alpha = 1;
    }
    
    /// <summary>
    /// Divid UI 숨기는 함수 ( alpha = 0 )
    /// </summary>
    public void DividUIClose()
    {
        canvasGroup.alpha = 0;
    }
}