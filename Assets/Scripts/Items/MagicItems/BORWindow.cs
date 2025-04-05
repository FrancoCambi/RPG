using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class BORWindow : MonoBehaviour
{
	private static BORWindow instance;
	
	public static BORWindow Instance 
	{
		get 
		{
			if (instance == null) 
			{
				instance = FindObjectOfType<BORWindow>();
			}
			return instance;
		}
	}
	
	
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
	
	private Item toUpgrade;
	
	private Item book;
	
	private GoldCurrency gold;
	
	private Mat mat1;
	
	
	public Item ToUpgrade 
	{
		get
		{
			return toUpgrade;
		}
		set 
		{
			toUpgrade = value;
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
			string barText = LanguageManager.Instance.Translate("Refining...", "Refinando...");
			
			CastBar.Instance.StartCast(barText, 2f, EndUpgrade);
		}
	}
	
	private void EndUpgrade() 
	{
		string successText = LanguageManager.Instance.Translate("Refined successfully! New quality: ", "¡Refinación exitosa! Nueva calidad: ");
		if (toUpgrade is Sword) 
		{
			(toUpgrade as Sword).SetRandomQuality();
			MessageFeedManager.instance.WriteMessage(successText + (ToUpgrade as Sword).EquipmentQuality.ToString(), FontSize.NORMAL, MessageType.Warning, 2f);

		}
		else 
		{
			(toUpgrade as Armor).SetRandomQuality();

			MessageFeedManager.instance.WriteMessage(successText + (ToUpgrade as Armor).EquipmentQuality.ToString(), FontSize.NORMAL, MessageType.Warning, 2f);
		}
		TakeMats();	
		SetMats();
		book.Remove();
	}
	
	private bool HaveMats() 
	{
		if (PlayerCurrency.Instance.MyGoldCurrency >= gold && Inventory.Instance.GetItemCount(mat1.Item.ItemName) >= mat1.Count)
		{
			return true;
		}
		
		string enoughMaterialsText = LanguageManager.Instance.Translate("You don't have enough materials.", "No tienes suficientes materiales.");
		MessageFeedManager.instance.WriteMessage(enoughMaterialsText, FontSize.NORMAL, MessageType.Warning, 2f);
		return false;
	}
	
	
	private void SetMats() 
	{
		gold = new GoldCurrency(1,50,25);
		mat1 = new Mat("Yellow Gem", 3);
			
		goldIcon.gameObject.SetActive(true);
		string goldWord = LanguageManager.Instance.Translate("Gold\n", "Oro\n");
		goldText.text = goldWord + gold.ToString(); 
			
		matSlot1.Item = mat1.Item;
		matSlot1.Icon.gameObject.SetActive(true);
		matSlot1.Icon.sprite = mat1.Item.Icon;
		matSlot1.ItemName.text = mat1.Item.ItemName + "\n" + mat1.Count.ToString();

	}
	
	private void TakeMats() 
	{
		PlayerCurrency.Instance.TakeMoney(gold);
		Inventory.Instance.TakeItems(mat1.Item.ItemName, mat1.Count);
	}
	
	public void Open() 
	{
		BOPWindow.Instance.Close();
		canvasGroup.alpha = 1;
		canvasGroup.blocksRaycasts = true;
	}
	public void Close() 
	{
		slot.Close();
		matSlot1.Clear();
		goldText.text = string.Empty;
		goldIcon.gameObject.SetActive(false);
		canvasGroup.alpha = 0;
		canvasGroup.blocksRaycasts = false;
		toUpgrade = null;
		CastBar.Instance.StopCast();
	}
}
