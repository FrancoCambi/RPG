using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class Item : ScriptableObject, IMoveable, IDescribable
{

    [SerializeField]
    private Sprite icon;

    [SerializeField]
    private string itemName;

    [SerializeField]
    private Quality quality;

    [SerializeField]
    private int stackSize;

    [SerializeField]
    private int itemLevel;

    [SerializeField]
    private int requiredLevel;

    public string description;


    private Slot slot;

    public Sprite Icon
    {
        get
        {
            return icon;
        }
    }

    public int StackSize
    {
        get
        {
            return stackSize;
        }
    }

    public Slot Slot
    {
        get
        {
            return slot;
        }
        set
        {
            slot = value;
        }
    }

    public Quality Quality
    {
        get
        {
            return quality;
        }

    }

    public string ItemName
    {
        get
        {
            return itemName;
        }
    }

    public virtual string GetDescription()
    {
        
        return string.Format("<color={0}>{1}</color>\n<color=#FFD400>Item Level {2}</color>\nRequires Level {3}", QualityColor.Colors[quality], itemName, itemLevel, requiredLevel);
    }

    public void Remove()
    {
        if (Slot != null)
        {
            Slot.RemoveItem(this);
        }
    }
}
