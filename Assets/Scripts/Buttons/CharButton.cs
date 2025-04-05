using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CharButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private ArmorType armorType;

	private Armor equippedArmor;

	[SerializeField]
	private Sprite defaultIcon;

	[SerializeField]
	private Image icon;

	public Armor EquippedArmor
	{
		get
		{
			return equippedArmor;

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
			if (HandScript.Instance.MyMoveable is Armor)
			{
				Armor tmp = (Armor) HandScript.Instance.MyMoveable;

				if (tmp.ArmorType == armorType)
				{
					EquipArmor(tmp);
					CharacterPanel.Instance.UpdateStatsText();
					GameController.Instance.ShowTooltip(transform.position, new Vector2(0f,0f), equippedArmor);
				}

			}
			else if (HandScript.Instance.MyMoveable == null && equippedArmor != null)
			{
				HandScript.Instance.TakeMoveable(equippedArmor);
				CharacterPanel.Instance.MySelectedButton = this;
				icon.color = Color.grey;
			}

		}
	}

	public void EquipArmor(Armor armor)
	{
		armor.Remove();
		armor.GetStats();

		if (equippedArmor != null)
		{

			if (equippedArmor != armor)
			{
				if (armor.Slot != null) 
				{
					armor.Slot.AddItem(equippedArmor);
				}
				equippedArmor.LoseStats();
			}
			GameController.Instance.RefreshTooltip(equippedArmor);
		}
		else
		{
			GameController.Instance.HideTooltip();
		}

		icon.enabled = true;
		icon.sprite = armor.Icon;
		icon.color = Color.white;
		this.equippedArmor = armor;
		
		if (HandScript.Instance.MyMoveable == (armor as IMoveable))
		{
			HandScript.Instance.Drop();
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (equippedArmor != null)
		{
			GameController.Instance.ShowTooltip(transform.position, new Vector2(0f,0f), equippedArmor);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameController.Instance.HideTooltip();
	}

	public void DequipItem()
	{
		if (equippedArmor != null) 
		{
			equippedArmor.LoseStats();
			equippedArmor = null;
		}
		icon.color = Color.white;
		icon.sprite = defaultIcon;
		CharacterPanel.Instance.UpdateStatsText();
	}
}
