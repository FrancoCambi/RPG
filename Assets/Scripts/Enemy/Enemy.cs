using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.EventSystems;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Enemy : MonoBehaviour, IInteractable
{

	[Header("Misc")]
	
	[SerializeField]
	private AggroZone aggroZone;
	
	[SerializeField]
	private AttackZone attackZone;
	
	[SerializeField]
	private CircleCollider2D wanderZone;
	
	[SerializeField]
	private int maxWanderCd;

	[SerializeField]
	private Collider2D lootCol;
	
	[SerializeField]
	private TextMeshProUGUI levelText;
	
	[SerializeField]
	private TextMeshProUGUI nameText;

	[Header("Info")]
	
	[SerializeField]
	private int id;
	
	[SerializeField]
	private int level;

	[SerializeField]	
	private string enemyName;
	
	[SerializeField]
	private EnemyAttackType attackType;
	
	[SerializeField]
	private int minDamage;
	
	[SerializeField]	
	private int maxDamage;
	
	[SerializeField]
	private float critChance;
	
	[SerializeField]
	private float knockbackForce;
	
	[SerializeField]
	private float attackCd;
	
	[SerializeField]
	private float moveSpeed;
	
	[SerializeField]
	private float wanderSpeed; 

	[Header("Loot")]
	
	[SerializeField]
	private int experiencePlus;
	
	[SerializeField]
	private LootTable lootTable;

	private EnemyAttack enemyAttack;
	private Rigidbody2D rb;
	
	private Animator animator;
	private bool isAlive = true;
	
	private bool justAttacked = false;
	
	private bool wanderAgain = true;
	
	Vector3 wanderPoint;
	
	Vector2 wanderZoneCentre;
	Vector2 direction;

	public bool IsAlive
	{
		get
		{
			return isAlive;
		}
		set
		{
			isAlive = value;
			if (isAlive == false) 
			{
				lootCol.enabled = true;
				StopCoroutine(nameof(Wander));
			}
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
	
	public CircleCollider2D WanderZone 
	{
		get 
		{
			return wanderZone;
		}
		set 
		{
			wanderZone = value;
		}
	}
 

	void Start() 
	{
		rb = GetComponent<Rigidbody2D>();
		enemyAttack = GetComponentInParent<EnemyAttack>();
		animator = GetComponent<Animator>();
		levelText.text = level.ToString();
		nameText.text = enemyName;
	}

	void FixedUpdate() 
	{
		if (aggroZone.detectedObjs.Count > 0 && isAlive) 
		{
			Collider2D detectedObject0 = aggroZone.detectedObjs[0];
			Vector2 direction = (detectedObject0.transform.position - transform.position).normalized;
			rb.AddForce(moveSpeed * Time.fixedDeltaTime * direction);
			if (animator.GetBool("Attack") != true) 
			{
				animator.SetBool("isMoving", true);
				
			}
			
			if (attackZone.DetectedObjs.Count > 0 && !justAttacked) 
			{
				enemyAttack.Attack(attackType, detectedObject0.transform);
				animator.SetBool("isMoving", false);
				justAttacked = true;
				StartCoroutine(nameof(AttackAgain));
			}
		}
		else if (aggroZone.detectedObjs.Count == 0 && isAlive)
		{
			if (wanderAgain == true) 
			{
				wanderAgain = false;
				StartCoroutine(nameof(Wander));
			}
			
			if (Vector3.Distance(transform.localPosition, wanderZone.offset) > wanderZone.radius) 
			{
				StopCoroutine(nameof(Wander));
				wanderAgain = true;
			}
						
		}

	}
	
	private IEnumerator Wander() 
	{
		wanderPoint = GetRandomPointInsideCircunference(wanderZone);
		direction = (wanderPoint - transform.localPosition).normalized;
		
		while (Vector3.Distance(wanderPoint, transform.localPosition) > 0.05f) 
		{
			animator.SetBool("isMoving", true);
			rb.AddForce(wanderSpeed * Time.deltaTime * direction);
			yield return null;
		}

		animator.SetBool("isMoving", false);
		float wanderCd = UnityEngine.Random.Range(0, maxWanderCd + 1);
		yield return new WaitForSeconds(wanderCd);
		wanderAgain = true;
	}
	
	private IEnumerator AttackAgain() 
	{
		yield return new WaitForSeconds(attackCd);
		justAttacked = false;
	}

	private (int, bool) CalculateDamage()
	{
		int damage = UnityEngine.Random.Range(minDamage, maxDamage + 1);
		float randomFloat = UnityEngine.Random.Range(0f, 100f);

		bool crit = false;
		if (randomFloat < critChance)
		{
			damage *= 2;
			crit = true;
		}

		return (damage, crit);
	}

	public Vector2 GetRandomPointInCircunference(CircleCollider2D circle) {

		Vector2 localCentre = circle.offset;
		float angle = UnityEngine.Random.Range(0, 2*Mathf.PI);

		float x = localCentre.x + circle.radius * Mathf.Cos(angle);
		float y = localCentre.y + circle.radius * Mathf.Sin(angle);
		
		return new Vector2(x, y);
	}
	
	public Vector2 GetRandomPointInsideCircunference(CircleCollider2D circle) 
	{
		Vector2 localCentre = circle.offset;
		
		float randomX = UnityEngine.Random.Range(-circle.radius, circle.radius);
		
		float maxY = Mathf.Sqrt(Mathf.Pow(circle.radius, 2) - Mathf.Pow(randomX, 2));
		
		float randomY = UnityEngine.Random.Range(-maxY, maxY);
		
		return localCentre + new Vector2(randomX, randomY);
	}
	

	public void Interact()
	{
		LootEnemy();
	}

	public void StopInteract()
	{
		if (LootWindow.instance.IsOpen) 
		{
			LootWindow.instance.Close();
		}
	}

	public void LootEnemy()
	{
		lootTable.ShowLoot(gameObject);
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
		else 
		{
			Vector2 direction = (Vector2)(transform.position - col.gameObject.transform.position).normalized;
			rb.AddForce(0.25f * direction, ForceMode2D.Impulse);
			StopCoroutine(nameof(Wander));
			wanderAgain = true;
		}
	}
	
	
}
