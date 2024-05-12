using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 스킬 순서: RemoteBomb, RemoteBomb_Cube, MagnetCatch, IceMaker, TimeLock
public class SkillManager : MonoBehaviour
{
    public GameObject[] skills;

    public Skill this[SkillName skillName] => skills[(int)skillName].GetComponent<Skill>();

    PlayerSkills playerSkill;
    public PlayerSkills PlayerSkill => playerSkill;

    public int SkillCount => Enum.GetValues(typeof(SkillName)).Length;

    public void Initialize()
    {
        playerSkill = FindAnyObjectByType<PlayerSkills>();
    }
}
