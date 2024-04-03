using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 플레이어의 스킬내용만 받는 스크립트
/// </summary>
public class Test_99_PlayerSkills : MonoBehaviour
{
    PlayerinputActions playerInputAction;

    void Awake()
    {
        playerInputAction = new PlayerinputActions();
    }

    void OnEnable()
    {
        //playerInputAction.Skill.OnSkill.performed += onsl
        playerInputAction.Enable();
    }

    void OnDisable()
    {
        playerInputAction.Player.Disable();
    }
}
