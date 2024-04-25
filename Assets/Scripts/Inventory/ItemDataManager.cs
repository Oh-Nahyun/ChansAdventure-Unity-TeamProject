using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 아이템 등록 및 접근을 위한 클래스 ( UI 포함 )
/// </summary>
public class ItemDataManager : MonoBehaviour
{
    /// <summary>
    /// 아이템 데이터들
    /// </summary>
    public ItemData[] datas;

    /// <summary>
    /// 아이템 데이터 접근을 위한 인덱서
    /// </summary>
    /// <param name="index">datas 인덱스</param>
    /// <returns></returns>
    public ItemData this[int index] => datas[index];

    /// <summary>
    /// 아이템 데이터 코드로 접근 하기 위한 인덱서
    /// </summary>
    /// <param name="code">아이템 코드 값</param>
    /// <returns></returns>
    public ItemData this[ItemCode code] => datas[(int)code];

    /// <summary>
    /// 인벤토리 UI 클래스
    /// </summary>
    InventoryUI inventoryUI;

    /// <summary>
    /// 인벤토리 UI 클래스 접근을 위한 프로퍼티
    /// </summary>
    public InventoryUI InventoryUI => inventoryUI;

    /// <summary>
    /// Inventory RenderTexture Object Point
    /// </summary>
    public GameObject CharaterRenderCameraPoint;

    public void InitializeItemDataUI()
    {
        inventoryUI = FindAnyObjectByType<InventoryUI>(); // find inventoryUI
    }
}
