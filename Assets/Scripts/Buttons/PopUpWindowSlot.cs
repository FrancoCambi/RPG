using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PopUpWindowSlot : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private Text itemName;
	
	[SerializeField]
	private Image icon;
	
	[SerializeField]
	private BOPWindow bopWindow;
	
	[SerializeField]
	private BORWindow borWindow;
	
	private Item item;

	private void Start() 
	{
		if (bopWindow != null && borWindow != null) 
		{
			Debug.LogError("Bad window assignment. (PopUp)");
		}
	}
	
	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (HandScript.Instance.MyMoveable is Armor || HandScript.Instance.MyMoveable is Sword)
			{
				item = (Item) HandScript.Instance.MyMoveable;
				itemName.text = item.ItemName;
				icon.gameObject.SetActive(true);
				icon.sprite = item.Icon;
				GameController.Instance.RefreshTooltip(item);
				Inventory.Instance.FromSlot.Icon.color = Color.white;
				HandScript.Instance.Drop();
				
				SetToUpgrade(item);
			}


		}
	}
	
	private void SetToUpgrade(Item item) 
	{
		if (bopWindow != null) 
		{
			BOPWindow.Instance.ToUpgrade = item;
			
		}
		else if (borWindow != null) 
		{
			BORWindow.Instance.ToUpgrade = item;
		}
		
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (item != null)
		{
			GameController.Instance.ShowTooltip(transform.position, new Vector2(1f,0f), item);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameController.Instance.HideTooltip();
	}
	
	public void Close() 
	{
		icon.gameObject.SetActive(false);
		itemName.text = string.Empty;
		item = null;
	}
}
