using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// MapUI의 이벤트 관리 클래스
/// </summary>
public class LargeMapUI : MonoBehaviour, 
    IPointerClickHandler, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
{
    Vector2 mousePos;

    public Vector2 MousePos => mousePos;

    float screenWidth => Screen.width;

    public Action<Vector2> onClick;
    public Action<Vector2> onPointerInMark;
    public Action<Vector2> onPointerExitMark;

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.pointerClick)
        {
            mousePos = eventData.position;
            onClick?.Invoke(mousePos);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log(eventData.position);
        
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        mousePos = eventData.position;
        onPointerInMark?.Invoke(mousePos);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
    }
}