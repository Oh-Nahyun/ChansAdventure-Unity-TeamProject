using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemPanel : MonoBehaviour
{
    Image itemImage;
    TextMeshProUGUI itemNameText;
    TextMeshProUGUI itemStockText;
    TextMeshProUGUI itemPriceText;

    public ItemData itemData;

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


}
