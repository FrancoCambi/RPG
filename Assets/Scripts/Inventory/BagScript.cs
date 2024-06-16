using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BagScript : MonoBehaviour
{
	[SerializeField]
	private GameObject slotPrefab;

	private CanvasGroup canvasGroup;

	private List<Slot> slots = new();
	
	public int MyBagIndex {get; set;}

	public bool IsOpen
	{
		get
		{
			return canvasGroup.alpha > 0;
		}
	}

	public List<Slot> Slots
	{
		get
		{
			return slots;
		}
	}

	public int EmptySlotsCount
	{
		get
		{
			int count = 0;
			foreach (Slot slot in Slots)
			{
				if (slot.IsEmpty)
				{
					count++;
				}
			}

			return count;
		}
	}

	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
	}

	public List<Item> GetItems()
	{
		List<Item> items = new();

		foreach (Slot slot in slots)
		{
			if (!slot.IsEmpty)
			{
				foreach (Item item in slot.Items)
				{
					items.Add(item);
				}
			}
		}

		return items;
	}

	public void AddSlots(int slotCount)
	{
		for (int i = 0; i < slotCount; i++)
		{
			Slot slot = Instantiate(slotPrefab, transform).GetComponent<Slot>();
			slot.Index = i;
			slot.MyBag = this;
			slots.Add(slot);

		}
	}

	public bool AddItem(Item item)
	{
		foreach (Slot slot in slots)
		{
			if (slot.IsEmpty)
			{
				slot.AddItem(item);
				return true;
			}
		}

		return false;

	}

	public void OpenClose()
	{
		canvasGroup.alpha = canvasGroup.alpha > 0 ? 0 : 1;

		canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts == true ? false : true;
	}

	public void Close()
	{
		canvasGroup.alpha = canvasGroup.alpha = 0;

		canvasGroup.blocksRaycasts = canvasGroup.blocksRaycasts = false;
	}
}
