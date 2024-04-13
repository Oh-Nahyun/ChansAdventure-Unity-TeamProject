using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventoryPlayerUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    /// <summary>
    /// 드래그 시작위치
    /// </summary>
    float startDragPosition = 0;

    /// <summary>
    /// 현재 드래그 중인 포인터 위치
    /// </summary>
    float currentDragPosition = 0;

    /// <summary>
    /// 드래그 시작 위치에서 드래그한 길이 (현재 위치 - 시작 위치)
    /// </summary>
    float dragValue = 0;

    /// <summary>
    /// dragValue 접근하기위한 프로퍼티
    /// </summary>
    float DragValue
    {
        get => dragValue;
        set
        {
            if(dragValue != value)
            {
                dragValue = Mathf.Clamp(value, -5f, 5f);
            }
        }
    }

    /// <summary>
    /// 마우스가 움직이는 지 체크하는 변수
    /// </summary>
    bool isMove = false;

    /// <summary>
    /// RenderCharater 텍스쳐를 보여주는 카메라 오브젝트
    /// </summary>
    GameObject rendererCamera;

    void Start()
    {
        rendererCamera = ItemDataManager.Instance.CharaterRenderCamera;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 변수값 초기화 ( 예상치 못한 회전 방지 )
        DragValue = 0f;
        currentDragPosition = 0f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        isMove = eventData.IsPointerMoving();
        if(isMove)
        {
            OnCharacterRenderPanelDrag(eventData.position.x);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // 초기화
        StartCoroutine(SpinActive());
    }

    /// <summary>
    /// 드래그가 끝나면 포인터가 끝난 방향으로 계속 돌게하는 함수
    /// </summary>
    IEnumerator SpinActive()
    {
        while(DragValue != 0.0f)
        {
            if(DragValue < 0) // Drag가 음수( 왼쪽 방향 회전 )
            {
                DragValue += Time.deltaTime;
                rendererCamera.transform.rotation = Quaternion.Euler(0, rendererCamera.transform.eulerAngles.y + DragValue * 0.4f, 0);
            }
            else if(DragValue > 0) // Drag가 양수 ( 오른쪽 방향 회전 )
            {
                DragValue -= Time.deltaTime;
                rendererCamera.transform.rotation = Quaternion.Euler(0, rendererCamera.transform.eulerAngles.y + DragValue * 0.4f, 0);
            }
            yield return null;
        }
    }

    void OnCharacterRenderPanelDrag(float pointerValue)
    {
        currentDragPosition = pointerValue;                     // 드래그한 포인터 위치 갱신
        DragValue = startDragPosition - currentDragPosition;    // 드래그 시작한 지점으로부터 크기 ( 드래그 시작 위치 - 현재 포인터 위치)
        startDragPosition = currentDragPosition;

        rendererCamera.transform.eulerAngles = new Vector3(0, rendererCamera.transform.eulerAngles.y + DragValue, 0); // 카메라 회전값 조정
    }
}