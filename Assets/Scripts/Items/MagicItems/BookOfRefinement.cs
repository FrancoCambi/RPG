using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BookOfRefinement", menuName = "Items/MagicItems/BookOfRefinement", order = 1)]
public class BookOfRefinement : Item, IUseable, IDestroyHandler
{
	public void Use()
	{

		BORWindow.Instance.Open();
		BORWindow.Instance.Book = this;
		
	}
	
	
	private string UseDescription() 
	{
		return LanguageManager.Instance.Translate("\n<color=#00ff00ff>Use: Try to refine your equipment's quality.</color>",
			"\n<color=#00ff00ff>Uso: Intenta refinar tu equipamiento.</color>");
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
		BORWindow.Instance.Close();
	}
}
