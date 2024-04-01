using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Skill_Player player;
    public Skill_Player Player
    {
        get
        {
            if(player == null)
                player = FindAnyObjectByType<Skill_Player>();
            return player;
        }
    }

    Skill_CameraManager cameraManager;
    public Skill_CameraManager Cam
    {
        get
        {
            if(cameraManager == null)
                cameraManager = GetComponent<Skill_CameraManager>();
            return cameraManager;
        }
    }

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Skill_Player>();
        cameraManager = GetComponent<Skill_CameraManager>();
    }
}
