using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VladimirLenin : CommunistSummon {
	/// <summary>
	/// Governs movement and effects of the Vladimir Lenin summon
	/// </summary>
	private int finalY = 20; // Final adjustment for Lenin's position (i.e. how much he rises up)
	
	void Start () {
		adjustY = -15;
		InitializeCamera();
		lifeTime = (int)(Time.time + 15);
	}
	
	// Update is called once per frame
	void Update () {
		if(adjustY < finalY)
			adjustY+=.25f; // Speed of rising
		else // Once we're done rising
		{
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player")) // Make players invulnerable
				g.GetComponent<ActorControl>().flags[0] = true;
		}
		lifeCounter = (int)(lifeTime - Time.time);
		if(lifeCounter < 0)
		{
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("Player")) // Make players vulnerable again
				g.GetComponent<ActorControl>().flags[0] = false;
			Destroy(gameObject);
		}
		MoveCamera();
	}
}
