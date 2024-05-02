using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_Equipment : ItemData, IEquipable
{
    /// <summary>
    /// 장착할 아이템 프리팹
    /// </summary>
    public GameObject EqiupPrefab;

    public EquipPart equipPart;

    /// <summary>
    /// 아이템 장착할 때 실행되는 함수
    /// </summary>
    /// <param name="owner">인벤토리를 가진 오브젝트 </param>
    /// <param name="slot">장착할 슬롯 인덱스</param>
    public void EquipItem(GameObject owner, InventorySlot slot)
    {
        IEquipTarget equipTarget = owner.GetComponent<IEquipTarget>();

        if(equipTarget != null)
        {
            equipTarget.CharacterEquipItem(EqiupPrefab, equipPart, slot);
        }
    }

    /// <summary>
    /// 아이템 착용 해제할 때 실행하는 함수
    /// </summary>
    public void UnEquipItem(GameObject owner, InventorySlot slot)
    {
        IEquipTarget equipTarget = owner.GetComponent<IEquipTarget>();

        if (equipTarget != null)
        {
            equipTarget.EquipPart[(int)equipPart] = null;
            equipTarget.CharacterUnequipItem(equipPart);
        }
    }
}
