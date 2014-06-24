using UnityEngine;
using System.Collections;

public class GreedDemon : ActorControl 
{
	public float patrolRadius = 1f; // How far to the left and right of starting location enemies patrol
	public float patrolSpeed = 5f; // How fast the enemy patrols
	public float chaseSpeed = 8f; // How fast the enemy chases players
	public float moveAnimSpeed = 1f;
	public float meleeAnimSpeed = 1f;
	public LayerMask attackLayer;
	public bool meleeAttackOn = false;
	public float meleeCooldown = 1.5587f;
	public float damageDelay = 0.5668f;
	public int meleeDamage = 1;
	public float meleeReach = 1.85f; // How far from center of enemy melee attack reaches
	public bool rangeAttackOn = false;
	public float rangeCooldown = 0.25f;
	public int rangeDamage = 1;
	public float rangeReach = 5f;
	public Rigidbody2D GreedDemonProjectile; // The prefab for the projectile the enemy shoots
	public float projectileSpeed = 5f; // How fast projectiles travel
	[HideInInspector] public bool spawnedByCashtaroth = false;
	
	private Vector2 patrolSpeedVector, chaseSpeedVector, direction, movement, pointA, pointB, projectileVelocity, position2D, targetPosition2D;
	private float leftPatrolXBound, rightPatrolXBound;
	private GameObject currentTarget;
	private Animator enemyAnimator;
	private bool meleeOnCooldown = false;
	private float meleeAttackTimer = 0f;
	private float meleeDamageTimer = 0f;
	private bool rangeOnCooldown = false;
	private float rangeAttackTimer = 0f;
	private bool playingMovingAnimation = false;
	private bool playingAttackingAnimation = false;
	private Vector3 enemyViewportPosition;
	private bool enteredChaseState = false;
	private float chaseTimer = 0f;
	
	void Start () 
	{
		facingRight = true;
		patrolSpeedVector = new Vector2 (patrolSpeed, 0);
		chaseSpeedVector = new Vector2 (chaseSpeed, 0);
		if (transform.parent != null)
		{
			leftPatrolXBound = transform.parent.position.x - patrolRadius;
			rightPatrolXBound = transform.parent.position.x + patrolRadius;
		}
		else
		{
			leftPatrolXBound = transform.position.x - patrolRadius;
			rightPatrolXBound = transform.position.x + patrolRadius;
		}
		enemyAnimator = gameObject.GetComponent<Animator>();
		InitFlags ();
	}
	
	void Update () 
	{
		position2D = new Vector2 (transform.position.x, transform.position.y);
		
		playingMovingAnimation = (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Moving"));
		playingAttackingAnimation = (enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attacking"));
		
		if (playingMovingAnimation)
		{
			enemyAnimator.speed = moveAnimSpeed;
		}
		
		if (playingAttackingAnimation)
		{
			enemyAnimator.speed = meleeAnimSpeed;
		}
		
		enemyViewportPosition = new Vector3(Camera.main.WorldToViewportPoint(transform.position).x,
		                                    Camera.main.WorldToViewportPoint(transform.position).y,
		                                    Camera.main.WorldToViewportPoint(transform.position).z);
		
		currentTarget = FindClosestPlayer();
		
		// Below code handles enemy moving and attacking
		if (!flags[1]) // If not frozen
		{
			if (currentTarget == null )
			{
				Patrol (); // Just patrol if all players are dead
			}
			else if (rangeAttackOn)
			{
				targetPosition2D = new Vector2(currentTarget.transform.position.x, currentTarget.transform.position.y);
				
				if (Vector2.Distance (targetPosition2D, position2D) < rangeReach)
				{
					movement = new Vector2 (0, 0); // Stop moving if closest player is within range reach
					if (transform.position.x <= currentTarget.transform.position.x) // Make sure enemy is facing player
					{
						direction.x = 1;
					}
					else
					{
						direction.x = -1;
					}
					RangeAttack ();
					
				}
				else if (enemyViewportPosition.x > 0 && enemyViewportPosition.x < 1 && enemyViewportPosition.y > 0
				         && enemyViewportPosition.y < 1 && currentTarget.transform.position.y > (transform.position.y - 1f)
				         && currentTarget.transform.position.y < (transform.position.y + 1f))
				{
					Chase (); // Move toward closest player to get within range reach
				}
				else
				{
					if (enteredChaseState)
					{
						chaseTimer = Time.time + 1f;
						enteredChaseState = false;
					}
					else if (Time.time > chaseTimer)
					{
						Patrol (); // Just patrol if closest player is on a different level and not within range reach
					}
				}
			}
			else if (meleeAttackOn)
			{
				if (currentTarget.transform.position.x >= (transform.position.x - meleeReach)
				    && currentTarget.transform.position.x <= (transform.position.x + meleeReach)
				    && currentTarget.transform.position.y > (transform.position.y - 1f)
				    && currentTarget.transform.position.y < (transform.position.y + 1f))
				{
					movement = new Vector2 (0, 0); // Stop moving if closest player is within melee reach
					if (transform.position.x <= currentTarget.transform.position.x) // Make sure enemy is facing player
					{
						direction.x = 1;
					}
					else
					{
						direction.x = -1;
					}
					MeleeAttack ();
				}
				else if (enemyViewportPosition.x > 0 && enemyViewportPosition.x < 1 && enemyViewportPosition.y > 0
				         && enemyViewportPosition.y < 1 && currentTarget.transform.position.y > (transform.position.y - 1f)
				         && currentTarget.transform.position.y < (transform.position.y + 1f))
				{
					Chase (); // Move toward closest player to get within melee reach
				}
				else
				{
					if (enteredChaseState)
					{
						chaseTimer = Time.time + 1f;
						enteredChaseState = false;
					}
					else if (Time.time > chaseTimer)
					{
						Patrol (); // Just patrol if closest player is on a different level and not within range reach
					}
				}
			}
			if (direction.x > 0 && !facingRight)
			{
				Flip ();
			}
			else if (direction.x < 0 && facingRight)
			{
				Flip ();
			}
		}
		
		if (meleeOnCooldown)
		{
			if (Time.time > meleeDamageTimer && meleeDamageTimer != 0f)
			{
				MeleeDamage();
				meleeDamageTimer = 0f;
			}
			if (Time.time > meleeAttackTimer)
			{
				meleeOnCooldown = false;
			}
		}
		
		if (rangeOnCooldown)
		{
			if (Time.time > rangeAttackTimer)
			{
				rangeOnCooldown = false;
			}
		}
	}
	
	private void MeleeAttack()
	{
		if(!meleeOnCooldown)
		{
			enemyAnimator.Play("Attacking");
			meleeDamageTimer = Time.time + damageDelay;
			meleeAttackTimer = Time.time + meleeCooldown;
			meleeOnCooldown = true;
		}
	}
	
	private void MeleeDamage()
	{
		if(facingRight)
		{
			pointA = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
			pointB = new Vector2(gameObject.transform.position.x + meleeReach, gameObject.transform.position.y);
		}
		else
		{
			pointA = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y);
			pointB = new Vector2(gameObject.transform.position.x - meleeReach, gameObject.transform.position.y);
		}
		Collider2D[] targets = Physics2D.OverlapAreaAll(pointA, pointB, attackLayer);
		foreach(Collider2D col in targets)
		{
			if(col.gameObject.tag == "Player")
			{
				Debug.Log ("Player collision detected");
				col.gameObject.GetComponent<ActorHealth>().Hurt(meleeDamage);
			}
		}
		meleeDamageTimer = 0f;
	}
	
	private void RangeAttack()
	{
		if (!rangeOnCooldown)
		{
			projectileVelocity = new Vector2(currentTarget.transform.position.x - transform.position.x, currentTarget.transform.position.y - transform.position.y);
			projectileVelocity.Normalize ();
			projectileVelocity *= projectileSpeed;
			
			if (facingRight)
			{
				Rigidbody2D projectileInstance = Instantiate (GreedDemonProjectile, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0,0,0))) as Rigidbody2D;
				projectileInstance.velocity = projectileVelocity;
				var projectileScript = projectileInstance.GetComponent<GreedDemonProjectile>();
				projectileScript.projectileDamage = rangeDamage;
				projectileScript.travelDistance = rangeReach;
			}
			else
			{
				Rigidbody2D projectileInstance = Instantiate (GreedDemonProjectile, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.Euler(new Vector3(0,0,180f))) as Rigidbody2D;
				projectileInstance.velocity = projectileVelocity;
				var projectileScript = projectileInstance.GetComponent<GreedDemonProjectile>();
				projectileScript.projectileDamage = rangeDamage;
				projectileScript.travelDistance = rangeReach;
			}
			
			rangeAttackTimer = Time.time + rangeCooldown;
			rangeOnCooldown = true;
		}
	}

	void FixedUpdate ()
	{
		if(!flags[1]) // If not frozen
		{
			rigidbody2D.velocity = movement;
		}
	}
	
	void Patrol ()
	{
		if (transform.position.x <= leftPatrolXBound) // Make sure enemy is facing patrolling direction
		{
			direction.x = 1;
		}
		if (transform.position.x >= rightPatrolXBound)
		{
			direction.x = -1;
		}
		movement = new Vector2 (patrolSpeedVector.x * direction.x, patrolSpeedVector.y * direction.y);
	}
	
	void Chase ()
	{
		enteredChaseState = true;
		if (transform.position.x <= currentTarget.transform.position.x) // Make sure enemy is facing player
		{
			direction.x = 1;
		}
		else
		{
			direction.x = -1;
		}
		movement = new Vector2 (chaseSpeedVector.x * direction.x, chaseSpeedVector.y * direction.y);
	}
	
	void Flip ()
	{
		facingRight = !facingRight;
		Vector3 scale = transform.localScale;
		scale.x *= -1;
		transform.localScale = scale;
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
	
	public Vector2 Direction
	{
		get {return direction;}
		set {direction = value;}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(pointA, pointB);
	}	
}