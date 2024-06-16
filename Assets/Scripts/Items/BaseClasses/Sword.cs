using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Sword", menuName = "Items/Sword", order = 3)]
public class Sword : Item
{

    [Header("Damage")]
    [SerializeField]
    private int minDamage;

    [SerializeField]
    private int maxDamage;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float knockBackForce;

    [Header("Stats")]
    [SerializeField]
    private int stamina;

    [SerializeField]
    private int strength;

    [SerializeField]
    private int agility;

    [SerializeField]
    private int critPlus;

    public int MinDamage
    {
        get
        {
            return minDamage;
        }
    }

    public int MaxDamage
    {
        get
        {
            return maxDamage;
        }
    }

    public float Speed
    {
        get
        {
            return speed;
        }
    }

    public float KnockBackForce
    {
        get
        {
            return knockBackForce;
        }
    }

    public int Stamina
    {
        get
        {
            return stamina;
        }
    }

    public int Strength
    {
        get
        {
            return strength;
        }
    }

    public int Agility
    {
        get
        {
            return agility;
        }
    }

    public int CritPlus
    {
        get
        {
            return critPlus;
        }
    }

    public override string GetDescription()
    {
        string stats = String.Empty;
        string description = String.Empty;

        if (stamina > 0)
        {
            stats += String.Format("\n+{0} Stamina", stamina);
        }
        if (strength > 0)
        {
            stats += String.Format("\n+{0} Strength", strength);
        }
        if (agility > 0)
        {
            stats += String.Format("\n+{0} Agility", agility);
        }

        string damage = String.Format("\n{0} - {1} Damage\n{2} Speed", minDamage, maxDamage, speed);
        string equipCrit = String.Empty;
        
        if (this.critPlus > 0)
        {
            equipCrit = String.Format("\n<color=#00FF00FF>Equip: Increases critical strike chance by {0}%</Color>", this.critPlus);
        }

        if (this.description != String.Empty)
        {
            description = String.Format("\n<color=#808080FF>{0}</Color>", this.description);
        }

        return base.GetDescription() + damage + stats + equipCrit + description;
    }

    public void Equip()
    {
        CharacterPanel.instance.EquipSword(this);
    }

    public void GetStats()
    {
        PlayerStats stats = PlayerStats.Instance;

        stats.Stamina += stamina;
        stats.Strength += strength;
        stats.Agility += agility;
        stats.CritChance += critPlus;
        stats.MinDamage += minDamage;
        stats.MaxDamage += maxDamage;


    }

    public void LoseStats()
    {
        PlayerStats stats = PlayerStats.Instance;

        stats.Stamina -= stamina;
        stats.Strength -= strength;
        stats.Agility -= agility;
        stats.CritChance -= critPlus;
        stats.MinDamage -= minDamage;
        stats.MaxDamage -= maxDamage;


    }


}
