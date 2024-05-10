using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_GetItemToPlayer : TestBase
{
    Player player;
    SphereCollider GetObjectRange;

    Transform target;

    public float range = 5f;

    private void Start()
    {
        // √ ±‚»≠
        GetObjectRange = GetComponent<SphereCollider>();
        GetObjectRange.isTrigger = true;
        GetObjectRange.radius = range;

        target = transform.GetChild(0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Test_EquipCharacter playerObj = other.gameObject.GetComponent<Test_EquipCharacter>();
            Inventory playerInventory = playerObj.Inventory;

            Debug.Log("Player Inventory Accessed");
            GameManager.Instance.ItemDataManager.SellPanelUI.GetTarget(playerInventory);
        }
    }

    protected override void OnTest1(InputAction.CallbackContext context)
    {
        GameManager.Instance.ItemDataManager.SellPanelUI.OpenSellUI();
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        Factory.Instance.GetEnemy(target.transform.position);
    }



    private void OnDrawGizmos()
    {
        Handles.DrawWireDisc(transform.position, Vector3.up, range);
    }
}