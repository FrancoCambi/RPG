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
	public override string GetDescription()
	{
		string description = string.Empty;
		if (this.description != string.Empty)
		{
			description = string.Format("\n<color=#808080FF>{0}</Color>", this.description);
		}
		
		return base.GetDescription() + string.Format("\n<color=#00ff00ff>Use: Try to upgrade your equipment.</color>") + description;
	}

    public void OnDestroyHandler()
    {
        BOPWindow.Instance.Close();
    }
}
