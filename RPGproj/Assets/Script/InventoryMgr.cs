using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMgr : MonoBehaviour
{
    // �κ��丮 �̱��� //
    private InventoryMgr() { } 
    private static InventoryMgr instance = null;
    public static InventoryMgr GetInstance()
    {
        if (instance == null)
        {

            Debug.LogError("������Ʈ�� ã�� �� �����ϴ�.");
        }
        return instance;
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (this != instance)
            {
                Destroy(gameObject);
            }
        }
    }

    bool B_inventoryActive = false;

    [SerializeField]
    GameObject InventoryUI; // �κ��丮
    [SerializeField]
    GameObject SlotParent;  // �׸���

    public Slot[] slots; // ���� �迭

    // bool InventoryIsFull;

    Slot PreSlot, CurSlot;
    // ������ ����ȭ �� ����

    [SerializeField]
    GameObject EquipSlotParent; // ��񽽷�
    [SerializeField]
    EquipSlot_Weapon EquipSlot_Weapon;
    [SerializeField]
    EquipSlot_Helmet EquipSlot_Helmet;
    [SerializeField]
    EquipSlot_Armor EquipSlot_Armor;
    [SerializeField]
    EquipSlot_Boot EquipSlot_Boot;
    bool B_EquipSlotActive = false;

    [SerializeField]
    GameObject SkillUIParent; // ��ųUI
    bool B_SkillUIActive = false;


    private void Start()
    {
        PreSlot = GetComponent<Slot>();
        CurSlot = GetComponent<Slot>();
        slots = SlotParent.GetComponentsInChildren<Slot>();
    }

    private void Update()
    {
        OpenInventory();
        OpenEquipSlot();
        OpenSkillUI();
    }

    public void Equip(Item.EquipType _Type, Slot _slot)
    {
        switch (_Type)
        {
            case Item.EquipType.Weapon:
                {
                    EquipSlot_Weapon.RegistSlot(_slot);
                }
                break;
            case Item.EquipType.Armor:
                {
                        EquipSlot_Armor.RegistSlot(_slot);
                }
                break;
            case Item.EquipType.Helmet:
                {
                        EquipSlot_Helmet.RegistSlot(_slot);
                }
                break;
            case Item.EquipType.Boot:
                {
                    EquipSlot_Boot.RegistSlot(_slot);
                }
                break;
            default:
                break;
        }
    }
    private void OpenInventory()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            B_inventoryActive = !B_inventoryActive;
            InventoryUI.SetActive(B_inventoryActive);
        }
    }

    private void OpenEquipSlot()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            B_EquipSlotActive = !B_EquipSlotActive;
            EquipSlotParent.SetActive(B_EquipSlotActive);
        }
    }

    private void OpenSkillUI()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            B_SkillUIActive = !B_SkillUIActive;
            SkillUIParent.SetActive(B_SkillUIActive);
        }
    }

    public void GainItem(Item _item, int _count = 1) // ������ ȹ��
    {
        if(Item.ItemType.Equip != _item.itemType)
        { 
            for(int i = 0; i < slots.Length; i++)
            {
                if (slots[i].item != null) // ����ó��
                {
                    if (slots[i].item.itemName == _item.itemName) // ���� �������� ���� �� (��� ����) ������ �߰����ش�.
                    {
                        slots[i].UpdateSlotCount(_count);
                        return;
                    }
                }
            }
        }

        for(int i = 0; i < slots.Length; i++) // ���� �������� ���� �� �� ĭ�� ������ �߰�
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(_item, _count);
                return;
            }
        }

        // �κ��丮 ���� ��. (�����ؾ���)
        // InventoryIsFull = true;
    }

    public void SetChangedSlots(Slot _s1, Slot _s2)
    {
        PreSlot = _s1;
        CurSlot = _s2;
    }

    public Slot GetChangedSlot(int i)// 0�� PreSlot�� 1�� CurSlot �� ��ȯ
    {
        if (i == 0)
        {
            return PreSlot;
        }
        else if(i == 1)
        {
            return CurSlot;
        }
        else
        {
            return null;
        }
    }

    public bool SearchItem(int idx, int count)
    {
        for(int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                if (slots[i].item.itemidx == idx && slots[i].itemCount >= count)
                {
                    return true;
                }
            }
        }

        return false;
    }

    public void SpendItem(int idx, int count)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].item != null)
            {
                if (slots[i].item.itemidx == idx && slots[i].itemCount >= count)
                {
                    slots[i].UpdateSlotCount(-count);
                    return;
                }
            }
        }
    }

    public void SpendPotion(Item item)
    {
        Debug.Log(item.itemName + "�� ����߽��ϴ�.");
        if (item.RecoveryHP == 0)
        {
            if (GameMgr.GetInstance().PMP + item.RecoveryMP > GameMgr.GetInstance().PMaxMP)
                GameMgr.GetInstance().PMP = GameMgr.GetInstance().PMaxMP;
            else
                GameMgr.GetInstance().PMP += item.RecoveryMP;
            Debug.Log("������" + item.RecoveryMP + "��ŭ ȸ���߽��ϴ�.");
        }
        else if (item.RecoveryMP == 0)
        {
            if (GameMgr.GetInstance().PHP + item.RecoveryHP > GameMgr.GetInstance().PMaxHP)
                GameMgr.GetInstance().PHP = GameMgr.GetInstance().PMaxHP;
            else
                GameMgr.GetInstance().PHP += item.RecoveryHP;
            Debug.Log("ü����" + item.RecoveryHP + "��ŭ ȸ���߽��ϴ�.");
        }
    }
}
