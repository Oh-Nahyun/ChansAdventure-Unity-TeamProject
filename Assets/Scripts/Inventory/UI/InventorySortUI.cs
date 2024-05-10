using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventorySortUI : MonoBehaviour
{
    TMP_Dropdown dropDown;
    Button checkBtn;
    //Button AsceningBtn;

    uint sortValue = 0;
    bool isAcending = false;

    /// <summary>
    /// 아이템 정렬을 할 때 실행되는 델리게이트
    /// </summary>
    public Action<uint, bool> onSortItem;

    void Awake()
    {
        Transform child = transform.GetChild(0);
        dropDown = child.GetComponent<TMP_Dropdown>();

        dropDown.onValueChanged.AddListener((int value) =>
        {   // dropDown에서 정렬할 기준 선택
            sortValue = (uint)value; 
        });

        child = transform.GetChild(1);
        checkBtn = child.GetComponent<Button>();
        checkBtn.onClick.AddListener(() =>
        {
            onSortItem?.Invoke(sortValue, isAcending);
        });
    }
}
