using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogEvents
{
    public event Action<string> onDialogFinished;
    public void DialogFinished(string name)
    {
        if (onDialogFinished != null)
        {
            onDialogFinished(name);
        }
    }
}
