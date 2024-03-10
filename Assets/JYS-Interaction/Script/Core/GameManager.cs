using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    TestPlayer player;
    public TestPlayer Player => player;

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<TestPlayer>();
    }


    bool isTalk = false;
    public Action onTalk;
    public void StartTalk()
    {
        if (!isTalk)
        {
            onTalk?.Invoke();
            isTalk = true;
            Debug.Log("대화중");
        }
        else
        {
            onTalk?.Invoke();
            isTalk = false;
            Debug.Log("대화 종료");
        }
    }


}
