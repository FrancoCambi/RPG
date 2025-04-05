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
        UpdateGoldText();
    }


    public void UpdateGoldText()
    {
        goldText.text = PlayerCurrency.Instance.MyGoldCurrency.Gold.ToString();
        silverText.text = PlayerCurrency.Instance.MyGoldCurrency.Silver.ToString();
        copperText.text = PlayerCurrency.Instance.MyGoldCurrency.Copper.ToString();
    }


}
