using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EquipSlot_Armor :MonoBehaviour, IDropHandler, IPointerClickHandler
{
    // 아이템 정보
    Item Item;
    [SerializeField]
    Image ItemImage;

    Item TempItem; // 아이템 임시 저장용

    public void OnDrop(PointerEventData eventData)
    {
        if (DragSlot.instance.TargetSlot != null)
        {
            Slot TargetSlot = DragSlot.instance.TargetSlot;

            RegistSlot(TargetSlot);
        }
    }
    public void RegistSlot(Slot _slot)
    {
        if (_slot.item.equiptype == Item.EquipType.Armor)
        {
            if (_slot.item.RequiredLevel <= GameMgr.GetInstance().PLevel)
            {
                if(Item != null)                                            // 이미 장착중인 장비가 있을 경우 임시저장 후 인벤토리에 추가
                    TempItem = Item;
                Item = _slot.item;                                         // 아이템에 정보 등록
                ItemImage.sprite = _slot.item.ItemImage;                   // 이미지 정보 등록 및 불투명화
                SetColor(1);
                _slot.UpdateSlotCount(-1);                                 // 인벤토리 슬롯에서 삭제
                if (TempItem != null)
                {
                    _slot.AddItem(TempItem);
                    GameMgr.GetInstance().PArmorDef -= TempItem.Defense;
                }
                GameMgr.GetInstance().PArmorDef += Item.Defense;

                TempItem = null;
            }
            else
            {
                Debug.Log("레벨이 부족합니다." + _slot.item.RequiredLevel + "이상이 되어야 착용할 수 있습니다.");
            }

        }
        else
        {
            Debug.Log("갑옷만 장착 할 수 있습니다.");
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Item != null && eventData.button == PointerEventData.InputButton.Right)     // 우클릭 시
        {
            GameMgr.GetInstance().PArmorDef -= Item.Defense;
            InventoryMgr.GetInstance().GainItem(Item);      // 아이템 인벤토리에 추가
            ClearSlot();                                    // 장비칸 클리어
        }
    }

    void ClearSlot()
    {
        Item = null;
        ItemImage.sprite = null;
        SetColor(0);
    }

    private void SetColor(float _alpha) // 아이템 아이콘 투명도 조절
    {
        Color color = ItemImage.color;
        color.a = _alpha;
        ItemImage.color = color;
    }
}
