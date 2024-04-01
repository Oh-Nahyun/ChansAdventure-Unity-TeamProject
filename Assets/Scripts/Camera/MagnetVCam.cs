using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetVCam : SkillVCam
{
    public override void OffSkillCamera()
    {
        base.OffSkillCamera();
        //VCam.LookAt = null;
    }

    public void SetLookAtTransform(Transform lookAt)
    {
        VCam.LookAt = lookAt;
    }
}
