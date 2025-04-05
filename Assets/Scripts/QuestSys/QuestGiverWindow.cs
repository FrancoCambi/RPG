using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestGiverWindow : MonoBehaviour
{
	public static QuestGiverWindow instance;
	
	[SerializeField]
	private CanvasGroup canvasGroup;
	
	[SerializeField]
	private GameObject questPrefab;
	
	[SerializeField]
	private Transform questArea;

	[SerializeField]
	private GameObject backBtn, acceptBtn, completeBtn, questDescription;
	
	private QuestGiver questGiver;
	
	private List<GameObject> quests = new();
	
	private Quest selectedQuest;

	public bool IsOpen 
	{
		get 
		{
			return canvasGroup.alpha > 0;
		}
	}
	
	private void Start() 
	{
		instance = this;
	}
	
	public void ShowQuests(QuestGiver giver) 
	{
		questArea.gameObject.SetActive(true);
		questDescription.SetActive(false);
		
		foreach (GameObject go in quests) 
		{
			Destroy(go);	
		}
		
		questGiver = giver;
		
		foreach (Quest quest in giver.Quests) 
		{
			if (quest != null) 
			{
				
				GameObject go = Instantiate(questPrefab, questArea);
				go.GetComponent<Text>().text = "["+ quest.Level + "] " + quest.Title;
				
				go.GetComponent<QGQeustScript>().MyQuest = quest;
				
				quests.Add(go);
				
				if (QuestLog.Instance.HasQuest(quest) && quest.IsComplete) 
				{
					go.GetComponent<Text>().text += " (C)";
				}
				else if (QuestLog.Instance.HasQuest(quest)) 
				{
					Color c = go.GetComponent<Text>().color;
					
					c.a = 0.5f;
					
					go.GetComponent<Text>().color = c;
				}
			}
			
		}
	}
	
	public void ShowQuestInfo(Quest quest) 
	{
		selectedQuest = quest;
		
		if (QuestLog.Instance.HasQuest(quest) && quest.IsComplete) 
		{
			acceptBtn.SetActive(false);
			completeBtn.SetActive(true);
		}
		else if (!QuestLog.Instance.HasQuest(quest))
		{
			acceptBtn.SetActive(true);
		}
		
		backBtn.SetActive(true);
		questArea.gameObject.SetActive(false);
		questDescription.SetActive(true);
		
		string objective = string.Empty;

		foreach (Objective obj in quest.CollectObjectives)
		{
			objective += obj.Type + ": " + obj.CurrentAmount + "/" + obj.Amount + "\n";
		}
		
		foreach (Objective obj in quest.KillObjectives)
		{
			objective += obj.Type + ": " + obj.CurrentAmount + "/" + obj.Amount + "\n";
		}

		string title = quest.Title;
		
		string onlyOneReward = string.Empty;
		
		if (!quest.AllRewards && quest.IsComplete) 
		{
			onlyOneReward = "Choose your reward.";
		}
		else if (!quest.AllRewards && !quest.IsComplete) 
		{
			onlyOneReward = "You will be able to choose one of these rewards:";
		}
		

		questDescription.GetComponent<Text>().text = string.Format("<size=15><b><color=#000000>{0}</color></b></size>\n" +
				"<size=10>{1}\n\n<size=15><b><color=#000000>Objectives</color></b></size>\n{2}</size>\n" + 
				"<size=15><b><color=#000000>Rewards</color></b></size>\n{3}", title, quest.Description, objective, onlyOneReward);
				
		
		RewardsManager.Instance.ClearRewards();
		RewardsManager.Instance.InstantiateRewards(quest.Rewards, quest);
		
	}
	
	public void CompleteQuest() 
	{
		if (RewardsManager.Instance.Selected == null && !selectedQuest.AllRewards)
		{
			MessageFeedManager.instance.WriteMessage("You need to select a reward.", FontSize.NORMAL, UnityEditor.MessageType.Warning, 2f);
		}
		else 
		{
			
			if (selectedQuest.IsComplete) 
			{
				for (int i = 0; i < questGiver.Quests.Length; i++) 
				{
					if (selectedQuest == questGiver.Quests[i] && !selectedQuest.Repeteable) 
					{
						questGiver.CompletedQuests.Add(selectedQuest.Title);
						questGiver.Quests[i] = null;
						selectedQuest.MyQuestGiver.UpdateQuestStatus();
					}
				}
				
				foreach (CollectObjective o in selectedQuest.CollectObjectives) 
				{
					Inventory.Instance.onItemCountChanged -= new ItemCountChanged(o.UpdateItemCount);
					o.Complete();
				}
				
				foreach (KillObjective o in selectedQuest.KillObjectives) 
				{
					GameEventsManager.instance.enemyDieEvents.enemyKilledEvent -= new EnemyKilled(o.UpdateKillCount);
				}
				
				if (selectedQuest.Repeteable) 
				{
					selectedQuest.Reset();			
				}
				
				PlayerStats.Instance.RecieveExp(XpManager.CalculateXp(selectedQuest));
				PlayerCurrency.Instance.GetMoney(selectedQuest.Gold);
				if (selectedQuest.AllRewards) 
				{
					foreach (Reward reward in selectedQuest.Rewards) 
					{
						for (int i = 0; i < reward.Count; i++) 
						{
							Item itemReward = Instantiate(reward.Item);
							
							if (itemReward is Armor) 
							{
								(itemReward as Armor).SetRandomQuality();
							}
							else if (itemReward is Sword)
							{
								(itemReward as Sword).SetRandomQuality();
							}
							
							Inventory.Instance.AddItem(itemReward);
						}
					}
				}
				else 
				{	
					for (int i = 0; i < RewardsManager.Instance.Selected.Count; i++)  
					{
						Item selected = GameController.Instance.InstantiateItemsAndEquip(RewardsManager.Instance.Selected.Item);
						Inventory.Instance.AddItem(selected);
					}
				}
				
				QuestLog.Instance.RemoveQuest(selectedQuest.MyQuestScript);
				RewardsManagerLog.Instance.ClearRewards();
				Back();
				
			}
		}
		
		
		
	}
	
	public void Back()
	{
		backBtn.SetActive(false);
		acceptBtn.SetActive(false);
		completeBtn.SetActive(false);
		
		ShowQuests(questGiver);
	}
	
	public void Accept() 
	{
		QuestLog.Instance.AcceptQuest(selectedQuest);
		Back();
	}

	public void Open(QuestGiver qg) 
	{
		if (questGiver == null) 
		{
			ShowQuests(qg);
		}
		
		questGiver = qg;
		canvasGroup.alpha = 1;
		canvasGroup.blocksRaycasts = true;
	}
	
	public void Close() 
	{
		questGiver = null;
		canvasGroup.alpha = 0;
		canvasGroup.blocksRaycasts = false;
	}
}
