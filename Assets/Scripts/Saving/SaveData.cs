using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
	public PlayerData MyPlayerData {  get; set; }
	
	public InventoryData MyInventoryData {get; set;}
	
	public CurrencyData MyCurrencyData {get; set;}
	
	public List<EquipmentData> MyEquipmentData {get; set;}
	
	public SwordData MySwordData {get; set;}
	
	public List<QuestData> MyQuestData {get; set;}
	
	public List<QuestGiverData> MyQuestGiverData {get; set;}
	
	public Language MyLanguageData {get; set;}

	public DateTime DateTime {get; set;}
	
	public string Scene {get; set;}

	public SaveData()
	{
		MyInventoryData = new();
		MyEquipmentData = new();
		MyQuestData = new();
		MyQuestGiverData = new();
		DateTime = DateTime.Now;
	}

}

[Serializable]
public class PlayerData
{
	public int Armor { get; set; }
	public int ArmorFromGear {get; set;}
	public int Stamina { get; set; }
	public int Strength { get; set; }
	public int Agility { get; set; }
	public float CritChance { get; set; }
	public int MaxHealth { get; set; }
	public int CurrentHealth { get; set; }
	public int Level { get; set; }
	public int CurrentExp { get; set; }
	public int ExpToLevel { get; set; }
	public float MyX { get; set; }
	public float MyY { get; set; }

	public PlayerData(int armor, int armorFromGear, int stamina, int strength, int agility, float critChance, int maxHealth, int currentHealth, int level, int currentExp, int expToLevel, Vector2 position)
	{
		Armor = armor;
		ArmorFromGear = armorFromGear;
		Stamina = stamina;
		Strength = strength;
		Agility = agility;
		CritChance = critChance;
		MaxHealth = maxHealth;
		CurrentHealth = currentHealth;
		Level = level;
		CurrentExp = currentExp;
		ExpToLevel = expToLevel;
		MyX = position.x;
		MyY = position.y;
	}
}

[Serializable]
public class ItemData 
{
	public string Title {get; set;}
	
	public int StackCount {get; set;}
	
	public int SlotIndex {get; set;}
	
	public int BagIndex {get; set;}
	
	public EquipmentQuality EquipmentQuality {get; set;}
	
	public int Upgrade {get; set;}
	
	public ItemData(string title, int stackCount = 0, int slotIndex = 0, int bagIndex = 0, EquipmentQuality equipmentQuality = EquipmentQuality.None, int upgrade = 0)
	{
		Title = title;
		StackCount = stackCount;
		SlotIndex = slotIndex;
		BagIndex = bagIndex;
		EquipmentQuality = equipmentQuality;
		Upgrade = upgrade;
	}
}

[Serializable]
public class InventoryData 
{
	public List<BagData> Bags {get; set;}
	
	public List<ItemData> Items {get; set;}

	public InventoryData() 
	{
		Bags = new();
		Items = new();
	}
}

[Serializable]
public class CurrencyData 
{
	public GoldCurrency Gold {get; set;}
	
	
	public CurrencyData(GoldCurrency gold) 
	{
		Gold = gold;
	}
}


[Serializable]
public class BagData 
{
	public string ItemName {get; set;}
	
	public int BagIndex {get; set; }
	
	public BagData (string itemName, int index) 
	{
		ItemName = itemName;
		BagIndex = index;
	}
}

[Serializable]
public class EquipmentData 
{
	public string Title {get; set;}
	
	public string Type {get; set;}
	
	public bool IsEmpty {get; set;}
	
	public EquipmentData(string title, string type, bool isEmpty) 
	{
		Title = title;
		Type = type;
		IsEmpty = isEmpty;
	}
}

[Serializable]
public class SwordData 
{
	public string Title {get; set;}
	
	public string Type {get; set;}
	
	public bool IsEmpty {get; set;}
	
	public SwordData(string title, string type, bool isEmpty) 
	{
		Title = title;
		Type = type;
		IsEmpty = isEmpty;
	}
}

[Serializable]
public class QuestData 
{
	public string Title {get; set;}
	
	public string Description {get; set;}
	
	public CollectObjective[] CollectObjectives {get; set;}
	
	public KillObjective[] KillObjectives {get; set;}
	
	public string QuestGiverName {get; set;}
	
	public QuestData(string title, string description, CollectObjective[] collectObjectives, KillObjective[] killObjectives, string questGiverName) 
	{
		Title = title;
		Description = description;
		CollectObjectives = collectObjectives;
		KillObjectives = killObjectives;
		QuestGiverName = questGiverName;
	}
}

[Serializable]
public class QuestGiverData 
{
	public List<string> CompletedQuests {get; set;}
	
	public string QuestGiverName {get; set;}
	
	public QuestGiverData(List<string> completedQuests, string questGiverName) 
	{
		CompletedQuests = completedQuests;
		QuestGiverName = questGiverName;
	}
}

[Serializable]
public class LanguageData 
{
	public Language Lang {get; set;}
	
	public LanguageData(Language lang) 
	{
		Lang = lang;
	}
}