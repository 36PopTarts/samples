using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BrosephStalin : CommunistSummon {
	/// <summary>
	/// Governs movement and effects of the Broseph Stalin summon
	/// </summary>
	private int finalY = 5; // Final adjustment for Stalin's position (i.e. how much he rises up)
	
	void Start () {
		adjustY = -15;
		InitializeCamera();
		lifeTime = (int)(Time.time + 15);
	}
	
	// Update is called once per frame
	void Update () {
		if(adjustY < finalY)
			adjustY+=.10f; // Speed of rising
		else // Once we're done rising
		{
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("Enemy")) // Make enemies frozen
				g.GetComponent<ActorControl>().flags[1] = true;
		}
		lifeCounter = (int)(lifeTime - Time.time);
		if(lifeCounter < 0)
		{
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("Enemy")) // Thaw enemies
				g.GetComponent<ActorControl>().flags[1] = false;
			Destroy(gameObject);
		}
		MoveCamera();
	}
}
