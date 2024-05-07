using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;

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

    /// <summary>
    /// 슬롯 내용
    /// </summary>
    TextMeshProUGUI saveDesc;

    /// <summary>
    /// 슬롯을 초기화 하는 함수
    /// </summary>
    public void InitializeComponent()
    {
        handler = GetComponentInParent<SaveHandler>();
        Transform child = transform.GetChild(1);
        saveName = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(2);
        saveDesc = child.GetComponent<TextMeshProUGUI>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PointerEventData.InputButton buttonValue = eventData.button;
        
        if(buttonValue == PointerEventData.InputButton.Left) // 왼쪽 클릭하면 세이브
        {
            Debug.Log($"{saveIndex}번에 저장됨");
            handler.onClickSaveSlot?.Invoke(saveIndex);
        }
        
        if(buttonValue == PointerEventData.InputButton.Right)
        {
            Debug.Log($"{saveIndex}번 로드함");
            handler.onClickLoadSlot?.Invoke(saveIndex);
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
    public void CheckSave(bool isEmtpy, int sceneNumber)
    {
        if(isEmtpy)
        {
            saveName.text = $"SaveData {saveIndex} ";
            saveDesc.text = $"Empty";
        }
        else
        {
            saveName.text = $"SaveData {saveIndex}";
            saveDesc.text = $"{SceneManager.GetSceneByBuildIndex(sceneNumber).name}";
        }
    }
}
