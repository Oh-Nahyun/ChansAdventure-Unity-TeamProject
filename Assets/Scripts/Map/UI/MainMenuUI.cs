using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    Button StartBtn;
    Button LoadBtn;
    Button ExitBtn;
    SaveHandler_Menu LoadMenu;

    PlayerinputActions playerInputAcitons;

    private void Awake()
    {
        playerInputAcitons = new PlayerinputActions();        
    }

    private void Start()
    {

        Transform child = transform.GetChild(0);
        StartBtn = child.GetComponent<Button>();
        StartBtn.onClick.AddListener(OnStart);

        child = transform.GetChild(1);
        LoadBtn = child.GetComponent<Button>();
        LoadBtn.onClick.AddListener(OnLoad);

        child = transform.GetChild(2);
        ExitBtn = child.GetComponent<Button>();
        ExitBtn.onClick.AddListener(OnExit);

        LoadMenu = FindAnyObjectByType<SaveHandler_Menu>();
    }

    private void OnEnable()
    {
        playerInputAcitons.UI.Enable();
        playerInputAcitons.UI.Close.performed += OnClose;
    }

    private void OnDisable()
    {
        playerInputAcitons.UI.Close.performed -= OnClose;
        playerInputAcitons.UI.Disable();        
    }

    // 게임 스타트 버튼
    void OnStart()
    {
        // 게임 처음메인 씬으로 이동
        string sceneName = $"Main_Map_Test";
        GameManager.Instance.ChangeToTargetScene(sceneName, GameManager.Instance.Player.gameObject);
    }

    /// <summary>
    /// 게임 로드 버튼
    /// </summary>
    void OnLoad()
    {
        LoadMenu.ShowSavePanel();
    }

    /// <summary>
    /// 게임 종료 버튼
    /// </summary>
    void OnExit()
    {
        Application.Quit();
    }

    private void OnClose(InputAction.CallbackContext context)
    {
        LoadMenu.CloseSavePanel();
    }
}