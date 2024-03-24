using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class InventorySlotUI : SlotUI_Base, IBeginDragHandler, IDragHandler, IEndDragHandler,
                                            IPointerClickHandler, 
                                            IPointerEnterHandler, IPointerExitHandler
{
    InventoryUI invenUI;

    void Awake()
    {
        invenUI = ItemDataManager.Instance.InventoryUI;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // temp로 아이템 옮기기 (slot -> temp)
        invenUI.onSlotDragBegin?.Invoke(InventorySlotData.SlotIndex);
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log($"드래그 중 : {eventData}");
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        if(obj != null)
        {
            invenUI.onSlotDragEnd?.Invoke(obj.GetComponent<SlotUI_Base>().InventorySlotData.SlotIndex);
        }
        else
        {
            // 슬롯이 아니다
            invenUI.onSlotDragEndFail?.Invoke();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        // 클릭하면 장비인지 확인
        // 장비면 착용
        // 다른 아이템 무시
        // 오른쪽 클릭하면 메뉴?

        if (obj != null)
        {
            bool isPressedQ = Keyboard.current.qKey.ReadValue() > 0;

            if(isPressedQ) // dividUI 열기
            {
                invenUI.onDivdItem(InventorySlotData.SlotIndex);
                Debug.Log($"나누기");
            }
        }
        else
        {
            Debug.Log(obj);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        invenUI.onShowDetail?.Invoke(InventorySlotData.SlotIndex);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        invenUI.onCloseDetail?.Invoke();
    }
}
