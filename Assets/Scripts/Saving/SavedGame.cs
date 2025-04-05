using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SavedGame : MonoBehaviour
{
	[SerializeField]
	private Text dateTime;
	
	[SerializeField]
	private Image rage;
	
	[SerializeField]
	private Image health;
	
	[SerializeField]
	private Image xp;
	
	[SerializeField]
	private Text rageText;
	
	[SerializeField]
	private Text healthText;
	
	[SerializeField]
	private Text xpText;
	
	[SerializeField]
	private TextMeshProUGUI levelText;
	
	[SerializeField]
	private GameObject visuals;
	
	[SerializeField]
	private int index;
	
	public int Index 
	{
		get 
		{
			return index;
		}
	}
	
	private void Awake() 
	{
		visuals.SetActive(false);
	}
	
	public void ShowInfo(SaveData savedData) 
	{
		visuals.SetActive(true);
		
		dateTime.text = LanguageManager.Instance.Translate("Date: " + savedData.DateTime.ToString("dd/MM/yyy") + " - Time: " + savedData.DateTime.ToString("H:mm"),
			"Fecha: " + savedData.DateTime.ToString("dd/MM/yyy") + " - Hora: " + savedData.DateTime.ToString("H:mm"));
		health.fillAmount = (float)savedData.MyPlayerData.CurrentHealth / savedData.MyPlayerData.MaxHealth;
		healthText.text = savedData.MyPlayerData.CurrentHealth + "/" + savedData.MyPlayerData.MaxHealth;
		
		xp.fillAmount = (float)savedData.MyPlayerData.CurrentExp / savedData.MyPlayerData.ExpToLevel;
		xpText.text = savedData.MyPlayerData.CurrentExp + "/" + savedData.MyPlayerData.ExpToLevel;
		
		levelText.text = savedData.MyPlayerData.Level.ToString();

		
		
	}
	
	public void HideVisuals() 
	{
		visuals.SetActive(false);
	}
}
