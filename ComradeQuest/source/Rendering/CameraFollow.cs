using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public float adjustY = 2;
	public float adjustZ = 1;
	
	private GameObject[] playerArray;
	private int numAlivePlayers;
	private float xComponent, yComponent, zComponent;
	
	void Update () 
	{
		playerArray = GameObject.FindGameObjectsWithTag("Player");
		numAlivePlayers = playerArray.Length;
		xComponent = 0f;
		yComponent = 0f;
		
		switch (numAlivePlayers)
		{
			case 1:
				playerArray = GameObject.FindGameObjectsWithTag("Player");
				foreach (GameObject playerObject in playerArray)
				{
					xComponent = playerObject.transform.position.x;
					yComponent = playerObject.transform.position.y;
				}
				Camera.main.transform.position = new Vector3(xComponent, yComponent + adjustY, adjustZ);
				break;
			case 2:
				foreach (GameObject playerObject in playerArray)
				{
					xComponent += playerObject.transform.position.x;
					yComponent += playerObject.transform.position.y;
				}
				xComponent /= 2;
				yComponent /= 2;
				Camera.main.transform.position = new Vector3(xComponent, yComponent + adjustY, adjustZ);
				break;
			default:
				break;	
		}
		
	}
}