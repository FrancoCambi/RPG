using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class KeyBindManager : MonoBehaviour
{
	public static KeyBindManager instance;
	
	private string bindName;

	public Dictionary<string, (KeyCode, EventModifiers)> keyBinds { get; private set; }

	public Dictionary<string, (KeyCode, EventModifiers)> ActionBinds { get; private set; }



	// Start is called before the first frame update
	void Start()
	{
		keyBinds = new Dictionary<string, (KeyCode, EventModifiers)>();

		ActionBinds = new Dictionary<string, (KeyCode, EventModifiers)>();

		instance = this;

		DefaultKeyBinds();
	}

	public void BindKey(string key, KeyCode keyBind, EventModifiers modifier)
	{
		Dictionary<string, (KeyCode, EventModifiers)> currentDictionary = ActionBinds;

		if (key.Contains("ACT"))
		{
			currentDictionary = ActionBinds;
		}

		if (!currentDictionary.ContainsKey(key))
		{
			currentDictionary.Add(key, (keyBind, modifier));
			GameController.Instance.UpdateKeyText(key, keyBind, modifier);
		}
		else if (currentDictionary.ContainsValue((keyBind, modifier)))
		{
			string myKey = currentDictionary.FirstOrDefault(x => x.Value == (keyBind, modifier)).Key;

			currentDictionary[myKey] = (KeyCode.None, EventModifiers.None);

			GameController.Instance.UpdateKeyText(myKey, KeyCode.None, EventModifiers.None);


		}

		currentDictionary[key] = (keyBind, modifier);
		GameController.Instance.UpdateKeyText(key, keyBind, modifier);
		bindName = string.Empty;
	}

	public void KeyBindOnClick(string bindName)
	{
		this.bindName = bindName;
	}

	private void OnGUI()
	{
		if (bindName != string.Empty)
		{

			if (Event.current.modifiers != EventModifiers.None  && Event.current.keyCode != EventModifierToKeyCode(Event.current.modifiers))
			{
				BindKey(bindName, Event.current.keyCode, Event.current.modifiers);
			}

			else if (Event.current.modifiers == EventModifiers.None && Event.current.isKey)
			{
				BindKey(bindName, Event.current.keyCode, EventModifiers.None);
			}

			
		}
	}

	public void DefaultKeyBinds()
	{

		BindKey("ACT1", KeyCode.Alpha1, EventModifiers.None);
		BindKey("ACT2", KeyCode.Alpha2, EventModifiers.None);
		BindKey("ACT3", KeyCode.Alpha3, EventModifiers.None);
		BindKey("ACT4", KeyCode.Alpha4, EventModifiers.None);
		BindKey("ACT5", KeyCode.Alpha5, EventModifiers.None);
		BindKey("ACT6", KeyCode.F, EventModifiers.None);
		BindKey("ACT7", KeyCode.G, EventModifiers.None);
		BindKey("ACT8", KeyCode.Z, EventModifiers.None);
		BindKey("ACT9", KeyCode.X, EventModifiers.None);
		BindKey("ACT10", KeyCode.Alpha6, EventModifiers.None);
		BindKey("ACT11", KeyCode.Alpha7, EventModifiers .None);
		BindKey("ACT12", KeyCode.Alpha8, EventModifiers.None);
		BindKey("ACT13", KeyCode.Alpha9, EventModifiers.None);
		BindKey("ACT14", KeyCode.Alpha0, EventModifiers.None);



	}

	public KeyCode EventModifierToKeyCode(EventModifiers mod)
	{
		if (mod == EventModifiers.Alt)
		{
			return KeyCode.LeftAlt;
		}
		else if (mod == EventModifiers.Control)
		{
			return KeyCode.LeftControl;
		}
		else if (mod == EventModifiers.Shift)
		{
			return KeyCode.LeftShift;
		}
		else
		{
			return KeyCode.None;
		}
	}
	
}
