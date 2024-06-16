using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UiBarHealth : MonoBehaviour
{
    private Image image;
    private Text text;

    void Start()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        text.text = PlayerStats.Instance.CurrentHealth.ToString() + "/" + PlayerStats.Instance.maxHealth.ToString();
    }

    private void OnEnable()
    {
        GameEventsManager.instance.playerEvents.onPlayerHealthChange += FillHeath;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.playerEvents.onPlayerHealthChange -= FillHeath;
    }

    private void FillHeath()
    {
        int currentHealth = PlayerStats.Instance.CurrentHealth;
        int maxHealth = PlayerStats.Instance.maxHealth;

        image.fillAmount = (float)currentHealth / maxHealth;

        text.text = currentHealth.ToString() + " / " + maxHealth.ToString();
    }

}
