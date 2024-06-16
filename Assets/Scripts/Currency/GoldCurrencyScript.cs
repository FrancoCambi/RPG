using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoldCurrencyScript : MonoBehaviour
{
    public static GoldCurrencyScript instance;


    [SerializeField]
    private Text goldText;

    [SerializeField]
    private Text silverText;

    [SerializeField]
    private Text copperText;

    // Start is called before the first frame update
    private void Start()
    {
        instance = this;
        UpdateGold();
    }


    public void UpdateGold()
    {
        goldText.text = PlayerCurrency.instance.MyGoldCurrency.Gold.ToString();
        silverText.text = PlayerCurrency.instance.MyGoldCurrency.Silver.ToString();
        copperText.text = PlayerCurrency.instance.MyGoldCurrency.Copper.ToString();
    }


}
