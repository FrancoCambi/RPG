using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FrameManager : MonoBehaviour
{
	private static FrameManager instance;
	
	public static FrameManager Instance 
	{
		get 
		{
			if (instance == null) 
			{
				instance = FindObjectOfType<FrameManager>();
			}
			return instance;
		}
	}
	
	[SerializeField]
	private Image xpBar;
	
	[SerializeField]
	private Image healthBar;
	
	[SerializeField]
	private Text xpText;
	
	[SerializeField]
	private Text healthText;
	
	[SerializeField]
	private TextMeshProUGUI levelText;
	public void FillExp()
	{
		int currentExp = PlayerStats.Instance.CurrentExp;
		int expToLevel = PlayerStats.Instance.ExpToLevel;

		xpBar.fillAmount = (float)currentExp / expToLevel;

		int percentage;
		if (currentExp == 0)
		{
			percentage = 0;
		}
		else
		{

			percentage = (int)((float)currentExp / expToLevel * 100);
		}
		xpText.text = currentExp.ToString() + "/" + expToLevel.ToString() + " " +  "(" + percentage.ToString() + " %" + ")";
	


	}
	
	public void FillHealth() 
	{
		healthBar.fillAmount = (float)PlayerStats.Instance.CurrentHealth / PlayerStats.Instance.MaxHealth;
		healthText.text = PlayerStats.Instance.CurrentHealth + "/" + PlayerStats.Instance.MaxHealth;
	}
	
	public void UpdateLevelUiText()
	{
		levelText.text = PlayerStats.Instance.Level.ToString();
	}
}
