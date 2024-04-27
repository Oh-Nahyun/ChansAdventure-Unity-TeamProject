using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : ObjectPool<ItemDataObject>
{
    /// <summary>
    /// ItemPoolï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½Ï´ï¿½ ï¿½Ô¼ï¿½
    /// </summary>
    /// <param name="slot">ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½</param>
    /// <param name="position">ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½Ä¡</param>
    /// <returns></returns>
    public GameObject GetItemObject(InventorySlot slot, Vector3? position = null)
    {
        GameObject itemObj = slot.SlotItemData.ItemPrefab;                      // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½

        ItemDataObject parentObj = GetObject(position);                         // Ç®ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½
        parentObj.GetComponent<ItemDataObject>().SetData(slot.SlotItemData);    // ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½
        
        Instantiate(itemObj, parentObj.transform);                              // ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½

        return parentObj.gameObject;                                            // Factoryï¿½ï¿½ ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½Æ® ï¿½ï¿½È¯ ( ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ï¿½ ï¿½Ú½ï¿½ 0ï¿½ï¿½Â° )
    }

    public GameObject GetItemDataObject(ItemData data, Vector3? position = null)
    {
        GameObject itemObj = data.ItemPrefab;                                   // ÇÁ¸®ÆÕ ¾ÆÀÌÅÛ »ý¼º

        ItemDataObject parentObj = GetObject(position);                         // Ç®¿¡¼­ ¾ÆÀÌÅÛ ²¨³»±â
        parentObj.GetComponent<ItemDataObject>().SetData(data);                 // ²¨³½ ¾ÆÀÌÅÛÀÇ ¾ÆÀÌÅÛ µ¥ÀÌÅÍ ¼³Á¤

        Instantiate(itemObj, parentObj.transform);                              // ¼³Á¤µÈ ¾ÆÀÌÅÛ ÇÁ¸®ÆÕ »ý¼º

        return parentObj.gameObject;                                            // FactoryÀÇ ¿ÀºêÁ§Æ® ¹ÝÈ¯ ( ÇÁ¸®ÆÕÀº ÀÚ½Ä 0¹øÂ° )
    }
}
