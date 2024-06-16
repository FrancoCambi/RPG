using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.AnimatedValues;
using UnityEngine;

public class Enemy : MonoBehaviour, IInteractable
{

	[Header("Misc")]
	
	[SerializeField]
	private AggroZone aggroZone;

	[SerializeField]
	private Collider2D lootCol;
	
	[SerializeField]
	private TextMeshProUGUI levelText;

	[Header("Info")]
	
	[SerializeField]
	private int id;
	
	[SerializeField]
	private int level;

	[SerializeField]	
	private string enemyName;
	
	[SerializeField]
	private int minDamage;
	
	[SerializeField]	
	private int maxDamage;
	
	[SerializeField]
	private float critChance;
	
	[SerializeField]
	private float knockbackForce;
	
	[SerializeField]
	private float moveSpeed = 1f;

	[Header("Loot")]
	
	[SerializeField]
	private int experiencePlus;
	
	[SerializeField]
	private LootTable lootTable;

	private Rigidbody2D rb;
	private bool isAlive = true;

	public bool IsAlive
	{
		get
		{
			return isAlive;
		}
		set
		{
			isAlive = value;
			lootCol.enabled = true;
		}
	}
	
	public TextMeshProUGUI LevelText 
	{
		get 
		{
			return levelText;
		}
	}
	
	public int Id 
	{
		get 
		{
			return id;
		}
	}
	
	public int Level 
	{
		get 
		{
			return level;
		}
	}
	
	public string EnemyName 
	{
		get 
		{
			return enemyName;
		}
	}
	
	public int ExperiencePlus
	{
		get 
		{
			return experiencePlus;
		}
	}
 

	void Start() 
	{
		rb = GetComponent<Rigidbody2D>();
		levelText.text = level.ToString();
	}

	void FixedUpdate() 
	{
		if (aggroZone.detectedObjs.Count > 0 && isAlive) 
		{
			Collider2D detectedObject0 = aggroZone.detectedObjs[0];
			Vector2 direction = (detectedObject0.transform.position - transform.position).normalized;

			rb.AddForce(moveSpeed * Time.fixedDeltaTime * direction);
		}

	}

	private (int, bool) CalculateDamage()
	{
		int damage = Random.Range(minDamage, maxDamage + 1);
		float randomFloat = Random.Range(0f, 100f);

		bool crit = false;
		if (randomFloat < critChance)
		{
			damage *= 2;
			crit = true;
		}

		return (damage, crit);
	}

	public void OnCollisionEnter2D(Collision2D col)
	{
		IDamageable damageableObject  = col.collider.GetComponent<IDamageable>();

		if (damageableObject != null && col.gameObject.CompareTag(aggroZone.tagTarget))
		{
			Vector2 direction = (Vector2)(col.gameObject.transform.position - transform.position).normalized;
			Vector2 knockback = direction * knockbackForce;

			(int, bool) damage = CalculateDamage();

			damageableObject.OnHit(damage.Item1, damage.Item2, knockback);
		}
	}

	public void Interact()
	{
		LootEnemy();
	}

	public void StopInteract()
	{
		LootWindow.instance.Close();

		
	}

	public void LootEnemy()
	{
		lootTable.ShowLoot(gameObject);
	}

}
