using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 로딩 이미지를 컨트롤하는 UI
/// </summary>
public class LoadingImageUI : MonoBehaviour
{
    /// <summary>
    /// 로딩 이미지들
    /// </summary>
    public Image[] loadImages;

    /// <summary>
    /// 전환할 로딩 이미지 ( 0 : 빈칸, 1 : 채워진 칸 )
    /// </summary>
    public Sprite[] onoffImages;

    const int offImage = 0;
    const int onImage = 1;

    /// <summary>
    /// 로딩 이미지 개수
    /// </summary>
    const int loadImageCount = 4;

    /// <summary>
    /// 로딩 이미지 인덱스
    /// </summary>
    public int index = 0;

    /// <summary>
    /// 경과한 시간
    /// </summary>
    public float timeElapsed = 0.0f;

    /// <summary>
    /// 시간 속도
    /// </summary>
    public float speed = 2.0f;

    private void Awake()
    {
        // 초기화
        loadImages = new Image[loadImageCount];
        loadImages = GetComponentsInChildren<Image>();
    }

    /// <summary>
    /// 호출 될 때마다 로딩 이미지를 바꾸는 함수
    /// </summary>
    public void ChangeLoadingImages()
    {
        StartCoroutine(ImageCoroutine());
    }

    IEnumerator ImageCoroutine()
    {
        while(true)
        {
            timeElapsed += Time.deltaTime * speed;

            if (timeElapsed > 1.0f)
            {
                if (index > loadImages.Length - 1)
                {
                    // index값이 loadImages 수보다 많으면 초기화
                    index = 0;

                    // 이미지 초기화
                    foreach (Image image in loadImages)
                    {
                        image.sprite = onoffImages[offImage];
                    }
                }

                loadImages[index].sprite = onoffImages[onImage];  // 이미지 바꾸기
                index++;

                timeElapsed = 0f;
            }

            yield return null;
        }
    }

    /// <summary>
    /// 로딩이 끝났을 때 이미지를 바꾸는 함수
    /// </summary>
    public void FinishLoadingImage()
    {
        StopAllCoroutines();

        foreach (Image image in loadImages)
        {
            image.sprite = onoffImages[onImage];
        }
    }
}
