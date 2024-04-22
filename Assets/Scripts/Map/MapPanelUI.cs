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
    Camera mapCamera;

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

    /// <summary>
    /// 드래그 시작 Vector값
    /// </summary>
    Vector3 startDragVector = Vector3.zero;

    /// <summary>
    /// 드래그 중인 포인터 Vector 값
    /// </summary>
    Vector3 onDragingVector = Vector3.zero;

    /// <summary>
    /// 드래그가 끝나는 위치 Vector 값
    /// </summary>
    Vector3 endDragVector = Vector3.zero;

    /// <summary>
    /// 드래그를 하는지 확인하는 플래그 변수
    /// </summary>
    bool isDrag = false;

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
        mapUI.onScroll += OnScroll;
    }

    private void OnScroll(Vector2 scrollDelta)
    {
        float scroll = -scrollDelta.y;

        mapCamera.orthographicSize += scroll;
        mapCamera.orthographicSize = Mathf.Clamp(mapCamera.orthographicSize, 50, 100);
    }

    private void Start()
    {
        mapCamera = MapManager.Instance.MapCamera;
    }

    /// <summary>
    /// 드래그를 시작할 때 실행하는 함수
    /// </summary>
    /// <param name="vector"></param>
    private void OnDragEnter(Vector2 vector)
    {
        startDragVector = new Vector3(vector.x, 0, vector.y);
        startDragVector += mapCamera.transform.position;
        MapManager.Instance.SetCaemraPosition(startDragVector);
    }

    /// <summary>
    /// 드래그가 진행중일 때 실행하는 함수
    /// </summary>
    /// <param name="vector"></param>
    private void OnDraging(Vector2 vector)
    {
        isDrag = true;

        onDragingVector = new Vector3(vector.x, 0, vector.y);

        Vector3 result = startDragVector - onDragingVector;

        MapManager.Instance.SetCaemraPosition(result);
        endDragVector = result;
    }

    /// <summary>
    /// 드래그가 끝나면 실행하는 함수
    /// </summary>
    /// <param name="vector"></param>
    private void OnDragEnd(Vector2 vector)
    {
        isDrag = false;
        //endDragVector = mapCamera.ScreenToWorldPoint(endDragVector);
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
        }
        else if(button == InputButton.Right)
        {
            MapPointMark mark = hit.transform.gameObject?.GetComponent<MapPointMark>(); // 닿은 오브젝트가 Mark 오브젝트인지 확인

            if (mark != null)   // 닿은 곳에 Mark가 있다.
            {
                mark.ShowMarkInfo();
            }
            else // Mark가 없다 : Mark 생성
            {
                Instantiate(mapPingPrefab, instantiateVector, Quaternion.identity);  // PointObject
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

        if (isDrag || hit.collider == null)
            return;

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
}