using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class QuestScript : MonoBehaviour
{
	public Quest MyQuest { get; set; }

	private bool markedComplete = false;

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{
		
	}

	public void Select()
	{
		GetComponent<Text>().color = Color.yellow;
		QuestLog.Instance.ShowDescription(MyQuest);
	}

	public void DeSelect()
	{
		GetComponent<Text>().color = Color.white;
	}

	public void IsComplete()
	{
		if (MyQuest.IsComplete && !markedComplete)
		{
			markedComplete = true;
			GetComponent<Text>().text = "["+ MyQuest.Level + "] " + MyQuest.Title + "(C)";
			MessageFeedManager.instance.WriteMessage(string.Format("{0} (Complete)", MyQuest.Title), FontSize.NORMAL, MessageType.Info, 2f);
		}
		else
		{
			markedComplete = false;
			GetComponent<Text>().text = "["+ MyQuest.Level + "] " + MyQuest.Title;
		}

	}
}
