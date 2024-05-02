using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestNPC : TestBase
{
    public QuestInfo quest;
    public Action<int> EnemyId;
    public Action<int> KillEnemy;
    int kill;
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

    protected override void OnTest5(InputAction.CallbackContext context)
    {
        kill++;
        EnemyId?.Invoke(1);
        KillEnemy?.Invoke(kill);
    }

#endif
}
