using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackZone : MonoBehaviour
{
   
	[SerializeField]
	private string tagTarget = "Player";
	
	[SerializeField]
	private Collider2D col;

	[SerializeField]
	private List<Collider2D> detectedObjs = new();
	
	public List<Collider2D> DetectedObjs 
	{
		get 
		{
			return detectedObjs;
		}
	}

	// Start is called before the first frame update
	void Start()
	{
		col = GetComponent<Collider2D>();
	}

	void OnTriggerEnter2D(Collider2D collider) 
	{
		if (collider.gameObject.CompareTag(tagTarget)) 
		{

			detectedObjs.Add(collider);
		}

	}

	void OnTriggerExit2D(Collider2D collider) 
	{
		detectedObjs.Remove(collider);
		
	}
}
