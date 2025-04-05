using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum EnemyAttackType {Jump, Follow, Hit, Throw}
public class EnemyAttack : MonoBehaviour
{
	
	[SerializeField]
	private float jumpMultiplier;
	private Rigidbody2D rb;
	
	private Transform player;
	
	private Animator animator;
	
	private bool attacked = false;
	
	private EnemyAttackType prevType;
	
	private Transform prevPlayer;
	
	private void Start() 
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
	}
	
	public void Attack(EnemyAttackType type, Transform player) 
	{
		animator.SetBool("Attack", true);
		this.player = player;
		attacked = true;
		prevType = type;
		prevPlayer = player;
		
		switch (type)
		{
			case EnemyAttackType.Jump:
				break;
			case EnemyAttackType.Follow:
				FollowAttack();
				break;
			case EnemyAttackType.Hit:
				HitAttack();
				break;
			case EnemyAttackType.Throw:
				ThrowAttack();
				break;
		}
	}
	
	public void StopAttack() 
	{
		animator.SetBool("Attack", false);
	}
	
	public void AttackAgain() 
	{
		animator.SetBool("Hit", false);
		if (attacked == true) 
		{
			Attack(prevType, prevPlayer);
		}
	}

	private void JumpAttack() 
	{
		Vector3 directionVector = player.position - transform.position;
		
		rb.AddForce(directionVector * jumpMultiplier, ForceMode2D.Impulse);
		attacked = false;
	}
	
	private void FollowAttack() 
	{
		
	}
	
	private void HitAttack() 
	{
		
	}

	private void ThrowAttack() 
	{
		
	}
}
