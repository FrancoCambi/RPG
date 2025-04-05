using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;


public abstract class Item : ScriptableObject, IMoveable, IDescribable
{

	[SerializeField]
	private Sprite icon;
	
	[SerializeField]
	private int id;

	[SerializeField]
	private string itemNameEnglish;
	
	[SerializeField]
	private string itemNameSpanish;
	
	[SerializeField]
	private ItemQuality itemQuality;
	
	[SerializeField]
	private int stackSize;

	[SerializeField]
	private int itemLevel;

	[SerializeField]
	private int requiredLevel;

	[SerializeField]
	private string descriptionEnglish;
	
	[SerializeField]
	private string descriptionSpanish;


	private Slot slot;

	public Sprite Icon
	{
		get
		{
			return icon;
		}
	}
	
	public int Id 
	{
		get 
		{
			return id;
		}
	}

	public int StackSize
	{
		get
		{
			return stackSize;
		}
	}

	public Slot Slot
	{
		get
		{
			return slot;
		}
		set
		{
			slot = value;
		}
	}

	public ItemQuality ItemQuality
	{
		get
		{
			return itemQuality;
		}

	}
	
	public string ItemNameEnglish 
	{
		get 
		{
			return itemNameEnglish;
		}
	}

	public string ItemName
	{
		get
		{
			if (LanguageManager.Instance.CurrentLanguage == Language.English) 
			{
				
				return itemNameEnglish;
			}
			else 
			{
				return itemNameSpanish;
			}
		}
	}
	
	public string Description 
	{
		get 
		{
			if (LanguageManager.Instance.CurrentLanguage == Language.English) 
			{
				
				return descriptionEnglish;
			}
			else 
			{
				return descriptionSpanish;
			}
		}
	}
	

	public virtual string GetDescription()
	{
		if (this is Armor) 
		{
			string upgr = string.Empty;
			if ((this as Armor).Upgrade > 0) 
			{
				upgr = "+" + (this as Armor).Upgrade.ToString();
			}
			
			string tooltipText = LanguageManager.Instance.Translate("<color={0}>{1} {2}</color>\n<color=#FFD400>Item Level {3}</color>\n<color={4}>Quality: {5}</color>\nRequires Level {6}",
				"<color={0}>{1} {2}</color>\n<color=#FFD400>Nivel de objeto {3}</color>\n<color={4}>Calidad: {5}</color>\nRequiere Nivel {6}");
			
			return string.Format(tooltipText, ItemQualityColor.Colors[itemQuality], ItemName, upgr, itemLevel, EquipmentQualityClass.Colors[(this as Armor).EquipmentQuality], 
				GetTranslatedQualityName((this as Armor).EquipmentQuality), requiredLevel);
			

		}
		else if (this is Sword) 
		{
			string upgr = string.Empty;
			if ((this as Sword).Upgrade > 0) 
			{
				upgr = "+" + (this as Sword).Upgrade.ToString();
			}
			
			string tooltipText = LanguageManager.Instance.Translate("<color={0}>{1} {2}</color>\n<color=#FFD400>Item Level {3}</color>\n<color={4}>Quality: {5}</color>\nRequires Level {6}",
				"<color={0}>{1} {2}</color>\n<color=#FFD400>Nivel de objeto {3}</color>\n<color={4}>Calidad: {5}</color>\nRequiere Nivel {6}");
			
			return string.Format(tooltipText, ItemQualityColor.Colors[itemQuality], ItemName, upgr, itemLevel, EquipmentQualityClass.Colors[(this as Sword).EquipmentQuality], 
				GetTranslatedQualityName((this as Sword).EquipmentQuality), requiredLevel);
		}
		else 
		{
			string reqLevelText = LanguageManager.Instance.Translate("\nRequires Level", "\nRequiere Nivel");
			string reqLevel = requiredLevel > 0 ? reqLevelText + requiredLevel.ToString() : string.Empty;
			string tooltipText = LanguageManager.Instance.Translate("<color={0}>{1}</color>\n<color=#FFD400>Item Level {2}</color>{3}",
				"<color={0}>{1}</color>\n<color=#FFD400>Nivel de Objeto {2}</color>{3}");
			
			return string.Format(tooltipText, ItemQualityColor.Colors[itemQuality], ItemName, itemLevel, reqLevel);
		}
		
	}
	
	public void Remove()
	{
		if (Slot != null)
		{
			if (this is IDestroyHandler) 
			{
				(this as IDestroyHandler).OnDestroyHandler();
			}
			Slot.RemoveItem(this);

		}
	}
	
	public bool PlayerMeetsRequirements() 
	{
		if (PlayerStats.Instance.Level >= requiredLevel) 
		{
			return true;
		}
		else 
		{
			MessageFeedManager.instance.WriteMessage("You don't meet the requirements for this item.", FontSize.NORMAL, MessageType.Warning, 2f);
			return false;
		}
	}
	
	private string GetTranslatedQualityName(EquipmentQuality quality) 
	{
		string name = string.Empty;
		
		switch (quality) 
		{
			case EquipmentQuality.Damaged:
				name = LanguageManager.Instance.Translate("Damaged", "Dañado");
				break;
			case EquipmentQuality.Low:
				name = LanguageManager.Instance.Translate("Low", "Baja");
				break;
			case EquipmentQuality.Common:
				name = LanguageManager.Instance.Translate("Common", "Común");
				break;
			case EquipmentQuality.Useful:
				name = LanguageManager.Instance.Translate("Useful", "Útil");
				break;
			case EquipmentQuality.Good:
				name = LanguageManager.Instance.Translate("Good", "Buena");
				break;
			case EquipmentQuality.Excellent:
				name = LanguageManager.Instance.Translate("Excellent", "Excelente");
				break;
			case EquipmentQuality.Ancient:
				name = LanguageManager.Instance.Translate("Ancient", "Ancestral");
				break;
			case EquipmentQuality.Mysterious:
				name = LanguageManager.Instance.Translate("Mysterious", "Misteriosa");
				break;
			default:
				break;

		}
		
		return name;
	}
}
