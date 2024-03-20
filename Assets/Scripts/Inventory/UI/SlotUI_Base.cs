using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// 슬롯 UI 베이스 클래스
/// </summary>
public class SlotUI_Base : MonoBehaviour
{
    /// <summary>
    /// 슬롯 아이콘
    /// </summary>
    Image slotIcon;

    /// <summary>
    /// 아이템 개수
    /// </summary>
    TextMeshProUGUI slotItemCount;

    void Awake()
    {
        Transform child = transform.GetChild(0);
        slotIcon = child.GetComponent<Image>();

        child = transform.GetChild(1);
        slotItemCount = child.GetComponent<TextMeshProUGUI>();
    }
}
