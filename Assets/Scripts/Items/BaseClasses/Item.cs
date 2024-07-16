using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEditor;
using UnityEngine;


public abstract class Item : ScriptableObject, IMoveable, IDescribable
{

	[SerializeField]
	private Sprite icon;

	[SerializeField]
	private string itemName;
	
	[SerializeField]
	private ItemQuality itemQuality;
	
	[SerializeField]
	private int stackSize;

	[SerializeField]
	private int itemLevel;

	[SerializeField]
	private int requiredLevel;

	public string description;


	private Slot slot;

	public Sprite Icon
	{
		get
		{
			return icon;
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

	public string ItemName
	{
		get
		{
			return itemName;
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
			return string.Format("<color={0}>{1} {2}</color>\n<color=#FFD400>Item Level {3}</color>\n<color={4}>Quality: {5}</color>\nRequires Level {6}", 
			ItemQualityColor.Colors[itemQuality], itemName, upgr, itemLevel, EquipmentQualityClass.Colors[(this as Armor).EquipmentQuality], (this as Armor).EquipmentQuality.ToString(), requiredLevel);
			

		}
		else if (this is Sword) 
		{
			string upgr = string.Empty;
			if ((this as Sword).Upgrade > 0) 
			{
				upgr = "+" + (this as Sword).Upgrade.ToString();
			}
			return string.Format("<color={0}>{1} {2}</color>\n<color=#FFD400>Item Level {3}</color>\n<color={4}>Quality: {5}</color>\nRequires Level {6}", 
			ItemQualityColor.Colors[itemQuality], itemName, upgr, itemLevel, EquipmentQualityClass.Colors[(this as Sword).EquipmentQuality], (this as Sword).EquipmentQuality.ToString(), requiredLevel);
		}
		else 
		{
			string reqLevel = requiredLevel > 0 ? "\nRequires Level " + requiredLevel.ToString() : string.Empty;
			
			return string.Format("<color={0}>{1}</color>\n<color=#FFD400>Item Level {2}</color>{3}", ItemQualityColor.Colors[itemQuality], itemName, itemLevel, reqLevel);
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
}
