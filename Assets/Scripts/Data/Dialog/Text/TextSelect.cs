using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using System;

public class TextSelect : MonoBehaviour
{

    private TextBox textBox;

    private void Awake()
    {
        textBox = FindObjectOfType<TextBox>(); // TextBox 클래스의 인스턴스를 찾음

        Transform child = transform.GetChild(0);
        Button select1 = child.GetComponent<Button>();
        select1.onClick.AddListener(() => Select(1));

        child = transform.GetChild(1);
        Button select2 = child.GetComponent<Button>();
        select2.onClick.AddListener(() => Select(2));

        child = transform.GetChild(2);
        Button select3 = child.GetComponent<Button>();
        select3.onClick.AddListener(() => Select(3));
    }

    private void Start()
    {
        onSeletEnd();
    }

    public void onSeletStart()
    {
        gameObject.SetActive(true);
    }

    public void onSeletEnd()
    {
        gameObject.SetActive(false);
    }

    void Select(int selectId)
    {
        textBox.OnSelect(selectId);
    }


}
