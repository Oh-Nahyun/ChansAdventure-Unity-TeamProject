using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 체력 회복 아이템 데이터
/// </summary>
[CreateAssetMenu(fileName = "ItemData-Healing-HP", menuName = "ScriptableObjects/ItemData-Healing-HP", order = 2)]
public class ItemData_Healing_HP : ItemData, IConsumable
{
    /// <summary>
    /// 회복할 체력의 량 ( 틱 회복 x)
    /// </summary>
    public float healing_Hp;

    /// <summary>
    /// 회복하는데 걸리는 시간
    /// </summary>
    public float duration;

    /// <summary>
    /// 해당 아이템이 소비될 대 실행되는 함수
    /// </summary>
    /// <param name="owner">아이템 사용하는 오브젝트</param>
    /// <param name="slot">사용되는 아이템 슬롯</param>
    public void Consum(GameObject owner, InventorySlot slot)
    {
        IHealth health = owner.GetComponent<IHealth>();
       
        if(health != null)
        {
            // 아이템 제거
            slot.DiscardItem(1);    // 아이템 1개 감소
            // IHealth의 체력 회복 
            health.HealthRegenerate(healing_Hp, duration);
        }
        else
        {
            Debug.Log($"[{owner.name}] 오브젝트에는 IHealth 인터페이스가 존재하지 않습니다.");
        }
    }    
}
