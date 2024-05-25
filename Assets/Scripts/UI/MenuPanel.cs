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
    
    TextMeshProUGUI currnetPanelName;
    TextMeshProUGUI prePanelName;
    TextMeshProUGUI nextPanelName;
    
    PlayerinputActions inputActions;

    SaveHandler_Base saveHandler;
    NormalPanelUI normalPanel;

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
        Transform topPanel = transform.GetChild(0);
        Transform child = topPanel.transform.GetChild(0);
        currnetPanelName = child.GetComponent<TextMeshProUGUI>();
        child = topPanel.transform.GetChild(2);
        prePanelName = child.GetComponent<TextMeshProUGUI>();
        child = topPanel.transform.GetChild(4);
        nextPanelName = child.GetComponent<TextMeshProUGUI>();

        topPanel = transform.GetChild(1);
        normalPanel = topPanel.GetComponent<NormalPanelUI>();

        inputActions = new PlayerinputActions();
    }

    void OnEnable()
    {
        canvasGroup = gameObject.GetComponent<CanvasGroup>();
        saveHandler = FindAnyObjectByType<SaveHandler_Base>();
    }

    void OnDisable() // 비활성화 되면 인풋 제거
    {
        inputActions.UI.PrePanel.performed -= OnLeftArrow;
        inputActions.UI.NextPanel.performed -= OnRightArrow;
        inputActions.UI.Close.performed -= OnClose;
        inputActions.UI.Disable();
    }

    void OnDestroy()
    {
        canvasGroup = null;
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
        CloseMenu();
    }

    public void ShowNormal()
    {
        GameManager.Instance.ItemDataManager.InventoryUI.CloseInventory();
        GameManager.Instance.MapManager.CloseMapUI();
        saveHandler.CloseSavePanel(); // 세이브 패널 닫기
        normalPanel.ShowUI();

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
        saveHandler.CloseSavePanel(); // 세이브 패널 닫기
        normalPanel.CloseUI();

        GameManager.Instance.MapManager.CloseMiniMapUI();
    }

    /// <summary>
    /// 모든 UI를 닫고 맵을 여는 함수
    /// </summary>
    public void ShowMap()
    {
        GameManager.Instance.ItemDataManager.InventoryUI.CloseInventory();
        GameManager.Instance.MapManager.OpenMapUI();
        saveHandler.CloseSavePanel(); // 세이브 패널 닫기
        normalPanel.CloseUI();

        GameManager.Instance.MapManager.CloseMiniMapUI();
    }

    /// <summary>
    /// 모든 UI를 닫고 세이브창 여는 함수
    /// </summary>
    public void ShowSave()
    {
        GameManager.Instance.ItemDataManager.InventoryUI.CloseInventory();
        GameManager.Instance.MapManager.CloseMapUI();
        saveHandler.ShowSavePanel(); // 세이브 패널 열기
        normalPanel.CloseUI();

        GameManager.Instance.MapManager.CloseMiniMapUI();
    }

    /// <summary>
    /// 매뉴패널을 활성화 하는 함수
    /// </summary>
    public void ShowMenu(MenuState setState)
    {
        State = setState;
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

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
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        inputActions.UI.PrePanel.performed -= OnLeftArrow;
        inputActions.UI.NextPanel.performed -= OnRightArrow;
        inputActions.UI.Close.performed -= OnClose;
        inputActions.UI.Disable();

        GameManager.Instance.MapManager.OpenMiniMapUI();

        GameManager.Instance.MapManager.IsOpenedLargeMap = false;
        GameManager.Instance.Player.UIPanelClose();

        // close all
        GameManager.Instance.ItemDataManager.InventoryUI.CloseInventory();
        GameManager.Instance.MapManager.CloseMapUI();
        saveHandler.CloseSavePanel(); // 세이브 패널 열기
        normalPanel.CloseUI();
        GameManager.Instance.MapManager.CloseMiniMapUI();
    }
}
