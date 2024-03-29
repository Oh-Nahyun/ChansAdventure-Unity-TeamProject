using System;
using System.Collections;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    Character player;
    public Character Player => player;

    Weapon weapon;
    public Weapon Weapon => weapon;

    protected override void OnInitialize()
    {
        player = FindAnyObjectByType<Character>();
        weapon = FindAnyObjectByType<Weapon>();
    }
}
