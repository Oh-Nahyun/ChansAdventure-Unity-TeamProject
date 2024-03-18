using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Character player;
    public Character Player => player;

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Character>();
    }
}
