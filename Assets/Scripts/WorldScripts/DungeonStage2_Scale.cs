using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonStage2_Scale : MonoBehaviour
{
    /// <summary>
    /// 트리거가 활성화 됐을 때 실행하는 델리게이트
    /// </summary>
    public Action onTriggerEnter;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Weighable>() != null)
        {
            onTriggerEnter?.Invoke();
        }
    }
}