using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HandScript : MonoBehaviour
{
    public static HandScript instance;


    public IMoveable MyMoveable { get; set; }

    private Image icon;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        icon = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        icon.transform.position = Input.mousePosition;
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject() && instance.MyMoveable != null)
        {
            if (CharacterPanel.instance.MySelectedButton != null)
            {
                CharacterPanel.instance.MySelectedButton.Icon.color = Color.white;
                Drop();
                CharacterPanel.instance.MySelectedButton = null;
            }
            else if (CharacterPanel.instance.MySelectedSwordButton != null)
            {
                CharacterPanel.instance.MySelectedSwordButton.Icon.color = Color.white;
                Drop();
                CharacterPanel.instance.MySelectedSwordButton = null;
            }
            else
            {

                DeleteItem();
            }


        }

    }

    public void TakeMoveable(IMoveable moveable)
    {
        this.MyMoveable = moveable;
        icon.sprite = moveable.Icon;
        icon.color = Color.white;
    }

    public IMoveable Put()
    {
        IMoveable tmp = MyMoveable;

        MyMoveable = null;

        icon.color = new Color(0, 0, 0, 0);

        return tmp;
    }

    public void Drop()
    {
        MyMoveable = null;
        icon.color = new Color(0, 0, 0, 0);
        Inventory.Instance.FromSlot = null;
    }

    public void DeleteItem()
    {

        if (MyMoveable is Item && Inventory.Instance.FromSlot != null)
        {
            (MyMoveable as Item).Slot.Clear();
        }

        Drop();

        Inventory.Instance.FromSlot = null;
    }

}
