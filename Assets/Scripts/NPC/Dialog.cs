using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialog
{
	public List<string> linesEnglish;
	public List<string> linesSpanish;

	public List<string> Lines
	{
		get 
		{
			if (LanguageManager.Instance.CurrentLanguage == Language.English) 
			{
				return linesEnglish;
			}
			else 
			{
				return linesSpanish;
			}
		}

	}
}
