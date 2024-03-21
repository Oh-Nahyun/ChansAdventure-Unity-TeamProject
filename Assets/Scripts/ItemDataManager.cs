using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{

    public ItemData[] datas;
    public ItemData this[int index] => datas[index];
    public ItemData this[ItemCode code] => datas[(int)code];

    #region GameManager
    /// <summary>
    /// ItemDataManager Singleton
    /// </summary>
    public static ItemDataManager Instance;

    public InventoryUI inventoryUI;
    public InventoryUI InventoryUI => inventoryUI;

    #endregion

    void Awake()
    {
        Instance = this;

        inventoryUI = FindAnyObjectByType<InventoryUI>();
    }
}
