using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataObject : RecycleObject
{
    // 활성화 될 때 인벤토리의 데이터 받기
    public ItemData Data;

    void Start()
    {
        onDisable += OnItemDisable;
    }

    private void OnItemDisable()
    {
        Debug.Log($"아이템 제거중");

        int childObjCount = transform.childCount;

        Debug.Log($"{childObjCount}");

        for(int i = 0; i < childObjCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
