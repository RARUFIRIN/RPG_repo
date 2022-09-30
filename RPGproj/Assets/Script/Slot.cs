using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Item item;
    public int itemCount;
    public Image ItemImage;

    [SerializeField]
    private TextMeshProUGUI text_count;
    [SerializeField]
    private GameObject image_count;

    #region 인벤토리 상호작용
    public void OnPointerClick(PointerEventData eventData)      // 마우스 우클릭 이벤트
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(item != null)
            {
                if(item.equiptype == Item.EquipType.Weapon)
                {
                    // 무기 교체 or 무기 장착
                    InventoryMgr.GetInstance().Equip(Item.EquipType.Weapon, this);

                }
                else if (item.equiptype == Item.EquipType.Armor)
                {
                    // 방어구 교체 or 방어구 장착
                    InventoryMgr.GetInstance().Equip(Item.EquipType.Armor, this);


                }
                else if (item.equiptype == Item.EquipType.Boot)
                {
                    // 방어구 교체 or 방어구 장착
                    InventoryMgr.GetInstance().Equip(Item.EquipType.Boot, this);



                }
                else if (item.equiptype == Item.EquipType.Helmet)
                {
                    // 방어구 교체 or 방어구 장착
                    InventoryMgr.GetInstance().Equip(Item.EquipType.Helmet, this);


                }
                else if (item.itemType == Item.ItemType.Food)
                {
                    // 소비아이템 소비
                    InventoryMgr.GetInstance().SpendPotion(item);
                    UpdateSlotCount(-1);
                }
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)         // 드래그 시작 이벤트
    {                                                           // 해당 슬롯의 아이템 이미지를 할당
        if(item != null)
        {
            DragSlot.instance.TargetSlot = this;
            DragSlot.instance.DragSetImage(ItemImage);
            DragSlot.instance.transform.position = eventData.position;
        }
    }

    public void OnDrag(PointerEventData eventData)          // 드래그 중 이미지가 포인터를 따라다니게 함
    {
       if(item != null)
       DragSlot.instance.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)       // 드래그 끝남 이벤트
    {

        DragSlot.instance.SetColor(0);                      // 이미지 투명화
        DragSlot.instance.TargetSlot = null;                // 타겟 정보 초기화

    }



    public void OnDrop(PointerEventData eventData)          // 드래그 후 드롭 이벤트
    {
        if (DragSlot.instance.TargetSlot != null && DragSlot.instance.TargetSlot.item.itemType != Item.ItemType.Skill)           // 드롭할 슬롯과 교체
            ChangeSlot();
    }

    private void ChangeSlot()
    {
        Item tempitem = item;                               // 임시객체에 정보 저장
        int tempitemCount = itemCount;

        AddItem(DragSlot.instance.TargetSlot.item, DragSlot.instance.TargetSlot.itemCount); // 드롭할 슬롯에 아이템 추가

        InventoryMgr.GetInstance().SetChangedSlots(DragSlot.instance.TargetSlot , this);

        if (tempitem != null)
            DragSlot.instance.TargetSlot.AddItem(tempitem, tempitemCount);      // 드롭할 슬롯에 아이템이 있었다면 드래그 시작 슬롯에 추가
        else
            DragSlot.instance.TargetSlot.ClearSlot();                           // 없었다면 드래그 시작 슬롯 클리어
    }

    #endregion

    private void SetColor(float _alpha) // 아이템 아이콘 투명도 조절
    {
        Color color = ItemImage.color;
        color.a = _alpha;
        ItemImage.color = color;
    }

    public void AddItem(Item _item, int _count = 1) // 아이템 추가
    {
        item = _item;
        itemCount = _count;
        ItemImage.sprite = item.ItemImage;

        if(item.itemType != Item.ItemType.Equip) // 무기가 아니면 갯수표시를 활성화
        {
            image_count.SetActive(true);
            text_count.text = itemCount.ToString();
        }
        else // 장비 일 때 갯수표시를 비활성화
        {
            text_count.text = "0";
            image_count.SetActive(false);
        }

        SetColor(1); // 아이템 이미지 불투명화
    }

    public void UpdateSlotCount(int _count) // 아이템 갯수 업데이트
    {
        itemCount += _count;
        text_count.text = itemCount.ToString();

        if(itemCount <= 0)
        {
            ClearSlot();
        }
    }

    private void ClearSlot() // 슬롯 정보 초기화
    {
        // 각종 정보 초기화
        item = null;
        itemCount = 0;
        ItemImage.sprite = null;
        SetColor(0); // 해당 아이템 이미지 투명화

        text_count.text = "0";
        image_count.SetActive(false); // 아이템 갯수표시를 비활성화

    }


}
