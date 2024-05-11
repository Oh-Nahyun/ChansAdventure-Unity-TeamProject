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
    /// 占쏙옙占쏙옙占쏙옙 占쏙옙占쏙옙占쏙옙 클占쏙옙占쏙옙 占쏙옙占쏙옙占쏙옙 占쏙옙占쏙옙 占쏙옙占쏙옙占쏙옙티
    /// </summary>
    public ItemDataManager ItemDataManager => itemDataManager;

    MapManager mapManager;

    /// <summary>
    /// mapManager 접근을 위한 프로퍼티
    /// </summary>
    public MapManager MapManager => mapManager;

    public CameraManager Cam
    {
        get
        {
            if (cameraManager == null)
                cameraManager = GetComponent<CameraManager>();
            return cameraManager;
        }
    }

    SkillManager skillManager;
    public SkillManager Skill => skillManager;

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        weapon = FindAnyObjectByType<Weapon>();
        cameraManager = GetComponent<CameraManager>();
        itemDataManager = GetComponent<ItemDataManager>();
        mapManager = GetComponent<MapManager>();
        skillManager = GetComponent<SkillManager>();
        skillManager.Initialize();
    }

#if UNITY_EDITOR
    public bool isNPC = false;
    public Action onTalkNPC;
    public Action onTalkObj;
    public void StartTalk()
    {
        //onTalk?.Invoke();

        if (!isNPC)
        {
            onTalkNPC?.Invoke();
            Debug.Log("占쏙옙호占쌜울옙 키 占쏙옙占쏙옙");
        }
        else
        {
            onTalkObj?.Invoke();
            Debug.Log("占쏙옙占쏙옙占쏙옙트占쏙옙 占쏙옙화");
        }
    }

    public Action onNextTalk;
    public void NextTalk()
    {
        onNextTalk?.Invoke();
    }

    public void IsNPCObj()
    {
        isNPC = !isNPC;
    }

    public Action openChase;
    public void OpenChest()
    {
        openChase?.Invoke();
    }
#endif
}
