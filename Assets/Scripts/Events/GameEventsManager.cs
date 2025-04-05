using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEventsManager : MonoBehaviour
{
    public static GameEventsManager instance {  get; private set; }

    public DialogEvents dialogEvents;
    
    public PlayerEvents playerEvents;

    public EnemyDieEvents enemyDieEvents;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in this scene.");
        }
        instance = this;

        dialogEvents = new DialogEvents();

        playerEvents = new PlayerEvents();

        enemyDieEvents = new EnemyDieEvents();
    }
}
