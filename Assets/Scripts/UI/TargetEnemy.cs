using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TargetEnemy : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	[SerializeField]
	private CanvasGroup canvasGroupChange;
	
	[SerializeField]
	private CanvasGroup canvasGroupName;
	
	[SerializeField]
	private DamageableEnemy enemy;

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (enemy.ShowingHealth != true) 
		{
			canvasGroupChange.alpha = 1;
			canvasGroupName.alpha = 1;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (enemy.ShowingHealth != true) 
		{
			canvasGroupChange.alpha = 0;
			canvasGroupName.alpha = 0;
		}
	}
}
