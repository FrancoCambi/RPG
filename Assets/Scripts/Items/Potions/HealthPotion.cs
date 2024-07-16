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
	public override string GetDescription()
	{
		return base.GetDescription() + string.Format("\n<color=#00ff00ff>Use: Restores {0} health</color>", health);
	}

	
}
