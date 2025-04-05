using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Sword", menuName = "Items/Sword", order = 3)]
public class Sword : Item
{
	
	[Header("Sword")]

	[SerializeField]
	private EquipmentQuality equipmentQuality;
	
	[SerializeField]
	private int upgrade;
	
	[SerializeField]
	private Sword so;


	[Header("Damage")]
	
	[SerializeField]
	private int minDamage;

	[SerializeField]
	private int maxDamage;

	[SerializeField]
	private float speed;

	[SerializeField]
	private float knockBackForce;

	[Header("Stats")]
	
	[SerializeField]
	private int stamina;

	[SerializeField]
	private int strength;

	[SerializeField]
	private int agility;

	[SerializeField]
	private int critPlus;
	
	private int defaultMinDamage;
	private int defaultMaxDamage;
	private int defaultStamina;
	private int defaultStrength;
	private int defaultAgility;
	private int defaultCritPlus;
	
	private int qualityMinDamage;
	
	private int qualityMaxDamage;
	
	private bool defaultSetted = false;
	
	private bool qualityDmgSetted = false;

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

	public int MinDamage
	{
		get
		{
			return minDamage;
		}
	}

	public int MaxDamage
	{
		get
		{
			return maxDamage;
		}
	}

	public float Speed
	{
		get
		{
			return speed;
		}
	}

	public float KnockBackForce
	{
		get
		{
			return knockBackForce;
		}
	}

	public int Stamina
	{
		get
		{
			return stamina;
		}
	}

	public int Strength
	{
		get
		{
			return strength;
		}
	}

	public int Agility
	{
		get
		{
			return agility;
		}
	}

	public int CritPlus
	{
		get
		{
			return critPlus;
		}
	}

	public override string GetDescription()
	{
		string stats = String.Empty;
		string description = String.Empty;

		if (stamina > 0)
		{
			string stamText = LanguageManager.Instance.Translate("Stamina", "Aguante");
			
			stats += string.Format("\n+{0} {1}", stamina, stamText);
		}
		if (strength > 0)
		{
			string strText = LanguageManager.Instance.Translate("Strength", "Fuerza");
			
			stats += string.Format("\n+{0} {1}", strength, strText);
		}
		if (agility > 0)
		{
			string agiText = LanguageManager.Instance.Translate("Agility", "Agilidad");
			
			stats += string.Format("\n+{0} {1}", agility, agiText);
		}

		string damageText = LanguageManager.Instance.Translate("Damage", "De daño");
		string speedText = LanguageManager.Instance.Translate("Speed", "De velocidad");
		string damage = String.Format("\n{0} - {1} {2}\n{3} {4}", minDamage, maxDamage, damageText, speed, speedText);
		
		string equipCrit = String.Empty;
		if (this.critPlus > 0)
		{
			string effectText = LanguageManager.Instance.Translate("Equip: Increases critical strike chance by", 
				"Equipar: Incrementa probabilidad de golpe crítico");
			
			equipCrit = String.Format("\n<color=#00FF00FF>{0} {1}%</Color>", effectText, this.critPlus);
		}

		if (this.Description != String.Empty)
		{
			description = String.Format("\n<color=#808080FF>{0}</Color>", this.Description);
		}

		return base.GetDescription() + damage + stats + equipCrit + description;
	}

	public void Equip()
	{
		if (PlayerMeetsRequirements()) 
		{
			
			CharacterPanel.Instance.EquipSword(this);
		}

	}

	public void GetStats()
	{
		PlayerStats stats = PlayerStats.Instance;

		stats.Stamina += stamina;
		stats.Strength += strength;
		stats.Agility += agility;
		stats.CritChance += critPlus;
		stats.MinDamage += minDamage;
		stats.MaxDamage += maxDamage;


	}

	public void LoseStats()
	{
		PlayerStats stats = PlayerStats.Instance;

		stats.Stamina -= stamina;
		stats.Strength -= strength;
		stats.Agility -= agility;
		stats.CritChance -= critPlus;
		stats.MinDamage -= minDamage;
		stats.MaxDamage -= maxDamage;


	}
	
private void AdjustStats(float statsVal, float critVal, float dmgVal) 
	{
		stamina = (int)Math.Round(defaultStamina * statsVal);
		strength = (int)Math.Round(defaultStrength * statsVal);
		agility = (int)Math.Round(defaultAgility * statsVal);
		
		critPlus = (int)Math.Round(defaultCritPlus * critVal);
		
		minDamage = (int)(Math.Round(defaultMinDamage * dmgVal) * EquipmentUpgrade.Bonuses[upgrade]);
		qualityMinDamage = minDamage;
		maxDamage = (int)(Math.Round(defaultMaxDamage * dmgVal) * EquipmentUpgrade.Bonuses[upgrade]);
		qualityMaxDamage = maxDamage;
	}
	
	private void SetStatsByQuality()
	{
		if (!defaultSetted) 
		{
			defaultStamina = stamina;
			defaultStrength = strength;
			defaultAgility = agility;
			defaultCritPlus = critPlus;
			defaultMinDamage = minDamage;
			defaultMaxDamage = maxDamage;
			
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
				AdjustStats(0.85f, 0.5f, 0.85f);
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
	
		minDamage = (int)Math.Round(qualityMinDamage * EquipmentUpgrade.Bonuses[upgrade]);
		maxDamage = (int)Math.Round(qualityMinDamage * EquipmentUpgrade.Bonuses[upgrade]);
	
	}
	
	public void SetRandomQuality() 
	{
		
		int upDown = UnityEngine.Random.Range(0, 100);
		EquipmentQuality tmp;
		
		// UP
		if (upDown < EquipmentQualityClass.UpDownProbs[equipmentQuality].Item2) 
		{
			tmp = EquipmentQualityClass.GetRandomQuality(equipmentQuality, UpgradeDirection.Up);
		
		}
		// Down
		else 
		{
			tmp = EquipmentQualityClass.GetRandomQuality(equipmentQuality, UpgradeDirection.Down);
		}
		
		EquipmentQuality = tmp;
		GameController.Instance.RefreshTooltip(this);
		
	}
	
	public void TryUpgrade() 
	{
		if (upgrade < 10) 
		{
			if (!qualityDmgSetted) 
			{
				qualityMinDamage = minDamage;
				qualityMinDamage = maxDamage;
				
				qualityDmgSetted = true;
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
