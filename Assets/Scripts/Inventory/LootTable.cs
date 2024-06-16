using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootTable : MonoBehaviour
{
    [SerializeField]
    private Loot[] loot;

    public List<Item> droppedItems = new();

    private bool rolled = false;

    
    public void ShowLoot(GameObject lootFrom)
    {
        if (!rolled)
        {
            RollLoot();

        }
        LootWindow.instance.CreatePages(droppedItems, lootFrom);
    }

    private void RollLoot()
    {
        foreach (Loot item in loot)
        {
            int roll = Random.Range(0, 100);

            if (roll <= item.DropChance)
            {
                droppedItems.Add(item.Item);
            }
        }

        rolled = true;
    }
}
