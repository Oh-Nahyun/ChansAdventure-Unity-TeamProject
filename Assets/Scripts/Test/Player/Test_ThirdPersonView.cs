using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ThirdPersonView : TestBase
{
#if UNITY_EDITOR
    Transform spawnTarget;

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        spawnTarget = transform.GetChild(0);
        ItemDataManager dataManager = GameManager.Instance.ItemDataManager;
        Factory.Instance.GetItemObject(dataManager[ItemCode.Sword], spawnTarget.position);
        Factory.Instance.GetItemObject(dataManager[ItemCode.Bow], spawnTarget.position);
        Factory.Instance.GetItemObjects(dataManager[ItemCode.Arrow], 10, spawnTarget.position, true);
        Factory.Instance.GetItemObjects(dataManager[ItemCode.HP_portion_Tick], 2, spawnTarget.position);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        GameManager.Instance.Player.Stamina = 0.0f;
    }
#endif
}
