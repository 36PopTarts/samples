using UnityEngine;
using System.Collections;

public class DamageZone : Attack {
	
	// Update is called once per frame
	void OnTriggerEnter2D (Collider2D col) {
		col.gameObject.GetComponent<ActorHealth>().Hurt(damage); // Call player function to take damage
	}
}
