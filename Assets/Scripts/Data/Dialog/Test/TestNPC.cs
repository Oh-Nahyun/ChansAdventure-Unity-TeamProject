using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestNPC : TestBase
{
    public QuestInfo quest;
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

    }

#endif
}
