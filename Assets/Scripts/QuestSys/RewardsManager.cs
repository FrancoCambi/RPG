using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RewardsManager : MonoBehaviour
{
	private static RewardsManager instance;
	
	public static RewardsManager Instance 
	{
		get 
		{
			if (instance == null) 
			{
				instance = FindObjectOfType<RewardsManager>();
			}
			
			return instance;
		}
	}
	
	
	[SerializeField]
	private GameObject rewardPrefab;
	
	public Reward Selected {get; set;}
	
	public List<GameObject> rewardsList {get; set;}
	
	private void Start()
	{
		rewardsList = new();
	}
	
	public void InstantiateRewards(Reward[] rewards, Quest quest) 
	{
		foreach (Reward reward in rewards) 
		{
			RewardScript rewScript  = Instantiate(rewardPrefab, transform).GetComponent<RewardScript>();
			
			rewScript.MyReward = reward;
			rewScript.MyQuest = quest;
			rewScript.Icon.sprite = reward.Item.Icon;
			
			string count = string.Empty;
			if (reward.Count > 1) 
			{
				count = reward.Count.ToString();
			}
			
			rewScript.Count.text = count;
			rewScript.Title.text = reward.Item.ItemName;
			
			
			rewardsList.Add(rewScript.gameObject);
			
			
		}
	}
	
	public void ClearRewards() 
	{
		foreach (GameObject go in rewardsList) 
		{
			Destroy(go);
		}
		
		Selected = null;
	}

}
