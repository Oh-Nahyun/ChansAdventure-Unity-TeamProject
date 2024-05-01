using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_EquipWeapons : TestBase
{
    Transform target;

    private void Start()
    {
        target = transform.GetChild(0);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        ItemDataManager manager = GameManager.Instance.ItemDataManager;
        Factory.Instance.GetItemObject(manager.datas[(uint)ItemCode.Bow], 1, target.position);
        Factory.Instance.GetItemObject(manager.datas[(uint)ItemCode.Sword], 1, target.position);
    }
}
