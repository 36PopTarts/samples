using UnityEngine;
using System.Collections;

public class PlayerJumpAttack : Attack {
	/// <summary>
	/// It's the player's jump attack (Olaf for now)
	/// </summary>
	
	void OnTriggerEnter2D (Collider2D col) 
	{
		if(col.tag == "Enemy" || col.tag == "Destroyable") // Hit the enemy
		{
			col.gameObject.GetComponent<ActorHealth>().SetLastAttacker(gameObject);
			col.gameObject.GetComponent<ActorHealth>().Hurt(); // Call player function to take damage
		}
	}
}
