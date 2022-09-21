using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    static public DragSlot instance;
    public Slot TargetSlot;
    [SerializeField]
    Image ItemImage;

    void Start()
    {
        instance = this;
    }

    public void DragSetImage(Image _img)
    {
        ItemImage.sprite = _img.sprite;
        SetColor(1);
    }
    
    public void SetColor(float _f)
    {
        Color color = ItemImage.color;
        color.a = _f;
        ItemImage.color = color;
    }
    public Sprite GetImage()
    {
        return ItemImage.sprite;
    }
}
