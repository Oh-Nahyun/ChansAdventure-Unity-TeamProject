using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TempSlotUI : SlotUI_Base
{
    /// <summary>
    /// 임시 슬롯 UI창이 열렸는지 확인하는 프로퍼티 ( true : 열려있음 , false : 닫혀있음 )
    /// </summary>
    public bool IsOpen => transform.localScale == Vector3.one;
    void Start()
    {
        CloseTempSlot();    
    }

    void Update()
    {
        if(IsOpen)
        {
            transform.position = Input.mousePosition;
        }
    }

    /// <summary>
    /// 임시 슬롯을 여는 함수
    /// </summary>
    public void OpenTempSlot()
    {
        transform.localScale = Vector3.one;
    }

    /// <summary>
    /// 임시 슬롯을 닫는 함수
    /// </summary>
    public void CloseTempSlot()
    {
        transform.localScale = Vector3.zero;
    }
}
