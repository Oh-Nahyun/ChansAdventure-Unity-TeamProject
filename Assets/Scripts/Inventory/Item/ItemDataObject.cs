using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 드랍 아이템 오브젝트가 ItemData를 저장하기 위한 클래스
/// </summary>
public class ItemDataObject : RecycleObject
{
    // 활성화 될 때 인벤토리의 데이터 받기
    ItemData Data;          // 아이템 데이터

    uint currentItemCode = 0;  // 아이템 코드

    void Start()
    {
        onDisable += OnItemDisable;
    }

    private void OnItemDisable()
    {
        Debug.Log($"아이템 제거중");                  

        int childObjCount = transform.childCount;           // 자식 오브젝트 개수

        Debug.Log($"{childObjCount}");

        for(int i = 0; i < childObjCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);      // 모든 자식 오브젝트 파괴
        }
    }

    /// <summary>
    /// 드롭 아이템의 아이템 데이터를 설정하는 함수
    /// </summary>
    /// <param name="data">설정할 아이템 데이터</param>
    public void SetData(ItemData data)
    {
        Data = data;
        currentItemCode = (uint)Data.itemCode;
    }

    /// <summary>
    /// 획득한 아이템을 인벤토리에 아이템을 추가하는 함수
    /// </summary>
    /// <param name="ownerInventory">아이템을 넣을 인벤토리</param>
    public void AdditemToInventory(Inventory ownerInventory)
    {
        if(currentItemCode == (uint)ItemCode.Coin)  // 아이템이 코인이면
        {
            ownerInventory.AddGold(Data.price);     // 골드 증가
        }
        else
        {
            ownerInventory.AddSlotItem(currentItemCode);   // 아이템 추가
        }
        gameObject.SetActive(false);            // 아이템 비활성화
    }
}
