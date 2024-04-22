using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

/// <summary>
/// MapUI의 이벤트 관리 클래스
/// </summary>
public class LargeMapUI : MonoBehaviour, 
    IPointerClickHandler, IPointerMoveHandler, IDragHandler, 
    IBeginDragHandler, IEndDragHandler
{
    Vector2 mousePos;

    public Vector2 MousePos => mousePos;

    float screenWidth => Screen.width;

    /// <summary>
    /// 지도에 클릭했을 때 실행하는 델리게이트
    /// </summary>
    public Action<PointerEventData.InputButton, Vector2> onClick;

    /// <summary>
    /// 지도안에 Mark 오브젝트에 Pointer가 들어가면 실행되는 델리게이트
    /// </summary>
    public Action<Vector2> onPointerInMark;

    //public Action<Vector2> onPointerExitMark;

    public Action<Vector2> onPointerDragBegin;
    public Action<Vector2> onPointerDraging;
    public Action<Vector2> onPointerDragEnd;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.pointerClick)
        {
            mousePos = eventData.position;
            onClick?.Invoke(eventData.button, mousePos);
        }
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        mousePos = eventData.position;
        onPointerInMark?.Invoke(mousePos);
    }

    public void OnDrag(PointerEventData eventData)
    {
        onPointerDraging?.Invoke(eventData.position);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        onPointerDragBegin?.Invoke(eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        onPointerDragEnd?.Invoke(eventData.position);
    }
}