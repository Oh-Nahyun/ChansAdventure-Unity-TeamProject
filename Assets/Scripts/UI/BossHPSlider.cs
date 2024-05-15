using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPSlider : MonoBehaviour
{
    public Slider hpSlider; // Inspector에서 연결할 슬라이더
    public Boss boss;       // Inspector에서 연결할 Boss 오브젝트

    const float sliderReduceValue = 25f; // 슬라이더 감소 수치

    CanvasGroup canvasGroup;

    void Start()
    {
        hpSlider = GetComponent<Slider>();
        boss = FindObjectOfType<Boss>();
        canvasGroup = GetComponent<CanvasGroup>();

        // 게임 시작 시 슬라이더 최대값을 설정
        hpSlider.maxValue = boss.MaxHP;
        hpSlider.value = boss.HP;
    }

    void Update()
    {
        // 매 프레임마다 Boss의 HP를 슬라이더에 반영

        if(hpSlider.value > boss.HP)
        {
            if(!boss.IsAlive) // 보스가 사망했으면 체력바 숨기기
            {
                gameObject.SetActive(false);
            }

            hpSlider.value -= Time.deltaTime * sliderReduceValue;
        }
    }

    /// <summary>
    /// 체력바 패널 활성화 함수
    /// </summary>
    public void ShowPanel()
    {
        canvasGroup.alpha = 1.0f;
    }
}