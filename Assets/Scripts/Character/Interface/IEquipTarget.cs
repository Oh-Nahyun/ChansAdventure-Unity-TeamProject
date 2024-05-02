using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ��� ������ �� �ִ� ĳ���Ͱ� ������ �������̽�
/// </summary>
public interface IEquipTarget
{
    /// <summary>
    /// ĳ���Ͱ� ������ ������ �����ϱ� ���� ������Ƽ
    /// </summary>
    public InventorySlot[] EquipPart { get; set; }

    /// <summary>
    /// ĳ���Ͱ� ��� ������ �� ����Ǵ� �Լ�
    /// </summary>
    public void CharacterEquipItem(GameObject Equipment, EquipPart partNumber, InventorySlot slot);

    /// <summary>
    /// ĳ���Ͱ� ��� ���� ���� �� �� ����Ǵ� �Լ�
    /// </summary>
    public void CharacterUnequipItem(EquipPart partNumber);
}
