using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : ObjectPool<ItemDataObject>
{
    /// <summary>
    /// ItemPool에서 아이템을 가져오는 함수
    /// </summary>
    /// <param name="itemData">생성할 아이템 데이터</param>
    /// <param name="position">아이템 위치</param>
    /// <returns></returns>
    public GameObject GetItemObject(ItemData itemData, Vector3? position = null)
    {
        GameObject itemObj = itemData.ItemPrefab;                               // 프리팹 아이템 생성
        ItemDataObject parentObj = GetObject(position);                         // 풀에서 아이템 꺼내기

        parentObj.GetComponent<ItemDataObject>().SetData(itemData);             // 꺼낸 아이템의 아이템 데이터 설정        
        Instantiate(itemObj, parentObj.transform);                              // 설정된 아이템 프리팹 생성

        return parentObj.gameObject;                                            // Factory의 오브젝트 반환 ( 프리팹은 자식 0번째 )
    }
}