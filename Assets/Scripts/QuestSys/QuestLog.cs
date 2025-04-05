using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class QuestLog : MonoBehaviour
{
	private static QuestLog instance;

	[SerializeField]
	private GameObject questPrefab;

	[SerializeField]
	private Transform questList;

	[SerializeField]
	private Text questDescription;
	
	[SerializeField]
	private CanvasGroup canvasGroup;
	
	[SerializeField]
	private Text questCountText;
	
	[SerializeField]
	private int maxCount;
	
	private int currentCount;

	private Quest selected;

	private List<QuestScript> questScripts = new();
	
	private List<Quest> quests = new();
	
	public static QuestLog Instance 
	{
		get 
		{
			if (instance == null) 
			{
				instance = FindObjectOfType<QuestLog>();
			}
			
			return instance;
		}
	}
	
	public bool IsOpen 
	{
		get 
		{
			return canvasGroup.alpha > 0;
		}
	}
	
	public List<Quest> Quests 
	{
		get 
		{
			return quests;
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		instance = this;
		
		questCountText.text = currentCount + "/" + maxCount;   
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void AcceptQuest(Quest quest)
	{
		if (currentCount < maxCount) 
		{
			currentCount++;
			questCountText.text = currentCount + "/" + maxCount; 
			
			foreach (CollectObjective o in quest.CollectObjectives)
			{
				Inventory.Instance.onItemCountChanged += new ItemCountChanged(o.UpdateItemCount);
				
				o.UpdateItemCount();
			}
			
			foreach (KillObjective o in quest.KillObjectives) 
			{
				GameEventsManager.instance.enemyDieEvents.enemyKilledEvent += new EnemyKilled(o.UpdateKillCount);
			}
			
			quests.Add(quest);

			GameObject go = Instantiate(questPrefab, questList);

			QuestScript qs = go.GetComponent<QuestScript>();
			quest.MyQuestScript = qs;
			qs.MyQuest = quest;

			go.GetComponent<Text>().text = quest.Title;

			questScripts.Add(qs);
			
			CheckCompletion();
		}
		else 
		{
			MessageFeedManager.instance.WriteMessage("Your quest log is full.", FontSize.NORMAL, MessageType.Warning, 3f);
		}
		
	}
	
	public void RemoveQuest(QuestScript qs) 
	{
		questScripts.Remove(qs);
		Destroy(qs.gameObject);
		quests.Remove(qs.MyQuest);
		questDescription.text = string.Empty;
		selected = null;
		currentCount--;
		questCountText.text = currentCount + "/" + maxCount; 
		qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
		qs = null;
	}
	
	public void AbandonQuest() 
	{
		if (selected != null) 
		{
			foreach (CollectObjective o in selected.CollectObjectives) 
			{
				Inventory.Instance.onItemCountChanged -= new ItemCountChanged(o.UpdateItemCount);
			}
				
			foreach (KillObjective o in selected.KillObjectives) 
			{
				GameEventsManager.instance.enemyDieEvents.enemyKilledEvent -= new EnemyKilled(o.UpdateKillCount);
			}
			
			RemoveQuest(selected.MyQuestScript);
		}
		
	}
	

	public void UpdateSelected()
	{
		ShowDescription(selected);
	}

	public void ShowDescription(Quest quest)
	{
		if (quest != null)
		{

			if (selected != null && selected != quest)
			{
				selected.MyQuestScript.DeSelect();


			}

			selected = quest;
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
			
			if (!quest.AllRewards) 
			{
				onlyOneReward = "You will be able to choose one of these rewards:";
			}
	
			questDescription.text = string.Format("<size=15><b>{0}</b></size>\n" +
				"<size=10>{1}\n\n<size=15><b>Objectives</b></size>\n{2}</size>\n" +
				"<size=15><b>Rewards</b></size>\n{3}", title, quest.Description, objective, onlyOneReward);

			
			RewardsManagerLog.Instance.ClearRewards();
			RewardsManagerLog.Instance.InstantiateRewards(quest.Rewards, quest);
			
		}
	}

	public void CheckCompletion()
	{
		foreach (QuestScript qs in questScripts)
		{
			qs.MyQuest.MyQuestGiver.UpdateQuestStatus();
			qs.IsComplete();
		}
	}
	
	public bool HasQuest(Quest quest) 
	{
		return quests.Exists(x => x.Title == quest.Title);
	}
	
	public void Open() 
	{
		canvasGroup.alpha = 1;
		canvasGroup.blocksRaycasts = true;
	}
	
	public void Close() 
	{
		
		canvasGroup.alpha = 0;
		canvasGroup.blocksRaycasts = false;
	}
	
	public void OpenClose()
	{
		if (canvasGroup.alpha == 1) 
		{
			Close();
		}
		else 
		{
			Open();
		}
	}
	
	
}
