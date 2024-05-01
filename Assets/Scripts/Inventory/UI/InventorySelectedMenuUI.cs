using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySelectedMenuUI : MonoBehaviour
{
    CanvasGroup canvasGroup;
    Button dividButton;
    Button dropButton;

    /// <summary>
    /// 매뉴 보여줄때 alpha값
    /// </summary>
    readonly float ShowPanelValue = 1f;

    /// <summary>
    /// 매뉴 숨길때 alpha값
    /// </summary>
    readonly float HidePanelValue = 0f;

    /// <summary>
    /// 나누기 버튼 눌렀을 때 실행하는 델리게이트
    /// </summary>
    public Action OnDividButtonClick;

    /// <summary>
    /// 드롭 버튼 눌렀을 때 실행하는 델리게이트
    /// </summary>
    public Action OnDropButtonClick;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform child = transform.GetChild(0); // background Object
        dividButton = child.GetChild(0).GetComponent<Button>();
        dividButton.onClick.AddListener(() => 
        {
            OnDividButtonClick?.Invoke();
        });

        dropButton = child.GetChild(1).GetComponent<Button>();
        dropButton.onClick.AddListener(() =>
        {
            OnDropButtonClick?.Invoke();
        });

        HideMenu();
    }

    public void SetPosition(Vector2 postiion)
    {
        transform.position = postiion;
    }

    /// <summary>
    /// SelecetdMenuUI 보여주기
    /// </summary>
    public void ShowMenu()
    {
        canvasGroup.alpha = ShowPanelValue;
    }

    /// <summary>
    /// SelectedMenuUI 숨기기
    /// </summary>
    public void HideMenu()
    {
        canvasGroup.alpha = HidePanelValue;
    }
}