using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BOPWindow : MonoBehaviour
{
	private static BOPWindow instance;
	
	public static BOPWindow Instance 
	{
		get 
		{
			if (instance == null) 
			{
				instance = FindObjectOfType<BOPWindow>();
			}
			return instance;
		}
	}
	
	[SerializeField]
	private Text oddsText;
	
	[SerializeField]
	private CanvasGroup canvasGroup;
	
	[SerializeField]
	private PopUpWindowSlot slot;
	
	[SerializeField]
	private Image goldIcon;
	
	[SerializeField]
	private Text goldText;
	
	
	[SerializeField]
	private MaterialSlot matSlot1;
	
	[SerializeField]
	private MaterialSlot matSlot2;
	
	private Item toUpgrade;
	
	private Item book;
	
	private GoldCurrency gold;
	
	private Mat mat1;
	
	private Mat mat2;
	
	public Item ToUpgrade 
	{
		get
		{
			return toUpgrade;
		}
		set 
		{
			toUpgrade = value;
			SetOdds();
			SetMats();

		}
	}
	
	public Item Book 
	{
		get 
		{
			return book;
		}
		set 
		{
			book = value;
		}
	}
	
	public bool IsOpen 
	{
		get 
		{
			return canvasGroup.alpha == 1;
		}
	}
	
	public void Upgrade() 
	{
		if (toUpgrade != null && HaveMats() && !CastBar.Instance.Casting) 
		{
			string castText = LanguageManager.Instance.Translate("Upgrading...", "Mejorando...");
			
			CastBar.Instance.StartCast(castText, 2f, EndUpgrade);
		}
	}
	
	private void EndUpgrade() 
	{
		if (toUpgrade is Sword) 
		{
			(toUpgrade as Sword).TryUpgrade();
		}
		else 
		{
			(toUpgrade as Armor).TryUpgrade();
		}

		TakeMats();	
		SetOdds();
		SetMats();
		book.Remove();
	}
	
	private bool HaveMats() 
	{
		if (PlayerCurrency.Instance.MyGoldCurrency >= gold && Inventory.Instance.GetItemCount(mat1.Item.ItemName) >= mat1.Count && 
			Inventory.Instance.GetItemCount(mat2.Item.ItemName) >= mat2.Count) 
		{
			return true;
		}
		
		string enoughMaterialsText = LanguageManager.Instance.Translate("You don't have enough materials.", "No tienes suficientes materiales.");
		
		
		MessageFeedManager.instance.WriteMessage(enoughMaterialsText, FontSize.NORMAL, MessageType.Warning, 2f);
		return false;
	}
	
	private void SetOdds() 
	{
		string successOddsText = LanguageManager.Instance.Translate("Success: ", "Éxito: ");
		
		if (toUpgrade is Sword) 
		{
			
			oddsText.text = successOddsText + EquipmentUpgrade.Probs[(toUpgrade as Sword).Upgrade + 1] + "%";
		}
		else 
		{
			oddsText.text = successOddsText + EquipmentUpgrade.Probs[(toUpgrade as Armor).Upgrade + 1] + "%";
		}
	}
	
	private void SetMats() 
	{
		if (toUpgrade is Sword) 
		{
			Sword sword = toUpgrade as Sword;
			gold = EquipmentUpgrade.Mats[sword.Upgrade + 1].Item1;
			mat1 = EquipmentUpgrade.Mats[sword.Upgrade + 1].Item2;
			mat2 = EquipmentUpgrade.Mats[sword.Upgrade + 1].Item3;
			
			goldIcon.gameObject.SetActive(true);
			string goldWord = LanguageManager.Instance.Translate("Gold\n", "Oro\n");
			goldText.text = goldWord + gold.ToString(); 
			
			matSlot1.Item = mat1.Item;
			matSlot1.Icon.gameObject.SetActive(true);
			matSlot1.Icon.sprite = mat1.Item.Icon;
			matSlot1.ItemName.text = mat1.Item.ItemName + "\n" + mat1.Count.ToString();
			
			matSlot2.Item = mat2.Item;
			matSlot2.Icon.gameObject.SetActive(true);
			matSlot2.Icon.sprite = mat2.Item.Icon;
			matSlot2.ItemName.text = mat2.Item.ItemName + "\n" + mat2.Count.ToString();	
			
		}
		else if (toUpgrade is Armor) 
		{
			Armor sword = toUpgrade as Armor;
			gold = EquipmentUpgrade.Mats[sword.Upgrade + 1].Item1;
			mat1 = EquipmentUpgrade.Mats[sword.Upgrade + 1].Item2;
			mat2 = EquipmentUpgrade.Mats[sword.Upgrade + 1].Item3;
			
			goldIcon.gameObject.SetActive(true);
			string goldWord = LanguageManager.Instance.Translate("Gold\n", "Oro\n");
			goldText.text = goldWord + gold.ToString(); 
			
			matSlot1.Item = mat1.Item;
			matSlot1.Icon.gameObject.SetActive(true);
			matSlot1.Icon.sprite = mat1.Item.Icon;
			matSlot1.ItemName.text = mat1.Item.ItemName + "\n" + mat1.Count.ToString();
			
			matSlot2.Item = mat2.Item;
			matSlot2.Icon.gameObject.SetActive(true);
			matSlot2.Icon.sprite = mat2.Item.Icon;
			matSlot2.ItemName.text = mat2.Item.ItemName + "\n" + mat2.Count.ToString();	
			
		}
	}
	
	private void TakeMats() 
	{
		PlayerCurrency.Instance.TakeMoney(gold);
		Inventory.Instance.TakeItems(mat1.Item.ItemName, mat1.Count);
		Inventory.Instance.TakeItems(mat2.Item.ItemName, mat2.Count);
	}
	
	public void Open() 
	{
		BORWindow.Instance.Close();
		canvasGroup.alpha = 1;
		canvasGroup.blocksRaycasts = true;
	}
	public void Close() 
	{
		slot.Close();
		matSlot1.Clear();
		matSlot2.Clear();
		goldText.text = string.Empty;
		goldIcon.gameObject.SetActive(false);
		canvasGroup.alpha = 0;
		canvasGroup.blocksRaycasts = false;
		toUpgrade = null;
		oddsText.text = "NaN";
		CastBar.Instance.StopCast();
	}
	
}
