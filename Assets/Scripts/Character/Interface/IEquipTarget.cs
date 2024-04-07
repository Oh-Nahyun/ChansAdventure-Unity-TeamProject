using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장비를 장착할 수 있는 캐릭터가 가지는 인터페이스
/// </summary>
public interface IEquipTarget
{
    /// <summary>
    /// 캐릭터가 장착한 장비들을 접근하기 위한 프로퍼티
    /// </summary>
    public InventorySlot[] EquipPart { get; set; }

    /// <summary>
    /// 캐릭터가 장비를 장착할 때 실행되는 함수
    /// </summary>
    public void CharacterEquipItem(GameObject Equipment, EquipPart partNumber, InventorySlot slot);

    /// <summary>
    /// 캐릭터가 장비 장착 해제 할 때 실행되는 함수
    /// </summary>
    public void CharacterUnequipItem(EquipPart partNumber);
}
