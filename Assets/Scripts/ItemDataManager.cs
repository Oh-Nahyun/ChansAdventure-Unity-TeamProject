using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDataManager : MonoBehaviour
{
    /// <summary>
    /// ItemDataManager Singleton
    /// </summary>
    public static ItemDataManager itemDataManager;

    public ItemData[] datas;
    public ItemData this[int index] => datas[index];
    public ItemData this[ItemCode code] => datas[(int)code];

    void Awake()
    {
        itemDataManager = this;
    }
}
