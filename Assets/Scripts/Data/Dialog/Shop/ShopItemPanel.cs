using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopItemPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    Image itemImage;
    TextMeshProUGUI itemNameText;
    TextMeshProUGUI itemStockText;
    TextMeshProUGUI itemPriceText;

    public ItemData itemData;
    ShopItemData shopItemData;

    /// <summary>
    /// 아이템의 남은 재고
    /// </summary>
    public int itemStock = 10;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        itemImage = child.GetComponent<Image>();

        child = transform.GetChild(1);
        itemNameText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(2);
        itemStockText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(3);
        child = child.transform.GetChild(1);
        itemPriceText = child.GetComponent<TextMeshProUGUI>();
        shopItemData = FindAnyObjectByType<ShopItemData>();
    }

    private void Start()
    {
        if (itemData != null)
        {
            itemImage.sprite = itemData.itemIcon;
            itemNameText.text = itemData.itemName;
            itemStockText.text = itemStock.ToString();
            itemPriceText.text = itemData.price.ToString();
        }
    }

    /// <summary>
    /// 마우스가 들어왔을 때 실행하는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("마우스 들어옴");
        if (shopItemData != null)
        {
            // 해당 패널의 ItemData를 건내줌
            shopItemData.GetItemData(itemData);
        }
    }

    /// <summary>
    /// 마우스가 나갔을 때 실행하는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("마우스 나감");
        if (shopItemData != null)
        {
            shopItemData.gameObject.SetActive(false);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("마우스 클릭");
        
    }
}
