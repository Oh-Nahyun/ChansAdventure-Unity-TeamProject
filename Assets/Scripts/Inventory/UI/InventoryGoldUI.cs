using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryGoldUI : MonoBehaviour
{
    TextMeshProUGUI goldText;

    public Action<int> onGoldChange;

    void Awake()
    {
        Transform child = transform.GetChild(0);
        goldText = child.GetComponent<TextMeshProUGUI>();

        onGoldChange += OnGoldChange;
    }

    void OnGoldChange(int gold)
    {
        goldText.text = $"{gold:D}";
    }
}