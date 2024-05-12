using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_01_EnemySpawn : TestBase
{
    Transform spawnTarget;
    public int waypointIndex = 1;

    private void Start()
    {
        spawnTarget = transform.GetChild(0);
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        Factory.Instance.GetEnemy(waypointIndex, spawnTarget.position);
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Factory.Instance.GetNightmareDragonEnemy(waypointIndex, spawnTarget.position);
    }

    protected override void OnTest3(InputAction.CallbackContext context)
    {
        ItemDataManager dataManager = GameManager.Instance.ItemDataManager;
        Factory.Instance.GetItemObject(dataManager[ItemCode.Sword], spawnTarget.position);
        Factory.Instance.GetItemObject(dataManager[ItemCode.Bow], spawnTarget.position);
        Factory.Instance.GetItemObjects(dataManager[ItemCode.Arrow], 10, spawnTarget.position, true);
        Factory.Instance.GetItemObjects(dataManager[ItemCode.HP_portion_Tick], 2, spawnTarget.position);
    }
}