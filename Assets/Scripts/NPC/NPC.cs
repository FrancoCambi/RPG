using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
	[SerializeField]
	private string npcName;
	
	[SerializeField]
	private int id;
	
	public string NpcName 
	{
		get 
		{
			return npcName;
		}
	}
	
	public int Id 
	{
		get 
		{
			return id;
		}
	}
	
	
}
