using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public enum ArmorType { Head, Shoulders, Chest, Hands, Neck, Ring, Wrist, Waist, Legs, Feet, Sword  }

[CreateAssetMenu(fileName = "Armor", menuName = "Items/Armor", order = 2)]
public class Armor : Item
{
	[Header("Armor")]
	
	[SerializeField]
	private ArmorType armorType;
	
	[SerializeField]
	private EquipmentQuality equipmentQuality;
	
	[SerializeField]
	private int upgrade;

	[SerializeField]
	private Armor so;
	
	[Header("Stats")]

	[SerializeField]
	private int stamina;

	[SerializeField]
	private int strength;

	[SerializeField]
	private int agility;

	[SerializeField]
	private int critPlus;

	[SerializeField]
	private int armor;
	
	private int defaultStamina;
	private int defaultStrength;
	private int defaultAgility;
	private int defaultCritPlus;
	private int defaultArmor;
	
	private int qualityArmor;
	
	private bool defaultSetted = false;
	
	private bool qualityArmorSetted = false;

	public int Stamina 
	{
		get 
		{
			return stamina;
		}
	}

	public ArmorType ArmorType
	{
		get
		{
			return armorType;
		}
	}
	
	public EquipmentQuality EquipmentQuality 
	{
		get 
		{
			return equipmentQuality;
		}
		set 
		{
			equipmentQuality = value;
			SetStatsByQuality();
		}
	}
	
	public int Upgrade 
	{
		get 
		{
			return upgrade;
		}
		set 
		{
			upgrade = value;
			SetStatsByUpgrade();
		}		
	}
	

	public override string GetDescription()
	{
		string stats = string.Empty;

		if (stamina > 0)
		{
			stats += string.Format("\n +{0} Stamina", stamina);
		}
		if (strength > 0)
		{
			stats += string.Format("\n +{0} Strength", strength);
		}
		if (agility > 0)
		{
			stats += string.Format("\n +{0} Agility", agility);
		}
		if (armor > 0)
		{
			stats += string.Format("\n{0} Armor", armor);
		}

		string equipCrit = string.Empty;

		if (this.critPlus > 0)
		{
			equipCrit = string.Format("\n<color=#00FF00FF>Equip: Increases critical strike chance by {0}%</Color>", this.critPlus);
		}

		string description = string.Empty;

		if (this.description != string.Empty)
		{
			description = string.Format("\n<color=#808080FF>{0}</Color>", this.description);
		}

		return base.GetDescription() + stats + equipCrit + description;
	}

	public void Equip()
	{
		if (PlayerMeetsRequirements()) 
		{
			
			CharacterPanel.Instance.EquipArmor(this);
		}

	}

	public void GetStats()
	{
		PlayerStats stats = PlayerStats.Instance;

		stats.Stamina += stamina;
		stats.Strength += strength;
		stats.Agility += agility;
		stats.CritChance += critPlus;
		stats.Armor += armor;
		stats.ArmorFromGear += armor;

		CharacterPanel.Instance.UpdateStatsText();

	}

	public void LoseStats()
	{
		PlayerStats stats = PlayerStats.Instance;

		stats.Stamina -= stamina;
		stats.Strength -= strength;
		stats.Agility -= agility;
		stats.CritChance -= critPlus;
		stats.Armor -= armor;
		stats.ArmorFromGear -= armor;

		CharacterPanel.Instance.UpdateStatsText();

	}
	
	private void AdjustStats(float statsVal, float critVal, float armorVal) 
	{
		stamina = (int)Math.Round(defaultStamina * statsVal);
		strength = (int)Math.Round(defaultStrength * statsVal);
		agility = (int)Math.Round(defaultAgility * statsVal);
		
		critPlus = (int)Math.Round(defaultCritPlus * critVal);
		armor = (int)(Math.Round(defaultArmor * armorVal) * EquipmentUpgrade.Bonuses[upgrade]);
	}
	
	private void SetStatsByQuality()
	{
		if (!defaultSetted) 
		{
			defaultStamina = stamina;
			defaultStrength = strength;
			defaultAgility = agility;
			defaultCritPlus = critPlus;
			defaultArmor = armor;			
			defaultSetted = true;
		}
		
		
		switch (equipmentQuality)
		{
			case EquipmentQuality.None:
				break;
			case EquipmentQuality.Damaged:
				AdjustStats(0.5f, 0, 0.5f);
				break;
			case EquipmentQuality.Low:
				AdjustStats(0.85f, 0.5f, 0.80f);
				break;
			case EquipmentQuality.Common:
				AdjustStats(1f, 1f, 1f);
				break;
			case EquipmentQuality.Useful:
				AdjustStats(1.10f, 1f, 1.05f);
				break;
			case EquipmentQuality.Good:
				AdjustStats(1.20f, 1f, 1.10f);
				break;
			case EquipmentQuality.Excellent:
				AdjustStats(1.30f, 1f, 1.15f);
				break;
			case EquipmentQuality.Ancient:
				AdjustStats(1.40f, 1.5f, 1.20f);
				break;
			case EquipmentQuality.Mysterious:
				AdjustStats(1.50f, 2f, 1.25f);
				if (critPlus == 0) 
				{
					critPlus += 1;
				}
				break;
		}
	}
	
	
	private void SetStatsByUpgrade() 
	{
	
		armor = (int)Math.Round(qualityArmor * EquipmentUpgrade.Bonuses[upgrade]);
	
	}
	public void SetRandomQuality() 
	{
		
		int upDown = UnityEngine.Random.Range(0, 100);
		
		// UP
		if (upDown < EquipmentQualityClass.UpDownProbs[equipmentQuality].Item2) 
		{
			EquipmentQuality = EquipmentQualityClass.GetRandomQuality(equipmentQuality, UpgradeDirection.Up);
		
		}
		// Down
		else 
		{
			EquipmentQuality = EquipmentQualityClass.GetRandomQuality(equipmentQuality, UpgradeDirection.Down);
		}
		
		GameController.Instance.RefreshTooltip(this);
		
	}
	
	public void TryUpgrade() 
	{
		if (upgrade < 10) 
		{
			
			if (!qualityArmorSetted) 
			{
				qualityArmor = armor;

				qualityArmorSetted = true;
			}
			
			if (EquipmentUpgrade.TryUpgrade(upgrade)) 
			{
				Upgrade++;
				GameController.Instance.RefreshTooltip(this);
			}
		}
		else 
		{
			MessageFeedManager.instance.WriteMessage("You have reached the maximum upgrade.", FontSize.NORMAL, MessageType.Warning, 2f);
		}
	}

}
