using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public enum GameState { FreeRoam, Dialog, Config}
public class GameController : MonoBehaviour
{
	public static GameController instance;

	[SerializeField]
	private PlayerController playerController;

	[SerializeField]
	private RectTransform tooltipRect;

	[SerializeField]
	private GameObject tooltip;

	[SerializeField]
	private TextMeshProUGUI levelUiText;

	[SerializeField]
	private ActionButton[] actionButtons;

	[SerializeField]
	private CanvasGroup keyBindsGroup;

	private Text tooltipText;

	public GameState state = GameState.FreeRoam;

	private GameObject[] keyBindButtons;

	private void Awake()
	{
		keyBindButtons = GameObject.FindGameObjectsWithTag("Keybind");
	}

	private void Start()
	{
		DialogManager.Instance.OnShowDialog += () =>
		{
			state = GameState.Dialog;
		};
		DialogManager.Instance.OnHideDialog += () =>
		{
			if (state == GameState.Dialog)
				state = GameState.FreeRoam;
		};



		instance = this;
		tooltipText = tooltip.GetComponentInChildren<Text>();
		levelUiText.text = PlayerStats.Instance.Level.ToString();


	}

	private void Update()
	{
		// Game States

	   if (state == GameState.FreeRoam || state == GameState.Config)
	   {
			playerController.HandleUpdate();
	   }
	   else if (state == GameState.Dialog)
	   {
			DialogManager.Instance.HandleUpdate();
	   }

	   // Some keybinds for ui

	   if (Input.GetKeyDown(KeyCode.Escape))
	   {
			if (AnyUiOpen())
			{
				Inventory.Instance.Close();
				CharacterPanel.instance.Close();
				VendorWindow.instance.Close();
				LootWindow.instance.Close();
				QuestLog.Instance.Close();
				QuestGiverWindow.instance.Close();

			}
			else
			{

				OpenCloseMenu();

			}
	   }

	   if (Input.GetKeyDown(KeyCode.B))
	   {
			Inventory.Instance.OpenClose();
	   }

	   if (Input.GetKeyDown(KeyCode.P))
	   {
		   CharacterPanel.instance.OpenClose();
	   }
	   
	   if (Input.GetKeyDown(KeyCode.L)) 
	   {
			QuestLog.Instance.OpenClose();
	   }


		ClickTarget();

	   // DEBUG

	   if (Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.G))
	   {
		   PlayerCurrency.instance.MyGoldCurrency += new GoldCurrency(0,375,0);

	   }

	   // DEBUG



	}


	private bool AnyUiOpen()
	{
		return Inventory.Instance.IsOpen || CharacterPanel.instance.IsOpen || LootWindow.instance.IsOpen 
			   || VendorWindow.instance.IsOpen || QuestLog.Instance.IsOpen || QuestGiverWindow.instance.IsOpen;
	}

	private void ClickTarget()
	{
		if (Input.GetMouseButtonDown(1) && !EventSystem.current.IsPointerOverGameObject())
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, Mathf.Infinity, LayerMask.GetMask("Clickable"));

			if (hit.collider != null && state == GameState.FreeRoam)
			{
				playerController.Interact();
			}
		}
	}

	public void UpdateStackSize(IClickable clickable)
	{

		if (clickable.Count > 1)
		{
			clickable.StackText.text = clickable.Count.ToString();
			clickable.StackText.color = Color.white;
			clickable.Icon.color = Color.white;
		}
		else
		{
			clickable.StackText.color = new Color(0, 0, 0, 0);
			clickable.Icon.color = Color.white;
		}

		if (clickable.Count == 0)
		{
			clickable.Icon.color = new Color(0,0,0,0);
			clickable.StackText.color = new Color(0, 0, 0, 0);
		}
	}

	public void ShowTooltip(Vector3 position, Vector2 pivot, IDescribable description)
	{
		tooltipRect.pivot = pivot;
		tooltip.SetActive(true);
		tooltip.transform.position = position;
		tooltipText.text = description.GetDescription();
	}

	public void HideTooltip()
	{
		tooltip.SetActive(false);
	}

	public void RefreshTooltip(IDescribable description)
	{
		tooltipText.text = description.GetDescription();
	}

	public void OpenCloseMenu()
	{
		if (keyBindsGroup.alpha == 0)
		{
			state = GameState.Config;
		}
		else
		{
			state = GameState.FreeRoam;
		}

		keyBindsGroup.alpha = keyBindsGroup.alpha > 0 ? 0 : 1;
		keyBindsGroup.blocksRaycasts = keyBindsGroup.blocksRaycasts == false;
	}

	public void UpdateKeyText(string key, KeyCode code, EventModifiers mod)
	{
		Text tmp = Array.Find(keyBindButtons, x => x.name == key).GetComponentInChildren<Text>();

		string t = string.Empty;
		string m = string.Empty;

		if (code == KeyCode.Alpha0)
		{
			m = mod == EventModifiers.None ? "" : mod.ToString() + "+";

			t = string.Format("{0}0", m);
		}
		else if (code == KeyCode.Alpha1)
		{
			m = mod == EventModifiers.None ? "" : mod.ToString() + "+";
			t = string.Format("{0}1", m);
		}
		else if (code == KeyCode.Alpha2)
		{
			m = mod == EventModifiers.None ? "" : mod.ToString() + "+";
			t = string.Format("{0}2", m);
		}
		else if (code == KeyCode.Alpha3)
		{
			m = mod == EventModifiers.None ? "" : mod.ToString() + "+";
			t = string.Format("{0}3", m);
		}
		else if (code == KeyCode.Alpha4)
		{
			m = mod == EventModifiers.None ? "" : mod.ToString() + "+";
			t = string.Format("{0}4", m);
		}
		else if (code == KeyCode.Alpha5)
		{
			m = mod == EventModifiers.None ? "" : mod.ToString() + "+";
			t = string.Format("{0}5", m);
		}
		else if (code == KeyCode.Alpha6)
		{
			m = mod == EventModifiers.None ? "" : mod.ToString() + "+";
			t = string.Format("{0}6", m);

		}
		else if (code == KeyCode.Alpha7)
		{
			m = mod == EventModifiers.None ? "" : mod.ToString() + "+";
			t = string.Format("{0}7", m);
		}
		else if (code == KeyCode.Alpha8)
		{
			m = mod == EventModifiers.None ? "" : mod.ToString() + "+";
			t = string.Format("{0}8", m);
		}
		else if (code == KeyCode.Alpha9)
		{
			m = mod == EventModifiers.None ? "" : mod.ToString() + "+";
			t = string.Format("{0}9", m);
		}
		else
		{
			m = mod == EventModifiers.None ? "" : mod.ToString() + "+";
			t = string.Format("{0}{1}", m, code.ToString());
		}

		tmp.text = t;
	}

	public void UpdateLevelUiText()
	{
		GameEventsManager.instance.playerEvents.PlayerExpChange();
		levelUiText.text = PlayerStats.Instance.Level.ToString();
	}

	public void ClickActionButton(string buttonName)
	{
		Array.Find(actionButtons, x => x.gameObject.name == buttonName).OnClick();
	}

}
