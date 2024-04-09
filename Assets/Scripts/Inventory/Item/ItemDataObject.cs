using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataObject : RecycleObject
{
    // 활성화 될 때 인벤토리의 데이터 받기
    public ItemData Data;

    GameObject itemObj;

    protected override void OnEnable()
    {
        base.OnEnable();

        if (Data != null)
        {            
            itemObj = Data.ItemPrefab;
            Instantiate(itemObj, this.transform);        
        }
    }
}
