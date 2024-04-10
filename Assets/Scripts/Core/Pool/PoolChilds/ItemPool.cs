using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : ObjectPool<ItemDataObject>
{
    /// <summary>
    /// ItemPool에서 아이템을 생성하는 함수
    /// </summary>
    /// <param name="slot">생성할 아이템의 슬롯</param>
    /// <param name="position">생성할 위치</param>
    /// <returns></returns>
    public GameObject GetItemObject(InventorySlot slot, Vector3? position = null)
    {
        GameObject itemObj = slot.SlotItemData.ItemPrefab;

        ItemDataObject parentObj = GetObject(position);
        parentObj.GetComponent<ItemDataObject>().Data = slot.SlotItemData;
        
        Instantiate(itemObj, parentObj.transform);

        return parentObj.gameObject;
    }
}
