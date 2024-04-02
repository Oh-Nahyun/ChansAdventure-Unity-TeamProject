using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager_JYS : Singleton_JYS<GameManager_JYS>
{
    /*
    TestPlayer player;
    public TestPlayer Player => player;

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<TestPlayer>();
    }
    */

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

}
