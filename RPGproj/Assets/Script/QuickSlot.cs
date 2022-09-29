using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour, IDropHandler
{
    public Item item;
    public int itemCount;
    public Image ItemImage;
    public Slot LinkedSlot;

    [SerializeField]
    private TextMeshProUGUI text_count;
    [SerializeField]
    private GameObject image_count;
    [SerializeField]
    private GameObject Parent;
    public int OnClear;
    bool IsChangeNow;
    private void Awake()
    {
    }
    private void Update()
    {
        if (item != null && LinkedSlot.item != item) // 퀵 슬롯 등록 상태에서 인벤토리 내부에서 자리변화가 일어났을 때 호출
        {
            if (InventoryMgr.GetInstance().GetChangedSlot(0) != null && InventoryMgr.GetInstance().GetChangedSlot(1) != null)
            {
                if (InventoryMgr.GetInstance().GetChangedSlot(0) == LinkedSlot)
                {
                    LinkedSlot = InventoryMgr.GetInstance().GetChangedSlot(1);
                    UpdateSlot();
                }
                else if (InventoryMgr.GetInstance().GetChangedSlot(1) == LinkedSlot)
                {
                    LinkedSlot = InventoryMgr.GetInstance().GetChangedSlot(0);
                    UpdateSlot();
                }
                StopCoroutine(ResetLink());
                StartCoroutine(ResetLink());
            }
        }

        if (LinkedSlot != null && LinkedSlot.itemCount > 0)
        {
            itemCount = LinkedSlot.itemCount;
            text_count.text = itemCount.ToString();
        }
        else if(LinkedSlot != null && LinkedSlot.item == null)
        {
            QuickInven.GetInstance().ClearSlot(this);
            OnClear = 1;
        }

        if (OnClear == 1 && itemCount == 0)
        {
            image_count.SetActive(false);
            OnClear = 0;
        }


    }

    public void OnDrop(PointerEventData eventData)
    {
        if(DragSlot.instance.TargetSlot != null)
        {
            RegistSlot();
        }
    }


    private void RegistSlot()
    {
        if (DragSlot.instance.TargetSlot.item.itemType == Item.ItemType.Food)
        {
            QuickInven.GetInstance().SlotCheck();           // 퀵 슬롯에 등록할 아이템이 이미 등록되어있으면 삭제

            QuickInven.GetInstance().ClearSlot(this);       // 등록할 슬롯을 비움
            

            LinkedSlot = DragSlot.instance.TargetSlot;      // 가져올 슬롯정보를 저장함.
            item = LinkedSlot.item;
            ItemImage.sprite = LinkedSlot.item.ItemImage;
            SetColor(1);
            if (DragSlot.instance.TargetSlot.itemCount > 0)
            {
                image_count.SetActive(true);
                itemCount = DragSlot.instance.TargetSlot.itemCount;
            }
        }
        else
        {
            Debug.Log("소모품만 등록할 수 있습니다.");
        }
    }

    public void SetColor(float _alpha) // 아이템 아이콘 투명도 조절
    {
        Color color = ItemImage.color;
        color.a = _alpha;
        ItemImage.color = color;
    }
    
    void UpdateSlot()
    {
        item = LinkedSlot.item;
        itemCount = LinkedSlot.itemCount;
        ItemImage.sprite = LinkedSlot.ItemImage.sprite;
    }

    IEnumerator ResetLink()
    {
        yield return new WaitForSeconds(0.2f);
        InventoryMgr.GetInstance().SetChangedSlots(null, null);
    }

}

