using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ActionButton : MonoBehaviour, IPointerClickHandler, IClickable
{

    [SerializeField]
    private Image icon;

    [SerializeField]
    private Text stackText;

    private IUseable useable;

    private Stack<IUseable> useables = new();

    private int count;

    public Button Button { get; private set; }

    public Image Icon
    {
        get
        {
            return icon;
        }
        set
        {
            icon = value;
        }
    }

    public IUseable Useable
    {
        get
        {
            return useable;
        }
        set
        {
            useable = value;
        }
    }

    public int Count
    {
        get
        {
            return count;
        }
    }

    public Text StackText
    {
        get
        {
            return stackText;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        Button = GetComponent<Button>();
        Inventory.Instance.onItemCountChanged += new ItemCountChanged(UpdateItemCount);
    }

    public void OnClick()
    {
        if (useable != null)
        {
            useable.Use();
        }
        if (useables != null && useables.Count > 0)
        {
            useables.Peek().Use();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
       if (useable != null && eventData.button == PointerEventData.InputButton.Right)
       {
            useable.Use();
       }
       if (useables != null && useables.Count > 0 && eventData.button == PointerEventData.InputButton.Right)
       {
            useables.Peek().Use();
       }
       else if (eventData.button == PointerEventData.InputButton.Left)
       {
            if (HandScript.Instance.MyMoveable != null && HandScript.Instance.MyMoveable is IUseable)
            {
                SetUseable(HandScript.Instance.MyMoveable as IUseable);
            }
            else if (HandScript.Instance.MyMoveable == null)
            {
                if (Count > 0)
                {

                    HandScript.Instance.TakeMoveable(useables.Peek() as IMoveable);
                    Remove();
                }
            }
       }
    }

    public void SetUseable(IUseable useable)
    {
        if (useable is Item)
        {
            useables = Inventory.Instance.GetUSeables(useable);
            count = useables.Count;

            if (Inventory.Instance.FromSlot != null)
            {

                Inventory.Instance.FromSlot.Icon.color = Color.white;
                Inventory.Instance.FromSlot = null;
            }
        }
        else
        {

            this.Useable = useable;
        }

        UpdateVisual();
    }

    public void UpdateVisual()
    {
        Icon.sprite = HandScript.Instance.Put().Icon;
        Icon.color = Color.white;

        if (useables.Count > 1)
        {
            GameController.Instance.UpdateStackSize(this);
        }
    }

    public void UpdateItemCount(Item item)
    {
        if (item is IUseable && useables.Count > 0)
        {
            if (useables.Peek().GetType() == item.GetType())
            {
                useables = Inventory.Instance.GetUSeables(item as IUseable);

                count = useables.Count;

                GameController.Instance.UpdateStackSize(this);
            }
        }
    }

    public void Remove()
    {
        useables = new();
        count = 0;
        useable = null;
        Icon.color = new Color(0, 0, 0, 0);
        GameController.Instance.UpdateStackSize(this);
    }
}
