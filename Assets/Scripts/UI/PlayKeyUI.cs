using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayKeyUI : MonoBehaviour
{
    Player player;

    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
    }

    private void LateUpdate()
    {
        if(player.IsAnyUIPanelOpened)
        {
            canvasGroup.alpha = 0;
        }
        else
        {
            canvasGroup.alpha = 1;
        }
    }
}