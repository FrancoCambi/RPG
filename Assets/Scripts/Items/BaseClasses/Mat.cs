using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mat
{
	public Item Item {get; set;}
	
	public int Count {get; set;}
	
	public Mat(string itemName, int count) 
	{
		Item = Array.Find(SaveManager.Instance.Items, x => x.ItemNameEnglish == itemName);
		Count = count;
	}
}
