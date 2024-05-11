using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    readonly Vector2 UnderUserPosition = new Vector2(-10f, -60f);
    Image image;
    RectTransform rectTransform;
    Canvas canvas;

    Color originColor;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        image = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        originColor = image.color;
        Close();
    }

    public void Open()
    {
        image.color = originColor;
    }

    public void Close()
    {
        image.color = Color.clear;
    }

    public void SetPosition(bool isCenter, Vector3 worldPosition)
    {
        if (isCenter)
        {
            rectTransform.anchoredPosition = Vector3.zero;
        }
        else
        {
            Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
            //Vector3 anchorPosition = new Vector3(screenPosition.x - canvas.transform.localPosition.x, screenPosition.y - canvas.transform.localPosition.y, screenPosition.z - canvas.transform.localPosition.z);
            transform.position = screenPosition;
        }
    }

}
