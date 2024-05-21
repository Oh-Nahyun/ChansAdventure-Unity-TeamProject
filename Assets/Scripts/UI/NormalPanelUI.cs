using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NormalPanelUI : MonoBehaviour
{
    CanvasGroup canvasGroup;
    Button exitButton;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        exitButton = GetComponentInChildren<Button>();
    }

    private void Start()
    {
        exitButton.onClick.AddListener(ExitGame);
    }

    private void ExitGame()
    {
        //SceneManager.LoadScene("Main_Menu");
        GameManager.Instance.ChangeToTargetScene("Main_Menu", GameManager.Instance.Player.gameObject);
        GameManager.Instance.gameState = GameState.NotStart;
        GameManager.Instance.isField = false;
    }

    public void ShowUI()
    {
        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }   
    
    public void CloseUI()
    {
        canvasGroup.alpha = 0;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
