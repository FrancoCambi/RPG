using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EnemyKilled(Enemy enemy);
public class EnemyDieEvents
{
	public event EnemyKilled enemyKilledEvent;
	public void OnEnemyKilled(Enemy enemy)
	{
		if (enemyKilledEvent != null)
		{
			enemyKilledEvent(enemy);
		}
	}

	public event Action<int> onGiveExp;
	public void GiveExp(int exp)
	{
		if (onGiveExp != null)
		{
			onGiveExp(exp);
		}
	}
}
