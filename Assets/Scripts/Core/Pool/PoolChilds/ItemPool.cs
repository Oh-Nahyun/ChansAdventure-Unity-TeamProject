using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPool : ObjectPool<ItemDataObject>
{
    /// <summary>
    /// ItemPool���� �������� �����ϴ� �Լ�
    /// </summary>
    /// <param name="slot">������ �������� ����</param>
    /// <param name="position">������ ��ġ</param>
    /// <returns></returns>
    public GameObject GetItemObject(InventorySlot slot, Vector3? position = null)
    {
        GameObject itemObj = slot.SlotItemData.ItemPrefab;                      // ������ ������ ����

        ItemDataObject parentObj = GetObject(position);                         // Ǯ���� ������ ������
        parentObj.GetComponent<ItemDataObject>().SetData(slot.SlotItemData);    // ���� �������� ������ ������ ����
        
        Instantiate(itemObj, parentObj.transform);                              // ������ ������ ������ ����

        return parentObj.gameObject;                                            // Factory�� ������Ʈ ��ȯ ( �������� �ڽ� 0��° )
    }

    // 임시로 함수 추가(나중에 뺴야딤) -------------------------------------------------------------------
    public GameObject GetItemObject(ItemData data, Vector3? position = null)
    {
        GameObject itemObj = data.ItemPrefab;                      // ������ ������ ����

        ItemDataObject parentObj = GetObject(position);                         // Ǯ���� ������ ������
        parentObj.GetComponent<ItemDataObject>().SetData(data);    // ���� �������� ������ ������ ����

        Instantiate(itemObj, parentObj.transform);                              // ������ ������ ������ ����

        return parentObj.gameObject;                                            // Factory�� ������Ʈ ��ȯ ( �������� �ڽ� 0��° )
    }
    // ------------------------------------------------------------------------------------------
}
