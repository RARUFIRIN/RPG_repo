using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDB : MonoBehaviour
{
    private ItemDB() { }
    private static ItemDB instance = null;
    public static ItemDB GetInstance()
    {
        if (instance == null)
        {
            Debug.LogError("오브젝트를 찾을 수 없습니다.");
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
    public Item Apple;
    public Item Beer;

    public Item WoodenSword;

    public Item IronSword;
    public Item LeatherArmor;
    public Item LeatherBoots;
    public Item LeatherHelmet;

    public Item SilverSword;
    public Item IronArmor;
    public Item IronBoots;
    public Item IronHelmet;

    public Item GoldenSword;


    Dictionary<int, Item> D_Items = new Dictionary<int, Item>();

    private void Start()
    {
        RegistItem();
    }

    void RegistItem()
    {
        D_Items.Add(101, Apple);
        D_Items.Add(102, Beer);
        D_Items.Add(201, WoodenSword);
        D_Items.Add(202, IronSword);
        D_Items.Add(203, SilverSword);
        D_Items.Add(204, GoldenSword);
        D_Items.Add(301, LeatherArmor);
        D_Items.Add(302, LeatherBoots);
        D_Items.Add(303, LeatherHelmet);
        D_Items.Add(311, IronArmor);
        D_Items.Add(312, IronBoots);
        D_Items.Add(313, IronHelmet);
    }

    public enum ItemName
    {
        Apple = 101,
        Beer = 102,
        WoodenSword = 201,
        IronSword = 202,
        SilverSword = 203,
        GoldenSword = 204,
        LeatherArmor = 301,
        LeatherBoots = 302,
        LeatherHelmet = 303,
        IronArmor = 311,
        IronBoots = 312,
        IronHelmet = 313,
    }

    public Item GetItem(int idx)
    {
        return D_Items[idx];
    }
    public Item GetItem(ItemName _name)
    {
        return D_Items[(int)_name];
    }
}
