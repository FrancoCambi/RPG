using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour, IPointerClickHandler, IClickable, IPointerEnterHandler, IPointerExitHandler
{
	private ObservableStack<Item> items = new();

	[SerializeField]
	private Image icon;

	[SerializeField]
	private Text stackSize;
	
	[SerializeField]
	private int index;

	public BagScript MyBag { get; set; }

	public bool IsEmpty
	{
		get
		{
			return items.Count == 0;
		}
	}

	public bool IsFull
	{
		get
		{
			if (IsEmpty || Count < MyItem.StackSize)
			{
				return false;
			}

			return true;
		}
	}

	public Item MyItem
	{
		get
		{
			if (!IsEmpty)
			{
				return items.Peek();
			}

			return null;
			
		}
	}

	public Image Icon
	{
		get
		{
			return icon;
		}
		set
		{
			icon = value;
		}
	}

	public int Count
	{
		get
		{
			return items.Count;
		}

	}

	public Text StackText
	{
		get
		{
			return stackSize;
		}
	}

	public ObservableStack<Item> Items
	{
		get
		{
			return items;
		}
	}
	
	public int Index 
	{
		get 
		{
			return index;
		}
		set 
		{
			index = value;
		}
	}

	private void Awake()
	{
		items.onPush += new UpdateStackEvent(UpdateSlot);
		items.onPop += new UpdateStackEvent(UpdateSlot);
		items.onClear += new UpdateStackEvent(UpdateSlot);
	}

	public bool AddItem(Item item)
	{
		items.Push(item);
		icon.sprite = item.Icon;
		icon.color = Color.white;
		item.Slot = this;

		return true;
	}

	public bool AddItems(ObservableStack<Item> newItems)
	{
		if (IsEmpty || newItems.Peek().GetType() == MyItem.GetType())
		{
			int count = newItems.Count;

			for (int i = 0; i < count; i++)
			{
				if (IsFull)
				{
					return false;
				}

				AddItem(newItems.Pop());
			}
			return true;
		}

		return false;

	}

	public void RemoveItem(Item item)
	{
		if (!IsEmpty)
		{
			Inventory.Instance.OnItemCountChanged(items.Pop());
		}
	}

	public void Clear()
	{
		int initCount = Items.Count;

		if (initCount > 0)
		{
			for (int i = 0; i < initCount; i++)
			{

				Inventory.Instance.OnItemCountChanged(items.Pop());
			}

		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (Inventory.Instance.FromSlot == null && !IsEmpty)
			{

				if (HandScript.Instance.MyMoveable != null)
				{
					if (HandScript.Instance.MyMoveable is Armor)
					{
						if (MyItem is Armor && (MyItem as Armor).ArmorType == (HandScript.Instance.MyMoveable as Armor).ArmorType)
						{
							(MyItem as Armor).Equip();
							GameController.Instance.RefreshTooltip(HandScript.Instance.MyMoveable as IDescribable);
							HandScript.Instance.Drop();

						}
					}
					else if (HandScript.Instance.MyMoveable is Sword)
					{
						if (MyItem is Sword)
						{
							(MyItem as Sword).Equip();
							GameController.Instance.RefreshTooltip(HandScript.Instance.MyMoveable as IDescribable);
							HandScript.Instance.Drop();

						}
					}
				}
				else
				{

					HandScript.Instance.TakeMoveable(MyItem as IMoveable);
					Inventory.Instance.FromSlot = this;
				}

			}
			else if (Inventory.Instance.FromSlot == null && IsEmpty)
			{
				if (HandScript.Instance.MyMoveable is Bag)
				{
					Bag bag = (Bag)HandScript.Instance.MyMoveable;
					if (bag.MyBagScript != MyBag && Inventory.Instance.EmptySlotCount - bag.SlotsCount > 0)
					{
						AddItem(bag);
						bag.MyBagButton.RemoveBag();
						HandScript.Instance.Drop();

					}

				}
				else if (HandScript.Instance.MyMoveable is Armor)
				{
					Armor armor = (Armor)HandScript.Instance.MyMoveable;
					AddItem(armor);
					CharacterPanel.Instance.MySelectedButton.DequipItem();
					HandScript.Instance.Drop();
					GameController.Instance.ShowTooltip(transform.position, new Vector2(1f,0f), armor);


				}
				else if (HandScript.Instance.MyMoveable is Sword)
				{
					Sword sword = (Sword)HandScript.Instance.MyMoveable;
					AddItem(sword);
					CharacterPanel.Instance.MySelectedSwordButton.DequipItem();
					HandScript.Instance.Drop();
					GameController.Instance.ShowTooltip(transform.position, new Vector2(1f,0f), sword);
				}

			}
			else if (Inventory.Instance.FromSlot != null)
			{
				if (PutItemBack() || MergeItems(Inventory.Instance.FromSlot) || SwapItems(Inventory.Instance.FromSlot) || AddItems(Inventory.Instance.FromSlot.items))
				{
					HandScript.Instance.Drop();
					Inventory.Instance.FromSlot = null;
					GameController.Instance.RefreshTooltip(items.Peek());
				}
			}
		}

		if (eventData.button == PointerEventData.InputButton.Right && HandScript.Instance.MyMoveable == null)
		{
			UseItem();
		}
	}

	public void UseItem()
	{
		if (MyItem is IUseable)
		{
			(MyItem as IUseable).Use();
		}
		else if (MyItem is Armor)
		{
			(MyItem as Armor).Equip();
		}
		else if (MyItem is Sword)
		{
			(MyItem as Sword).Equip();
		}
	}

	public bool StackItem(Item item)
	{
		
		if (!IsEmpty && item.ItemName == MyItem.ItemName && items.Count < MyItem.StackSize)
		{
			items.Push(item);
			item.Slot = this;
			return true;

		}

		return false;
	}

	private bool PutItemBack()
	{
		if (Inventory.Instance.FromSlot == this)
		{
			Inventory.Instance.FromSlot.icon.color = Color.white;
			return true;
		}

		return false;
	}

	private bool SwapItems(Slot from) 
	{
		if (IsEmpty)
		{
			return false;
		}
		if (from.MyItem.ItemName != MyItem.ItemName || from.Count + Count > MyItem.StackSize)
		{
			ObservableStack<Item> tmpFrom = new(from.items);

			from.items.Clear();
			from.AddItems(items);
			items.Clear();
			AddItems(tmpFrom);

			return true;
		}

		return false;
	}

	private bool MergeItems(Slot from)
	{
		if (IsEmpty)
		{
			return false;
		}
		if (from.MyItem.ItemName == MyItem.ItemName && !IsFull)
		{
			int free = MyItem.StackSize - Count;

			for (int i = 0; i < free; i++)
			{
				AddItem(from.items.Pop());
			}

			return true;
		}

		return false;

	}

	private void UpdateSlot()
	{
		GameController.Instance.UpdateStackSize(this);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (!IsEmpty)
		{
			GameController.Instance.ShowTooltip(transform.position,new Vector2(1f, 0f), MyItem);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameController.Instance.HideTooltip();
	}
}
