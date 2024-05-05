using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 플레이어 데이터 구조체
/// </summary>
[System.Serializable]
public struct PlayerData
{
    public Vector3 position;
    public Vector3 rotation;
    Inventory inventory;
    InventorySlot[] slots;
    public List<InventorySlot> slotLists;

    public PlayerData(Vector3 pos, Vector3 rot, Inventory inven)
    {
        this.position = pos;
        this.rotation = rot;  
        this.inventory = inven;

        // 인벤토리가 NULL이면 임시 슬롯 개수 부여 ( 1개 )
        uint slotSize = this.inventory == null ? 1 : this.inventory.SlotSize;
        this.slots = new InventorySlot[slotSize];

        if(slotSize == 1)
        {
            this.slots[0] = new InventorySlot(0);
            slotLists = new List<InventorySlot>(0);
        }
        else
        {
            slotLists = new List<InventorySlot>((int)this.inventory.SlotSize);
            for (int i = 0; i < slotSize; i++)
            {
                this.slots[i] = inventory[(uint)i];
                slotLists.Add(slots[i]);
            }
            int a = 0;
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

    //public PlayerData[] playerInfos;
    public List<PlayerData> playerInfos;
}
