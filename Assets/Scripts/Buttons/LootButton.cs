using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LootButton : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text title;

    private LootWindow lootWindow;

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
            return title;
        }
    }

    public Item Loot { get; set; }

    private void Awake()
    {
        lootWindow = GetComponentInParent<LootWindow>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Inventory.Instance.AddItem(Loot))
        {
            gameObject.SetActive(false);
            lootWindow.TakeLoot(Loot);
            GameController.instance.HideTooltip();
        }

        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameController.instance.ShowTooltip(transform.position, new Vector2(1,0), Loot);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameController.instance.HideTooltip();
    }
}
