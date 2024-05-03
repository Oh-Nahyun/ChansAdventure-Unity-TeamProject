using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartCheckUI : MonoBehaviour
{
    /// <summary>
    /// 플레이어의 하트 개수 (기본 3개)
    /// </summary>
    int numOfHearts = 3;

    /// <summary>
    /// 하트 이미지 배열
    /// </summary>
    public Image[] hearts;

    /// <summary>
    /// 체력이 있을 때의 스프라이트
    /// </summary>
    public Sprite fullHeart;

    /// <summary>
    /// 체력이 없을 때의 스프라이트
    /// </summary>
    public Sprite emptyHeart;

    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    private void Awake()
    {
        player = GameManager.Instance.Player;
    }

    private void Update()
    {
        PrintHearts();
    }

    /// <summary>
    /// 체력에 따른 Heart UI 생성 함수
    /// </summary>
    private void PrintHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
        {

        }
    }
}
