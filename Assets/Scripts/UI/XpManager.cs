using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class XpManager
{
	public static int CalculateXp(Enemy e) 
	{
		int playerLevel = PlayerStats.Instance.Level;
		
		int baseXP = (playerLevel * 5) + 45;
		
		int grayLevel = playerLevel - 10;
		
		int totalXp = 0;
		
		if (e.Level >= playerLevel) 
		{
			totalXp = (int)(baseXP * (1 + 0.05 * (e.Level - playerLevel)));
		}
		else if (e.Level > grayLevel)
		{
			totalXp = (int)(baseXP * (1 - (playerLevel - e.Level) / ZeroDifference()));

		}
		
		return totalXp + e.ExperiencePlus;
		
		
	}
	
	public static int CalculateXp(Quest q) 
	{
		int playerLevel = PlayerStats.Instance.Level;
		
		if (playerLevel <= q.Level + 5) 
		{
			return q.Xp;
		}
		else if (playerLevel == q.Level + 6) 
		{
			return ((int)(q.Xp * 0.8/5))*5;
		}
		else if (playerLevel == q.Level + 7) 
		{
			return ((int)(q.Xp * 0.6/5))*5;
		}
		else if (playerLevel == q.Level + 8) 
		{
			return ((int)(q.Xp * 0.4/5))*5;
		}
		else if (playerLevel == q.Level + 9) 
		{
			return ((int)(q.Xp * 0.2/5))*5;
		}
		else if (playerLevel == q.Level + 10) 
		{
			return ((int)(q.Xp * 0.1/5))*5;
		}
		
		return 0;

	}
	
	private static float ZeroDifference() 
	{
		int playerLevel = PlayerStats.Instance.Level;
		
		if (playerLevel <= 7) 
		{
			return 5f;
		}
		else if (playerLevel >= 8 && playerLevel <= 9) 
		{
			return 6f;
		}
		else if (playerLevel >= 10 && playerLevel <= 11) 
		{
			return 7f;
		}
		else if (playerLevel >= 12 && playerLevel <= 15) 
		{
			return 8f;
		}
		else if (playerLevel >= 16 && playerLevel <= 19) 
		{
			return 9f;
		}
		else if (playerLevel >= 20 && playerLevel <= 29) 
		{
			return 11f;
		}
		else if (playerLevel >= 30 && playerLevel <= 39) 
		{
			return 12f;
		}
		else if (playerLevel >= 40 && playerLevel <= 44) 
		{
			return 13f;
		}
		else if (playerLevel >= 45 && playerLevel <= 49) 
		{
			return 14f;
		}
		else if (playerLevel >= 50 && playerLevel <= 54) 
		{
			return 15f;
		}
		else if (playerLevel >= 55 && playerLevel <= 59) 
		{
			return 16f;
		}
		else
		{
			return 17f;
		}
	}
}
