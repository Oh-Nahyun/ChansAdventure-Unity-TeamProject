using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryGoldUI : MonoBehaviour
{
    TextMeshProUGUI goldText;

    /// <summary>
    /// 골드량이 바뀔 때 실행하는 델리게이트
    /// </summary>
    public Action<uint> onGoldChange;

    void Awake()
    {
        Transform child = transform.GetChild(0);
        goldText = child.GetComponent<TextMeshProUGUI>();

        onGoldChange += OnGoldChange;
    }

    /// <summary>
    /// 골드량 출력하는 함수
    /// </summary>
    /// <param name="gold">출력할 골드량</param>
    void OnGoldChange(uint gold)
    {
        goldText.text = $"{gold:D}";
    }
}