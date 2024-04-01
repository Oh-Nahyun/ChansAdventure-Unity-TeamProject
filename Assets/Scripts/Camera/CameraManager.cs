using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    PlayerVCam playerVcam;
    public PlayerVCam PlayerCam => playerVcam;
    SkillVCam skillVcam;
    public SkillVCam SkillCam => skillVcam;
    MagnetVCam magnetVcam;
    public MagnetVCam MagnetCam => magnetVcam;

    private void Awake()
    {
        playerVcam = GetComponentInChildren<PlayerVCam>();
        skillVcam = GetComponentInChildren<SkillVCam>();
        magnetVcam = GetComponentInChildren<MagnetVCam>();
    }
}
