using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetVCam : SkillVCam
{
    protected override void StartSkillCamera()
    { 
    }

    protected override void UsingSkillCamera()
    {
        VCam.Priority = 20;
    }
}
