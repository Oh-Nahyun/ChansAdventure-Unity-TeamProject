using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveResetUI : MonoBehaviour
{
    Button resetButton;

    public Action onReset;

    private void Awake()
    {
        resetButton = GetComponent<Button>();
        resetButton.onClick.AddListener(() =>
        {
            onReset?.Invoke();
        });
    }
}