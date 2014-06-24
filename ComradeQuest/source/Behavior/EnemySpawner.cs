using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour 
{
	public GameObject enemyPrefab; // Prefab of the enemy to spawn
	public int numSpawnAtOnce = 1; // Number of enemies to spawn at once
	public float startDelay = 0.0f;
	public float playerDetect = 20f;
	public bool waitForDeath = false;
	public float spawnCooldown = 1.0f; // Delay between spawns in seconds
	public bool spawnCapOn = false;
	public int spawnCapNum = 1; // Number of enemies to spawn if spawn cap is on
	public bool randStartPatDir = false;
	public bool rightStartPatDir = false;
	public bool leftStartPatDir = false;
	public float minPatRadius = 3f;
	public float maxPatRadius = 4f;
	public float rangeCooldown = 0.25f;
	public float rangeReach = 5f;
	public float projectileSpeed = 5f;

    [HideInInspector] public bool arenaSpawner = false; // sets all spawned enemies to "arenaEnemy"
    [HideInInspector] public ArenaControl aC;
	[HideInInspector] public bool bossSpawner;

	private float startSpawnTime = Mathf.Infinity;
	private float nextSpawnTime = 0.0f;
	private bool playerDetected = false;
	private int totalSpawned = 0;
	private GameObject spawnedEnemy;
	private GameObject currentTarget;
	private Vector2 position2D, targetPosition2D;
	private Vector3 enemySpawnPosition;
	private bool newSpawnTimeSet = true;
	private float greedDemonWidth = 2.7682f;
	private float gluttonyDemonWidth = 2.7682f;
	
	
	void Update () 
	{
		position2D = new Vector2 (transform.position.x, transform.position.y);
		
		currentTarget = FindClosestPlayer();
		
		if (currentTarget == null)
		{
			// All players are dead, what do I do with my life?!
		}
		else
		{
			targetPosition2D = new Vector2(currentTarget.transform.position.x, currentTarget.transform.position.y);
			
			if (Vector2.Distance (targetPosition2D, position2D) < playerDetect)
			{
				if (playerDetected == false)
				{
					playerDetected = true;
					startSpawnTime = Time.time + startDelay;
				}
			}
		}
		
		if (playerDetected && waitForDeath && (Time.time > startSpawnTime))
		{
			if (Time.time > nextSpawnTime) // Next spawn time has passed
			{
				for (int i = 0; i < numSpawnAtOnce; i++)
				{
					if (spawnCapOn)
					{
						if (totalSpawned < spawnCapNum)
						{
							enemySpawnPosition = CalculateSpawnPosition (i, numSpawnAtOnce);
							spawnedEnemy = Instantiate(enemyPrefab, enemySpawnPosition, Quaternion.identity) as GameObject;
							if (spawnedEnemy.transform.name == "GreedDemonMelee(Clone)" || spawnedEnemy.transform.name == "GreedDemonRange(Clone)")
							{
								var greedDemonScript = spawnedEnemy.GetComponent<GreedDemon>();
								if (rightStartPatDir)
								{
									greedDemonScript.Direction = new Vector2 (1, 0);
								}
								else if (leftStartPatDir)
								{
									greedDemonScript.Direction = new Vector2 (-1, 0);
								}
								else if (randStartPatDir)
								{
									var greedDemonPatrolDirection = Random.Range (0, 100);
									if (greedDemonPatrolDirection < 50)
									{
										greedDemonScript.Direction = new Vector2 (-1, 0);
									}
									else
									{
										greedDemonScript.Direction = new Vector2 (1, 0);
									}
								}
								greedDemonScript.patrolRadius = Random.Range (minPatRadius, maxPatRadius);
								greedDemonScript.rangeCooldown = rangeCooldown;
								greedDemonScript.rangeReach = rangeReach;
								greedDemonScript.projectileSpeed = projectileSpeed;
							}
							else if (spawnedEnemy.transform.name == "GluttonyDemon(Clone)")
							{
								var gluttonyDemonScript = spawnedEnemy.GetComponent<GluttonyDemon>();
								if (rightStartPatDir)
								{
									gluttonyDemonScript.Direction = new Vector2 (1, 0);
								}
								else if (leftStartPatDir)
								{
									gluttonyDemonScript.Direction = new Vector2 (-1, 0);
								}
								else if (randStartPatDir)
								{
									var gluttonyDemonPatrolDirection = Random.Range (0, 100);
									if (gluttonyDemonPatrolDirection < 50)
									{
										gluttonyDemonScript.Direction = new Vector2 (-1, 0);
									}
									else
									{
										gluttonyDemonScript.Direction = new Vector2 (1, 0);
									}
								}
								gluttonyDemonScript.patrolRadius = Random.Range (minPatRadius, maxPatRadius);
							}
                            if(arenaSpawner) // pass the info down the hierarchy...
                            {
                                var ah =spawnedEnemy.GetComponent<ActorHealth>();
                                ah.arenaEnemy = true;
                                ah.aC = aC;
                                if(bossSpawner)
                                {
									Debug.Log ("Boss successfully assigned");
                                	aC.triggerEnemy = spawnedEnemy;
									aC.triggerSet = true;
                                }
                            }
							spawnedEnemy.transform.parent = transform;
							totalSpawned += 1;
						}
					}
					else
					{
						enemySpawnPosition = CalculateSpawnPosition (i, numSpawnAtOnce);
						spawnedEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity) as GameObject;
						if (spawnedEnemy.transform.name == "GreedDemonMelee(Clone)" || spawnedEnemy.transform.name == "GreedDemonRange(Clone)")
						{
							var greedDemonScript = spawnedEnemy.GetComponent<GreedDemon>();
							if (rightStartPatDir)
							{
								greedDemonScript.Direction = new Vector2 (1, 0);
							}
							else if (leftStartPatDir)
							{
								greedDemonScript.Direction = new Vector2 (-1, 0);
							}
							else if (randStartPatDir)
							{
								var greedDemonPatrolDirection = Random.Range (0, 100);
								if (greedDemonPatrolDirection < 50)
								{
									greedDemonScript.Direction = new Vector2 (-1, 0);
								}
								else
								{
									greedDemonScript.Direction = new Vector2 (1, 0);
								}
							}
							greedDemonScript.patrolRadius = Random.Range (minPatRadius, maxPatRadius);
							greedDemonScript.rangeCooldown = rangeCooldown;
							greedDemonScript.rangeReach = rangeReach;
							greedDemonScript.projectileSpeed = projectileSpeed;
						}
						else if (spawnedEnemy.transform.name == "GluttonyDemon(Clone)")
						{
							var gluttonyDemonScript = spawnedEnemy.GetComponent<GluttonyDemon>();
							if (rightStartPatDir)
							{
								gluttonyDemonScript.Direction = new Vector2 (1, 0);
							}
							else if (leftStartPatDir)
							{
								gluttonyDemonScript.Direction = new Vector2 (-1, 0);
							}
							else if (randStartPatDir)
							{
								var gluttonyDemonPatrolDirection = Random.Range (0, 100);
								if (gluttonyDemonPatrolDirection < 50)
								{
									gluttonyDemonScript.Direction = new Vector2 (-1, 0);
								}
								else
								{
									gluttonyDemonScript.Direction = new Vector2 (1, 0);
								}
							}
							gluttonyDemonScript.patrolRadius = Random.Range (minPatRadius, maxPatRadius);
						}
						if(arenaSpawner) // pass the info down the hierarchy...
						{
							var ah =spawnedEnemy.GetComponent<ActorHealth>();
							ah.arenaEnemy = true;
							ah.aC = aC;
							if(bossSpawner)
							{
								Debug.Log ("Boss successfully assigned");
								aC.triggerEnemy = spawnedEnemy;
								aC.triggerSet = true;
							}
						}
						spawnedEnemy.transform.parent = transform;
						totalSpawned += 1;
					}
				}
				
				if (newSpawnTimeSet)
				{
					nextSpawnTime = Mathf.Infinity;
					newSpawnTimeSet = false;
				}
			}
		}
		
		if (playerDetected && !waitForDeath && (Time.time > startSpawnTime))
		{
			if (Time.time > nextSpawnTime) // Next spawn time has passed
			{
				
				for (int i = 0; i < numSpawnAtOnce; i++)
				{
					if (spawnCapOn)
					{
						if (totalSpawned < spawnCapNum)
						{
							enemySpawnPosition = CalculateSpawnPosition (i, numSpawnAtOnce);
							spawnedEnemy = Instantiate(enemyPrefab, enemySpawnPosition, Quaternion.identity) as GameObject;
							if (spawnedEnemy.transform.name == "GreedDemonMelee(Clone)" || spawnedEnemy.transform.name == "GreedDemonRange(Clone)")
							{
								var greedDemonScript = spawnedEnemy.GetComponent<GreedDemon>();
								if (rightStartPatDir)
								{
									greedDemonScript.Direction = new Vector2 (1, 0);
								}
								else if (leftStartPatDir)
								{
									greedDemonScript.Direction = new Vector2 (-1, 0);
								}
								else if (randStartPatDir)
								{
									var greedDemonPatrolDirection = Random.Range (0, 100);
									if (greedDemonPatrolDirection < 50)
									{
										greedDemonScript.Direction = new Vector2 (-1, 0);
									}
									else
									{
										greedDemonScript.Direction = new Vector2 (1, 0);
									}
								}
								greedDemonScript.patrolRadius = Random.Range (minPatRadius, maxPatRadius);
								greedDemonScript.rangeCooldown = rangeCooldown;
								greedDemonScript.rangeReach = rangeReach;
								greedDemonScript.projectileSpeed = projectileSpeed;
							}
							else if (spawnedEnemy.transform.name == "GluttonyDemon(Clone)")
							{
								var gluttonyDemonScript = spawnedEnemy.GetComponent<GluttonyDemon>();
								if (rightStartPatDir)
								{
									gluttonyDemonScript.Direction = new Vector2 (1, 0);
								}
								else if (leftStartPatDir)
								{
									gluttonyDemonScript.Direction = new Vector2 (-1, 0);
								}
								else if (randStartPatDir)
								{
									var gluttonyDemonPatrolDirection = Random.Range (0, 100);
									if (gluttonyDemonPatrolDirection < 50)
									{
										gluttonyDemonScript.Direction = new Vector2 (-1, 0);
									}
									else
									{
										gluttonyDemonScript.Direction = new Vector2 (1, 0);
									}
								}
								gluttonyDemonScript.patrolRadius = Random.Range (minPatRadius, maxPatRadius);
							}
							if(arenaSpawner) // pass the info down the hierarchy...
							{
								var ah =spawnedEnemy.GetComponent<ActorHealth>();
								ah.arenaEnemy = true;
								ah.aC = aC;
								if(bossSpawner)
								{
									Debug.Log ("Boss successfully assigned");
									aC.triggerEnemy = spawnedEnemy;
									aC.triggerSet = true;
								}
							}
							spawnedEnemy.transform.parent = transform;
							totalSpawned += 1;
						}
					}
					else
					{
						enemySpawnPosition = CalculateSpawnPosition (i, numSpawnAtOnce);
						spawnedEnemy = Instantiate(enemyPrefab, enemySpawnPosition, Quaternion.identity) as GameObject;
						if (spawnedEnemy.transform.name == "GreedDemonMelee(Clone)" || spawnedEnemy.transform.name == "GreedDemonRange(Clone)")
						{
							var greedDemonScript = spawnedEnemy.GetComponent<GreedDemon>();
							if (rightStartPatDir)
							{
								greedDemonScript.Direction = new Vector2 (1, 0);
							}
							else if (leftStartPatDir)
							{
								greedDemonScript.Direction = new Vector2 (-1, 0);
							}
							else if (randStartPatDir)
							{
								var greedDemonPatrolDirection = Random.Range (0, 100);
								if (greedDemonPatrolDirection < 50)
								{
									greedDemonScript.Direction = new Vector2 (-1, 0);
								}
								else
								{
									greedDemonScript.Direction = new Vector2 (1, 0);
								}
							}
							greedDemonScript.patrolRadius = Random.Range (minPatRadius, maxPatRadius);
							greedDemonScript.rangeCooldown = rangeCooldown;
							greedDemonScript.rangeReach = rangeReach;
							greedDemonScript.projectileSpeed = projectileSpeed;
						}
						else if (spawnedEnemy.transform.name == "GluttonyDemon(Clone)")
						{
							var gluttonyDemonScript = spawnedEnemy.GetComponent<GluttonyDemon>();
							if (rightStartPatDir)
							{
								gluttonyDemonScript.Direction = new Vector2 (1, 0);
							}
							else if (leftStartPatDir)
							{
								gluttonyDemonScript.Direction = new Vector2 (-1, 0);
							}
							else if (randStartPatDir)
							{
								var gluttonyDemonPatrolDirection = Random.Range (0, 100);
								if (gluttonyDemonPatrolDirection < 50)
								{
									gluttonyDemonScript.Direction = new Vector2 (-1, 0);
								}
								else
								{
									gluttonyDemonScript.Direction = new Vector2 (1, 0);
								}
							}
							gluttonyDemonScript.patrolRadius = Random.Range (minPatRadius, maxPatRadius);
						}
						if(arenaSpawner) // pass the info down the hierarchy...
						{
							var ah =spawnedEnemy.GetComponent<ActorHealth>();
							ah.arenaEnemy = true;
							ah.aC = aC;
							if(bossSpawner)
							{
								Debug.Log ("Boss successfully assigned");
								aC.triggerEnemy = spawnedEnemy;
								aC.triggerSet = true;
							}
						}
						spawnedEnemy.transform.parent = transform;
						totalSpawned += 1;
					}
				}
				
				nextSpawnTime = Time.time + spawnCooldown;
			}
		}
		
		if (waitForDeath && !newSpawnTimeSet)
		{
			if (transform.childCount == 0)
			{
				nextSpawnTime = Time.time + spawnCooldown;
				newSpawnTimeSet = true;
			}
		}
	}
	
    public void Reset()
    {
        totalSpawned = 0;
    }

	private Vector3 CalculateSpawnPosition(int i, int numSpawnAtOnce)
	{
		if (i == 0) // First enemy in wave
		{
			if (numSpawnAtOnce == 1)
			{
				enemySpawnPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
			}
			else if (numSpawnAtOnce == 2)
			{
				enemySpawnPosition = new Vector3 (transform.position.x - (greedDemonWidth / 2), transform.position.y, transform.position.z);
			}
			else if (numSpawnAtOnce == 3)
			{
				enemySpawnPosition = new Vector3 (transform.position.x - greedDemonWidth, transform.position.y, transform.position.z);
			}
			else if (numSpawnAtOnce == 4)
			{
				enemySpawnPosition = new Vector3 (transform.position.x - (greedDemonWidth * 1.5f), transform.position.y, transform.position.z);
			}
			else if (numSpawnAtOnce == 5)
			{
				enemySpawnPosition = new Vector3 (transform.position.x - (greedDemonWidth * 2), transform.position.y, transform.position.z);
			}
		}
		if (i == 1) // Second enemy in wave
		{
			if (numSpawnAtOnce == 2)
			{
				enemySpawnPosition = new Vector3 (transform.position.x + (greedDemonWidth / 2), transform.position.y, transform.position.z);
			}
			else if (numSpawnAtOnce == 3)
			{
				enemySpawnPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
			}
			else if (numSpawnAtOnce == 4)
			{
				enemySpawnPosition = new Vector3 (transform.position.x - (greedDemonWidth / 2), transform.position.y, transform.position.z);
			}
			else if (numSpawnAtOnce == 5)
			{
				enemySpawnPosition = new Vector3 (transform.position.x - greedDemonWidth, transform.position.y, transform.position.z);
			}
		}
		if (i == 2) // Third enemy in wave
		{
			if (numSpawnAtOnce == 3)
			{
				enemySpawnPosition = new Vector3 (transform.position.x + greedDemonWidth, transform.position.y, transform.position.z);
			}
			else if (numSpawnAtOnce == 4)
			{
				enemySpawnPosition = new Vector3 (transform.position.x + (greedDemonWidth / 2), transform.position.y, transform.position.z);
			}
			else if (numSpawnAtOnce == 5)
			{
				enemySpawnPosition = new Vector3 (transform.position.x, transform.position.y, transform.position.z);
			}
		}
		if (i == 3) // Fourth enemy in wave
		{
			if (numSpawnAtOnce == 4)
			{
				enemySpawnPosition = new Vector3 (transform.position.x + (greedDemonWidth * 1.5f), transform.position.y, transform.position.z);
			}
			else if (numSpawnAtOnce == 5)
			{
				enemySpawnPosition = new Vector3 (transform.position.x + greedDemonWidth, transform.position.y, transform.position.z);
			}
		}
		if (i == 4) // Fifth enemy in wave
		{
			if (numSpawnAtOnce == 5)
			{
				enemySpawnPosition = new Vector3 (transform.position.x + (greedDemonWidth * 2), transform.position.y, transform.position.z);
			}
		}
		
		return enemySpawnPosition;
	}
	
	GameObject FindClosestPlayer() // Returns the closest player object
	{
		GameObject[] playerObjects;
		playerObjects = GameObject.FindGameObjectsWithTag("Player");
		GameObject closestPlayer = null;
		float distance = Mathf.Infinity;
		Vector3 position = transform.position;
		foreach (GameObject playerObject in playerObjects) 
		{
			Vector3 diff = playerObject.transform.position - position;
			float currentDistance = diff.sqrMagnitude;
			if (currentDistance < distance) 
			{
				closestPlayer = playerObject;
				distance = currentDistance;
			}
		}
		return closestPlayer;
	}
}