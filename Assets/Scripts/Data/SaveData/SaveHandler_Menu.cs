using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveHandler_Menu : SaveHandler_Base
{
    protected override void Start()
    {
        base.Start();

        onClickSaveSlot = null;
    }

    protected override void LoadPlayerData(int loadIndex)
    {
        base.LoadPlayerData(loadIndex);
        GameManager.Instance.CurrnetGameState = GameState.Started;
    }
}
