using UnityEngine;
using System.Collections;

public class CashtarothProjectile : MonoBehaviour
{
	[HideInInspector] public int projectileDamage;
	[HideInInspector] public float travelDistance;
	
	private float distanceTraveled = 0.0f;
	private Vector3 lastPosition;
	
	void Start ()
	{
		lastPosition = transform.position;
	}
	
	void Update ()
	{
		distanceTraveled += Vector3.Distance (transform.position, lastPosition);
		lastPosition = transform.position;
		
		if (distanceTraveled > travelDistance)
		{
			Destroy (gameObject);
		}
	}
	
	void OnTriggerEnter2D (Collider2D col) 
	{
		if (col.tag == "Player") // Hit the player
		{
			col.gameObject.GetComponent<ActorHealth>().Hurt(projectileDamage); // Call player function to take damage
			
			Destroy (gameObject); // Destroy the projectile
		}

		if (col.tag == "Bulletproof") // Hit something bulletproof
		{
			Destroy (gameObject); // Destroy the projectile
		}
	}
}