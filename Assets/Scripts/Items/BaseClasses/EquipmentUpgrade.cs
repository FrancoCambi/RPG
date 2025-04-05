using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class EquipmentUpgrade
{
	private static readonly Dictionary<int, float> probs = new() 
	{
		{1, 100},
		{2, 100},
		{3, 90},
		{4, 80},
		{5, 60},
		{6, 40},
		{7, 20},
		{8, 5},
		{9, 1},
		{10, 0.2f},
		{11, 0}
	};
	
	private static readonly Dictionary<int, float> bonuses = new() 
	{
		{0, 1f},
		{1, 1.10f},
		{2, 1.15f},
		{3, 1.20f},
		{4, 1.30f},
		{5, 1.40f},
		{6, 1.50f},
		{7, 1.65f},
		{8, 1.90f},
		{9, 2.20f},
		{10, 3f}
	};
	
	private static readonly Dictionary<int, (GoldCurrency, Mat, Mat)> mats = new() 
	{
		{1, (new GoldCurrency(0, 10, 0), new Mat("Sea Powder", 10), new Mat("Red Gem", 1))},
		{2, (new GoldCurrency(0, 25, 0), new Mat("Sea Powder", 20), new Mat("Red Gem", 2))},
		{3, (new GoldCurrency(0, 50, 0), new Mat("Sea Powder", 40), new Mat("Red Gem", 3))},
		{4, (new GoldCurrency(1, 0, 0), new Mat("Fire Powder", 10), new Mat("Red Gem", 4))},
		{5, (new GoldCurrency(1, 50, 0), new Mat("Fire Powder", 20), new Mat("Red Gem", 5))},
		{6, (new GoldCurrency(2, 50, 0), new Mat("Fire Powder", 40), new Mat("Yellow Gem", 6))},
		{7, (new GoldCurrency(4, 50, 0), new Mat("Rock Powder", 10), new Mat("Yellow Gem", 7))},
		{8, (new GoldCurrency(6, 75, 0), new Mat("Rock Powder", 20), new Mat("Yellow Gem", 8))},
		{9, (new GoldCurrency(9, 0, 0), new Mat("Rock Powder", 40), new Mat("Yellow Gem", 9))},
		{10, (new GoldCurrency(12, 50, 0), new Mat("Heaven Powder", 25), new Mat("Yellow Gem", 10))}
	
	};
	
	public static Dictionary<int, float> Probs 
	{
		get 
		{
			return probs;
		}
	}
	
	public static Dictionary<int, float> Bonuses
	{
		get 
		{
			return bonuses;
		}
	}
	
	public static Dictionary<int, (GoldCurrency, Mat, Mat)> Mats 
	{
		get 
		{
			return mats;
		}
	}
	
	public static bool TryUpgrade(int original) 
	{
		float randomNumber = MathF.Round(UnityEngine.Random.Range(0f, 100f), 1);
		
		if (randomNumber < probs[original + 1]) 
		{
			string successText = LanguageManager.Instance.Translate("Upgrade successful! New grade: +", "¡Mejora exitosa! Nuevo grado: +");
			
			
			MessageFeedManager.instance.WriteMessage(successText + (original + 1), FontSize.NORMAL, MessageType.Warning, 2f);
			return true;
		}
		else 
		{
			string failText = LanguageManager.Instance.Translate("Upgrade failed.. better luck next time!", "La mejora falló.. ¡mejor suerte la próxima!");
			
			
			MessageFeedManager.instance.WriteMessage(failText, FontSize.NORMAL, MessageType.Warning, 2f);
			return false;
		}

	}
}
