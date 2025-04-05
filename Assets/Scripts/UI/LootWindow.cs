using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LootWindow : MonoBehaviour
{
	public static LootWindow instance;

	[SerializeField]
	private LootButton[] lootButtons;

	[SerializeField]
	private Text pageNumberText;

	[SerializeField]
	private GameObject nextButton, prevButton;

	[SerializeField]
	private Item[] items;

	private List<List<Item>> pages = new();

	private List<Item> droppedLoot = new();

	private CanvasGroup canvasGroup;

	private int pageIndex = 0;

	private GameObject lootFrom;


	public bool IsOpen
	{
		get
		{
			return canvasGroup.alpha > 0;
		}
	}

   

	private void Awake()
	{
		canvasGroup = GetComponent<CanvasGroup>();
	}

	// Start is called before the first frame update
	void Start()
	{
		instance = this;

	}

	public void CreatePages(List<Item> items, GameObject lootFrom)
	{
		this.lootFrom = lootFrom;

		if (!IsOpen)
		{
			List<Item> page = new();

			droppedLoot = items;

			for (int i = 0; i < items.Count; i++)
			{
				page.Add(items[i]);

				if (page.Count == 5 || i == items.Count - 1)
				{
					pages.Add(page);
					page = new();
				}
			}

		}


		AddLoot();

		Open();
	}


	private void AddLoot()
	{

		if (pages.Count > 0)
		{
			pageNumberText.text = pageIndex + 1 + "/" + pages.Count;

			prevButton.SetActive(pageIndex > 0);
			nextButton.SetActive(pages.Count > 1 && pageIndex < pages.Count - 1);

			for (int i = 0; i < pages[pageIndex].Count; i++)
			{
				if (pages[pageIndex][i] != null)
				{
					lootButtons[i].Icon.sprite = pages[pageIndex][i].Icon;

					lootButtons[i].Loot = pages[pageIndex][i];

					string title = string.Format("<color={0}>{1}</color>", ItemQualityColor.Colors[pages[pageIndex][i].ItemQuality], pages[pageIndex][i].ItemName);

					lootButtons[i].Title.text = title;

					lootButtons[i].gameObject.SetActive(true);

				}


			} 

		}


	}

	public void ClearButtons()
	{
		foreach (LootButton button in lootButtons)
		{
			button.gameObject.SetActive(false);
		}
	}

	public void NextPage()
	{
		if (pageIndex < pages.Count - 1)
		{
			pageIndex++;
			ClearButtons();
			AddLoot();
		}
	}

	public void PreviousPage()
	{
		if (pageIndex > 0)
		{
			pageIndex--;
			ClearButtons();
			AddLoot();
		}
	}

	public void TakeLoot(Item item)
	{
		pages[pageIndex].Remove(item);

		droppedLoot.Remove(item);
		

		if (pages[pageIndex].Count == 0)
		{
			pages.Remove(pages[pageIndex]);

			if (pageIndex == pages.Count && pageIndex > 0)
			{
				pageIndex--;
			}

			AddLoot();
		}
	}

	public void Open()
	{
		canvasGroup.alpha = 1;
		canvasGroup.blocksRaycasts = true;
	}


	public void Close()
	{

		canvasGroup.alpha = 0;
		canvasGroup.blocksRaycasts = false;
		pages.Clear();
		ClearButtons();
		GameController.Instance.HideTooltip();

		if (droppedLoot.Count == 0)
		{
			if (lootFrom != null && lootFrom.GetComponent<DamageableEnemy>() != null) 
			{

				lootFrom.GetComponent<DamageableEnemy>().RemoveEnemy();
			}
		}

	   
	}
}
