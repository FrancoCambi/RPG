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
	public override string GetDescription()
	{
		string description = string.Empty;
		if (this.description != string.Empty)
		{
			description = string.Format("\n<color=#808080FF>{0}</Color>", this.description);
		}
		
		return base.GetDescription() + string.Format("\n<color=#00ff00ff>Use: Try to refine your equipment's quality.</color>") + description;
	}

    public void OnDestroyHandler()
    {
        BORWindow.Instance.Close();
    }
}
