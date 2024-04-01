using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill_CameraManager : MonoBehaviour
{
    Skill_PlayerVCam playerVcam;
    public Skill_PlayerVCam PlayerCam => playerVcam;
    SkillVCam skillVcam;
    public SkillVCam SkillCam => skillVcam;
    MagnetVCam magnetVcam;
    public MagnetVCam MagnetCam => magnetVcam;

    private void Awake()
    {
        playerVcam = GetComponentInChildren<Skill_PlayerVCam>();
        skillVcam = GetComponentInChildren<SkillVCam>();
        magnetVcam = GetComponentInChildren<MagnetVCam>();
    }
}
