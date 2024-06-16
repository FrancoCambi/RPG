using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
    public static PlayerCurrency instance;


    [SerializeField]
    private GoldCurrency goldCurrency;

    public GoldCurrency MyGoldCurrency
    {
        get
        {
            return goldCurrency;
        }
        set
        {
            goldCurrency = value;
            GoldCurrencyScript.instance.UpdateGold();
        }
    }

    private void Start()
    {
        instance = this;
    }

    public void GetMoney(int g, int s, int c)
    {
        goldCurrency += new GoldCurrency(g, s, c);
    }
}
