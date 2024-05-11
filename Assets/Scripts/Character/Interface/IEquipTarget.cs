using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� ������ �� �ִ� ĳ���Ͱ� ������ �������̽�
/// </summary>
public interface IEquipTarget
{
    /// <summary>
    /// 아이템 장착부위 프로퍼티
    /// </summary>
    public InventorySlot[] EquipPart { get; set; }

    /// <summary>
    /// 장비를 장착할 때 호출하는 함수
    /// </summary>
    public void CharacterEquipItem(GameObject Equipment, EquipPart partNumber, InventorySlot slot);

    /// <summary>
    /// 장비를 장착해제 할 때 호출하는 함수
    /// </summary>
    public void CharacterUnequipItem(EquipPart partNumber);
}
