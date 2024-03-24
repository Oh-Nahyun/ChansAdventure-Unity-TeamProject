using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DividUI : MonoBehaviour
{
    CanvasGroup canvasGroup;

    Image itemIcon;
    TMP_InputField inputField;
    Slider slider;
    Button decreaseBtn;
    Button increaseBtn;
    Button okBtn;
    Button cancelBtn;

    InventorySlot targetSlot = null;
    int dividCount = 1;

    public Action<InventorySlot, int> onDivid;

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
            dividCount = (int)count;
            UpdateValue(dividCount);
        });

        child = transform.GetChild(3);
        decreaseBtn = child.GetComponent<Button>();
        decreaseBtn.onClick.AddListener(() =>
        {
            dividCount--;
            UpdateValue(dividCount);
        });

        child = transform.GetChild(4);
        increaseBtn = child.GetComponent<Button>();
        increaseBtn.onClick.AddListener(() =>
        {
            dividCount++;
            UpdateValue(dividCount);
        });

        child = transform.GetChild(5);
        okBtn = child.GetComponent<Button>();
        okBtn.onClick.AddListener(() =>
        {
            onDivid?.Invoke(targetSlot, dividCount);
            DividUIClose();
        });

        child = transform.GetChild(6);
        cancelBtn = child.GetComponent<Button>();
        cancelBtn.onClick.AddListener(() =>
        {
            DividUIClose();
        });
    }

    void Start()
    {
        DividUIClose();
    }

    public void InitializeValue(InventorySlot slot, int minCount, int maxCount)
    {       
        itemIcon.sprite = slot.SlotItemData.itemIcon;

        slider.minValue = minCount;
        slider.maxValue = maxCount;

        dividCount = Mathf.Clamp(dividCount, minCount, maxCount);
        targetSlot = slot;
    }

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
