using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static UnityEditor.Progress;

public class VendorButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text itemNameText;

    [SerializeField]
    private Text priceText;

    [SerializeField]
    private Text quantityText;

    private LootWindow lootWindow;

    private VendorItem myItem;
    public Image Icon
    {
        get
        {
            return icon;
        }
    }

    public Text Title
    {
        get
        {
            return itemNameText;
        }
    }


    public void AddItem(VendorItem item)
    {
        myItem = item;

        icon.sprite = item.Item.Icon;
        if (item.Quantity > 0 || item.Unlimited)
        {
            icon.color = Color.white;
            priceText.color = Color.white;
            itemNameText.text = string.Format("<color={0}>{1}</color>", QualityColor.Colors[item.Item.Quality], item.Item.ItemName);

        }
        else
        {
            icon.color = Color.grey;
            priceText.color = Color.grey;
            itemNameText.text = string.Format("<color={0}>{1}</color>", "#808080", myItem.Item.ItemName);
        }

        if (!item.Unlimited)
        {
            quantityText.text = item.Quantity.ToString();
        }
        else
        {
            quantityText.text = string.Empty;
        }
        if (!item.Price.Free)
        {
            priceText.text = string.Format("Price: {0}g {1}s {2}c", item.Price.Gold, item.Price.Silver, item.Price.Copper);
        }
        else
        {
            priceText.text = "Free";
        }
        gameObject.SetActive(true);
        
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if ((myItem.Quantity > 0 || myItem.Unlimited) && (PlayerCurrency.instance.MyGoldCurrency >= myItem.Price) && Inventory.Instance.AddItem(Instantiate(myItem.Item)))
        {
            BuyItem();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameController.instance.ShowTooltip(transform.position, new Vector2(1, 0), myItem.Item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameController.instance.HideTooltip();
    }

    private void BuyItem()
    {
        PlayerCurrency.instance.MyGoldCurrency -= myItem.Price;

        if (!myItem.Unlimited)
        {
            myItem.Quantity--;
            quantityText.text = myItem.Quantity.ToString();

            if (myItem.Quantity == 0)
            {
                icon.color = Color.grey;
                itemNameText.text = string.Format("<color={0}>{1}</color>", "#808080", myItem.Item.ItemName);
                priceText.color = Color.grey;
            }
        }
    }

  
}
