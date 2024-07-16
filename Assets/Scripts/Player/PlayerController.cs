using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

	public float moveSpeed = 1f;
	public float collisionOffset = 0.05f;
	public ContactFilter2D movementFilter;
	public SwordAttack swordAttack;
	public LayerMask interactableLayer;

	Vector2 movementInput;
	Rigidbody2D rb;
	Animator animator;
	SpriteRenderer spriteRenderer;
	
	Camera mainCamera;
	List<RaycastHit2D> castCollisions = new();
	bool canMove = true;
	private GameObject interactable;
	private List<string> noColTags = new List<string>();

	private bool attacking = false;


	// Start is called before the first frame update
	void Start()
	{
		rb = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		mainCamera = Camera.main;

		noColTags.Add("Enemy");
		noColTags.Add("QuestPoint");

	}

	public void HandleUpdate()
	{
		// Movement Handler (bad code)
		if (canMove)
		{
			if (movementInput != Vector2.zero)
			{
				bool success = TryMove(movementInput);

				// Avoid edges
				if (!success)
				{
					success = TryMove(new Vector2(movementInput.x, 0));

					if (!success)
					{
						TryMove(new Vector2(0, movementInput.y));
					}
				}

					if (movementInput.x != 0)
					{
					animator.SetBool("IsMovingLR", true);
					animator.SetBool("IsMovingUp", false);
					animator.SetBool("IsMovingDown", false);
					animator.SetBool("JustMovedLR", true);
					animator.SetBool("JustMovedDown", false);
					animator.SetBool("JustMovedUp", false);
					}

				else if (movementInput.y > 0)
				{
					animator.SetBool("IsMovingUp", true);
					animator.SetBool("IsMovingDown", false);
					animator.SetBool("IsMovingLR", false);
					animator.SetBool("JustMovedLR", false);
					animator.SetBool("JustMovedDown", false);
					animator.SetBool("JustMovedUp", true);

				}
				else if (movementInput.y < 0)
				{
					animator.SetBool("IsMovingUp", false);
					animator.SetBool("IsMovingDown", true);
					animator.SetBool("IsMovingLR", true);
					animator.SetBool("JustMovedLR", false);
					animator.SetBool("JustMovedDown", true);
					animator.SetBool("JustMovedUp", false);



				}
			}
			else
			{
				animator.SetBool("IsMovingLR", false);
				animator.SetBool("IsMovingUp", false);
				animator.SetBool("IsMovingDown", false);
			}

			// Flip sprite for animation to look good
			if (movementInput.x < 0 && !attacking)
			{
				spriteRenderer.flipX = true;
			}
			else if (movementInput.x > 0 && !attacking)
			{
				spriteRenderer.flipX = false;

			}

		}

		// Action buttons handler
		
		if (!MainMenu.Instance.AnyOpen())
		{
			foreach (string action in KeyBindManager.instance.ActionBinds.Keys)
			{
				if (KeyBindManager.instance.ActionBinds[action].Item2 == EventModifiers.None)
				{
					if (Input.GetKeyDown(KeyBindManager.instance.ActionBinds[action].Item1))
					{

						GameController.Instance.ClickActionButton(action);
					}
				}
				else
				{
					EventModifiers mod = KeyBindManager.instance.ActionBinds[action].Item2;
					KeyCode key = KeyBindManager.instance.ActionBinds[action].Item1;

					if (Input.GetKey(KeyBindManager.instance.EventModifierToKeyCode(mod)) && Input.GetKeyDown(key))
					{
						GameController.Instance.ClickActionButton(action);
					}
				}
			}

		}


	}

	private bool ColWith(List<RaycastHit2D> cols, List<string> tags)
	{
		foreach (RaycastHit2D col in cols)
		{
			if (tags.Contains(col.collider.gameObject.tag))
			{
				return true;
			}
		}

		return false;
	}

	private bool TryMove(Vector2 direction)
	{
		if (direction != Vector2.zero)
		{
			CastBar.Instance.StopCast();
			int count = rb.Cast(direction, movementFilter, castCollisions, moveSpeed * Time.fixedDeltaTime + collisionOffset);

			if (count == 0 || ColWith(castCollisions, noColTags))
			{
				rb.MovePosition(rb.position + moveSpeed * Time.fixedDeltaTime * direction);
				return true;
			}
			else
			{
				return false;
			}
		}
		else
		{
			return false;
		}
	}
	public void SwordAttackFunc()
	{
		if (!UI.instance.IsPointerOverUIElement())
		{
			LockMovement();
			swordAttack.Attack();
			attacking = true;
		}


	}

	public void EndSwordAttack()
	{
		UnlockMovement();
		swordAttack.StopAttack();
		attacking = false;
	}
	
	public void StopAttacking() 
	{
		attacking = false;
	}

	public void LockMovement()
	{
		canMove = false;
	}

	public void UnlockMovement()
	{
		canMove = true;
	}


	void OnMove(InputValue movementValue)
	{
		movementInput = movementValue.Get<Vector2>();
	}

	void OnFire()
	{
		if (!UI.instance.IsPointerOverUIElement() && HandScript.Instance.MyMoveable == null && !attacking) 
		{
			attacking = true;
			CastBar.Instance.StopCast();
			
			float offsetYUp = 0.035f;
			float offsetYDown = 0.17f;
			float offsetX = 0.3f;
			Vector3 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
			
			if ((mousePos.x > transform.position.x - offsetX && mousePos.y <= transform.position.y + offsetYUp && mousePos.y >= transform.position.y - offsetYDown 
					&& mousePos.x <= transform.position.x) || mousePos.x <= transform.position.x - offsetX) 
			{
				spriteRenderer.flipX = true;
				animator.SetTrigger("AttackLR");
				swordAttack.attackDirection = AttackDirection.LEFT;
				animator.SetTrigger("SwordAttack");
			}
			else if ((mousePos.x > transform.position.x - offsetX && mousePos.y <= transform.position.y + offsetYUp && mousePos.y >= transform.position.y - offsetYDown
					&& mousePos.x >= transform.position.x) || mousePos.x >= transform.position.x + offsetX) 
			{
				spriteRenderer.flipX = false;
				animator.SetTrigger("AttackLR");
				swordAttack.attackDirection = AttackDirection.RIGHT;
				animator.SetTrigger("SwordAttack");
			}
			else if (mousePos.x <= transform.position.x + offsetX && mousePos.x >= transform.position.x - offsetX && mousePos.y >= transform.position.y) 
			{
				animator.SetTrigger("AttackUp");
				swordAttack.attackDirection = AttackDirection.UP;
				animator.SetTrigger("SwordAttack");
			}
			else if (mousePos.x <= transform.position.x + offsetX && mousePos.x >= transform.position.x - offsetX && mousePos.y < transform.position.y)
			{
				animator.SetTrigger("AttackDown");
				swordAttack.attackDirection = AttackDirection.DOWN;
				animator.SetTrigger("SwordAttack");
			}
			
		}
	}

	public void Interact(string tag)
	{
		if (interactable != null && interactable.CompareTag(tag))
		{
			animator.SetBool("IsMovingLR", false);
			animator.SetBool("IsMovingUp", false);
			animator.SetBool("IsMovingDown", false);
			interactable.GetComponent<IInteractable>().Interact();
		}
	}

	public void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.GetComponent<IInteractable>() != null)
		{
			interactable = collision.gameObject;
		}
	}

	public void OnTriggerExit2D(Collider2D collision)
	{
		if (!attacking)
		{
			if (collision.GetComponent<IInteractable>() != null)
			{
				if (interactable != null)
				{
					interactable.GetComponent<IInteractable>().StopInteract();
					interactable = null;
				}
			}
		}


	}

}
