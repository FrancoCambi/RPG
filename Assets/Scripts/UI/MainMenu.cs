using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
	private static MainMenu instance;
	
	public static MainMenu Instance 
	{
		get 
		{
			if (instance == null) 
			{
				instance = FindObjectOfType<MainMenu>();
			}
			return instance;
		}
	}
	
	[SerializeField]
	private CanvasGroup mainMenuCanvasGroup;
	
	[SerializeField]
	private CanvasGroup keybindsCanvasGroup;
	
	[SerializeField]
	private CanvasGroup savingCanvasGroup;
	
	public bool IsOpen 
	{
		get 
		{
			return mainMenuCanvasGroup.alpha == 1;
		}
	}
	
	
	public void CloseMainMenu() 
	{
		mainMenuCanvasGroup.alpha = 0;
		mainMenuCanvasGroup.blocksRaycasts = false;
	}
	
	public void CloseKeybinds() 
	{
		if (keybindsCanvasGroup.alpha == 1) 
		{
			
			keybindsCanvasGroup.alpha = 0;
			keybindsCanvasGroup.blocksRaycasts = false;
			OpenMainMenu();
		}
		
			
	}
	
	public void CloseSaving() 
	{
		if (savingCanvasGroup.alpha == 1) 
		{
			savingCanvasGroup.alpha = 0;
			savingCanvasGroup.blocksRaycasts = false;
			OpenMainMenu();
			
		}
		
			
	}
	
	public void OpenMainMenu() 
	{
		mainMenuCanvasGroup.alpha = 1;
		mainMenuCanvasGroup.blocksRaycasts = true;
	}
	
	public void OpenKeybinds() 
	{
		keybindsCanvasGroup.alpha = 1;
		keybindsCanvasGroup.blocksRaycasts = true;
		
		CloseMainMenu();
	}
	
	public void OpenSaving() 
	{
		savingCanvasGroup.alpha = 1;
		savingCanvasGroup.blocksRaycasts = true;
		
		CloseMainMenu();
	}
	
	public bool AnyOpen() 
	{
		return mainMenuCanvasGroup.alpha == 1 || keybindsCanvasGroup.alpha == 1 || savingCanvasGroup.alpha == 1;
	}
	
	public bool AnyOpenBesidesMain() 
	{
		return keybindsCanvasGroup.alpha == 1 || savingCanvasGroup.alpha == 1;
	}
	
}
