using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class LargeMapUI : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler
{
    Vector2 mousePos;

    public Vector2 MousePos => mousePos;

    float screenWidth => Screen.width;

    public Action<Vector2> onClick;

    private void Awake()
    {
        //Debug.Log();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.pointerClick)
        {
            mousePos = eventData.position;
            onClick?.Invoke(mousePos);
            Debug.Log($"LargeMapUI Mouse Position : {mousePos}");        
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log(eventData.position);
    }
    public void OnPointerMove(PointerEventData eventData)
    {
        //Debug.Log(eventData.position);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
       //Debug.Log(eventData.position);
    }

}
