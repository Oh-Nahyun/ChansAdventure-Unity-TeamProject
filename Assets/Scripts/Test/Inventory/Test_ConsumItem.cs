using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Test_ConsumItem : TestBase
{
    public GameObject owner;
    Test_EquipCharacter character;
    Inventory inven;
    public float hpValue = 1f;

    void Start()
    {
        character = owner.GetComponent<Test_EquipCharacter>();
        inven = new Inventory(owner);

        inven.AddSlotItem(5, 3);
        inven.AddSlotItem(1, 3);
        inven.AddSlotItem(3, 1);
        inven.AddSlotItem(4, 1);
        inven.AddSlotItem(6, 3);

        GameManager.Instance.ItemDataManager.InventoryUI.InitializeInventoryUI(inven);
    }
    protected override void OnTest1(InputAction.CallbackContext context)
    {
        character.HP -= hpValue;
    }

    protected override void OnTest2(InputAction.CallbackContext context)
    {
        character.HealthRegenerate(2f, 1.5f);
    }
}
