using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vendor : MonoBehaviour, IInteractable
{
    [SerializeField]
    private VendorItem[] items;
    
    public bool IsOpen { get; set; }

    public void Interact()
    {
        if (!IsOpen)
        {
            IsOpen = true;
            VendorWindow.instance.CreatePages(items);
            VendorWindow.instance.Open(this);

        }
    }

    public void StopInteract()
    {
        if (IsOpen)
        {
            IsOpen = false;
            VendorWindow.instance.Close();
        }
    }
    
}
