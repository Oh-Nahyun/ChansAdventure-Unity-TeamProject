using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObject : MonoBehaviour
{
    /// <summary>
    /// Map Object 레이어가 있는 오브젝트의 Material
    /// </summary>
    Renderer mapPointMaterial;

    /// <summary>
    /// MapPointMaterial의 색깔을 지정하기 위한 y좌표값
    /// </summary>
    float position_Y => transform.position.y;

    /// <summary>
    /// 지형별 색깔 배열
    /// </summary>
    public Color[] color;

    /// <summary>
    /// 색깔 개수
    /// </summary>
    private int colorCount = 3;

    /// <summary>
    /// 지형별 높이 차이 ( 각 차이별로 색깔이 다를 예정)
    /// </summary>
    public float colorGap = 5f;

    void Start()
    {
        Transform child = transform.GetChild(0);
        mapPointMaterial = child.GetChild(0).GetComponent<Renderer>();

        color = new Color[colorCount];
        for(int i = 0; i < color.Length; i++)
        {
            color[i] = new Color(i*0.3f, i*0.3f, i*0.3f);
        }

        SetColor();
    }

    void SetColor()
    {
        for(int i = 0; i < 3; i++)
        {
            if(i * colorGap < position_Y && position_Y <= (i + 1) * colorGap)
            {                
                mapPointMaterial.material.color = color[i];
            }
        }
    }
}
