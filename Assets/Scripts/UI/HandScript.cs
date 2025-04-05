using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
	private static HandScript instance;

	public static HandScript Instance 
	{
		get 
		{
			if (instance == null) 
			{
				instance = FindObjectOfType<HandScript>();
			}
			return instance;
		}
	}

	public IMoveable MyMoveable { get; set; }

	private Image icon;

	// Start is called before the first frame update
	void Start()
	{
		icon = GetComponent<Image>();
	}

	// Update is called once per frame
	void Update()
	{
		icon.transform.position = Input.mousePosition;
		if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && Instance.MyMoveable != null)
		{
			if (CharacterPanel.Instance.MySelectedButton != null)
			{
				CharacterPanel.Instance.MySelectedButton.Icon.color = Color.white;
				Drop();
				CharacterPanel.Instance.MySelectedButton = null;
			}
			else if (CharacterPanel.Instance.MySelectedSwordButton != null)
			{
				CharacterPanel.Instance.MySelectedSwordButton.Icon.color = Color.white;
				Drop();
				CharacterPanel.Instance.MySelectedSwordButton = null;
			}
			else
			{

				DeleteItem();
			}


		}

	}

	public void TakeMoveable(IMoveable moveable)
	{
		this.MyMoveable = moveable;
		icon.sprite = moveable.Icon;
		icon.color = Color.white;
	}

	public IMoveable Put()
	{
		IMoveable tmp = MyMoveable;

		MyMoveable = null;

		icon.color = new Color(0, 0, 0, 0);

		return tmp;
	}

	public void Drop()
	{
		MyMoveable = null;
		icon.color = new Color(0, 0, 0, 0);
		Inventory.Instance.FromSlot = null;
	}

	public void DeleteItem()
	{

		if (MyMoveable is Item && Inventory.Instance.FromSlot != null)
		{
			(MyMoveable as Item).Slot.Clear();
			
			if (MyMoveable is IDestroyHandler) 
			{
				(MyMoveable as IDestroyHandler).OnDestroyHandler();
			}
		}

		Drop();

		Inventory.Instance.FromSlot = null;
	}

}
