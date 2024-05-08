using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 세이브나 로드할 때 활성화되는 창의 컴포넌트
/// </summary>
public class SaveCheckUI : MonoBehaviour
{
    CanvasGroup canvasGroup;
    TextMeshProUGUI uiText;
    TextMeshProUGUI okBtnText;
    TextMeshProUGUI cancelBtnText;
    Button okBtn;
    Button cancelBtn;

    /// <summary>
    /// 세이브할 때 실행하는 델리게이드 ( OK 버튼 누르면 실행 )
    /// </summary>
    public Action<int> onSave;

    /// <summary>
    /// 로드할 때 실행하는 델리게이트 ( OK 버튼 누르면 실행 )
    /// </summary>
    public Action<int> onLoad;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(0);
        uiText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(1);
        okBtn = child.GetComponent<Button>();
        okBtnText = child.GetChild(0).GetComponent<TextMeshProUGUI>();
        okBtnText.text = $"OK";

        child = transform.GetChild(2);  
        cancelBtn = child.GetComponent<Button>();
        cancelBtn.onClick.AddListener(ClosePanel);
        cancelBtnText = child.GetChild(0).GetComponent<TextMeshProUGUI>();
        cancelBtnText.text = $"Cancel";
    }

    /// <summary>
    /// 세이브할 때 확인 하는 창을 띄우는 함수
    /// </summary>
    /// <param name="slotIndex">슬롯 인덱스</param>
    public void ShowSaveCheck(int slotIndex)
    {
        OpenPanel();
        uiText.text = $"{slotIndex}번에 세이브를 하시겠습니까?";

        okBtn.onClick.RemoveAllListeners();
        okBtn.onClick.AddListener(() =>
        {
            onSave?.Invoke(slotIndex);
            ClosePanel();
        });
    }

    /// <summary>
    /// 로드할 때 확인하는 창을 띄우는 함수
    /// </summary>
    /// <param name="slotIndex">슬롯 인덱스</param>
    public void ShowLoadCheck(int slotIndex)
    {
        OpenPanel();
        uiText.text = $"{slotIndex}번 데이터를 로드 하시겠습니까?";

        okBtn.onClick.RemoveAllListeners();
        okBtn.onClick.AddListener(() =>
        {
            onLoad?.Invoke(slotIndex);
            ClosePanel();
        });
    }

    /// <summary>
    /// 패널을 열 때 실행하는 함수
    /// </summary>
    private void OpenPanel()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    /// <summary>
    /// 패널을 닫을 때 실행하는 함수
    /// </summary>
    private void ClosePanel()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}