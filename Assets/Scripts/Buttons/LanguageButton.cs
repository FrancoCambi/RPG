using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class LanguageButton : MonoBehaviour
{
	public void SwitchLocale() 
	{
		if (LanguageManager.Instance.CurrentLanguage == Language.English) 
		{
			LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
			LanguageManager.Instance.SwitchLanguage();
		}
		else 
		{
			LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
			LanguageManager.Instance.SwitchLanguage();
			
		}
	}
}
