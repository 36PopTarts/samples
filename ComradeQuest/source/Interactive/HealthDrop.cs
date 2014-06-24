using UnityEngine;
using System.Collections;

public class HealthDrop : MonoBehaviour {

	public int healAmount = 25;
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.gameObject.tag == "Player")
		{
			foreach(GameObject p in GameObject.FindGameObjectsWithTag("Player"))
			{
				var health = p.gameObject.GetComponent<ActorHealth>();
				health.hitPoints += healAmount;
			}
			Destroy (gameObject);
		}
	}
}
