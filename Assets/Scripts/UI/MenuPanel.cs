using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public enum MenuState
{
    Nomal = 0,
    Inventory,
    Map,
    Save
}

public class MenuPanel : MonoBehaviour
{
    CanvasGroup canvasGroup;
    CanvasGroup Canvus
    {
        get
        {
            if(canvasGroup == null)
            {
                this.gameObject.AddComponent<CanvasGroup>();
                CanvasGroup cg = GetComponent<CanvasGroup>();
                canvasGroup = cg;
            }
            return canvasGroup;
        }
        set => canvasGroup = value;
    }
    
    TextMeshProUGUI currnetPanelName;
    TextMeshProUGUI prePanelName;
    TextMeshProUGUI nextPanelName;
    
    PlayerinputActions inputActions;

    /// <summary>
    /// 현재 패널 상태
    /// </summary>
    MenuState state = MenuState.Nomal;

    /// <summary>
    /// 패널 상태를 변경하는 프로퍼티 
    /// </summary>
    MenuState State
    {
        get => state;
        set
        {
            state = value;            
            switch(state)
            {
                case MenuState.Nomal:
                    prePanelName.text = $"세이브";
                    currnetPanelName.text = $"노말";
                    nextPanelName.text = $"인벤토리";
                    ShowNormal();
                    break;
                case MenuState.Inventory:
                    prePanelName.text = $"노말";
                    currnetPanelName.text = $"인벤토리";
                    nextPanelName.text = $"맵";
                    ShowInventory();
                    break;
                case MenuState.Map:
                    prePanelName.text = $"인벤토리";
                    currnetPanelName.text = $"맵";
                    nextPanelName.text = $"세이브";
                    ShowMap();
                    break;
                case MenuState.Save:
                    prePanelName.text = $"맵";
                    currnetPanelName.text = $"세이브";
                    nextPanelName.text = $"노말";
                    ShowSave();
                    break;
                default:
                    break;
            }
        }
    }

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();

        Transform topPanel = transform.GetChild(0);
        Transform child = topPanel.transform.GetChild(0);
        currnetPanelName = child.GetComponent<TextMeshProUGUI>();
        child = topPanel.transform.GetChild(2);
        prePanelName = child.GetComponent<TextMeshProUGUI>();
        child = topPanel.transform.GetChild(4);
        nextPanelName = child.GetComponent<TextMeshProUGUI>();

        inputActions = new PlayerinputActions();
    }

    private void OnRightArrow(InputAction.CallbackContext context)
    {
        if ((int)State == System.Enum.GetValues(typeof(MenuState)).Length - 1)
        {
            State = MenuState.Nomal;
        }
        else
        {
            State++;
        }
    }

    private void OnLeftArrow(InputAction.CallbackContext context)
    {
        if((int)State == 0)
        {
            State = MenuState.Save;
        }
        else
        {
            State--;
        }
    }

    private void OnClose(InputAction.CallbackContext context)
    {
        if(Canvus.alpha == 1) // 매뉴 열려있을 때만 작동
        {
            CloseMenu();
        }
    }

    public void ShowNormal()
    {
        GameManager.Instance.ItemDataManager.InventoryUI.CloseInventory();
        GameManager.Instance.MapManager.CloseMapUI();
        GameManager.Instance.SaveHandler.CloseSavePanel(); // 세이브 패널 닫기

        GameManager.Instance.MapManager.CloseMiniMapUI();
    }

    /// <summary>
    /// 모든 UI를 닫고 인벤토리를 여는 함수
    /// </summary>
    public void ShowInventory()
    {
        GameManager.Instance.ItemDataManager.InventoryUI.ShowInventory();
        GameManager.Instance.ItemDataManager.CharaterRenderCameraPoint.transform.eulerAngles = new Vector3(0, 180f, 0); // RenderTexture 플레이어 위치 초기화
        GameManager.Instance.MapManager.CloseMapUI();
        GameManager.Instance.SaveHandler.CloseSavePanel(); // 세이브 패널 닫기

        GameManager.Instance.MapManager.CloseMiniMapUI();
    }

    /// <summary>
    /// 모든 UI를 닫고 맵을 여는 함수
    /// </summary>
    public void ShowMap()
    {
        GameManager.Instance.ItemDataManager.InventoryUI.CloseInventory();
        GameManager.Instance.MapManager.OpenMapUI();
        GameManager.Instance.SaveHandler.CloseSavePanel(); // 세이브 패널 닫기

        GameManager.Instance.MapManager.CloseMiniMapUI();
    }

    /// <summary>
    /// 모든 UI를 닫고 세이브창 여는 함수
    /// </summary>
    public void ShowSave()
    {
        GameManager.Instance.ItemDataManager.InventoryUI.CloseInventory();
        GameManager.Instance.MapManager.CloseMapUI();
        GameManager.Instance.SaveHandler.ShowSavePanel(); // 세이브 패널 열기

        GameManager.Instance.MapManager.CloseMiniMapUI();
    }

    /// <summary>
    /// 매뉴패널을 활성화 하는 함수
    /// </summary>
    public void ShowMenu(MenuState setState)
    {
        State = setState;
        Canvus.alpha = 1;
        Canvus.interactable = true;
        Canvus.blocksRaycasts = true;

        inputActions.UI.Enable();
        inputActions.UI.PrePanel.performed += OnLeftArrow;
        inputActions.UI.NextPanel.performed += OnRightArrow;
        inputActions.UI.Close.performed += OnClose;

        GameManager.Instance.MapManager.CloseMiniMapUI();

        GameManager.Instance.MapManager.IsOpenedLargeMap = true;
    }


    /// <summary>
    /// 매뉴패널을 비활성화 하는 함수
    /// </summary>
    public void CloseMenu()
    {
        Canvus.alpha = 0;
        Canvus.interactable = false;
        Canvus.blocksRaycasts = false;

        inputActions.UI.PrePanel.performed -= OnLeftArrow;
        inputActions.UI.NextPanel.performed -= OnRightArrow;
        inputActions.UI.Close.performed -= OnClose;
        inputActions.UI.Disable();

        GameManager.Instance.MapManager.OpenMiniMapUI();

        GameManager.Instance.MapManager.IsOpenedLargeMap = false;
    }
}
