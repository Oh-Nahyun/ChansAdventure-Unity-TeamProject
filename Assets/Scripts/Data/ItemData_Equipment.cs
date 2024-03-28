using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData_Equipment : ItemData, IEquipable
{
    /// <summary>
    /// 장착할 아이템 프리팹
    /// </summary>
    public GameObject EqiupPrefab;

    /// <summary>
    /// 아이템 착용할 때 실행하는 함수
    /// </summary>
    public void EquipItem()
    {
        throw new System.NotImplementedException();
    }

    /// <summary>
    /// 아이템 착용 해제할 때 실행하는 함수
    /// </summary>
    public void UnEquipItem()
    {
        throw new System.NotImplementedException();
    }
}
