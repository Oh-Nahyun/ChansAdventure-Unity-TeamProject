using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using static UnityEngine.EventSystems.PointerEventData;

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

    /// <summary>
    /// 맵 핑 프리팹
    /// </summary>
    public GameObject mapPingPrefab;

    /// <summary>
    /// Mark 오브젝트를 강조해줄 UI 오브젝트 프리팹
    /// </summary>
    public GameObject highlightPingPrefab;

    /// <summary>
    /// Mark 오브젝트를 강조해줄 UI 오브젝트
    /// </summary>
    GameObject highlightObject;

    Vector3 startDragVector = Vector3.zero;
    Vector3 onDragingVector = Vector3.zero;


    private void Awake()
    {
        // Map UI 초기화
        mapUI = GetComponentInChildren<LargeMapUI>();

        mapUI.onClick += OnClickInput;

        highlightObject = Instantiate(highlightPingPrefab, transform);
        highlightObject.SetActive(false);

        mapUI.onPointerInMark += OnCheckMark;
        mapUI.onPointerDragBegin += OnDragEnter;
        mapUI.onPointerDraging += OnDraging;
        mapUI.onPointerDragEnd += OnDragEnd;
    }

    private void OnDragEnd(Vector2 vector)
    {
        onDragingVector = new Vector3(vector.x, 0, vector.y);
        Vector3 result = startDragVector - onDragingVector;

        // 지속적으로 이동함 
        //MapManager.Instance.MapCamera.transform.position += result;
    }

    private void OnDraging(Vector2 vector)
    {
        onDragingVector = new Vector3(vector.x, 0, vector.y);
        Vector3 result = startDragVector - onDragingVector;

        MapManager.Instance.MapCamera.transform.position += result * Time.deltaTime;
    }

    private void OnDragEnter(Vector2 vector)
    {
        startDragVector = new Vector3(vector.x, 0, vector.y);
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
    private void OnClickInput(InputButton button, Vector2 vector)
    {
        RaycastHit hit = GetObjectScreenToWorld(vector);
        Vector3 instantiateVector = hit.point;
        instantiateVector.y = 0;

        if(button == InputButton.Left)
        {
            Instantiate(mapPingPrefab, instantiateVector, Quaternion.identity);  // PointObject
        }
        else if(button == InputButton.Right)
        {
            MapPointMark mark = hit.transform.gameObject?.GetComponent<MapPointMark>(); // 닿은 오브젝트가 Mark 오브젝트인지 확인

            if (mark != null)
            {
                mark.ShowMarkInfo();
            }
        }
    }

    /// <summary>
    /// 맵 안에서 Mark에 포인터가 닿으면 실행되는 함수
    /// </summary>
    /// <param name="pointObject">닿은 오브젝트</param>
    private void OnCheckMark(Vector2 pointVector)
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
}