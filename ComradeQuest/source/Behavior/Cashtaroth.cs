using UnityEngine;
using System.Collections;

public class Cashtaroth : ActorControl 
{
	public bool attack = true;
	public bool spawnDemons = true;
	public float teleportDelay = 7f; // How long between teleports
	public float leftRoomBound = 193.7275f;
	public float rightRoomBound = 234.0996f;
	public float p1RangeCooldown = 0.50f;
	public float p2RangeCooldown = 0.25f;
	public GameObject greedDemonPrefab;
	public Rigidbody2D CashtarothMoneyBag; // The prefab for Cashtaroth's money bag
	public float bagSpeed = 5f; // How fast money bag travel
	public float bagDistance = 5f; // How far money bag travels
	public float coinSpeed = 5f; // How fast coins travel
	public float coinDistance = 5f; // How far coins travel
	public int coinDamage = 1; // How much damage coins do
	public int numCoins = 5;
	[HideInInspector] public int maxHitPoints;
	
	private GameObject[] teleportPointArray, enemyArray;
	private int currentHitPoints, teleportArrayLength, currentTeleportPoint, nextTeleportPoint;
	private float hitPointRatio;
	private float teleportTime;
	private GameObject spawnedGreedDemon, currentTarget;
	private Vector3 cashtarothViewportPosition;
	private Vector2 position2D, targetPosition2D, bagVelocity;
	private bool rangeOnCooldown = false;
	private float rangeAttackTimer = 0f;
	private bool encounterStarted = false;
	private int spawnedGreedDemons = 0;
	
	void Start () 
	{
		teleportPointArray = GameObject.FindGameObjectsWithTag("CashtarothTeleportPoint");
		
		Reset ();
		
		maxHitPoints = GetComponent<ActorHealth>().hitPoints;
		
		InitFlags ();
	}
	
	void Update () 
	{
		position2D = new Vector2 (transform.position.x, transform.position.y);
		
		currentTarget = FindClosestPlayer();
		
		currentHitPoints = GetComponent<ActorHealth>().hitPoints;
		hitPointRatio = (float)currentHitPoints / (float)maxHitPoints;
		
		if (currentTarget != null)
		{
			targetPosition2D = new Vector2(currentTarget.transform.position.x, currentTarget.transform.position.y);
			
			if (currentTarget.transform.position.x > leftRoomBound && currentTarget.transform.position.x < rightRoomBound)
			{
				if (!encounterStarted)
				{
					teleportTime = Time.time + teleportDelay;
					encounterStarted = true;
				}
				
				if (attack)
				{
					RangeAttack();
				}
				
				if (Time.time > teleportTime)
				{
					if (spawnDemons)
					{
						SpawnGreedDemon ();
					}
					Teleport();
				}
			}
			else
			{
				Reset();
			}
		}
		else
		{
			Reset();
		}
		
		if (rangeOnCooldown)
		{
			if (Time.time > rangeAttackTimer)
			{
				rangeOnCooldown = false;
			}
		}
	}
	
	void Teleport()
	{
		nextTeleportPoint = Random.Range (0, teleportPointArray.Length);
		
		while ((nextTeleportPoint + 1) == currentTeleportPoint)
		{
			nextTeleportPoint = Random.Range (0, teleportPointArray.Length);
		}
		
		foreach (GameObject teleportPoint in teleportPointArray)
		{
			if (teleportPoint.GetComponent<CashtarothTeleportPoint>().pointNumber == (nextTeleportPoint + 1))
			{
				transform.position = new Vector3 (teleportPoint.transform.position.x, teleportPoint.transform.position.y, teleportPoint.transform.position.z);
				currentTeleportPoint = teleportPoint.GetComponent<CashtarothTeleportPoint>().pointNumber;
				teleportTime = Time.time + teleportDelay;
				break;
			}
		}
	}
	
	void Reset()
	{
		DestroyGreedDemons ();
		if (currentTeleportPoint != 1)
		{
			foreach (GameObject teleportPoint in teleportPointArray)
			{
				if (teleportPoint.GetComponent<CashtarothTeleportPoint>().pointNumber == 1)
				{
					transform.position = new Vector3 (teleportPoint.transform.position.x, teleportPoint.transform.position.y, teleportPoint.transform.position.z);
					currentTeleportPoint = teleportPoint.GetComponent<CashtarothTeleportPoint>().pointNumber;
					break;
				}
			}
		}
		
		encounterStarted = false;
	}
	
	void SpawnGreedDemon ()
	{
		spawnedGreedDemon = Instantiate(greedDemonPrefab, transform.position, Quaternion.identity) as GameObject;
		var greedDemonScript = spawnedGreedDemon.GetComponent<GreedDemon>();
		var greedDemonPatrolDirection = Random.Range (0, 100);
		if (greedDemonPatrolDirection < 50)
		{
			greedDemonScript.Direction = new Vector2 (-1, 0);
		}
		else
		{
			greedDemonScript.Direction = new Vector2 (1, 0);
		}
		greedDemonScript.patrolRadius = 3f;
		greedDemonScript.spawnedByCashtaroth = true;
		spawnedGreedDemons++;
	}
	
	void DestroyGreedDemons ()
	{
		if (spawnedGreedDemons != 0)
		{
			enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
			foreach (GameObject enemyObject in enemyArray)
			{
				if (enemyObject.name == "GreedDemonMelee(Clone)")
				{
					if (enemyObject.GetComponent<GreedDemon>().spawnedByCashtaroth == true)
					{
						Destroy (enemyObject);
					}
				}
			}
			spawnedGreedDemons = 0;
		}
	}
	
	void RangeAttack()
	{
		if (!rangeOnCooldown)
		{
			bagVelocity = new Vector2(currentTarget.transform.position.x - transform.position.x, currentTarget.transform.position.y - transform.position.y);
			bagVelocity.Normalize ();
			bagVelocity *= bagSpeed;
			
			Rigidbody2D bagInstance = Instantiate (CashtarothMoneyBag, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as Rigidbody2D;
			bagInstance.velocity = bagVelocity;
			var bagScript = bagInstance.GetComponent<CashtarothMoneyBag>();
			bagScript.travelDistance = bagDistance;
			bagScript.coinSpeed = coinSpeed;
			bagScript.coinDistance = coinDistance;
			bagScript.coinDamage = coinDamage;
			bagScript.numCoins = numCoins;
			
			if (hitPointRatio < 0.50f)
			{
				rangeAttackTimer = Time.time + p2RangeCooldown;
			}
			else
			{
				rangeAttackTimer = Time.time + p1RangeCooldown;
			}
			rangeOnCooldown = true;
		}
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