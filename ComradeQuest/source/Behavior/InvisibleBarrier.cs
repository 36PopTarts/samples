using UnityEngine;
using System.Collections;

public class InvisibleBarrier : MonoBehaviour 
{
	public float distanceToPlayer = 50f;

	private GameObject[] playerArray;
	private bool barrierDisabled = false;
	private int numClosePlayers = 0;

	void Update ()
	{
		playerArray = GameObject.FindGameObjectsWithTag ("Player");
		numClosePlayers = 0;

		foreach (GameObject playerObject in playerArray)
		{
			if ((playerObject.transform.position - transform.position).sqrMagnitude < distanceToPlayer)
			{
				numClosePlayers++;
			}
		}

		if (playerArray.Length == 2 && numClosePlayers == 2)
		{
			barrierDisabled = true;
		}

		if (playerArray.Length == 1 && numClosePlayers == 1)
		{
			barrierDisabled = true;
		}

		if (barrierDisabled)
		{
			GetComponent<BoxCollider2D>().enabled = false;
		}
	}
}