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
    /// 맵에 표시할 오브젝트
    /// </summary>
    [Tooltip("Layer가 반드시 Map Object로 되있어야 한다.")]
    public GameObject mapObject;

    private void Awake()
    {
        if(mapObject == null)
        {
            Debug.LogWarning($"{gameObject.name}의 mapObject가 비어있습니다.");
        }
        else
        {
            mapPointMaterial = mapObject.GetComponent<Renderer>();
        }
    }

    void Start()
    {
        Color mapColor = MapManager.Instance.SetColor(position_Y);

        mapPointMaterial.material.color = mapColor;
    }
}
