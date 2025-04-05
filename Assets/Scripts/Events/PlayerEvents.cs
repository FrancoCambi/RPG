using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents
{
    public event Action<int> onPlayerLevelChange;
    public void PlayerLevelChange(int level)
    {
        if (onPlayerLevelChange != null)
        {
            onPlayerLevelChange(level);
        }
    }

    public event Action onPlayerHealthChange;
    public void PlayerHealthChange()
    {
        if (onPlayerHealthChange != null)
        {
            onPlayerHealthChange();
        }
    }

    public event Action onPlayerGoldChange;
    public void PlayerGoldChange()
    {
        if (onPlayerGoldChange != null)
        {
            onPlayerGoldChange();
        }
    }
}
