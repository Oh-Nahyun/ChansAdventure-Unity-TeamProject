using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SaveCheckUI : MonoBehaviour
{
    CanvasGroup canvasGroup;
    TextMeshProUGUI uiText;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        uiText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowSaveCheck()
    {

    }

    public void ShowLoadCheck()
    {

    }
}
