using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharSwordButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

	private Sword equippedSword;

	[SerializeField]
	private Sprite defaultIcon;

	[SerializeField]
	private Image icon;

	public Sword EquippedSword
	{
		get
		{
			return equippedSword;
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

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left)
		{
			if (HandScript.instance.MyMoveable is Sword)
			{
				Sword tmp = (Sword)HandScript.instance.MyMoveable;
				EquipSword(tmp);
				CharacterPanel.instance.UpdateStatsText();
				GameController.instance.RefreshTooltip(tmp);
			}
			else if (HandScript.instance.MyMoveable == null && equippedSword != null)
			{
				HandScript.instance.TakeMoveable(equippedSword);
				CharacterPanel.instance.MySelectedSwordButton = this;
				icon.color = Color.grey;
			}


		}
	}

	public void EquipSword(Sword sword)
	{
		sword.Remove();

		if (equippedSword != null)
		{
			if (equippedSword != sword)
			{

				sword.Slot.AddItem(equippedSword);
				equippedSword.LoseStats();
			}

			GameController.instance.RefreshTooltip(equippedSword);
		}
		else
		{
			GameController.instance.HideTooltip();
		}


		icon.enabled = true;
		icon.sprite = sword.Icon;
		icon.color = Color.white;
		this.equippedSword = sword;
		sword.GetStats();
		
		if (HandScript.instance.MyMoveable == (sword as IMoveable))
		{
			HandScript.instance.Drop();
		}

	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (equippedSword != null)
		{
			GameController.instance.ShowTooltip(transform.position, new Vector2(0f, -0.15f), equippedSword);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameController.instance.HideTooltip();
	}

	public void DequipItem()
	{
		if (equippedSword != null) 
		{
			equippedSword.LoseStats();
			equippedSword = null;
		}
		
		icon.color = Color.white;
		icon.sprite = defaultIcon;
		CharacterPanel.instance.UpdateStatsText();
	}
}
