using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ArenaControl : MonoBehaviour {

    public List<EnemySpawner> spawners;
    public List<DoorOpen> doorsToClose;
    public List<DoorOpen> doorsToOpen;
    public GameObject bossSpawner;
    public bool sameDoors = false; // check this if you want to just use the same doors in the closing list as the open list
    public int enemyLimit = 10;

    int deathCounter = 0;
    bool playersReady = false;
    public bool PlayersReady { get { return playersReady; } }
    bool spawnersOn = true;
    [HideInInspector] public bool completed = false; // did the players actually finish the arena? used so that finished arenas aren't reset when the players die
    List<GameObject> readyPlayers;
    [HideInInspector] public bool triggerSet = false;
	[HideInInspector] public GameObject triggerEnemy;
 	
	void Start () {
        readyPlayers = new List<GameObject>();
        if(bossSpawner != null)
        {
        	Debug.Log ("Boss spawner was set");
			bossSpawner.GetComponent<EnemySpawner>().bossSpawner = true;
        	spawners.Add(bossSpawner.GetComponent<EnemySpawner>());
        	/*	
			bossSpawner.GetComponent<EnemySpawner>().arenaSpawner = true;
			bossSpawner.GetComponent<EnemySpawner>().aC = this;*/
        }
        if (sameDoors)
            doorsToOpen = doorsToClose;
	}

	void Update () {
        //if(triggerEnemy != null)
          //  Debug.Log("Castharoth: " + triggerEnemy.GetComponent<ActorHealth>().hitPoints);
        if(playersReady && triggerSet)
        {
            if(triggerEnemy == null)
            {
                foreach (DoorOpen d in doorsToOpen)
                {
                    d.SendMessage("Open");
                }
            }
        }
	    if(!playersReady && spawnersOn)
        {
            if (deathCounter < enemyLimit)
            {
                foreach (EnemySpawner s in spawners)
                {
                    if (s != null)
                    {
                        s.enabled = false;
                        s.arenaSpawner = true;
                        s.aC = this;
                    }
                }
                spawnersOn = false;
            }
        }
        if (playersReady && !spawnersOn)
        {
            if (deathCounter < enemyLimit)
            {
                foreach (EnemySpawner s in spawners)
                {
                    if (s != null)
                        s.enabled = true;
                }
                spawnersOn = true;
                foreach(DoorOpen d in doorsToClose)
                {
                    d.SendMessage("Close");
                }
            }
        }
        if (playersReady && spawnersOn)
        {
            if(deathCounter >= enemyLimit)
            {
                foreach (EnemySpawner s in spawners)
                {
                    if (s != null)
                        s.enabled = false;
                }
                spawnersOn = false;
                foreach (DoorOpen d in doorsToOpen)
                {
                    d.SendMessage("Open");
                }
                completed = true;
            }
        }
	}

    void EnemyDied()
    {
        deathCounter++;
    }

    void Reset()
    {
    	Debug.Log ("Arena reset");
        deathCounter = 0;
        readyPlayers = new List<GameObject>();
        playersReady = false;
        if(triggerSet && bossSpawner != null)
        {
        	if(triggerEnemy != null)
        	{
        		Debug.Log ("Boss destroyed");
            	Destroy(triggerEnemy);
            }
        }
        foreach (EnemySpawner s in spawners)
        {
            if (s != null)
                s.Reset();
        }
        foreach(DoorOpen d in doorsToClose)
        {
            if (d != null)
                d.SendMessage("Open");
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player" && !readyPlayers.Contains(col.gameObject))
        {
            readyPlayers.Add(col.gameObject);
            if (readyPlayers.Count == GameObject.FindGameObjectsWithTag("Player").Length)
                playersReady = true;
        }
    }

    void OnGUI()
    {
        if(playersReady && spawnersOn)
        {
            //Camera.main.WorldToScreenPoint();
            //GUI.Label();
        }
    }
}
