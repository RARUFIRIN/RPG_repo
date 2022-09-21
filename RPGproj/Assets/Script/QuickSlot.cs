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
        if (item != null && LinkedSlot.item != item) // �� ���� ��� ���¿��� �κ��丮 ���ο��� �ڸ���ȭ�� �Ͼ�� �� ȣ��
        {
            IsChangeNow = true;
            if (InventoryMgr.GetInstance().GetChangedSlot(0) == LinkedSlot && IsChangeNow == true)
            {
                LinkedSlot = InventoryMgr.GetInstance().GetChangedSlot(1);
                UpdateSlot();
                IsChangeNow = false;
            }
            else if (InventoryMgr.GetInstance().GetChangedSlot(1) == LinkedSlot && IsChangeNow == true)
            {
                LinkedSlot = InventoryMgr.GetInstance().GetChangedSlot(0);
                UpdateSlot();
                IsChangeNow = false;
            }
        }

        if (IsChangeNow == false && LinkedSlot != null && LinkedSlot.itemCount >= 0)
        {
            itemCount = LinkedSlot.itemCount;
            text_count.text = itemCount.ToString();
            if (itemCount == 0)
            {
                QuickInven.GetInstance().ClearSlot(this);
            }
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
            QuickInven.GetInstance().SlotCheck();           // �� ���Կ� ����� �������� �̹� ��ϵǾ������� ����

            QuickInven.GetInstance().ClearSlot(this);       // ����� ������ ���
            

            LinkedSlot = DragSlot.instance.TargetSlot;      // ������ ���������� ������.
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
            Debug.Log("�Ҹ�ǰ�� ����� �� �ֽ��ϴ�.");
        }
    }

    public void SetColor(float _alpha) // ������ ������ ���� ����
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


}

