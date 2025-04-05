using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCurrency : MonoBehaviour
{
	private static PlayerCurrency instance;
	
	public static PlayerCurrency Instance 
	{
		get
		{
			if (instance == null) 
			{
				instance = FindObjectOfType<PlayerCurrency>();
			}
			return instance;
		}
	}


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
			GoldCurrencyScript.instance.UpdateGoldText();
		}
	}
	public void GetMoney(GoldCurrency gold)
	{
		MyGoldCurrency += gold;
	}
	
	public void TakeMoney(GoldCurrency gold) 
	{
		MyGoldCurrency -= gold;
	}
}
