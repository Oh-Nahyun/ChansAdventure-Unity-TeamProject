using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteBombCube : RemoteBomb
{
    protected override void Awake()
    {
        base.Awake();
        skillName = SkillName.RemoteBomb_Cube;
    }
}
