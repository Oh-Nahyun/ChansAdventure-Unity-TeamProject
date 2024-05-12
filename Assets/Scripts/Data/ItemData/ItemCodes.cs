using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 아이템 코드
/// </summary>
public enum ItemCode
{
    portion = 0,
    apple,
    cheese,
    Hammer,
    Sword,
    HP_portion,
    HP_portion_Tick,
    Coin,
    Bow,
    Arrow
}

/// <summary>
/// 정렬 방식
/// </summary>
public enum SortMode
{
    Name = 0,
    Price,
    Count
}

/// <summary>
/// IEquipTarget이 가지는 장비 파츠 부위
/// </summary>
public enum EquipPart
{
    Hand_R,
    Hand_L
}

/// <summary>
/// 무기 아이템의 아이템 타입 enum
/// </summary>
public enum WeaponType
{
    Melee = 0,
    Range
}