using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaCheckUI : MonoBehaviour
{
    /// <summary>
    /// outside 활성화에 따른 bool 변수
    /// </summary>
    bool IsActive = false;

    /// <summary>
    /// 스태미나 중심 이미지
    /// </summary>
    Image inside;

    /// <summary>
    /// 스태미나 바깥 이미지
    /// </summary>
    Image outside;

    /// <summary>
    /// 스태미나 바깥 배경 이미지
    /// </summary>
    Image backgroundOutside;

    /// <summary>
    /// 플레이어
    /// </summary>
    Player player;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        child = child.GetChild(1);
        backgroundOutside = child.GetComponent<Image>();
        backgroundOutside.gameObject.SetActive(false);

        child = transform.GetChild(1);
        inside = child.GetComponent<Image>();
        child = transform.GetChild(2);
        outside = child.GetComponent<Image>();
        outside.gameObject.SetActive(false);

        player = GameManager.Instance.Player;
    }
    private void Update()
    {
        PrintStamina();
    }

    /// <summary>
    /// 스태미나에 따른 Stamina UI 생성 함수
    /// </summary>
    private void PrintStamina()
    {
        if (player == null)
        {
            player = GameManager.Instance.Player;
            return;
        }

        float playerStamina = player.Stamina * 0.01f;

        if (!IsActive)
        {
            // outside가 활성화되지 않은 경우 >> inside 수치 변화
            inside.fillAmount = playerStamina;
        }
        else
        {
            // outside가 활성화된 경우 >> outside & inside 수치 동시에 고려
            if (playerStamina > 0.0f)
            {
                inside.fillAmount = playerStamina;
                outside.fillAmount = 0.0f;

                if (playerStamina > 1.0f)
                {
                    inside.fillAmount = 1.0f;
                    outside.fillAmount = playerStamina - 1.0f;
                }
            }
        }
    }

    /// <summary>
    /// 스태미나 총량을 증가시켜주는 함수
    /// (사당 스크립트에서 델리게이트로 퀘스트 클리어할 때마다 알릴 예정)
    /// (4개를 모아 스태미나로 변환해줄 때 사용하는 함수)
    /// </summary>
    public void PlusStamina()
    {
        // 스태미나 생성
        IsActive = true;
        outside.gameObject.SetActive(IsActive);
        backgroundOutside.gameObject.SetActive(IsActive);

        // 플레이어 기력 설정
        player.MaxStamina = 200.0f;         // 플레이어의 최대 기력 증가
        player.Stamina = player.MaxStamina; // 함수가 실행될 때, 플레이어의 기력 채우기
    }
}
