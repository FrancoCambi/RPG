using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QGQeustScript : MonoBehaviour
{
	public Quest MyQuest { get; set; }
	
	public void Select() 
	{
		QuestGiverWindow.instance.ShowQuestInfo(MyQuest);
	}
}
