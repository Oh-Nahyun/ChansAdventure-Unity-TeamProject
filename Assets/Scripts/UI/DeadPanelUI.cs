using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DeadPanelUI : MonoBehaviour
{
    GameManager gameManager;
    Player player;
    Vignette vignette;
    CanvasGroup canvasGroup;

    Button button;
    Image image;
    FadeInOutTextUI fadeInOutTextUI;
    SaveHandler_Base savePanel;

    /// <summary>
    /// 사망 씬 이벤트 시간
    /// </summary>
    public float totalTime = 1.7f;

    private void Start()
    {
        gameManager = GameManager.Instance;
        player = gameManager.Player;
        VolumeProfile profile = FindAnyObjectByType<Volume>().profile;
        profile.TryGet(out vignette);

        image = GetComponent<Image>();
        canvasGroup = GetComponent<CanvasGroup>();
        fadeInOutTextUI = GetComponentInChildren<FadeInOutTextUI>();
        button = GetComponentInChildren<Button>();
        savePanel = FindAnyObjectByType<SaveHandler_Base>();
        button.onClick.AddListener(OpenSavePanel);

        player.onDie += OnDie;

        Initialize();
    }

    void Initialize()
    {
        if(vignette != null)
        {
            vignette.color.value = Color.clear;
            vignette.intensity.value = 0f;
        }

        button.gameObject.SetActive(false);

        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        fadeInOutTextUI.fadeIntime = totalTime;
    }

    void OpenSavePanel()
    {
        Debug.Log("asdf");
        savePanel.ShowSavePanel();
    }

    void OnDie()
    {
        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;

        fadeInOutTextUI.StartFadeIn();          // 글자 페이드 인 시작
        StartCoroutine(DeadSceneCouroutine());
    }

    /// <summary>
    /// 사망할 때 시작하는 코루틴
    /// </summary>
    IEnumerator DeadSceneCouroutine()
    {
        float timeElapsed = 0f;         // 경과 시간
        float timeElapsedValue = 0f;    // 경과 시간에 따른 초당 비율값

        while(timeElapsed < totalTime)
        {
            float timeValue = Time.deltaTime / totalTime;
            timeElapsed += Time.deltaTime;
            timeElapsedValue += timeValue;

            // vignette 축소 
            vignette.intensity.value = timeElapsedValue;
            image.color = new Color(0f, 0f, 0f, timeElapsedValue);
            yield return null;
        }

       // 버튼 활성화
        button.gameObject.SetActive(true);
    }
}
