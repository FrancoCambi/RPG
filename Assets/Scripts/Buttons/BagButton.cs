using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class BagButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	private Bag bag;

	[SerializeField]
	private Image backgroundImage;

	[SerializeField]
	private int bagIndex;

	private Image background;

	private void Start()
	{
		background = GetComponent<Image>();
	}

	public Bag Bag
	{
		get
		{
			return bag;
		}
		set
		{
			if (value != null)
			{
				backgroundImage.color = new Color(255, 255, 255, 255);

			}
			else
			{
				backgroundImage.color = new Color(0.5f ,0.5f, 0.5f, 0.5f);
			}
			bag = value;
		}
	}
	
	public int BagIndex 
	{
		get 
		{
			return bagIndex;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{

		if (Input.GetKey(KeyCode.LeftShift))
		{
			HandScript.Instance.TakeMoveable(Bag);
		}

		else if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (bag != null)
			{
				bag.MyBagScript.OpenClose();
			}
		}


	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		background.color = new Color(255, 128, 0, 255);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		background.color = Color.white;
	}

	public void RemoveBag()
	{
		Inventory.Instance.RemoveBag(Bag);

		Bag.MyBagButton = null;

		foreach (Item item in Bag.MyBagScript.GetItems())
		{
			Inventory.Instance.AddItem(item);
		}

		Bag = null;
	}

}
