using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class SaveDataSlot : MonoBehaviour, IPointerClickHandler
{
    SaveHandler handler;

    /// <summary>
    /// 슬롯 인덱스
    /// </summary>
    int saveIndex;

    /// <summary>
    /// 슬롯 이름 텍스트
    /// </summary>
    TextMeshProUGUI saveName;

    void Awake()
    {
        handler = GetComponentInParent<SaveHandler>();
        saveName = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PointerEventData.InputButton buttonValue = eventData.button;
        
        if(buttonValue == PointerEventData.InputButton.Left) // 왼쪽 클릭하면 세이브
        {
            handler.onClickSaveSlot?.Invoke(handler.saveIndex);
        }
        
        if(buttonValue == PointerEventData.InputButton.Right)
        {
            handler.onClickLoadSlot?.Invoke(handler.saveIndex);
        }
    }

    /// <summary>
    /// 세이브 슬롯 초기화 함수
    /// </summary>
    /// <param name="index">슬롯 인덱스</param>
    public void SlotInitialize(int index)
    {
        saveIndex = index;
        saveName.text = $"SaveData {saveIndex}";
    }

    /// <summary>
    /// 세이브 데이터가 존재하는지 확인하는 함수
    /// </summary>
    /// <param name="isEmtpy">비어있으면 true 아니면 false</param>
    public void CheckSave(bool isEmtpy)
    {
        if(isEmtpy)
        {
            saveName.text = $"SaveData {saveIndex} ";
        }
        else
        {
            saveName.text = $"SaveData {saveIndex} VVVV";
        }
    }
}
