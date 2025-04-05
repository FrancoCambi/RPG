using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public enum EquipmentQuality 
{
	None, Damaged, Low, Common, Useful, Good, Excellent, Ancient, Mysterious
}

public enum UpgradeDirection 
{
	Up, Down
}
public static class EquipmentQualityClass
{
	private static readonly List<EquipmentQuality> qualities = new() 
	{
		EquipmentQuality.Damaged,
		EquipmentQuality.Low,
		EquipmentQuality.Common,
		EquipmentQuality.Useful,
		EquipmentQuality.Good,
		EquipmentQuality.Excellent,
		EquipmentQuality.Ancient,
		EquipmentQuality.Mysterious
	};
	
	private static readonly Dictionary<EquipmentQuality, string> colors = new()
	{
		{EquipmentQuality.Damaged, "#86340F" },
		{EquipmentQuality.Low, "#FF6E63" },
		{EquipmentQuality.Common, "#FFFFFFFF" },
		{EquipmentQuality.Useful, "#7CBBFF" },
		{EquipmentQuality.Good, "#86FF86" },
		{EquipmentQuality.Excellent, "#00FF00" },
		{EquipmentQuality.Ancient, "#C9FF00" },
		{EquipmentQuality.Mysterious, "#FFFF00" }

	};
	
	private static readonly Dictionary<EquipmentQuality, int> singleProbs = new() 
	{
		{EquipmentQuality.Damaged, 1},
		{EquipmentQuality.Low, 5},
		{EquipmentQuality.Common, 30},
		{EquipmentQuality.Useful, 25},
		{EquipmentQuality.Good, 20},
		{EquipmentQuality.Excellent, 15},
		{EquipmentQuality.Ancient, 3},
		{EquipmentQuality.Mysterious, 1}
	};
	
	private static readonly Dictionary<EquipmentQuality, (int, int)> upDownProbs = new() 
	{
		{EquipmentQuality.Damaged, (1, 99)},
		{EquipmentQuality.Low, (5, 95)},
		{EquipmentQuality.Common, (10, 90)},
		{EquipmentQuality.Useful, (20, 80)},
		{EquipmentQuality.Good, (40, 60)},
		{EquipmentQuality.Excellent, (60, 40)},
		{EquipmentQuality.Ancient, (80, 20)},
		{EquipmentQuality.Mysterious, (100, 0)}
	};
	

	public static List<EquipmentQuality> Qualities 
	{
		get 
		{
			return qualities;
		}
	}

	public static Dictionary<EquipmentQuality, string> Colors
	{
		get
		{
			return colors;
		}
	}
	
	public static Dictionary<EquipmentQuality, int> SingleProbs 
	{
		get 
		{
			return singleProbs;
		}
	}
	
	public static Dictionary<EquipmentQuality, (int, int)> UpDownProbs 
	{
		get 
		{
			return upDownProbs;
		}
	}
	
	public static int Count 
	{
		get 
		{
			return Colors.Count;
		}
	}
	
	private static int ProbsSum(EquipmentQuality q1, EquipmentQuality q2) 
	{
		int count = 0;
		
		for (int i = (int)q1 - 1; i < (int)q2; i++) 
		{
			count += singleProbs[qualities[i]];
		}
		
		return count;
	}
	
	public static EquipmentQuality GetRandomQuality(EquipmentQuality original, UpgradeDirection direction) 
	{
		
		int randomNumber = Random.Range(0, 100);
		
		
		EquipmentQuality tmp;
		
		if (randomNumber >= 0 && randomNumber < singleProbs[qualities[0]]) 
		{
			tmp = qualities[0];
		}
		else if (randomNumber >= singleProbs[qualities[0]] && randomNumber < ProbsSum(qualities[0], qualities[1])) 
		{
			tmp = qualities[1];
		}
		else if (randomNumber >= ProbsSum(qualities[0], qualities[1]) && randomNumber < ProbsSum(qualities[0], qualities[2])) 
		{
			tmp = qualities[2];
		}
		else if (randomNumber >= ProbsSum(qualities[0], qualities[2]) && randomNumber < ProbsSum(qualities[0], qualities[3])) 
		{
			tmp = qualities[3];
		}
		else if (randomNumber >= ProbsSum(qualities[0], qualities[3]) && randomNumber < ProbsSum(qualities[0], qualities[4])) 
		{
			tmp = qualities[4];
		}
		else if (randomNumber >= ProbsSum(qualities[0], qualities[4]) && randomNumber < ProbsSum(qualities[0], qualities[5])) 
		{
			tmp = qualities[5];
		}
		else if (randomNumber >= ProbsSum(qualities[0], qualities[5]) && randomNumber < ProbsSum(qualities[0], qualities[6])) 
		{
			tmp = qualities[6];
		}
		else
		{
			tmp = qualities[7];
		}
		
		if (direction == UpgradeDirection.Up && (int)tmp < (int)original) 
		{ 
			return GetRandomQuality(original, direction);
		}
		else if (direction == UpgradeDirection.Down && (int)tmp > (int)original)
		{
			return GetRandomQuality(original, direction);
		}
		else 
		{
			return tmp;
		}
	
		
	}
}
