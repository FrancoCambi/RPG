using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class CastBar : MonoBehaviour
{
	private static CastBar instance;
	
	public static CastBar Instance 
	{
		get 
		{
			if (instance == null) 
			{
				instance = FindObjectOfType<CastBar>();
			}
			return instance;
		}
	}
	
	[SerializeField]
	private Image castBar;
	
	[SerializeField]
	private TextMeshProUGUI castTime;
	
	[SerializeField]
	private TextMeshProUGUI castName;
	
	[SerializeField]
	private CanvasGroup canvasGroup;
	
	private Coroutine routine;
	
	private bool casting = false;
	
	public bool Casting 
	{
		get 
		{
			return casting;
		}
	}
	
	private IEnumerator Cast(float time, Action whenDone) 
	{
		casting = true;
		
		float timePassed = Time.deltaTime;
		
		float rate = 1.0f / time;
		
		float progress = 0.0f;
		
		castBar.fillAmount = 0;
		
		while (progress <= 1.0f) 
		{
			castBar.fillAmount = Mathf.Lerp(0, 1, progress);
			
			progress += rate * Time.deltaTime;
			
			timePassed += Time.deltaTime;
			
			castTime.text = (time - timePassed).ToString("F2");
			
			yield return null;
		}

		canvasGroup.alpha = 0;
		canvasGroup.blocksRaycasts = false;
		whenDone?.Invoke();
		routine = null;
		casting = false;
	}
	
	public void StartCast(string name, float time, Action whenDone) 
	{
		canvasGroup.alpha = 1;
		canvasGroup.blocksRaycasts = true;
		castName.text = name;
		routine = StartCoroutine(Cast(time, whenDone));
		
	}
	
	public void StopCast() 
	{
		if (routine != null) 
		{
			StopCoroutine(routine);
			canvasGroup.alpha = 0;
			canvasGroup.blocksRaycasts = false;
			routine = null;
			casting = false;
		}
	}
	
}
