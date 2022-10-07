using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class QuickInven : MonoBehaviour
{
    private QuickInven() { }
    private static QuickInven instance = null;
    public static QuickInven GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("오브젝트를 찾을 수 없습니다.");
        }
        return instance;
    }
    [SerializeField]
    GameObject QuickSlotsParent;
    [SerializeField]
    GameObject SkillSlotsParent;
    [SerializeField]
    GameObject Player;

    QuickSlot[] QuickSlots;
    QuickSlot[] SkillSlots;
    private void Awake()
    {
        instance = this;
        QuickSlots = QuickSlotsParent.GetComponentsInChildren<QuickSlot>();
        SkillSlots = SkillSlotsParent.GetComponentsInChildren<QuickSlot>();
    }
    private void Update()
    {
        UseItem();
    }
    void UseItem()
    {
        if (QuickSlots[0].LinkedSlot != null && Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpendItem(0);
        }
        if (QuickSlots[1].LinkedSlot != null && Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpendItem(1);
        }
        if (QuickSlots[2].LinkedSlot != null && Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpendItem(2);
        }
        if (QuickSlots[3].LinkedSlot != null && Input.GetKeyDown(KeyCode.Alpha4))
        {
            SpendItem(3);
        }
    }

    public void SlotCheck()
    {
        for (int i = 0; i < QuickSlots.Length; i++)
        {
            if (QuickSlots[i].item != null && QuickSlots[i].item == DragSlot.instance.TargetSlot.item)
            {
                ClearSlot(QuickSlots[i]);
            }
        }

        for (int i = 0; i < QuickSlots.Length; i++)
        {
            if (SkillSlots[i].item != null && SkillSlots[i].item == DragSlot.instance.TargetSlot.item)
            {
                ClearSlot(SkillSlots[i]);
            }
        }
    }

    public void ClearSlot(QuickSlot _s)
    {
        _s.item = null;
        _s.itemCount = 0;
        _s.ItemImage.sprite = null;
        _s.LinkedSlot = null;
        _s.OnClear = 1;
        _s.SetColor(0);
    }

    void SpendItem(int _i)
    {
        QuickSlots[_i].LinkedSlot.UpdateSlotCount(-1);
        InventoryMgr.GetInstance().SpendPotion(QuickSlots[_i].item);
    }


    public Item returnSkill(int i)
    {
        if (SkillSlots[i].item != null)
            return SkillSlots[i].item;
        else
            return null;
    }
}
