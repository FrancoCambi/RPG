using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using System;

public class PlayerStats : MonoBehaviour
{
	private static PlayerStats instance;
	
	public static PlayerStats Instance 
	{
		get 
		{
			if (instance == null) 
			{
				instance = FindObjectOfType<PlayerStats>();
			}
			return instance;
		}
	}

	[Header("Stats")]

	[SerializeField]
	private int armor;

	[SerializeField]
	private int stamina;

	[SerializeField]
	private int strength;

	[SerializeField]
	private int agility;

	[SerializeField]
	private float critChance;

	[Header("Real")]

	public int maxHealth;

	[SerializeField]
	private int currentHealth;

	[SerializeField]
	private int level;

	public int currentExp;
	public int expToLevel;

	private int baseArmor;
	private int armorFromGear = 0;
	private float baseHealthGrowthMultiplier = 1.02f;
	private int critCap = 45;
	private float damageReductionByArmorCap = 75;
	private int minDamage;
	private int maxDamage;
	private int attackPower;
	
	private Camera mainCamera;
	
	private Canvas canvas;

	public int Armor
	{
		
		get
		{
			return baseArmor + 2 * agility + armorFromGear;
		}
		set
		{
			armor = value;
		}
	}

	public int Stamina 
	{
		get
		{
			return stamina;
		}
		set
		{
			MaxHealth += (value - Stamina) * 10;
			stamina = value;
			FrameManager.Instance.FillHealth();
		}
	}

	public int Strength
	{
		get
		{
			return strength;
		}
		set
		{
			strength = value;
		}
	}

	public int Agility
	{
		get
		{
			return agility;
		}
		set
		{
			critChance += (float)(value - agility) / 12;
			agility = value;
		}
	}

	public float CritChance
	{
		get
		{
			return critChance;
		}

		set
		{
			critChance = value;
		}
	}

	public int ArmorFromGear
	{
		get
		{
			return armorFromGear;
		}
		set
		{
			armorFromGear = value;
		}
	}

	public int CurrentHealth
	{
		get
		{
			return currentHealth;
		}
		set
		{
			if (value > maxHealth)
			{
				currentHealth = maxHealth;
			}
			else
			{

				currentHealth = value;
			}
			FrameManager.Instance.FillHealth();
		}
	}
	
	public int MaxHealth 
	{
		get 
		{
			return CalculateBaseHealth() + (Stamina * 10);
		}
		set 
		{
			maxHealth = value;
		}
	}

	public int Level
	{
		get
		{
			return level;
		}
		set 
		{
			level = value;
			FrameManager.Instance.UpdateLevelUiText();
		}
	}
	
	public int CurrentExp 
	{
		get 
		{
			return currentExp;
		}
		set 
		{
			currentExp = value;
			FrameManager.Instance.FillExp();
		}
	}

	public int ExpToLevel 
	{
		get 
		{
			return CalculateExpToLevel();
		}
	
	}

	public int MinDamage
	{
		get
		{ 
			return CharacterPanel.Instance.SwordButton.EquippedSword.MinDamage + AttackPower;
		}
		set
		{
			minDamage = value;
		}
	}

	public int MaxDamage
	{
		get
		{
			return CharacterPanel.Instance.SwordButton.EquippedSword.MaxDamage + AttackPower;
		}

		set
		{
			maxDamage = value;
		}
	}

	public int AttackPower
	{
		get
		{
			return Strength / 2;
		}
	}

	private void Start()
	{
		canvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
		mainCamera = Camera.main;
	}

	public void SetInstance(int armor, int armorFromGear, int stamina, int strength, int agility, float critChance, int maxHealth, int currentHealth, int level, int currentExp, int expToLevel)
	{
		Instance.Armor = armor;
		Instance.ArmorFromGear = armorFromGear;
		Instance.Stamina = stamina;
		Instance.Strength = strength;
		Instance.Agility = agility;
		Instance.CritChance = critChance;
		Instance.MaxHealth = maxHealth;
		Instance.CurrentHealth = currentHealth;
		Instance.Level = level;
		Instance.CurrentExp = currentExp;
		Instance.expToLevel = expToLevel;
	}
	
	public void SetDefault() 
	{
		Armor = 60;
		Stamina = 5;
		Strength = 5;
		Agility = 5;
		CritChance = 5f;
		CurrentHealth = MaxHealth;
		Level = 1;
		CurrentExp = 0;
		expToLevel = CalculateExpToLevel();
	}

	public void RecieveExp(int exp)
	{
		CurrentExp += exp;
		CheckForLevelUp();

		FloatingTextManager.Instance.CreateText(mainCamera.WorldToScreenPoint(gameObject.transform.position), canvas, 
					exp.ToString() + " Xp", FloatingTextType.XP);
	}
	
	public void RecieveDamage(int damage, bool crit) 
	{
		CurrentHealth -= damage;

		string dmgString = crit ? "Crit! " + damage.ToString() : damage.ToString();
		
		FloatingTextManager.Instance.CreateText(mainCamera.WorldToScreenPoint(gameObject.transform.position), canvas, 
					dmgString, FloatingTextType.DAMAGE);
	}
	
	public void GetHealth(int health) 
	{
		CurrentHealth += health;
		
		FloatingTextManager.Instance.CreateText(mainCamera.WorldToScreenPoint(gameObject.transform.position), canvas, 
					health.ToString(), FloatingTextType.HEAL);
	}

	private void CheckForLevelUp()
	{
		if (CurrentExp >= ExpToLevel)
		{
			CurrentExp -= ExpToLevel; 
			LevelUp();
			CheckForLevelUp();
		}
	}

	private void LevelUp()
	{
		Level++;
		PlusOneStats();
		
		MaxHealth = CalculateMaxHealth();
		CurrentHealth = maxHealth;
		
		CharacterPanel.Instance.UpdateStatsText();
		FrameManager.Instance.FillExp();
		
		string dingText = LanguageManager.Instance.Translate("Your level has increased!", "¡Tu nivel ha aumentado!");
		
		MessageFeedManager.instance.WriteMessage(dingText, FontSize.BIG, MessageType.Warning, 4f);
	}

	private void PlusOneStats()
	{
		Stamina++;
		strength++;
		agility++;
	}

	private int CalculateExpToLevel()
	{

		return (int)(100 * Level * Math.Pow(Level, 0.5f));
	}

	private int CalculateBaseHealth()
	{
		return Mathf.RoundToInt(20 * Mathf.Pow(baseHealthGrowthMultiplier, level));
	}

	private int CalculateMaxHealth()
	{
		int baseHealth = CalculateBaseHealth();
		return Mathf.RoundToInt(baseHealth + (Stamina * 10));
	}

	public float CalculateDamageReductionByArmor()
	{
		float dmgReduction = Armor / 100;

		if (dmgReduction >= damageReductionByArmorCap)
		{
			dmgReduction = damageReductionByArmorCap;
		}

		return dmgReduction;
	}

}
