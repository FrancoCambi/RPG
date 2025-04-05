using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public enum FontSize {SMALL, NORMAL, BIG}

public class MessageFeedManager : MonoBehaviour
{
	public static MessageFeedManager instance;
	
	[SerializeField]
	private GameObject messageInfoPrefab;
	
	[SerializeField]
	private GameObject messageWarningPrefab;
	
	private GameObject current;
	
	// Start is called before the first frame update
	void Start()
	{
		instance = this;
	}

	public void WriteMessage(string message, FontSize fontSize, MessageType type, float time) 
	{
	
		if (current != null) 
		{
			Destroy(current);
		}
		
		if (type == MessageType.Info) 
		{
			GameObject go = Instantiate(messageInfoPrefab, transform);
			Text goText = go.GetComponent<Text>();
			
			goText.color = GetColorByType(type);
			goText.text = message;
			goText.fontSize = GetFontSizeByType(fontSize);
			
			go.transform.SetAsFirstSibling();
			
		}
		else if (type == MessageType.Warning) 
		{
			GameObject go = Instantiate(messageWarningPrefab, transform);
			Text goText = go.GetComponentInChildren<Text>();
			
			goText.color = GetColorByType(type);
			goText.text = message;
			goText.fontSize = GetFontSizeByType(fontSize);
			
			go.transform.SetAsFirstSibling();
			
			current = go;
			
		}
		else 
		{
			Debug.LogWarning("Type not matching for writing message.");
		}
		
		
	}
	
	private Color GetColorByType(MessageType type) 
	{
		Color c = new();
		
		switch (type) 
		{
			case MessageType.Info:
			c = Color.yellow;
			break;
			
			case MessageType.Warning:
			c = Color.white;
			break;
			
			default:
			c = Color.white;
			break;
		}
		
		return c;
	}
	
	private int GetFontSizeByType(FontSize size) 
	{
		int fontSize = 12;
		
		switch (size) 
		{
			case FontSize.SMALL:
			fontSize = 14;
			break;
			
			case FontSize.NORMAL:
			fontSize = 16;
			break;
			
			case FontSize.BIG:
			fontSize = 18;
			break;
		}
		
		return fontSize;
	}
}
