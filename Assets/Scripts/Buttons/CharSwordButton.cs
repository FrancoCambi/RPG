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
			if (HandScript.Instance.MyMoveable is Sword)
			{
				Sword tmp = (Sword)HandScript.Instance.MyMoveable;
				EquipSword(tmp);
				CharacterPanel.Instance.UpdateStatsText();
				GameController.Instance.ShowTooltip(transform.position, new Vector2(0f,0f), equippedSword);
			}
			else if (HandScript.Instance.MyMoveable == null && equippedSword != null)
			{
				HandScript.Instance.TakeMoveable(equippedSword);
				CharacterPanel.Instance.MySelectedSwordButton = this;
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
				if (sword.Slot != null) 
				{
					sword.Slot.AddItem(equippedSword);
				}
				equippedSword.LoseStats();
			}

			GameController.Instance.RefreshTooltip(equippedSword);
		}
		else
		{
			GameController.Instance.HideTooltip();
		}


		icon.enabled = true;
		icon.sprite = sword.Icon;
		icon.color = Color.white;
		this.equippedSword = sword;
		sword.GetStats();
		
		if (HandScript.Instance.MyMoveable == (sword as IMoveable))
		{
			HandScript.Instance.Drop();
		}

	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (equippedSword != null)
		{
			GameController.Instance.ShowTooltip(transform.position, new Vector2(0f, 0f), equippedSword);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameController.Instance.HideTooltip();
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
		CharacterPanel.Instance.UpdateStatsText();
	}
}
