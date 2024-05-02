using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData-Healing-HP-Tick", menuName = "ScriptableObjects/ItemData-Healing-HP-Tick", order = 3)]
public class ItemData_Healing_Hp_Tick : ItemData, IConsumable
{
    /// <summary>
    /// 틱당 회복할 량
    /// </summary>
    public float tickRegen;

    /// <summary>
    /// 틱 인터벌
    /// </summary>
    public float inverval;

    /// <summary>
    /// 회복 틱 개수
    /// </summary>
    public uint tickCount;

    /// <summary>
    /// 해당 아이템이 소비될 대 실행되는 함수
    /// </summary>
    /// <param name="owner">아이템 사용하는 오브젝트</param>
    /// <param name="slot">사용되는 아이템 슬롯</param>
    public void Consum(GameObject owner, InventorySlot slot)
    {
        IHealth health = owner.GetComponent<IHealth>();

        if (health != null)
        {
            // 아이템 제거
            slot.DiscardItem(1);    // 아이템 1개 감소
            // IHealth의 체력 회복 
            health.HealthRegenerateByTick(tickRegen, inverval, tickCount);
        }
        else
        {
            Debug.Log($"[{owner.name}] 오브젝트에는 IHealth 인터페이스가 존재하지 않습니다.");
        }
    }
}
