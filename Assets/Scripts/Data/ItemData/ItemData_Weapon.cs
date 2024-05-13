using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 무기 아이템 데이터
/// </summary>
[CreateAssetMenu(fileName = "ItemData-Weapon", menuName = "ScriptableObjects/ItemData-Weapon", order = 1)]
public class ItemData_Weapon : ItemData_Equipment
{
    [Header("무기 정보")]
    public float Damage;

    public WeaponType WeaponType;
}