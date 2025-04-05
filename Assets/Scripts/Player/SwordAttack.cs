using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum AttackDirection
{
	LEFT, RIGHT, UP, DOWN
}

public class SwordAttack : MonoBehaviour
{
	public Collider2D swordColliderR;
	public Collider2D swordColliderU;
	public Collider2D swordColliderD;
	public Collider2D swordColliderL;

	public AttackDirection attackDirection;

	public void Attack()
	{
		
		switch (attackDirection)
		{

			case AttackDirection.LEFT:
				AttackLeft();
				break;
			case AttackDirection.RIGHT: 
				AttackRight();
				break;
			case AttackDirection.UP:
				AttackUp();
				break;
			case AttackDirection.DOWN:
				AttackDown();
				break;

		}
	}


	public void AttackUp()
	{
		swordColliderU.enabled = true;
	}

	public void AttackDown()
	{
		swordColliderD.enabled = true;
	}

	public void AttackRight()
	{
		swordColliderR.enabled = true;

	}

	public void AttackLeft()
	{
		swordColliderL.enabled = true;
	}

	public void StopAttack()
	{
		swordColliderR.enabled = false;
		swordColliderL.enabled = false;
		swordColliderU.enabled = false;
		swordColliderD.enabled = false;

	}

	private (int, bool) CalculateDamage()
	{
		Sword sword = CharacterPanel.Instance.SwordButton.EquippedSword;

		int str = PlayerStats.Instance.Strength;
		int ap = str / 2;
		float critChance = PlayerStats.Instance.CritChance;

		int damage = Random.Range(sword.MinDamage + ap, sword.MaxDamage + ap + 1);

		float randomFloat = Random.Range(0f, 100f);

		bool crit = false;
		if (randomFloat < critChance)
		{
			damage *= 2;
			crit = true;
		}

		return (damage, crit);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		Sword sword = CharacterPanel.Instance.SwordButton.EquippedSword;

		if (other.CompareTag("Enemy"))
		{
			IDamageable damageableObject  = other.GetComponent<IDamageable>();

			if (damageableObject != null)
			{
				Vector3 parentPosition = gameObject.GetComponentInParent<Transform>().position;

				Vector2 direction = (Vector2)(other.gameObject.transform.position - parentPosition).normalized;
				Vector2 knockback = direction * sword.KnockBackForce;

				(int, bool) damage = CalculateDamage();

				damageableObject.OnHit(damage.Item1, damage.Item2, knockback);
				
				StopAttack();
			}

		}

	}
}
