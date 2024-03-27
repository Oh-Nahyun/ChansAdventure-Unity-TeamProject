using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 장착 가능한 아이템이 상속받는 인터페이스
/// </summary>
public interface IEquipable
{
    /// <summary>
    /// 아이템 장착하는 함수
    /// </summary>
    void EquipItem() { }

    /// <summary>
    /// 아이템 장착해제 하는 함수
    /// </summary>
    void UnEquipItem() { }
}
