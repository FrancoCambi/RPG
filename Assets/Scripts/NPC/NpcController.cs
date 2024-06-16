using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcController : NPC, IInteractable
{
    public Dialog dialog;

    public void Interact()
    {

        StartCoroutine(DialogManager.Instance.ShowDialog(dialog, NpcName));

    }

    public void StopInteract()
    {

    }
}
