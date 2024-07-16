using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor.AnimatedValues;
using UnityEngine;
using UnityEngine.EventSystems;

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
	private float wanderCd;

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
	private float moveSpeed = 1f;

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
	
	private bool moving = false;
	
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
		enemyAttack = GetComponentInParent<EnemyAttack>();
		animator = GetComponent<Animator>();
		levelText.text = level.ToString();
		nameText.text = enemyName;
		wanderZoneCentre = transform.TransformPoint(wanderZone.offset);
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
			if (moving == false) 
			{
				wanderPoint = GetRandomPoint(wanderZone);
				Debug.Log("Centro: " + wanderZoneCentre);
				Debug.Log(wanderPoint);
				direction = (wanderPoint - transform.position).normalized;
				moving = true;
			}
			if (moving == true) 
			{
				if (Vector3.Distance(wanderPoint, transform.position) <= 0.1f) 
				{	
					moving = false;
				}
				else 
				{	

					animator.SetBool("isMoving", true);
					rb.AddForce(moveSpeed * Time.fixedDeltaTime * direction);
				}
			}
			
			/*if (moving == false) 
			{
				moving = true;
				wanderPoint = GetRandomPoint(wanderZone);
				Debug.Log(wanderPoint);
				StartCoroutine(nameof(WanderAgain));
			}*/

						
		}

	}
	
	private IEnumerator WanderAgain() 
	{
		yield return new WaitForSeconds(1f);
		moving = false;
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

	public Vector2 GetRandomPoint(CircleCollider2D circle) {

		Vector2 globalCentre = transform.TransformPoint(wanderZoneCentre);
		float angle = UnityEngine.Random.Range(0, 2*Mathf.PI);

		float x = globalCentre.x + circle.radius * Mathf.Cos(angle);
		float y = globalCentre.y + circle.radius * Mathf.Sin(angle);
		

		return new Vector2(x, y);
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
	
}
