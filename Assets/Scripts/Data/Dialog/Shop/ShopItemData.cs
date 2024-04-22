using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemData : MonoBehaviour
{
    ItemData itemData;

    Image itemImage;
    TextMeshProUGUI itemNameText;
    TextMeshProUGUI itemDataText;
    TextMeshProUGUI itemPriceText;

    private void Awake()
    {
        Transform child = transform.GetChild(1);
        itemImage = child.GetComponent<Image>();

        child = transform.GetChild(2);
        itemNameText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(3);
        itemDataText = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(4);
        itemPriceText = child.GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (itemData != null)
        {
            itemImage.sprite = itemData.itemIcon;
            itemNameText.text = itemData.itemName;
            itemDataText.text = itemData.desc;
            itemPriceText.text = itemData.price.ToString();
        }
    }

    public void GetItemData(ItemData newItemData)
    {
        itemData = newItemData;
        gameObject.SetActive(true);
    }
}
