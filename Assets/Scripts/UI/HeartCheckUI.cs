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

    private void Start()
    {
        player = GameManager.Instance.Player;
        // 배열 초기화
        Array.Resize(ref heartImages, (int)(player.MaxHP * 0.01f) + 1);
        Hearts = new GameObject[(int)(player.MaxHP * 0.01f) + 1];

        for(int i = 0; i < transform.childCount; i++)
        {
            Hearts[i] = transform.GetChild(i).gameObject;
        }

        // 하트 이미지 배열 채우기
        for (int i = 0; i < Hearts.Length; i++)
        {
            Transform child = Hearts[i].transform.GetChild(1);
            heartImages[i] = child.GetComponent<Image>();
        }

        player.onDie += DisableUI;
    }

    private void Update()
    {
        player = GameManager.Instance.Player;   
        PrintHearts();
    }

    /// <summary>
    /// UI 비활성화 함수
    /// </summary>
    void DisableUI()
    {
        this.gameObject.SetActive(false);
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
        else // 플레이어 체력이 초기 값보다 높으면
        {
            UpdatePlayerHP();
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
        Instantiate(heartPrefab, transform);    // 프리팹 추가

        Array.Resize(ref Hearts, newSize);      // 오브젝트 배열 확장
        Hearts[newSize - 1] = transform.GetChild(newSize - 1).gameObject; // 배열에 저장

        Array.Resize(ref heartImages, newSize); // 하트 이미지 배열 확장

        // 이미지 배열에 추가
        Transform child = transform.GetChild(newSize - 1);
        child = child.transform.GetChild(1);
        heartImages[newSize - 1] = child.GetComponent<Image>();

        // 플레이어 체력 설정
        player.MaxHP = heartImages.Length * 100.0f; // 플레이어의 최대 체력 증가
        player.HP = player.MaxHP;                   // 함수가 실행될 때, 플레이어의 체력 채우기
    }

    /// <summary>
    /// 플레이어 HP가 UI랑 다르면 업데이트 해주는 함수
    /// </summary>
    void UpdatePlayerHP()
    {
        for (int i = 0; i < player.MaxHP * 0.01f; i++) // 체력 배열 확인
        {
            if (Hearts[i] == null)
            {
                Instantiate(heartPrefab, transform);    // 프리팹 추가
                Hearts[i] = transform.GetChild(i).gameObject;   // 추가한 프리팹 배열에 저장
            }
        }

        for (int i = 0; i < player.MaxHP * 0.01f; i++) // 체력 이미지 배열 확인
        {
            if (heartImages[i] == null)
            {
                Transform child = Hearts[i].transform.GetChild(1);
                heartImages[i] = child.GetComponent<Image>();   // 컴포넌트 추가
            }
        }
    }
}
