using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Reward
{
	[SerializeField]
	private Item item;
	
	[SerializeField]
	private int count;
	
	public RewardScript MyRewardScript {get; set;}
	
	
	public Item Item 
	{
		get 
		{
			return item;
		}
		set 
		{
			item = value;
		}
	}
	
	public int Count 
	{
		get
		{
			return count;
		}
	}
	
}
