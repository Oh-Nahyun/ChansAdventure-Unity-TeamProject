using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class TextSelect : MonoBehaviour
{
    private TextBox textBox;
    TextMeshProUGUI buttonText1;
    TextMeshProUGUI buttonText2;
    TextMeshProUGUI buttonText3;

    private void Awake()
    {
        textBox = FindObjectOfType<TextBox>(); // TextBox 클래스의 인스턴스를 찾음

        Transform child = transform.GetChild(0);
        Button select1 = child.GetComponent<Button>();
        select1.onClick.AddListener(() => Select(1)); //선택지 1번 누를시 id + 1
        buttonText1 = child.GetComponentInChildren<TextMeshProUGUI>();

        child = transform.GetChild(1);
        Button select2 = child.GetComponent<Button>();
        select2.onClick.AddListener(() => Select(2)); //선택지 2번 누를시 id + 1
        buttonText2 = child.GetComponentInChildren<TextMeshProUGUI>();

        child = transform.GetChild(2);
        Button select3 = child.GetComponent<Button>();
        select3.onClick.AddListener(() => Select(3)); //선택지 3번 누를시 id + 1
        buttonText3 = child.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {
        onSeletEnd();
    }

    public void setButtonText(string text1, string text2, string text3)
    {
        buttonText1.text = text1;
        buttonText2.text = text2;
        buttonText3.text = text3;
    }

    /// <summary>
    /// 선택 시작시 실행될 함수
    /// </summary>
    public void onSeletStart()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// 선택 종료시 실행될 함수
    /// </summary>
    public void onSeletEnd()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// 선택지에 따라 Id값을 보내는 함수
    /// </summary>
    /// <param name="selectId">선택지 값</param>
    void Select(int selectId)
    {
        textBox.OnSelect(selectId);
    }


}
