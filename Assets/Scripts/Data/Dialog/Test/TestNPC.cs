using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestNPC : TestBase
{
    public QuestInfo quest;
    public Action<int>[] EnemyQuestData;
    public int[] enemyQuestData = new int[2];

    int kill;
    int kill2;
#if UNITY_EDITOR
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        GameManager.Instance.StartTalk();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        GameManager.Instance.NextTalk();
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        quest.OnQuestInfo();
    }

    protected override void OnTest4(InputAction.CallbackContext context)
    {
        kill++;
        EnemyQuestData[0]?.Invoke(1);
        EnemyQuestData[1]?.Invoke(kill);
        Debug.Log(kill);
    }

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        kill2++;
        EnemyQuestData[0]?.Invoke(2);
        EnemyQuestData[1]?.Invoke(kill2);
        Debug.Log(kill2);
    }


#endif
}
