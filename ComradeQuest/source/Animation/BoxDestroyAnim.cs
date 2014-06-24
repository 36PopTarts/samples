using UnityEngine;
using System.Collections;

public class BoxDestroyAnim : DeathAnim {

	private ActorHealth boxHealth;

	void Start()
	{
		boxHealth = gameObject.GetComponent<ActorHealth>();
		deathCount = 20;
	}
	
	void Update()
	{
		if(boxHealth.hitPoints<1)
				gameObject.GetComponent<Collider2D>().enabled = false;
			
	}
}
