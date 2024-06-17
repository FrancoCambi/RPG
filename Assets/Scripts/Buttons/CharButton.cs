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
			if (HandScript.instance.MyMoveable is Armor)
			{
				Armor tmp = (Armor) HandScript.instance.MyMoveable;

				if (tmp.ArmorType == armorType)
				{
					EquipArmor(tmp);
					CharacterPanel.instance.UpdateStatsText();
				}

				GameController.instance.RefreshTooltip(tmp);
			}
			else if (HandScript.instance.MyMoveable == null && equippedArmor != null)
			{
				HandScript.instance.TakeMoveable(equippedArmor);
				CharacterPanel.instance.MySelectedButton = this;
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

				armor.Slot.AddItem(equippedArmor);
				equippedArmor.LoseStats();
			}
			GameController.instance.RefreshTooltip(equippedArmor);
		}
		else
		{
			GameController.instance.HideTooltip();
		}

		icon.enabled = true;
		icon.sprite = armor.Icon;
		icon.color = Color.white;
		this.equippedArmor = armor;
		
		if (HandScript.instance.MyMoveable == (armor as IMoveable))
		{
			HandScript.instance.Drop();
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (equippedArmor != null)
		{
			GameController.instance.ShowTooltip(transform.position, new Vector2(0f,-0.15f), equippedArmor);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		GameController.instance.HideTooltip();
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
		CharacterPanel.instance.UpdateStatsText();
	}
}
