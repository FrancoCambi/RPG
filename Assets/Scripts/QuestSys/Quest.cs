using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
[CreateAssetMenu(fileName = "Quest", menuName = "Quests", order = 1)]
public class Quest : ScriptableObject
{
	[SerializeField]
	private string title;

	[SerializeField]
	private string description;

	[SerializeField]
	private CollectObjective[] collectObjectives;
	
	[SerializeField]
	private KillObjective[] killObjectives;
	
	[SerializeField]
	private int level;
	
	[SerializeField]
	private int xp;
	
	[SerializeField]
	private GoldCurrency gold;
	
	[SerializeField]
	private Reward[] rewards;
	
	[SerializeField]
	private bool allRewards;
	
	[SerializeField]
	private bool repeteable;

	public bool IsComplete
	{
		get
		{
			foreach (Objective o in collectObjectives)
			{
				if (!o.IsComplete)
				{
					return false;
				}
			}
			
			foreach (Objective o in killObjectives)
			{
				if (!o.IsComplete)
				{
					return false;
				}
			}

			return true;
		}
	}


	public QuestScript MyQuestScript { get; set; }
	
	public QuestGiver MyQuestGiver {get; set;}

	public string Title
	{
		get
		{
			return title;
		}
	}

	public string Description
	{
		get
		{
			return description;
		}
	}

	public CollectObjective[] CollectObjectives
	{
		get
		{
			return collectObjectives;
		}
	}
	
	public KillObjective[] KillObjectives 
	{
		get
		{
			return killObjectives;
		}
		set 
		{
			killObjectives = value;
		}
	}
	
		public bool Repeteable 
	{
		get 
		{
			return repeteable;
		}
	}
	
	public int Level 
	{
		get 
		{
			return level;
		}
	}
	
	public int Xp 
	{
		get 
		{
			return xp;
		}
	}
	
	public GoldCurrency Gold 
	{
		get 
		{
			return gold;
		}
	}
	
	public Reward[] Rewards 
	{
		get
		{
			return rewards;
		}
	}
	
	public bool AllRewards 
	{
		get 
		{
			return allRewards;
		}
	}
	
	public void Reset() 
	{
		foreach (CollectObjective o in collectObjectives) 
		{
			o.CurrentAmount = 0;
		}
		
		foreach (KillObjective o in killObjectives) 
		{
			o.CurrentAmount = 0;
		}
	}

}

[System.Serializable]
public abstract class Objective
{
	[SerializeField]
	private int amount;

	[SerializeField]
	private string type;

	private int currentAmount;

	public int Amount
	{
		get
		{
			return amount;
		}
	}

	public int CurrentAmount
	{
		get
		{
			return currentAmount;
		}
		set
		{
			currentAmount = value;
		}
	}

	public string Type
	{
		get
		{
			return type;
		}
	}

	public bool IsComplete
	{
		get
		{
			return CurrentAmount >= Amount;
		}
	}
	

}

[System.Serializable]
public class CollectObjective : Objective
{
	public void UpdateItemCount(Item item)
	{
		if (Type.ToLower() == item.ItemName.ToLower())
		{
			
			CurrentAmount = Inventory.Instance.GetItemCount(item.ItemName);
			
			if (CurrentAmount <= Amount) 
			{
				MessageFeedManager.instance.WriteMessage(string.Format("{0}: {1}/{2}", item.ItemName, CurrentAmount, Amount), FontSize.NORMAL, MessageType.Info, 2f);
				QuestLog.Instance.UpdateSelected();
				QuestLog.Instance.CheckCompletion();
			}
			
		}
	}
	
	public void UpdateItemCount()
	{
		
		CurrentAmount = Inventory.Instance.GetItemCount(Type);
		QuestLog.Instance.UpdateSelected();
		QuestLog.Instance.CheckCompletion();
		
	}
	
	public void Complete() 
	{
		Stack<Item> items = Inventory.Instance.GetItems(Type, Amount);
		
		foreach (Item item in items) 
		{
			item.Remove();
		}
	}

}

[System.Serializable]
public class KillObjective : Objective
{
	
	public void UpdateKillCount(Enemy enemy) 
	{
		if (Type == enemy.EnemyName) 
		{
			if (CurrentAmount < Amount) 
			{
				
				CurrentAmount++;
				MessageFeedManager.instance.WriteMessage(string.Format("{0}: {1}/{2}", enemy.EnemyName, CurrentAmount, Amount), FontSize.NORMAL, MessageType.Info, 2f);
				QuestLog.Instance.UpdateSelected();
				QuestLog.Instance.CheckCompletion();
			}
			
		}
	}

}

