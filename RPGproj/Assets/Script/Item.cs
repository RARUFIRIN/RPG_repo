using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Item", menuName = "New Item/item")]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Equip,
        Food,
        Ingredient,
    }

    public enum EquipType
    {
        Weapon,
        Armor,
        Helmet,
        Boot,
        None,
    }

    public EquipType equiptype;
    public ItemType itemType;
    public string itemName;
    public int itemidx;
    public Sprite ItemImage;
    public GameObject itemPrefab;

    // 소모품용
    public int RecoveryHP;
    public int RecoveryMP;

    // 장비용
    public int RequiredLevel;
    public int AttackDamage;
    public int Defense;
}



