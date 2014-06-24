using UnityEngine;
using System.Collections;

public class CheckpointParticles : MonoBehaviour {

	bool hasPlayed = false;

	void OnTriggerEnter2D () 
	{
		if (!hasPlayed)
		{
			particleSystem.Play();
			hasPlayed = true;
		}
	}
}
