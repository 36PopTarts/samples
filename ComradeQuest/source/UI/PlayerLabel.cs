using UnityEngine;
using System.Collections;

public class PlayerLabel : MonoBehaviour 
{
	public GameObject targetPlayer;
	public Vector3 offset = Vector3.up;

	private int playerNumber;
	private GUIText displayedText;
	private GameObject[] playerArray;

	void Start ()
	{
		playerNumber = targetPlayer.GetComponent<PlayerControl>().playerNumber;
		displayedText = gameObject.GetComponent<GUIText>();
	}

	void Update () 
	{
		if (targetPlayer == null)
		{
			displayedText.text = "";
			playerArray = GameObject.FindGameObjectsWithTag("Player");
			foreach (GameObject playerObject in playerArray)
			{
				if(playerObject != null)
				{
					if (playerObject.GetComponent<PlayerControl>().playerNumber == playerNumber)
					{
						targetPlayer = playerObject;
					}
				}
			}
		}
		else
		{
			displayedText.text = "Player " + playerNumber;
			transform.position = Camera.main.WorldToViewportPoint(targetPlayer.transform.position + offset);
		}
	}
}