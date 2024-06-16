using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ArmorType { Head, Shoulders, Chest, Hands, Neck, Ring, Wrist, Waist, Legs, Feet, Sword  }

[CreateAssetMenu(fileName = "Armor", menuName = "Items/Armor", order = 2)]
public class Armor : Item
{
    [SerializeField]
    private ArmorType armorType;

    [SerializeField]
    private int stamina;

    [SerializeField]
    private int strength;

    [SerializeField]
    private int agility;

    [SerializeField]
    private int critPlus;

    [SerializeField]
    private int armor;

    public ArmorType ArmorType
    {
        get
        {
            return armorType;
        }
    }

    public override string GetDescription()
    {
        string stats = String.Empty;

        if (stamina > 0)
        {
            stats += String.Format("\n +{0} Stamina", stamina);
        }
        if (strength > 0)
        {
            stats += String.Format("\n +{0} Strength", strength);
        }
        if (agility > 0)
        {
            stats += String.Format("\n +{0} Agility", agility);
        }
        if (armor > 0)
        {
            stats += String.Format("\n {0} Armor", armor);
        }

        string equipCrit = String.Empty;

        if (this.critPlus > 0)
        {
            equipCrit = String.Format("\n<color=#00FF00FF>Equip: Increases critical strike chance by {0}%</Color>", this.critPlus);
        }

        string description = String.Empty;

        if (this.description != String.Empty)
        {
            description = String.Format("\n<color=#808080FF>{0}</Color>", this.description);
        }


        return base.GetDescription() + stats + equipCrit + description;
    }

    public void Equip()
    {
        CharacterPanel.instance.EquipArmor(this);
    }

    public void GetStats()
    {
        PlayerStats stats = PlayerStats.Instance;

        stats.Stamina += stamina;
        stats.Strength += strength;
        stats.Agility += agility;
        stats.CritChance += critPlus;
        stats.Armor += armor;
        stats.ArmorFromGear += armor;

        CharacterPanel.instance.UpdateStatsText();

    }

    public void LoseStats()
    {
        PlayerStats stats = PlayerStats.Instance;

        stats.Stamina -= stamina;
        stats.Strength -= strength;
        stats.Agility -= agility;
        stats.CritChance -= critPlus;
        stats.Armor -= armor;
        stats.ArmorFromGear -= armor;

        CharacterPanel.instance.UpdateStatsText();

    }
}
