using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Large Map에 맵을 그릴 오브젝트가 가지는 클래스
/// </summary>
public class MapObject : MonoBehaviour
{
    /// <summary>
    /// Map Object 레이어가 있는 오브젝트의 Material
    /// </summary>
    Renderer mapPointMaterial;

    /// <summary>
    /// MapPointMaterial의 색깔을 지정하기 위한 y좌표값
    /// </summary>
    float position_Y = 0f;

    bool isColored = false;

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

    private void Update()
    {
        Scan();
    }

    void Scan()
    {
        if (isColored) return;

        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 1000f))
        {
            //Debug.Log($"{gameObject.name}.y : {hit.point}");
            Debug.DrawRay(transform.position, transform.forward * 1000f, Color.red);
            position_Y = hit.point.y;

            Color mapColor = MapManager.Instance.SetColor(position_Y);

            if (mapPointMaterial == null)
            {
                mapPointMaterial = mapObject.GetComponent<Renderer>();
            }

            mapPointMaterial.material.color = mapColor;
            isColored = true;
        }
    }
}
