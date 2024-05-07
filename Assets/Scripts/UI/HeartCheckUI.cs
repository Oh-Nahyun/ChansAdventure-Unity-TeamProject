using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class HeartCheckUI : MonoBehaviour
{
    /// <summary>
    /// 하트가 채워진 갯수
    /// </summary>
    int filledHeart;

    /// <summary>
    /// 플레이어의 하트 총 개수 (기본 3개)
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

    /// <summary>
    /// 기본 하트 프리팹
    /// </summary>
    public GameObject heartPrefab;

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
        numOfHearts = hearts.Length;                                                    // 하트 이미지 배열 크기 = 하트의 총 개수
        filledHeart = Mathf.RoundToInt(player.HP * numOfHearts * (1 / player.MaxHP));   // 하트 중 채워진 하트 개수 = (int)(플레이어 체력 * 총 개수 * (1 / 플레이어 최대 체력))

        // 0 ~ 하트가 채워진 개수까지 : 빨간색 하트 스프라이트 적용
        for (int i = 0; i < filledHeart; i++)
        {
            hearts[i].sprite = fullHeart;
        }

        // 하트가 채워진 개수 다음 ~ 하트의 총 개수까지 : 검은색 하트 스프라이트 적용
        for (int i = filledHeart; i < numOfHearts; i++)
        {
            hearts[i].sprite = emptyHeart;
        }
    }

    /// <summary>
    /// 하트 총 개수를 증가시켜주는 함수
    /// (사당 스크립트에서 델리게이트로 퀘스트 클리어할 때마다 알릴 예정)
    /// (4개를 모아 하트로 변환해줄 때 사용하는 함수)
    /// </summary>
    public void PlusHeart()
    {
        int newSize = hearts.Length + 1;        // 증가된 배열 크기
        Array.Resize(ref hearts, newSize);      // 배열 크기 증가
        Instantiate(heartPrefab, transform);    // 프리팹 추가

        // 이미지 배열에 추가
        Transform child = transform.GetChild(newSize - 1);
        hearts[newSize - 1] = child.GetComponent<Image>();

        // 플레이어 체력 설정
        player.MaxHP = hearts.Length * 100.0f;  // 플레이어의 최대 체력 증가
        player.HP = player.MaxHP;               // 함수가 실행될 때, 플레이어의 체력 채우기
    }
}
