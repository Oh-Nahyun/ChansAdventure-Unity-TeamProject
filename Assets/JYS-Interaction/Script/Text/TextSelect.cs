using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine;
using System;

public class TextSelect : MonoBehaviour
{

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        Button select1 = child.GetComponent<Button>();
        select1.onClick.AddListener(Select);

        child = transform.GetChild(1);
        Button select2 = child.GetComponent<Button>();
        select2.onClick.AddListener(Select);

        child = transform.GetChild(2);
        Button select3 = child.GetComponent<Button>();
        select3.onClick.AddListener(Select);

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

    public Action OnSelectButton;
    void Select()
    {
        OnSelectButton?.Invoke();
    }


}
