using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MaterialSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private Image icon;
	
	[SerializeField]
	private Text itemName;
	
	public Item Item {get; set;}

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
	
	public Text ItemName 
	{
		get 
		{
			return itemName;
		}
		set 
		{
			itemName = value;
		}
	}
	
	public void OnPointerEnter(PointerEventData eventData)
	{
		if (Item != null)
		{
			GameController.Instance.ShowTooltip(transform.position, new Vector2(1f,0f), Item);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameController.Instance.HideTooltip();
	}
	
	public void Clear() 
	{
		Item = null;
		itemName.text = string.Empty;
		icon.gameObject.SetActive(false);
	}
}
