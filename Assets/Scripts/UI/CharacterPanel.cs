using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
	private static CharacterPanel instance;
	
	public static CharacterPanel Instance 
	{
		get 
		{
			if (instance == null) 
			{
				instance = FindObjectOfType<CharacterPanel>();
			}
			return instance;
		}
	}

	[SerializeField]
	private CanvasGroup canvasGroup;

	[SerializeField]
	private CanvasGroup statsCanvasGroup;

	[SerializeField]
	private CharButton head, shoulders, chest, hands, neck, ring1, ring2, wrist, waist, legs, feet;

	[SerializeField]
	private CharSwordButton swordButton;

	[SerializeField]
	private Text statsText;

	public CharButton MySelectedButton { get; set; }
	public CharSwordButton MySelectedSwordButton { get; set; }

	public bool IsOpen
	{
		get
		{
			return canvasGroup.alpha > 0;
		}
	}

	public CharSwordButton SwordButton
	{
		get
		{
			return swordButton;
		}
	}
	

	private void Start()
	{
		UpdateStatsText();
	}

	public void OpenClose()
	{
		if (canvasGroup.alpha <= 0)
		{
			canvasGroup.alpha = 1;
			canvasGroup.blocksRaycasts = true;
			statsCanvasGroup.alpha = 1;
			statsCanvasGroup.blocksRaycasts = true;
		}
		else
		{
			canvasGroup.alpha = 0;
			canvasGroup.blocksRaycasts = false;
			statsCanvasGroup.alpha = 0;
			statsCanvasGroup.blocksRaycasts = false;
		}

	}

	public void Close()
	{
		canvasGroup.alpha = 0;
		canvasGroup.blocksRaycasts = false;
		statsCanvasGroup.alpha = 0;
		statsCanvasGroup.blocksRaycasts = false;
	}

	public void EquipArmor(Armor armor)
	{
		switch (armor.ArmorType)
		{
			case ArmorType.Head:
				head.EquipArmor(armor);
				break;
			case ArmorType.Shoulders:
				shoulders.EquipArmor(armor);
				break;
			case ArmorType.Chest:
				chest.EquipArmor(armor);
				break;
			case ArmorType.Hands:
				hands.EquipArmor(armor);
				break;
			case ArmorType.Neck:
				neck.EquipArmor(armor);
				break;
			case ArmorType.Ring:
				if (ring1.EquippedArmor == null)
				{
					ring1.EquipArmor(armor);
				}
				else
				{
					ring2.EquipArmor(armor);
				}
				break;
			case ArmorType.Wrist:
				wrist.EquipArmor(armor);
				break;
			case ArmorType.Waist:
				waist.EquipArmor(armor);
				break;
			case ArmorType.Legs:
				legs.EquipArmor(armor);
				break;
			case ArmorType.Feet:
				feet.EquipArmor(armor);
				break;

		}
		UpdateStatsText();
	}

	public void EquipSword(Sword sword)
	{
		swordButton.EquipSword(sword);
		UpdateStatsText();
	}

	public void UpdateStatsText()
	{
		PlayerStats stats = PlayerStats.Instance;

		int minDamage;
		int maxDamage;

		if (swordButton.EquippedSword != null)
		{
			minDamage = stats.MinDamage;
			maxDamage = stats.MaxDamage;
		}
		else
		{
			minDamage = 0;
			maxDamage = 0;
		}

		if (LanguageManager.Instance.CurrentLanguage == Language.English) 
		{
			
			statsText.text = String.Format("Level: {0}\nDamage: {1} - {2}\nStamina: {3}\nStrength: {4}\nAgility: {5}\nCrit: {6:0.00}%\nArmor: {7}\n", stats.Level, minDamage, maxDamage, stats.Stamina, stats.Strength, stats.Agility, stats.CritChance, stats.Armor);
		}
		else 
		{
			statsText.text = String.Format("Nivel: {0}\nDaño: {1} - {2}\nAguante: {3}\nFuerza: {4}\nAgilidad: {5}\nCrítico: {6:0.00}%\nArmadura: {7}\n", stats.Level, minDamage, maxDamage, stats.Stamina, stats.Strength, stats.Agility, stats.CritChance, stats.Armor);
		}

	}
	
	public void SetDefault() 
	{
		Armor defaultFoot = (Armor)Instantiate(Array.Find(SaveManager.Instance.Items, x => x.ItemNameEnglish == "Apprentice Boots"));
		Armor defaultChest = (Armor)Instantiate(Array.Find(SaveManager.Instance.Items, x => x.ItemNameEnglish == "Apprentice Chest"));
		Armor defaultHead = (Armor)Instantiate(Array.Find(SaveManager.Instance.Items, x => x.ItemNameEnglish == "Apprentice Hood"));
		Armor defaultPants = (Armor)Instantiate(Array.Find(SaveManager.Instance.Items, x => x.ItemNameEnglish == "Apprentice Pants"));
		Armor defaultWrists = (Armor)Instantiate(Array.Find(SaveManager.Instance.Items, x => x.ItemNameEnglish == "Apprentice Wristband"));
		Armor defaultGloves = (Armor)Instantiate(Array.Find(SaveManager.Instance.Items, x => x.ItemNameEnglish == "Apprentice Gloves"));
		
		Sword defaultSword = (Sword)Instantiate(Array.Find(SaveManager.Instance.Items, x => x.ItemNameEnglish == "Apprentice Sword"));
		
		EquipArmor(defaultFoot);
		EquipArmor(defaultChest);
		EquipArmor(defaultHead);
		EquipArmor(defaultPants);
		EquipArmor(defaultWrists);
		EquipArmor(defaultGloves);
		
		EquipSword(defaultSword);
	}



}
