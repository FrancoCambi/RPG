using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class VendorItem
{
    [SerializeField]
    private Item item;

    [SerializeField]
    private GoldCurrency price;

    [SerializeField]
    private int quantity;

    [SerializeField]
    private bool unlimited;

    public Item Item
    {
        get
        {
            return item;
        }
    }

    public int Quantity
    {
        get
        {
            return quantity;
        }
        set
        {
            quantity = value;
        }
    }

    public bool Unlimited
    {
        get
        {
            return unlimited;
        }
    }

    public GoldCurrency Price
    {
        get
        {
            return price;
        }
    }
}
