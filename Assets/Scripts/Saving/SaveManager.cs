using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Collections;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
	[SerializeField]
	private Item[] items;
	
	private CharButton[] equipment;
	
	// Start is called before the first frame update
	void Start()
	{
		equipment = FindObjectsOfType<CharButton>();
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Q))
		{
			Save();
		}
		else if (Input.GetKeyDown(KeyCode.R))
		{
			Load();
		}
	}

	private void Save()
	{
		try
		{
			BinaryFormatter bf = new BinaryFormatter();

			FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.Create);

			SaveData data = new SaveData();
			
			SaveEquipment(data);
			
			SaveBags(data);
			
			SaveInventory(data);
			
			SavePlayer(data);
			
			SaveQuests(data);
			
			SaveQuestGivers(data);

			bf.Serialize(file, data);

			file.Close();
		}
		catch (System.Exception e)
		{
			Debug.LogError("Error happened: " + e);
		}
	}
	

	private void Load()
	{
		try
		{
			BinaryFormatter bf = new BinaryFormatter();

			FileStream file = File.Open(Application.persistentDataPath + "/" + "SaveTest.dat", FileMode.Open);

			SaveData data = (SaveData)bf.Deserialize(file);

			LoadEquipment(data);

			LoadBags(data);
			
			LoadInventory(data);

			LoadPlayer(data);
			
			LoadQuests(data);
			
			LoadQuestGiver(data);

			file.Close();
		}
		catch (System.Exception e)
		{
			Debug.LogError("Error happened: " + e);
		}
	}

	private void SavePlayer(SaveData data)
	{

		data.MyPlayerData = new PlayerData(PlayerStats.Instance.Armor, PlayerStats.Instance.Stamina, PlayerStats.Instance.Strength, PlayerStats.Instance.Agility, PlayerStats.Instance.CritChance, PlayerStats.Instance.maxHealth,
			PlayerStats.Instance.CurrentHealth, PlayerStats.Instance.Level, PlayerStats.Instance.currentExp, PlayerStats.Instance.expToLevel, 
			PlayerStats.Instance.transform.position);
	}
	
	private void SaveBags(SaveData data) 
	{
		for (int i = 1; i < Inventory.Instance.Bags.Count; i++) 
		{
			data.MyInventoryData.Bags.Add(new BagData(Inventory.Instance.Bags[i].ItemName, Inventory.Instance.Bags[i].MyBagButton.BagIndex));
		}
	}
	
	private void SaveEquipment(SaveData data) 
	{
		foreach (CharButton charButton in equipment) 
		{
			if (charButton.EquippedArmor != null) 
			{
				data.MyEquipmentData.Add(new EquipmentData(charButton.EquippedArmor.ItemName, charButton.name));
			}
		}
	}
	
	private void SaveInventory(SaveData data) 
	{
		List<Slot> slots = Inventory.Instance.GetAllItems();
		
		foreach (Slot slot in slots) 
		{
			data.MyInventoryData.Items.Add(new ItemData(slot.MyItem.ItemName, slot.Items.Count, slot.Index, slot.MyBag.MyBagIndex));
		}
	}
	
	private void SaveQuests(SaveData data) 
	{
		foreach (Quest quest in QuestLog.Instance.Quests) 
		{
			data.MyQuestData.Add(new QuestData(quest.Title, quest.Description, quest.CollectObjectives, quest.KillObjectives, quest.MyQuestGiver.NpcName));
		}
	}
	
	private void SaveQuestGivers(SaveData data) 
	{
		QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();
		
		foreach (QuestGiver questGiver in questGivers) 
		{
			data.MyQuestGiverData.Add(new QuestGiverData(questGiver.CompletedQuests, questGiver.NpcName));
		}
	}

	private void LoadPlayer(SaveData data)
	{
		PlayerData d = data.MyPlayerData;

		PlayerStats.Instance.SetInstance(d.Armor, d.Stamina, d.Strength, d.Agility, d.CritChance, d.MaxHealth, d.CurrentHealth, d.Level, d.CurrentExp, d.ExpToLevel);
		GameController.instance.UpdateLevelUiText();
		PlayerStats.Instance.transform.position = new Vector2(d.MyX, d.MyY);

	}
	
	private void LoadBags(SaveData data) 
	{
		foreach (BagData bagData in data.MyInventoryData.Bags) 
		{
			
			Bag newBag = (Bag)Instantiate(Array.Find(items, x => x.ItemName == bagData.ItemName));
			
			
			Inventory.Instance.AddBag(newBag, bagData.BagIndex);
		}
	}
	
	private void LoadEquipment(SaveData data) 
	{
		foreach (EquipmentData equipmentData in data.MyEquipmentData) 
		{
			CharButton cb = Array.Find(equipment, x => x.name == equipmentData.Type);
			
			cb.EquipArmor(Array.Find(items, x => x.ItemName == equipmentData.Title) as Armor);
		}
	}
	
	private void LoadInventory(SaveData data) 
	{
		foreach (ItemData itemData in data.MyInventoryData.Items) 
		{
			Item item = Array.Find(items, x => x.ItemName == itemData.Title);
			
			for (int i = 0; i < itemData.StackCount; i++) 
			{
				Inventory.Instance.PlaceInSpecific(item, itemData.SlotIndex, itemData.BagIndex);
			}
		}
	}
	
	private void LoadQuests(SaveData data) 
	{
		QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();
		
		foreach (QuestData questData in data.MyQuestData) 
		{
			QuestGiver qg = Array.Find(questGivers, x => x.NpcName == questData.QuestGiverName);
			Quest q = Array.Find(qg.Quests, x => x.Title == questData.Title);
			q.MyQuestGiver = qg;
			q.KillObjectives = questData.KillObjectives;
			
			QuestLog.Instance.AcceptQuest(q);
		}
	}
	
	private void LoadQuestGiver(SaveData data) 
	{
		QuestGiver[] questGivers = FindObjectsOfType<QuestGiver>();
		
		foreach (QuestGiverData questGiverData in data.MyQuestGiverData) 
		{
			QuestGiver questGiver = Array.Find(questGivers, x => x.NpcName == questGiverData.QuestGiverName);
			
			questGiver.CompletedQuests = questGiverData.CompletedQuests;
			
			questGiver.UpdateQuestStatus();
		}
	}
}
