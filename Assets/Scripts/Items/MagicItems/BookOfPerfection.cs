using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BookOfPerfection", menuName = "Items/MagicItems/BookOfPerfection", order = 1)]
public class BookOfPerfection : Item, IUseable, IDestroyHandler
{

	public void Use()
	{

		BOPWindow.Instance.Open();
		BOPWindow.Instance.Book = this;
		
	}
	
	
	private string UseDescription() 
	{
		return LanguageManager.Instance.Translate("\n<color=#00ff00ff>Use: Try to upgrade your equipment.</color>",
			"\n<color=#00ff00ff>Uso: Intenta mejorar tu equipamiento.</color>");
	}
	
	public override string GetDescription()
	{
		string description = string.Empty;
		if (this.Description != string.Empty)
		{
			description = string.Format("\n<color=#808080FF>{0}</Color>", this.Description);
		}

		return base.GetDescription() + UseDescription() + description;
	}

	public void OnDestroyHandler()
	{
		BOPWindow.Instance.Close();
	}
}
