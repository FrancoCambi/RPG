using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemQuality
{
    Common, Uncommon, Rare, Epic, Legendary
}

public static class ItemQualityColor
{
    private static Dictionary<ItemQuality, string> colors = new()
    {
        {ItemQuality.Common, "#FFFFFFFF" },
        {ItemQuality.Uncommon, "#00FF00FF" },
        {ItemQuality.Rare, "#0000FF" },
        {ItemQuality.Epic, "#FF00FF" },
        {ItemQuality.Legendary, "#FF8000" }

    };

    public static Dictionary<ItemQuality, string> Colors
    {
        get
        {
            return colors;
        }
    }
}
