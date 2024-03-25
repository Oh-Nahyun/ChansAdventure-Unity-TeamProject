using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SkillVCam : PlayerVCam
{
    public Vector3 skillCameraOffset = new Vector3(-0.8f, 1.2f, 0.0f);
    public float cameraSpeed = 10.0f;

    protected override void Start()
    {
        base.Start();
        if (player != null)
        {
            /*player.SkillController.startSkill += StartSkillCamera;
            player.SkillController.usingSkill += UsingSkillCamera;
            player.SkillController.cancelSkill += EndSkillCamera;*/
        }
        else
        {
            Debug.LogWarning("Player가 없습니다.");
        }
    }

    protected virtual void StartSkillCamera()
    {
        // 스킬 사용하기 위한 카메라 움직임
        vCam.Priority = 20;
    }

    protected virtual void UsingSkillCamera()
    {
        // 스킬 카메라 전용
        vCam.Priority = 1;
    }

    protected virtual void EndSkillCamera()
    {
        // 원래 카메라로 돌아가기
        vCam.Priority = 1;
    }

#if UNITY_EDITOR
    public void TestSkillCamera()
    {
        StartSkillCamera();
    }

    public void TestOriginCamera()
    {
        UsingSkillCamera();
    }
#endif
}
