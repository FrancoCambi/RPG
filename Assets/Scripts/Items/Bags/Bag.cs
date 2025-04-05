using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bag", menuName = "Items/Bag", order = 1)]
public class Bag : Item, IUseable
{
	[SerializeField]
	private int slotsCount;

	[SerializeField]
	private  GameObject bagPrefab;

	public BagScript MyBagScript { get; set; }

	public BagButton MyBagButton { get; set; }

	public int SlotsCount
	{
		get
		{
			return slotsCount;
		}
		
		set 
		{
			slotsCount = value;
		}
	}

	public void Use()
	{
		if (PlayerMeetsRequirements() && Inventory.Instance.CanAddBag)
		{
			Remove();
			MyBagScript = Instantiate(bagPrefab, Inventory.Instance.transform).GetComponent<BagScript>();
			MyBagScript.AddSlots(slotsCount);

			Inventory.Instance.AddBag(this);
			
			MyBagScript.MyBagIndex = MyBagButton.BagIndex;
		}
		
	}
	
	public void SetUpScript() 
	{
		MyBagScript = Instantiate(bagPrefab, Inventory.Instance.transform).GetComponent<BagScript>();
		MyBagScript.AddSlots(slotsCount);
	}

	public override string GetDescription()
	{
		string slotsText = LanguageManager.Instance.Translate("\n<color=#ffffff>{0} Slot bag</color>", 
			"\n<color=#ffffff>Bolsa de {0} espacios</color>");
		
		return base.GetDescription() + string.Format(slotsText, slotsCount);
	}


}
