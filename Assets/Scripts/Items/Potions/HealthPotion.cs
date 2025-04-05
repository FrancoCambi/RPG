using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealtPotion", menuName = "Items/Potion", order = 1)]
public class HealthPotion : Item, IUseable
{
	[SerializeField]
	private int health;

	public void Use()
	{

		if (PlayerMeetsRequirements()) 
		{
			
			Remove();

			PlayerStats.Instance.GetHealth(health);
		}	
	}
	
	private string UseDescriptionEnglish() 
	{
		return string.Format("\n<color=#00ff00ff>Use: Restores {0} health</color>", health);
	}
	
	private string UseDescriptionSpanish() 
	{
		return string.Format("\n<color=#00ff00ff>Use: Restaura {0}p de vida</color>", health);
	}
	
	private string UseDescription() 
	{
		if (LanguageManager.Instance.CurrentLanguage == Language.English) 
		{
			return UseDescriptionEnglish();
		}	
		else 
		{
			return UseDescriptionSpanish();
		}
	}
	
	public override string GetDescription()
	{
		return base.GetDescription() + UseDescription();
	}

	
}
