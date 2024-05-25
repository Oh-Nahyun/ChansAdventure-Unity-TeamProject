using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonStage2_RoomTrigger : MonoBehaviour
{
    /// <summary>
    /// 트리거에 들어갔을 때 실행되는 델리게이트
    /// </summary>
    public Action onTriggerEnter;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            onTriggerEnter?.Invoke();
        }
    }
}
