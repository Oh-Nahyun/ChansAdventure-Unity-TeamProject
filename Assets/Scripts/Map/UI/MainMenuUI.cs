using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    Button StartBtn;
    Button LoadBtn;
    Button ExitBtn;

    private void Start()
    {
        StartBtn = GetComponent<Button>();
        LoadBtn = GetComponent<Button>();
        ExitBtn = GetComponent<Button>();
    }
}