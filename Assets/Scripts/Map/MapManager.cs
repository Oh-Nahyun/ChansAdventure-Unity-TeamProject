using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Map UI의 각종 값을 다루는 Manager 클래스
/// </summary>
public class MapManager : MonoBehaviour
{
    /// <summary>
    /// 임시 Map Singleton
    /// </summary>
    public static MapManager Instance;

    /// <summary>
    /// 맵의 등고선 색 ( Color Gap마다 다른 색으로 표시 )
    /// </summary>
    public Color[] color;

    /// <summary>
    /// 등고선 색 개수
    /// </summary>
    public uint ColorCount;

    /// <summary>
    /// 색깔별 Object y값 차이
    /// </summary>
    public float colorGap = 5f;

    private void Awake()
    {
        Instance = this;

        InitializeMapColor();
    }

    private void Start()
    {

    }

    private void InitializeMapColor()
    {
        color = new Color[ColorCount];

        for (int i = 0; i < color.Length; i++)
        {
            float ratio = 1 / (float)ColorCount;    // 색깔개수 별로 색 비율 결정
            color[i] = Color.white * ratio * (i + 1);         // 색상 정하기
            color[i].a = 1f;                        // alpha값은 1로 다시 변경
        }
    }

    /// <summary>
    /// 오브젝트의 색깔을 정해주는 함수
    /// </summary>
    /// <param name="yPosition">오브젝트의 y좌표 값 ( World )</param>
    /// <returns>yPosition / ColorCount의 결과 값 배열 색</returns>
    public Color SetColor(float yPosition)
    {
        Color resultColor = Color.white;

        int colorIndex = Mathf.FloorToInt(yPosition / (float)ColorCount); // color 인덱스 값 설정

        resultColor = color[colorIndex];

        return resultColor;
    }
}
