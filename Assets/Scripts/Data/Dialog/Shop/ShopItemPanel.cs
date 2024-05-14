using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    SelectBox selectBox;

    Inventory inventory;

    Player player;

    bool buyItem = false;

    public Color inStockColor = Color.white;
    public Color noStockColor = Color.red;

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

        selectBox = FindAnyObjectByType<SelectBox>();
    }

    private void Start()
    {
        player = GameManager.Instance.Player;
        if (itemData != null)
        {
            itemImage.sprite = itemData.itemIcon;
            itemNameText.text = itemData.itemName;
            itemStockText.text = itemStock.ToString();
            itemPriceText.text = itemData.price.ToString();
        }
        selectBox.onButtonCheck += () => BuyItem();
        selectBox.onButtonCancel += () => buyItem = false;
    }

    private void Update()
    {
        SetShopItemText();
    }

    /// <summary>
    /// 마우스가 들어왔을 때 실행하는 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerEnter(PointerEventData eventData)
    {
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
        if (shopItemData != null)
        {
            shopItemData.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 상점 패널을 클릭했을 때 실항할 함수
    /// </summary>
    /// <param name="eventData"></param>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (inventory.Gold >= itemData.price)
        {
            if (itemStock > 0)
            {
                buyItem = true;
                selectBox.gameObject.SetActive(true);

                selectBox.selectText.text = $"정말로 {itemData.itemName}을(를) 구매하시겠습니까?";
                selectBox.buttonCheckText.text = "구매";
                selectBox.buttonCancelText.text = "취소";
            }
            else
            {
                Debug.Log($"{itemData.itemName} 재고 없음");
            }
        }
        else
        {
            Debug.Log("잔액이 모자름");
        }
    }

    /// <summary>
    /// 재고와 잔액에 따라 상점UI를 변화시키는 함수
    /// </summary>
    private void SetShopItemText()
    {
        if (inventory == null)
        {
            inventory = player.Inventory;
        }

        if (inventory.Gold >= itemData.price)
        {
            itemNameText.color = inStockColor;
            itemPriceText.color = inStockColor;
        }
        else
        {
            itemNameText.color = noStockColor;
            itemPriceText.color = noStockColor;
        }

        if (itemStock > 0)
        {
            itemStockText.color = inStockColor;
        }
        else
        {
            itemNameText.color = noStockColor;
            itemStockText.color = noStockColor;
        }
    }

    private void BuyItem()
    {
        if(buyItem) 
        {
            inventory.AddSlotItem((uint)itemData.itemCode);
            itemStock--;
            itemStockText.text = itemStock.ToString();

            inventory.SubCoin(itemData.price);
            selectBox.gameObject.SetActive(false);
            buyItem = false;
        }
    }

}
