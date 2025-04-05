using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
	public GameObject dialogBox;
	public Text dialogText;
	public Text npcNameText;
	public int lettersPerSecond;
	public event Action OnShowDialog;
	public event Action OnHideDialog;

	public static DialogManager Instance {  get; private set; }

	private Dialog dialog;
	private int currentLine = 0;
	private bool isTyping;
	private string npcName;
	
	private string playerName = "Knvi";
	
	public string PlayerName 
	{
		get 
		{
			return playerName;
		}
		set 
		{
			playerName = value;
		}
	}

	private void Awake()
	{
		Instance = this;
	}

	public void HandleUpdate()
	{
		if (Input.GetKeyDown(KeyCode.E) && !isTyping)
		{
			++currentLine;

			if (currentLine < dialog.Lines.Count)
			{
				StartCoroutine(TypeDialog(dialog.Lines[currentLine]));
			}
			else
			{

				currentLine = 0;
				dialogBox.SetActive(false);
				OnHideDialog?.Invoke();
			}
		}
	}

	public IEnumerator ShowDialog(Dialog dialog, string npcName)
	{
		this.npcName = npcName;

		yield return new WaitForEndOfFrame();

		OnShowDialog?.Invoke();
		this.dialog = dialog;
		npcNameText.text = npcName;
		dialogBox.SetActive(true);
		StartCoroutine(TypeDialog(dialog.Lines[0]));
	}

	public IEnumerator TypeDialog(string line)
	{
		string formattedLine = FormatLine(line);
		isTyping = true;
		dialogText.text = "";

		foreach (var letter in formattedLine.ToCharArray())
		{
			dialogText.text += letter;
			yield return new WaitForSeconds(1f / lettersPerSecond);
		}
		isTyping = false;
	}
	
	private string FormatLine(string line) 
	{
		string newLine = line.Replace("{playerName}", PlayerName);
		
		return newLine;
	}
}
