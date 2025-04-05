using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AggroZone : MonoBehaviour
{

    public string tagTarget = "Player";
    public Collider2D col;

    public List<Collider2D> detectedObjs = new();

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
