using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStamina
{
    /// <summary>
    /// 기력 확인 및 설정용 프로퍼티
    /// </summary>
    float Stamina { get; set; }

    /// <summary>
    /// 최대 기력 확인용 프로퍼티
    /// </summary>
    float MaxStamina { get; }

    /// <summary>
    /// 기력이 변경될 때마다 실행될 델리게이트용 프로퍼티
    /// </summary>
    Action<float> onStaminaChange { get; set; }

    /// <summary>
    /// 기력이 남아있는지 확인하기 위한 프로퍼티
    /// </summary>
    bool IsEnergetic { get; }

    /// <summary>
    /// 기력을 모두 소진했을 경우 처리용 함수
    /// </summary>
    void SpendAllStamina();

    /// <summary>
    /// 기력 소진을 알리기 위한 델리게이트용 프로퍼티
    /// </summary>
    Action onSpendAllStamina { get; set; }
}
