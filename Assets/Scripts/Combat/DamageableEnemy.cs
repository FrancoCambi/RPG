using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DamageableEnemy : MonoBehaviour, IDamageable
{
	private Animator animator;
	private Rigidbody2D rb;
	private Enemy enemy;
	private int enemyId;
	private int enemyExpReward;
	private FadeText text;
	private float healthBarShowTimeElapsed = 0;
	private bool showingHealth = false;
	private Canvas damageTextCanvas;
	
	private Camera mainCamera;

	private bool crit;
	public GameObject damageText;
	public Canvas myCanvas;
	public Image healthBar;
	public float barActiveTime;
	public int maxHealth;
	


	public int Health
	{
		set
		{
			if (value < health)
			{   
				
				animator.SetTrigger("Hit");
				string dmgString = crit ? "Crit! " + (health- value).ToString() : (health - value).ToString();
				FloatingTextManager.Instance.CreateText(mainCamera.WorldToScreenPoint(gameObject.transform.position), 
						damageTextCanvas, dmgString, FloatingTextType.DAMAGE);
			}

			health = value;
			if (health <= 0)
			{
				if (enemy != null)
				{
					PlayerStats.Instance.RecieveExp(XpManager.CalculateXp(enemy));
					GameEventsManager.instance.enemyDieEvents.OnEnemyKilled(enemy);
				}
				Defeated();
				Invoke(nameof(RemoveEnemy), 30f);
			}
		}
		get
		{
			return health;
		}
	}

	public int health = 1;

	public bool invincible = false;

	private void Start()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		enemy = GetComponent<Enemy>();
		if (enemy != null)
		{
			enemyId = enemy.Id;
			enemyExpReward = enemy.ExperiencePlus;
		}
		text = damageText.GetComponent<FadeText>();
		damageTextCanvas = GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>();
		mainCamera = Camera.main;
	}

	public void OnHit(int damage, bool crit)
	{
		
		this.crit = crit;
		Health -= damage;
		healthBar.fillAmount = (float)Health / maxHealth;
		ShowCanvas();
		UpdateEnemyColorByLevel();


		
	}
	public void OnHit(int damage, bool crit, Vector2 knockback)
	{
		this.crit = crit;
		Health -= damage;

		if (Health > 0)
		{
			healthBar.fillAmount = (float)Health / maxHealth;
			ShowCanvas();
			UpdateEnemyColorByLevel();
		}

		rb.AddForce(knockback, ForceMode2D.Impulse);

	}
	
	private void ShowCanvas() 
	{
		showingHealth = true;
		myCanvas.gameObject.SetActive(true);
		healthBarShowTimeElapsed = 0;
		
	}
	
	private void UpdateEnemyColorByLevel() 
	{
		int playerLevel = PlayerStats.Instance.Level;
		int dif = playerLevel - enemy.Level;
		
		
		if (dif >= 10) 
		{
			enemy.LevelText.color = Color.gray;
		}
		else if (dif < 10 && dif > 3) 
		{
			enemy.LevelText.color = Color.green;
		}
		else if (dif <= 3 && dif >= -3) 
		{
			enemy.LevelText.color = Color.yellow;
		}
		else if (dif < -3 && dif >= -5) 
		{
			enemy.LevelText.color = new Color32(255,124,0,255);
		}
		else 
		{
			enemy.LevelText.color = Color.red;
		}
		
	}

	public void Defeated()
	{
		enemy.IsAlive = false;
		enemy.tag = "DeadEnemy";
		GetComponent<Collider2D>().isTrigger = true;
		myCanvas.gameObject.SetActive(false);
		animator.Play("Defeated");
	}

	public void RemoveEnemy()
	{
		Destroy(gameObject);
	}

	public void FixedUpdate()
	{
		if (showingHealth == true)
		{
			healthBarShowTimeElapsed += Time.deltaTime;

			if (healthBarShowTimeElapsed > barActiveTime)
			{
				showingHealth = false;
				myCanvas.gameObject.SetActive(false);
			}
		}
	}

}
