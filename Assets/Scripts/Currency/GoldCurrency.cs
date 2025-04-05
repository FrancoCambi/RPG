using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GoldCurrency 
{
	[SerializeField]
	private int gold;

	[SerializeField]
	private int silver;

	[SerializeField]
	private int copper;

	public int Gold
	{
		get
		{
			return gold;
		}
		set
		{
			gold = value;



		}
	}

	public int Silver
	{
		get
		{
			return silver;
		}
		set
		{
			if (value >= 100)
			{
				int intNumber = (value / 100);
				Gold += intNumber;
				silver = (int)((((float)value / 100) - intNumber) * 100);
			}
			else if (value < 0)
			{
				Gold--;
				silver = 100 + value;
			}
			else
			{
				silver = value;
			}


		}
	}

	public int Copper
	{
		get
		{
			return copper;
		}
		set
		{
			if (value >= 100)
			{
				int intNumber = (value / 100);
				Silver += intNumber;
				copper = (int)((((float)value / 100) - intNumber) * 100);
			}
			else if (value < 0)
			{
				Silver--;
				copper = 100 + value;
			}
			else
			{
				copper = value;
			}

		}
	}

	public bool Free
	{
		get
		{
			return gold == 0 && silver == 0 && copper == 0;
		}
	}

	public GoldCurrency(int gold, int silver, int copper)
	{
		this.gold = gold;
		this.silver = silver;
		this.copper = copper;
	}

	public static GoldCurrency operator +(GoldCurrency a, GoldCurrency b)
	{
		GoldCurrency finalGoldCurrency = new GoldCurrency(0,0,0);

		finalGoldCurrency.Gold += a.Gold + b.Gold;
		finalGoldCurrency.Silver += a.Silver + b.Silver;
		finalGoldCurrency.Copper += a.Copper + b.Copper;

		return finalGoldCurrency;
	}

	public static GoldCurrency operator -(GoldCurrency a, GoldCurrency b)
	{
		GoldCurrency finalGoldCurrency = new GoldCurrency(0, 0, 0);

		finalGoldCurrency.Gold += a.Gold - b.Gold;
		finalGoldCurrency.Silver += a.Silver - b.Silver;
		finalGoldCurrency.Copper += a.Copper - b.Copper;

		return finalGoldCurrency;
	}

	public static bool operator >=(GoldCurrency a, GoldCurrency b)
	{
		if (a.Free && b.Free) 
		{
			return true;
		}
		
		else if (a.Free)
		{
			return false;
		}
		else if (b.Free)
		{
			return true;
		}


		int totalACopper = a.Gold * 10000 + a.Silver * 100 + a.Copper;
		int totalBCopper = b.Gold * 10000 + b.Silver * 100 + b.Copper;

		return totalACopper >= totalBCopper;
	}

	public static bool operator <=(GoldCurrency a, GoldCurrency b)
	{
		if (a.Free && b.Free) 
		{
			return true;
		}
		
		else if (a.Free)
		{
			return true;
		}
		else if (b.Free)
		{
			return false;
		}

		int totalACopper = a.Gold * 10000 + a.Silver * 100 + a.Copper;
		int totalBCopper = b.Gold * 10000 + b.Silver * 100 + b.Copper;

		return totalACopper <= totalBCopper;
	}
	
	public static bool operator <(GoldCurrency a, GoldCurrency b)
	{
		if (a.Free && b.Free) 
		{
			return true;
		}
		
		else if (a.Free)
		{
			return true;
		}
		else if (b.Free)
		{
			return false;
		}

		int totalACopper = a.Gold * 10000 + a.Silver * 100 + a.Copper;
		int totalBCopper = b.Gold * 10000 + b.Silver * 100 + b.Copper;

		return totalACopper < totalBCopper;
	}
	
		public static bool operator >(GoldCurrency a, GoldCurrency b)
	{
		if (a.Free && b.Free) 
		{
			return true;
		}
		
		else if (a.Free)
		{
			return true;
		}
		else if (b.Free)
		{
			return false;
		}

		int totalACopper = a.Gold * 10000 + a.Silver * 100 + a.Copper;
		int totalBCopper = b.Gold * 10000 + b.Silver * 100 + b.Copper;

		return totalACopper > totalBCopper;
	}

	public override string ToString()
	{
		return string.Format("{0}g {1}s {2}c", gold, silver, copper);
	}


}
