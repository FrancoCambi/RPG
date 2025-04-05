using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Language
{
	English, Spanish
}

public class LanguageManager : MonoBehaviour
{
	private static LanguageManager instance;
	
	public static LanguageManager Instance 
	{
		get 
		{
			if (instance == null) 
			{
				instance = FindObjectOfType<LanguageManager>();
			}
			return instance;
		}
	}
	
	private void Start() 
	{
		CurrentLanguage = Language.English;
	}
	
	public Language CurrentLanguage { get; private set; }
	
	public void SwitchLanguage() 
	{
		
		if (CurrentLanguage == Language.English) 
		{
			CurrentLanguage = Language.Spanish;
		}
		else 
		{
			CurrentLanguage = Language.English;
		}
		
		CharacterPanel.Instance.UpdateStatsText();
	}
	
	public string Translate(string englishString, string spanishString) 
	{
		if (CurrentLanguage == Language.English) 
		{
			return englishString;
		}
		else 
		{
			return spanishString;
		}
	}
}
