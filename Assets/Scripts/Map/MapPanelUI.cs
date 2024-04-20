using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Map 패널 UI를 관리하는 클래스
/// </summary>
public class MapPanelUI : MonoBehaviour
{
    /// <summary>
    /// Map을 찍을 카메라
    /// </summary>
    public Camera mapCamera;

    LargeMapUI mapUI;

    public GameObject mapPingPrefab;
    public GameObject highlightPingPrefab;

    GameObject highlightObject;


    private void Awake()
    {
        mapUI = GetComponentInChildren<LargeMapUI>();

        mapUI.onClick += OnClickInput;

        //
        highlightObject = Instantiate(highlightPingPrefab, transform);
        highlightObject.SetActive(false);

        mapUI.onPointerInMark += CheckMark;
        mapUI.onPointerExitMark += ExitMark;
    }

    /// <summary>
    /// 스크린에서 월드 오브젝트의 정보를 구하는 함수
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    private RaycastHit GetObjectScreenToWorld(Vector3 vector)
    {
        Ray ray = mapCamera.ScreenPointToRay(vector);   // ray
        RaycastHit hit;                                 // rayHit 정보

        if (Physics.Raycast(ray, out hit, 1000f, LayerMask.GetMask("Map Object"))) // Map Object 탐지
        {
            Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 5f);
        }

        return hit;
    }

    /// <summary>
    /// 맵에 클릭했을 때 실행되는 함수
    /// </summary>
    /// <param name="vector"></param>
    private void OnClickInput(Vector2 vector)
    {
        RaycastHit hit = GetObjectScreenToWorld(vector);
        Vector3 instantiateVector = hit.point;
        instantiateVector.y = 0;
        Instantiate(mapPingPrefab, instantiateVector, Quaternion.identity);  // PointObject
    }

    /// <summary>
    /// 맵 안에서 Mark에 포인터가 닿으면 실행되는 함수
    /// </summary>
    /// <param name="pointObject">닿은 오브젝트</param>
    private void CheckMark(Vector2 pointVector)
    {
        RaycastHit hit = GetObjectScreenToWorld(pointVector);

        GameObject pointObject = hit.transform.gameObject;

        MapPointMark mark = hit.transform.gameObject?.GetComponent<MapPointMark>(); // 닿은 오브젝트가 Mark 오브젝트인지 확인
        if (mark != null)
        {
            highlightObject.SetActive(true);
            highlightObject.transform.localPosition = pointObject.transform.position;
            Debug.Log("Map Mark 찍음");
        }
        else
        {
            highlightObject.SetActive(false);
        }
    }

    private void ExitMark(Vector2 pointVector)
    {

    }
}