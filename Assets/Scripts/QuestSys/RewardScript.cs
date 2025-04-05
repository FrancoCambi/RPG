using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RewardScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	
	[SerializeField]
	private Image slotBackground;
	
	[SerializeField]
	private Image icon;
	
	[SerializeField]
	private TextMeshProUGUI count;
	
	[SerializeField]
	private Text title;
	
	public Image Icon 
	{
		get 
		{
			return icon;
		}
		set 
		{
			icon = value;
		}
	}
	
	public Text Title 
	{
		get 
		{
			return title;
		}
		set 
		{
			title = value;
		}
	}
	
	public TextMeshProUGUI Count 
	{
		get 
		{
			return count;
		}
	}
	
	public Reward MyReward {get; set;}
	public Quest MyQuest {get; set;}
	
	public void Select() 
	{
		if (MyQuest.IsComplete && !MyQuest.AllRewards) 
		{
			if (RewardsManager.Instance.Selected != null && RewardsManager.Instance.Selected != MyReward) 
			{
				RewardsManager.Instance.Selected.MyRewardScript.DeSelect();
			}
			slotBackground.color = new Color32(00, 0xFF, 0xFF, 0xFF);
			MyReward.MyRewardScript = this;
			RewardsManager.Instance.Selected = MyReward;
		}
		
	}
	
	public void DeSelect() 
	{
		slotBackground.color = Color.white;

	}

    public void OnPointerExit(PointerEventData eventData)
    {
        GameController.Instance.HideTooltip();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameController.Instance.ShowTooltip(transform.position, new Vector2(0f, 0f), MyReward.Item);
    }
}
