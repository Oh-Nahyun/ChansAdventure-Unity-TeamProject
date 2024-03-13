using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Player player;
    public Player Player => player;

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Player>();
    }
}
