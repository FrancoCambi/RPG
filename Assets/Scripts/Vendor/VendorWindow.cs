using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VendorWindow : MonoBehaviour
{
    public static VendorWindow instance;

    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private VendorButton[] buttons;

    [SerializeField]
    private Text pageNumberText;

    private List<List<VendorItem>> pages = new();

    private int pageIndex = 0;

    private Vendor vendor;
    public bool IsOpen
    {
        get
        {
            return canvasGroup.alpha > 0;
        }
    }

    private void Start()
    {
        instance = this;
    }
    public void CreatePages(VendorItem[] items)
    {
        pages.Clear();

        List<VendorItem> page = new();

        for (int i = 0; i < items.Length; i++)
        {
            page.Add(items[i]);

            if (page.Count == 12 || i == items.Length - 1)
            {
                pages.Add(page);
                page = new();
            }
        }

        AddItems();
    }

    public void AddItems()
    {
        pageNumberText.text = pageIndex + 1 + "/" + pages.Count;

        if (pages.Count > 0)
        {
            for (int i = 0; i < pages[pageIndex].Count; i++)
            {
                if (pages[pageIndex][i] != null)
                {
                    buttons[i].AddItem(pages[pageIndex][i]);
                }
            }
        }
    }

    public void Open(Vendor vendor)
    {
        this.vendor = vendor;
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void Close()
    {
        if (vendor != null)
        {
            vendor.IsOpen = false;

        }
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;
        vendor = null;
        pageIndex = 0;
    }

    public void NextPage()
    {
        if (pageIndex < pages.Count - 1)
        {
            ClearButtons();
            pageIndex++;
            AddItems();
        }
    }

    public void PreviousPage()
    {
        if (pageIndex > 0)
        {
            ClearButtons();
            pageIndex--;
            AddItems();
        }
    }

    public void ClearButtons()
    {
        foreach (VendorButton btn in buttons)
        {
            btn.gameObject.SetActive(false);
        }
    }
}
