using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SkillBoxUI : MonoBehaviour
{
    Image selectImage;
    Image iconImage;

    private void Awake()
    {
        Transform child = transform.GetChild(1);
        selectImage = child.GetComponent<Image>();
        child = transform.GetChild(2);
        iconImage = child.GetComponent<Image>();
        Select(false);
    }

    public void SetIcon(Sprite icon)
    {
        iconImage.sprite = icon;
    }


    public void Select(bool isSelect)
    {
        selectImage.gameObject.SetActive(isSelect);
    }
}
