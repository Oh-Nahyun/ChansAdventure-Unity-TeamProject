using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player => player;

    Weapon weapon;
    public Weapon Weapon => weapon;

    CameraManager cameraManager;

    // ItemData
    ItemDataManager itemDataManager;

    /// <summary>
    /// 아이템 데이터 클래스 접근을 위한 프로퍼티
    /// </summary>
    public ItemDataManager ItemDataManager => itemDataManager;

    public CameraManager Cam
    {
        get
        {
            if (cameraManager == null)
                cameraManager = GetComponent<CameraManager>();
            return cameraManager;
        }
    }

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        weapon = FindAnyObjectByType<Weapon>();
        cameraManager = GetComponent<CameraManager>();
        itemDataManager = GetComponent<ItemDataManager>();
    }

#if UNITY_EDITOR
    public Action onTalkNPC;
    public Action onNextTalk;
    public void StartTalk()
    {
        onTalkNPC?.Invoke();
        Debug.Log("상호작용 키 누름");
    }

    public void NextTalk()
    {
        onNextTalk?.Invoke();
    }

#endif
}
