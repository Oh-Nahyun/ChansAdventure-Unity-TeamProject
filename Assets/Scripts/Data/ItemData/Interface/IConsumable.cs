using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 소비 가능한 아이템 데이터가 상속 받는 인터페이스
/// </summary>
public interface IConsumable
{
    /// <summary>
    /// 아이템 소비할 때 실행하는 함수
    /// </summary>
    /// <param name="slot">사용할 아이템 인벤토리 슬롯</param>
    public void Consum(GameObject owner, InventorySlot slot);
}
