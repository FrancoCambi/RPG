using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;


public delegate void ItemCountChanged(Item item);

public class Inventory : MonoBehaviour
{
	public event ItemCountChanged onItemCountChanged;

	private static Inventory instance;

	public static Inventory Instance
	{
		get
		{
			if (instance == null)
			{
				instance = FindObjectOfType<Inventory>();
			}

			return instance;
		}
	}

	private Slot fromSlot;

	private List<Bag> bags = new();

	[SerializeField]
	private BagButton[] bagButtons;

	[SerializeField]
	private Item[] items;

	[SerializeField]
	private Bag DefaultBag;

	public bool CanAddBag
	{
		get { return Bags.Count < 4; }
	}

	public int EmptySlotCount
	{
		get
		{
			int count = 0;
			
			foreach (Bag bag in Bags)
			{
				count += bag.MyBagScript.EmptySlotsCount;
			}

			return count;
		}
	}

	public Slot FromSlot
	{
		get
		{
			return fromSlot;
		}
		set
		{
			fromSlot = value;
			if (value != null)
			{
				fromSlot.Icon.color = Color.grey;
			}
		}
	}

	public bool IsOpen
	{
		get
		{
			foreach (Bag bag in Bags)
			{
				if (bag.MyBagScript.IsOpen)
				{
					return true;
				}
			}

			return false;
		}
	}

	public List<Bag> Bags 
	{
		get 
		{
			return bags;
		}
	}

	private void Awake()
	{
		
		Bag bag = (Bag)Instantiate(DefaultBag);
		bag.Use();
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.K))
		{
			for (int i = 1; i < items.Length; i++)
			{
				Item bag = Instantiate(items[i]);
				AddItem(bag);

			}
		}

	}

	public void AddBag(Bag bag)
	{
		foreach (BagButton bagButton in bagButtons)
		{
			if (bagButton.Bag == null)
			{
				bagButton.Bag = bag;
				Bags.Add(bag);
				bag.MyBagButton = bagButton;
				break;
			}
		}

	}
	
	public void AddBag(Bag bag, int index) 
	{
		bag.SetUpScript();
		bagButtons[index].Bag = bag;
		Bags.Add(bag);
		bag.MyBagButton = bagButtons[index];
		
	}

	public void RemoveBag(Bag bag)
	{
		Bags.Remove(bag);
		Destroy(bag.MyBagScript.gameObject);
	}

	public bool AddItem(Item item)
	{
		if (item.StackSize > 0)
		{
			if (PlaceInStack(item))
			{
				return true;
			}

		}

		return PlaceInEmpty(item);
	}

	private bool PlaceInEmpty(Item item)
	{
		foreach (Bag bag in Bags)
		{
			if (bag.MyBagScript.AddItem(item))
			{
				OnItemCountChanged(item);
				return true;
			}
		}

		return false;
	}

	private bool PlaceInStack(Item item)
	{
		foreach (Bag bag in Bags)
		{
			foreach (Slot slot in bag.MyBagScript.Slots)
			{
				if (slot.StackItem(item))
				{
					OnItemCountChanged(item);
					return true;
				}
			}
		}

		return false;
	}
	
	public Item PlaceInSpecific(Item item, int slotIndex, int bagIndex) 
	{
		Item itemIns = Instantiate(item);
		bags[bagIndex].MyBagScript.Slots[slotIndex].AddItem(itemIns);
		return itemIns;
	} 


	public void OpenClose()
	{
		bool closedBag = Bags.Find(x => !x.MyBagScript.IsOpen);

		foreach (Bag bag in Bags)
		{
			if (bag.MyBagScript.IsOpen != closedBag)
			{
				bag.MyBagScript.OpenClose();
			} 
		}
	}

	public void Close()
	{
		foreach (Bag bag in Bags)
		{
			bag.MyBagScript.Close();
		}
	}
	
	public List<Slot> GetAllItems() 
	{
		List<Slot> slots = new();
		
		foreach (Bag bag in bags) 
		{
			foreach (Slot slot in bag.MyBagScript.Slots) 
			{
				if (!slot.IsEmpty) 
				{
					slots.Add(slot);
				}
			}
		}
		
		return slots;
	}

	public Stack<IUseable> GetUSeables(IUseable type)
	{
		Stack<IUseable> useables = new();

		foreach (Bag bag in Bags)
		{
			foreach (Slot slot in bag.MyBagScript.Slots)
			{
				if (!slot.IsEmpty && slot.MyItem.GetType() == type.GetType())
				{
					foreach (Item item in slot.Items)
					{
						useables.Push(item as IUseable);
					}
				}
			}
		}

		return useables;
	}

	public int GetItemCount(string type)
	{
		int itemCount = 0;

		foreach (Bag bag in Bags)
		{
			foreach (Slot slot in bag.MyBagScript.Slots)
			{
				if (!slot.IsEmpty && slot.MyItem.ItemName == type)
				{
					itemCount += slot.Items.Count;
				}
			}
		}

		return itemCount;
	}
	
	public Stack<Item> GetItems(string type, int count) 
	{
		Stack<Item> items = new();
		
		foreach (Bag bag in Bags)
		{
			foreach (Slot slot in bag.MyBagScript.Slots)
			{
				if (!slot.IsEmpty && slot.MyItem.ItemName == type)
				{
					foreach (Item item in slot.Items) 
					{
						items.Push(item);
						
						if (items.Count == count) 
						{
							return items;
						}
					}
				}
			}
		}
		
		return items;
	}
	
	public void ClearSlot(int bagIndex, int slotIndex) 
	{
		bags[bagIndex].MyBagScript.Slots[slotIndex].Clear();
	}
	
	public bool TakeItems(string name, int count) 
	{
		if (GetItemCount(name) >= count) 
		{
			Stack<Item> items = GetItems(name, count);
			
			foreach (Item item in items) 
			{
				item.Remove();
			}
			
			return true;
		}
		
		return false;
	}

	public void OnItemCountChanged(Item item)
	{
		if (onItemCountChanged != null)
		{
			onItemCountChanged.Invoke(item);
		}
	}
}
