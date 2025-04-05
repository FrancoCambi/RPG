using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BasicItem", menuName = "Items/BasicItem", order = 1)]
public class BasicItem : Item
{
	public override string GetDescription()
	{
		
		string description = string.Empty;
		if (this.Description != string.Empty)
		{
			description = string.Format("\n<color=#808080FF>{0}</Color>", this.Description);
		}
		
		return base.GetDescription() + description;
	}
}
