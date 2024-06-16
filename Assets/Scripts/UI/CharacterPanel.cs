using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterPanel : MonoBehaviour
{
	public static CharacterPanel instance;

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
		instance = this;
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

		statsText.text = String.Format("Level: {0}\nDamage: {1} - {2}\nStamina: {3}\nStrength: {4}\nAgility: {5}\nCrit: {6:0.00}%\nArmor: {7}\n", stats.Level, minDamage, maxDamage, stats.Stamina, stats.Strength, stats.Agility, stats.CritChance, stats.Armor);

	}



}
