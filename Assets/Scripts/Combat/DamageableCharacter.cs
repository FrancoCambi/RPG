using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DamageableCharacter : MonoBehaviour, IDamageable
{
	private Animator animator;
	private Rigidbody2D rb;
	private float invincibleTimeElapsed = 0;
	private bool invincible = false;
	private FadeText text;

	public Canvas canvas;
	public GameObject damageText;
	public float invincibilityTime = 0.25f;
	public bool isInvincible = false;


	public bool Invincible
	{
		get
		{
			return invincible;
		}
		set
		{
			invincible = value;

			if (invincible == true)
			{
				invincibleTimeElapsed = 0f;
			}
		}
	}


	private void Start()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		text = damageText.GetComponent<FadeText>();

	}


	public void OnHit(int damage, bool crit)
	{
		if (!Invincible)
		{
			animator.SetTrigger("Hit");
			PlayerStats.Instance.RecieveDamage(CalculateDamage(damage), crit);

			if (isInvincible)
			{
				Invincible = true;
			}

		}
	}
	public void OnHit(int damage, bool crit, Vector2 knockback)
	{
		if (!Invincible)
		{

			animator.SetTrigger("Hit");
			PlayerStats.Instance.RecieveDamage(CalculateDamage(damage), crit);

			rb.AddForce(knockback, ForceMode2D.Impulse);

			if (isInvincible)
			{
				Invincible = true;
			}
		}
	}

	public void Defeated()
	{
		animator.SetTrigger("Defeated");
	}

	public void RemoveEnemy()
	{
		Destroy(gameObject);
	}

	private int CalculateDamage(int value)
	{
		float damageReduction = PlayerStats.Instance.CalculateDamageReductionByArmor();
		int damage = Convert.ToInt32(value * (1 - (damageReduction / 100)));
		return damage;
	}

	public void FixedUpdate()
	{
		if (Invincible == true)
		{
			invincibleTimeElapsed += Time.deltaTime;

			if (invincibleTimeElapsed > invincibilityTime)
			{
				Invincible = false;
			}
		}
	}

}
