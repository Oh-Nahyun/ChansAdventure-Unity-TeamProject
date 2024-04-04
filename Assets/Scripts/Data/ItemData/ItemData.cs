using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Item Data class
/// </summary>

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemData", order = 0)]
public class ItemData : ScriptableObject
{
    /// <summary>
    /// 해당 아이템 코드
    /// </summary>
    public ItemCode itemCode;

    /// <summary>
    /// 아이템 이름
    /// </summary>
    public string itemName;

    /// <summary>
    /// 아이템 아이콘
    /// </summary>
    public Sprite itemIcon;

    /// <summary>
    /// 아이템 설명
    /// </summary>
    public string desc;

    /// <summary>
    /// 아이템 가격
    /// </summary>
    public uint price;

    /// <summary>
    /// 아이템 최대 소지량
    /// </summary>
    public uint maxCount;
}
