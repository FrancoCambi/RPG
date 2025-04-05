using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FloatingTextType {DAMAGE, HEAL, XP, GOLD}
public class FloatingTextManager : MonoBehaviour
{
	private static FloatingTextManager instance;
	
	public static FloatingTextManager Instance 
	{
		get 
		{
			if (instance == null) 
			{
				instance = FindObjectOfType<FloatingTextManager>();
			}
			return instance;
		}
	}
	
	[SerializeField]
	private GameObject damagePrefab, healPrefab, xpPrefab, goldPrefab;
	
	private GameObject textPrefab;
	
	public void CreateText(Vector2 position, Canvas canvas, string text, FloatingTextType type) 
	{
		
		switch (type) 
		{
			case FloatingTextType.DAMAGE:
			textPrefab = damagePrefab;
			break;
			
			case FloatingTextType.HEAL:
			textPrefab = healPrefab;
			break;
			
			case FloatingTextType.XP:
			textPrefab = xpPrefab;
			break;
			
			case FloatingTextType.GOLD:
			textPrefab = goldPrefab;
			break;
		}
		
		Text textComp = textPrefab.GetComponent<Text>();
		textComp.text = text;
		RectTransform textR = Instantiate(textPrefab, canvas.transform).GetComponent<RectTransform>();
		textR.transform.position = position;
		
		
	}

	
}
