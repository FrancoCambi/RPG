using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiBarExp : MonoBehaviour
{
    private Image image;
    private Text text;

    void Start()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();

        int currentExp = PlayerStats.Instance.currentExp;
        int expToLevel = PlayerStats.Instance.expToLevel;

        image.fillAmount = (float)currentExp / expToLevel;

        int percentage;
        if (currentExp == 0)
        {
            percentage = 0;
        }
        else
        {

            percentage = (int)((float)currentExp / expToLevel * 100);
        }
        text.text = text.text = PlayerStats.Instance.CurrentExp.ToString() + "/" + PlayerStats.Instance.ExpToLevel.ToString() + " " + percentage.ToString() + " %";
    }

    private void OnEnable()
    {
        GameEventsManager.instance.playerEvents.onPlayerExpChange += FillExp;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.playerEvents.onPlayerExpChange -= FillExp;
    }

    private void FillExp()
    {
        int currentExp = PlayerStats.Instance.currentExp;
        int expToLevel = PlayerStats.Instance.expToLevel;

        image.fillAmount = (float)currentExp / expToLevel;

        int percentage;
        if (currentExp == 0)
        {
            percentage = 0;
        }
        else
        {

            percentage = (int)((float)currentExp / expToLevel * 100);
        }
        text.text = text.text = PlayerStats.Instance.CurrentExp.ToString() + "/" + PlayerStats.Instance.ExpToLevel.ToString() + " " + percentage.ToString() + " %";
    


    }
}
