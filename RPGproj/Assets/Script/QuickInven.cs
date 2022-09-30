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
        UseSkill();
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

    void UseSkill()
    {
        if (SkillSlots[0] != null && Input.GetKeyDown(KeyCode.Q))
        {
            ActiveSkill(0);
        }
        if (SkillSlots[1] != null && Input.GetKeyDown(KeyCode.W))
        {
            ActiveSkill(1);
        }
        if (SkillSlots[2] != null && Input.GetKeyDown(KeyCode.E))
        {
            ActiveSkill(2);
        }
        if (SkillSlots[3] != null && Input.GetKeyDown(KeyCode.R))
        {
            ActiveSkill(3);
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

    void ActiveSkill(int _i)
    {
        if(SkillSlots[_i].item != null)
        {
            Item ThisItem = SkillSlots[_i].item;

            switch (SkillSlots[_i].item.skilltype)
            {
                case Item.SkillType.Attack:
                    {
                        if (GameMgr.GetInstance().PPFlipX)
                            Instantiate(ThisItem.itemPrefab, new Vector2((ThisItem.SizeX / 2) + 0.5f + Player.transform.position.x * -1, (ThisItem.SizeY / 2) + Player.transform.position.y), Quaternion.identity);
                        else
                            Instantiate(ThisItem.itemPrefab, new Vector2((ThisItem.SizeX / 2) + 0.5f + Player.transform.position.x, (ThisItem.SizeY / 2) + Player.transform.position.y), Quaternion.identity);
                    }
                    break;
                case Item.SkillType.Jump:
                    break;
                case Item.SkillType.Recovery:
                    break;
                default:
                    break;
            }
        }
    }

}
