using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillVCam : Skill_PlayerVCam
{
    public Vector3 skillCameraOffset = new Vector3(-0.8f, 1.2f, 0.0f);
    public float cameraSpeed = 10.0f;


    public virtual void OnSkillCamera()
    {
        // 스킬 사용하기 위한 카메라 움직임
        VCam.Priority = 20;
    }

    public virtual void OffSkillCamera()
    {
        // 원래 카메라로 돌아가기
        VCam.Priority = 1;
    }

}
