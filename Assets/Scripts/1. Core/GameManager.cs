using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player
    {
        get
        {
            if(player == null)
                player = FindAnyObjectByType<Player>();
            return player;
        }
    }

    PlayerVCam playerCam;
    public PlayerVCam PlayerCam
    {
        get
        {
            if (playerCam == null)
            {
                playerCam = GameObject.Find("PlayerVCam").GetComponent<PlayerVCam>();
            }
            return playerCam;
        }
    }

    SkillVCam skillCam;
    public SkillVCam SkillCam
    {
        get
        {
            if(skillCam == null)
            {
                skillCam = GameObject.Find("SkillVCam").GetComponent<SkillVCam>();
            }
            return skillCam;
        }
    }

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
        skillCam = GameObject.Find("SkillVCam").GetComponent<SkillVCam>();
        playerCam = GameObject.Find("PlayerVCam").GetComponent<PlayerVCam>();
    }
}
