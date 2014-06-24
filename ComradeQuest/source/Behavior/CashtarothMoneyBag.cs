using UnityEngine;
using System.Collections;

public class CashtarothMoneyBag : MonoBehaviour
{
	[HideInInspector] public float travelDistance;
	[HideInInspector] public float coinSpeed;
	[HideInInspector] public float coinDistance;
	[HideInInspector] public int coinDamage;
	[HideInInspector] public int numCoins;
	public Rigidbody2D CashtarothProjectile;
	
	private float distanceTraveled = 0.0f;
	private Vector3 lastPosition;
	private Vector2 coinVelocity;
	
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
			while (numCoins > 0)
			{
				coinVelocity = new Vector2(Random.Range (-1f, 1f), Random.Range (-1f, 1f));
				coinVelocity.Normalize ();
				coinVelocity *= coinSpeed;
			
				Rigidbody2D coinInstance = Instantiate (CashtarothProjectile, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as Rigidbody2D;
				coinInstance.velocity = coinVelocity;
				var coinScript = coinInstance.GetComponent<CashtarothProjectile>();
				coinScript.travelDistance = coinDistance;

				numCoins--;
			}
			Destroy (gameObject);
		}
	}
	
	void OnTriggerEnter2D (Collider2D col) 
	{
		if (col.tag == "Bulletproof") // Hit something bulletproof
		{
			while (numCoins > 0)
			{
				coinVelocity = new Vector2(Random.Range (-1f, 1f), Random.Range (-1f, 1f));
				coinVelocity.Normalize ();
				coinVelocity *= coinSpeed;
			
				Rigidbody2D coinInstance = Instantiate (CashtarothProjectile, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as Rigidbody2D;
				coinInstance.velocity = coinVelocity;
				var coinScript = coinInstance.GetComponent<CashtarothProjectile>();
				coinScript.travelDistance = coinDistance;
				Destroy (gameObject); // Destroy the projectile

				numCoins--;
			}
		}
	}
}