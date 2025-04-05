using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SaveManager : MonoBehaviour
{
	private static SaveManager instance;
	
	public static SaveManager Instance 
	{
		get 
		{
			if (instance == null) 
			{
				instance = FindObjectOfType<SaveManager>();
			}
			return instance;
		}
	}
	
	[SerializeField]
	private Item[] items;
	
	[SerializeField]
	private SavedGame[] saveSlots;
	
	[SerializeField]
	private GameObject dialogue;
	
	[SerializeField]
	private Text dialogueText;
	
	private SavedGame current;
	
	private CharButton[] equipment;
	
	private CharSwordButton swordButton;
	
	private string action;
	
	private int currentLoad = -1;
	
	public Item[] Items 
	{
		get 
		{
			return items;
		}
	}
	
	public SavedGame[] SaveSlots 
	{
		get 
		{
			return saveSlots;
		}
	}
	
	public int CurrentLoad 
	{
		get 
		{
			return currentLoad;
		}
	}

	void Awake()
	{
		equipment = FindObjectsOfType<CharButton>();
		swordButton = FindObjectOfType<CharSwordButton>();
		
		foreach (SavedGame saved in saveSlots) 
		{

			ShowSavedFiles(saved);
		}
		
	}
	
	private void Start() 
	{
		if (PlayerPrefs.HasKey("Load")) 
		{
			Load(saveSlots[PlayerPrefs.GetInt("Load")]);
			PlayerPrefs.DeleteKey("Load");
		}
		else 
		{
			PlayerStats.Instance.SetDefault();
			CharacterPanel.Instance.SetDefault();
		}
	}
	
	public void SpawnStart() 
	{
		SceneManager.LoadScene(0);
		PlayerStats.Instance.SetDefault();
		CharacterPanel.Instance.SetDefault();
	}
	
	public void ShowDialogue(GameObject clickButton) 
	{
		action = clickButton.name;
		
		switch (action) 
		{
			case "Load":
				dialogueText.text = "Load Game?";
				break;
			case "Save":
				dialogueText.text = "Save Game?";
				break;
			case "Delete":
				dialogueText.text = "Delete save?";
				break;
		}
		
		current = clickButton.GetComponentInParent<SavedGame>();
		dialogue.SetActive(true);
	}
	
	public void ExecuteAction() 
	{
		switch (action) 
		{
			case "Load":
				LoadScene(current);
				break;
			case "Save":
				Save(current);
				break;
			case "Delete":
				Delete(current);
				break;
		}
		
		CloseDialogue();
		
	}
	
	private void LoadScene(SavedGame savedGame) 
	{
		if (File.Exists(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat")) 
		{
			BinaryFormatter bf = new();
			FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
			SaveData data = (SaveData)bf.Deserialize(file);
			file.Close();
			
			PlayerPrefs.SetInt("Load", savedGame.Index);
			
			SceneManager.LoadScene(data.Scene);
		}
	}
	
	public void CloseDialogue() 
	{
		dialogue.SetActive(false);
	}
	
	private void Delete(SavedGame savedGame) 
	{
		File.Delete(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat");
		savedGame.HideVisuals();
	}

	private void ShowSavedFiles(SavedGame savedGame) 
	{
		if (File.Exists(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat")) 
		{
			BinaryFormatter bf = new();
			FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);
			SaveData data = (SaveData)bf.Deserialize(file);
			file.Close();
			savedGame.ShowInfo(data);
		}
	}

	public void Save(SavedGame savedGame)
	{
		try
		{
			currentLoad = savedGame.Index;
			BinaryFormatter bf = new BinaryFormatter();

			FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Create);

			SaveData data = new SaveData();
			
			data.Scene = SceneManager.GetActiveScene().name;
			
			SaveLanguage(data);
			
			SaveEquipment(data);
			
			SaveBags(data);
			
			SaveInventory(data);
			
			SaveCurrency(data);
			
			SavePlayer(data);
			
			SaveQuests(data);
			
			SaveQuestGivers(data);

			bf.Serialize(file, data);

			file.Close();
			
			ShowSavedFiles(savedGame);
		}
		catch (System.Exception e)
		{
			Debug.LogError("Error happened: " + e);
		}
	}
	

	public void Load(SavedGame savedGame)
	{
		try
		{
			currentLoad = savedGame.Index;
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/" + savedGame.gameObject.name + ".dat", FileMode.Open);

			SaveData data = (SaveData)bf.Deserialize(file);

			LoadLanguage(data);

			LoadEquipment(data);

			LoadBags(data);
			
			LoadInventory(data);
			
			LoadCurrency(data);

			LoadPlayer(data);
			
			LoadQuests(data);
			
			LoadQuestGiver(data);

			file.Close();
		}
		catch (System.Exception e)
		{
			Debug.LogError("Error happened: " + e);
			Delete(savedGame);
			PlayerPrefs.DeleteKey("Load");
		}
	}

	private void SavePlayer(SaveData data)
	{

		data.MyPlayerData = new PlayerData(PlayerStats.Instance.Armor, PlayerStats.Instance.ArmorFromGear, PlayerStats.Instance.Stamina, PlayerStats.Instance.Strength, PlayerStats.Instance.Agility, PlayerStats.Instance.CritChance, PlayerStats.Instance.maxHealth,
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
				data.MyEquipmentData.Add(new EquipmentData(charButton.EquippedArmor.ItemName, charButton.name, false));
			}
			else 
			{
				data.MyEquipmentData.Add(new EquipmentData(null, charButton.name, true));
			}
		}
		
		if (swordButton.EquippedSword != null) 
		{
			data.MySwordData = new SwordData(swordButton.EquippedSword.ItemName, swordButton.name, false);
		}
		else 
		{
			data.MySwordData = new SwordData(null, swordButton.name, true);
		}
	}
	
	private void SaveInventory(SaveData data) 
	{
		List<Slot> slots = Inventory.Instance.GetAllItems();
		
		foreach (Slot slot in slots) 
		{
			if (slot.MyItem is Armor) 
			{
				data.MyInventoryData.Items.Add(new ItemData(slot.MyItem.ItemName, slot.Items.Count, slot.Index, slot.MyBag.MyBagIndex, 
				(slot.MyItem as Armor).EquipmentQuality, (slot.MyItem as Armor).Upgrade));
			}
			else if (slot.MyItem is Sword) 
			{
				data.MyInventoryData.Items.Add(new ItemData(slot.MyItem.ItemName, slot.Items.Count, slot.Index, slot.MyBag.MyBagIndex, 
				(slot.MyItem as Sword).EquipmentQuality, (slot.MyItem as Sword).Upgrade));
			}
			else 
			{
				data.MyInventoryData.Items.Add(new ItemData(slot.MyItem.ItemName, slot.Items.Count, slot.Index, slot.MyBag.MyBagIndex));
			}
			
		}
	}
	
	private void SaveCurrency(SaveData data) 
	{
		data.MyCurrencyData = new CurrencyData(PlayerCurrency.Instance.MyGoldCurrency);
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
	
	private void SaveLanguage(SaveData data) 
	{
		data.MyLanguageData = LanguageManager.Instance.CurrentLanguage;
	}

	private void LoadPlayer(SaveData data)
	{
		PlayerData d = data.MyPlayerData;

		PlayerStats.Instance.SetInstance(d.Armor, d.ArmorFromGear, d.Stamina, d.Strength, d.Agility, d.CritChance, d.MaxHealth, d.CurrentHealth, d.Level, d.CurrentExp, d.ExpToLevel);
		FrameManager.Instance.UpdateLevelUiText();
		CharacterPanel.Instance.UpdateStatsText();
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
			
			if (equipmentData.IsEmpty) 
			{
				cb.DequipItem();
			}
			else 
			{
			
				cb.EquipArmor(Array.Find(items, x => x.ItemName == equipmentData.Title) as Armor);
				
			}
		}
		
		if (data.MySwordData.IsEmpty) 
		{
			swordButton.DequipItem();
		}
		else 
		{
			swordButton.EquipSword(Array.Find(items, x => x.ItemName == data.MySwordData.Title) as Sword);
		}
		
		
		CharacterPanel.Instance.UpdateStatsText();	
	}
	
	private void LoadInventory(SaveData data) 
	{
		foreach (ItemData itemData in data.MyInventoryData.Items) 
		{
			Item item = Array.Find(items, x => x.ItemName == itemData.Title);
			
			for (int i = 0; i < itemData.StackCount; i++) 
			{
				Inventory.Instance.ClearSlot(itemData.BagIndex, itemData.SlotIndex);
				Item itemIns = Inventory.Instance.PlaceInSpecific(item, itemData.SlotIndex, itemData.BagIndex);
			
				if (itemIns is Armor) 
				{
					(itemIns as Armor).EquipmentQuality = itemData.EquipmentQuality;
					(itemIns as Armor).Upgrade = itemData.Upgrade;
				}
				else if (itemIns is Sword)
				{
					(itemIns as Sword).EquipmentQuality = itemData.EquipmentQuality;
					(itemIns as Sword).Upgrade = itemData.Upgrade;
					
				}
			}
		}
	}
	
	private void LoadCurrency(SaveData data) 
	{
		PlayerCurrency.Instance.GetMoney(data.MyCurrencyData.Gold);
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
	
	private void LoadLanguage(SaveData data) 
	{
		if (LanguageManager.Instance.CurrentLanguage != data.MyLanguageData) 
		{
			LanguageManager.Instance.SwitchLanguage();
		}
	}
	
}
