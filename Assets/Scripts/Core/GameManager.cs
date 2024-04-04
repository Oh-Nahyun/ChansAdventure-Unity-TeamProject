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
            Debug.Log("상호작용 키 누름");
        }
        else
        {
            onTalkObj?.Invoke();
            Debug.Log("오브젝트와 대화");
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
