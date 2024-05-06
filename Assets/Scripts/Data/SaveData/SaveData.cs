using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 아이템코드와 개수를 저장하는 클래스
/// </summary>
[System.Serializable]
public class ItemDataClass
{
    /// <summary>
    /// 저장된 아이템 코드
    /// </summary>
    public int itemCode;

    /// <summary>
    /// 저장된 아이템 개수
    /// </summary>
    public int count;
}

/// <summary>
/// 플레이어 데이터 구조체
/// </summary>
[System.Serializable]
public struct PlayerData
{
    /// <summary>
    /// 플레이어 위치값
    /// </summary>
    public Vector3 position;

    /// <summary>
    /// 플레이어 회전값
    /// </summary>
    public Vector3 rotation;

    /// <summary>
    /// 플레이어 인벤토리 - 저장할려는 플레이어 인벤토리 접근용
    /// </summary>
    Inventory inventory;

    /// <summary>
    /// 플레이어 인벤토리 슬롯
    /// </summary>
    InventorySlot[] slots;

    public ItemDataClass[] itemDataClass;

    /// <summary>
    /// 세이브 데이터 칸 수
    /// </summary>
    const int saveCount = 5;

    public PlayerData(Vector3 pos, Vector3 rot, Inventory inven)
    {
        this.position = pos;
        this.rotation = rot;  
        this.inventory = inven;

        // 인벤토리가 NULL이면 임시 슬롯 개수 부여 ( 1개 )
        uint slotSize = this.inventory == null ? 1 : this.inventory.SlotSize;
        this.slots = new InventorySlot[slotSize];                       // 슬롯 초기화
        this.itemDataClass = new ItemDataClass[slotSize];

        if(slotSize == 1) // 인벤토리가 NULL이면
        {
            this.slots[0] = new InventorySlot(0);
            this.itemDataClass[0] = new ItemDataClass();
        }
        else // 인벤토리가 NULL이 아니면
        {
            // 인벤토리 슬롯 데이터 초기화
            for (int i = 0; i < slotSize; i++)
            {
                this.slots[i] = inventory[(uint)i]; // 슬롯 데이터 추가
            }

            // 아이템 데이터 초기화
            for(int i = 0; i < saveCount; i++)
            {
                if (slots[i].SlotItemData == null)
                {
                    continue;
                }
                else
                {
                    this.itemDataClass[i] = new ItemDataClass();
                    itemDataClass[i].itemCode = (int)slots[i].SlotItemData.itemCode;
                    itemDataClass[i].count = slots[i].CurrentItemCount;
                }
            }
        }
    }
}

/// <summary>
/// json 파일 저장용 클래스 ( Scene번호, 플레이어 위치, 플레이어 인벤토리)
/// </summary>
[Serializable]
public class SaveData
{
    /// <summary>
    /// 저장된 씬 번호
    /// </summary>
    public int[] SceneNumber;

    public PlayerData[] playerInfos;
}