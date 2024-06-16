using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestGiver : NPC, IInteractable
{
	[SerializeField]
	private Quest[] quests;
	
	[SerializeField]
	private Sprite question, exclamation;
	
	[SerializeField]
	private SpriteRenderer statusRenderer;
	
	[SerializeField]
	private QuestGiverWindow window;
	
	private List<string> completedQuests = new();

	public Quest[] Quests 
	{
		get 
		{
			return quests;
		}
	}
	
	public List<string> CompletedQuests 
	{
		get 
		{
			return completedQuests;
		}
		set 
		{
			completedQuests = value;	
			
			foreach (string title in completedQuests) 
			{
				for (int i = 0; i < quests.Length; i++) 
				{
					if (quests[i] != null && quests[i].Title == title) 
					{
						quests[i] = null;
					}
				}
			}
		}
	}
	
	private void Start() 
	{
		foreach (Quest quest in quests) 
		{
			quest.MyQuestGiver = this;
		}
		
		UpdateQuestStatus();
		
	}
	
	public void UpdateQuestStatus() 
	{
		int count = 0;
		
		foreach (Quest quest in quests) 
		{
			if (quest != null) 
			{
				if (quest.IsComplete && QuestLog.Instance.HasQuest(quest)) 
				{
					statusRenderer.sprite = question;
					statusRenderer.color = new Color32(255,184,0,255);
					break;
				}
				else if (!QuestLog.Instance.HasQuest(quest)) 
				{
					statusRenderer.sprite = exclamation;
					statusRenderer.color = new Color32(255,184,0,255);
				}
				else if (!quest.IsComplete && QuestLog.Instance.HasQuest(quest)) 
				{
					statusRenderer.sprite = question;
					statusRenderer.color = Color.grey;
				}
				
			}
			else 
			{
				count++;
				
				if (count == quests.Length) 
				{
					statusRenderer.enabled = false;
				}
			}
		}
	}

	public void Interact()
	{
		window.Open(this);
	}

	public void StopInteract()
	{
		window.Close();
	}
}
