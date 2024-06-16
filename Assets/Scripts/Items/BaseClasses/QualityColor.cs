using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Quality
{
    Common, Uncommon, Rare, Epic, Legendary
}

public static class QualityColor
{
    private static Dictionary<Quality, string> colors = new()
    {
        {Quality.Common, "#FFFFFFFF" },
        {Quality.Uncommon, "#00FF00FF" },
        {Quality.Rare, "#0000FF" },
        {Quality.Epic, "#FF00FF" },
        {Quality.Legendary, "#FF8000" }

    };

    public static Dictionary<Quality, string> Colors
    {
        get
        {
            return colors;
        }
    }
}
