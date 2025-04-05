using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	[SerializeField]
	private GameObject enemyPrefab;
	
	[SerializeField]
	private CircleCollider2D wanderZone;
	
	[SerializeField]
	private float respawnTime;
	
	private Vector2 respawnPosition;
	
	public void StartRespawnEnemy(Vector2 pos) 
	{
		StartCoroutine(nameof(RespawnEnemy));
		respawnPosition = pos;
	}
	
	private IEnumerator RespawnEnemy() 
	{
		yield return new WaitForSeconds(respawnTime);
		Enemy enemy = Instantiate(enemyPrefab, transform).GetComponent<Enemy>();
		enemy.GetComponent<DamageableEnemy>().Manager = this;
		enemy.WanderZone = wanderZone;
		enemy.transform.localPosition = respawnPosition;
	}
	
}
