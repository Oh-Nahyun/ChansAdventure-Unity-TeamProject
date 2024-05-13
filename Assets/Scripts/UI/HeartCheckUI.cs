using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HeartCheckUI : MonoBehaviour
{
    /// <summary>
    /// 하트가 채워진 갯수
    /// </summary>
    int filledHeart = 0;

    /// <summary>
    /// 플레이어의 하트 총 개수 (기본 3개)
    /// </summary>
    int numOfHearts = 3;

    /// <summary>
    /// 하트 오브젝트 배열
    /// </summary>
    public GameObject[] Hearts;

    /// <summary>
    /// 하트 이미지 배열
    /// </summary>
    Image[] heartImages;

    /// <summary>
    /// 기본 하트 프리팹
    /// </summary>
    public GameObject heartPrefab;

    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    private void Awake()
    {
        player = GameManager.Instance.Player;
    }

    private void Start()
    {
        // 배열 초기화
        Array.Resize(ref heartImages, Hearts.Length);

        // 하트 이미지 배열 채우기
        for (int i = 0; i < Hearts.Length; i++)
        {
            Transform child = Hearts[i].transform.GetChild(1);
            heartImages[i] = child.GetComponent<Image>();
        }
    }

    private void Update()
    {
        player = GameManager.Instance.Player;   
        PrintHearts();
    }

    /// <summary>
    /// 체력에 따른 Heart UI 생성 함수
    /// </summary>
    private void PrintHearts()
    {
        if (player == null)
        {

            return;
        }

        // 하트 수 초기화
        numOfHearts = heartImages.Length;                                               // 하트 이미지 배열 크기 = 하트의 총 개수
        filledHeart = Mathf.CeilToInt(player.HP * numOfHearts * (1 / player.MaxHP));    // 하트 중 채워진 하트 개수 = (올림 이후 정수화)(플레이어 체력 * 총 개수 * (1 / 플레이어 최대 체력))

        // 하트 UI 설정
        if (filledHeart < 1)
        {
            // 체력이 0인 경우
            for (int i = filledHeart; i < numOfHearts; i++)
            {
                heartImages[i].fillAmount = 0.0f;
            }
        }
        else
        {
            // 0 ~ (채워진 하트 - 1)까지 : 빨간색 하트 스프라이트
            for (int i = 0; i < filledHeart - 1; i++)
            {
                heartImages[i].fillAmount = 1.0f;
            }

            // 채워진 하트 중 가장 마지막 하트
            int finalHealth = (int)player.HP % 100; // 마지막 하트 체력 (100으로 나눈 나머지)
            for (int i = 1; i < 5; i++)
            {
                if (25 * (i - 1) < finalHealth && finalHealth < 25 * i + 1)
                {
                    heartImages[filledHeart - 1].fillAmount = 0.25f * i;
                }
                else if (finalHealth < 1) // 나머지가 0인 경우
                {
                    heartImages[filledHeart - 1].fillAmount = 1.0f;
                }
            }

            // 하트가 채워진 개수 다음 ~ 하트의 총 개수까지 : 검은색 하트 스프라이트
            if (filledHeart < numOfHearts)
            {
                for (int i = filledHeart; i < numOfHearts; i++)
                {
                    heartImages[i].fillAmount = 0.0f;
                }
            }
        }
    }

    /// <summary>
    /// 하트 총 개수를 증가시켜주는 함수
    /// (사당 스크립트에서 델리게이트로 퀘스트 클리어할 때마다 알릴 예정)
    /// (4개를 모아 하트로 변환해줄 때 사용하는 함수)
    /// </summary>
    public void PlusHeart()
    {
        int newSize = heartImages.Length + 1;   // 증가된 배열 크기
        Array.Resize(ref heartImages, newSize); // 배열 크기 증가
        Instantiate(heartPrefab, transform);    // 프리팹 추가

        // 이미지 배열에 추가
        Transform child = transform.GetChild(newSize - 1);
        child = child.transform.GetChild(1);
        heartImages[newSize - 1] = child.GetComponent<Image>();

        // 플레이어 체력 설정
        player.MaxHP = heartImages.Length * 100.0f; // 플레이어의 최대 체력 증가
        player.HP = player.MaxHP;                   // 함수가 실행될 때, 플레이어의 체력 채우기
    }
}
