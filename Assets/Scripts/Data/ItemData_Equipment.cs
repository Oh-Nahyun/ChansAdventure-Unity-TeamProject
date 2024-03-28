using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_Equipment : ItemData, IEquipable
{
    /// <summary>
    /// 장착할 아이템 프리팹
    /// </summary>
    public GameObject EqiupPrefab;

    /// <summary>
    /// 아이템 착용할 때 실행하는 함수
    /// </summary>
    public void EquipItem(GameObject owner, InventorySlot slot)
    {
        IEquipTarget equipTarget = owner.GetComponent<IEquipTarget>();

        if(equipTarget != null)
        {            
            equipTarget.CharacterEquipItem(EqiupPrefab);
        }
    }

    /// <summary>
    /// 아이템 착용 해제할 때 실행하는 함수
    /// </summary>
    public void UnEquipItem(InventorySlot slot)
    {
        throw new System.NotImplementedException();
    }
}
