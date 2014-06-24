using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour 
{
	public GameObject playerPrefab;
	public int checkpointNumber = 0;
	private float hoverRate = 0.01f;
	private float hoverDist = 0.25f;
	[HideInInspector] public bool checkpointReached = false;

	private GameObject[] playerArray;
	private GameObject spawnedPlayer;
	private GameObject alivePlayer;
	private Animator anim;
	private bool animHasPlayed = false;
	private Vector2 initialPos;
	private Vector2 dest;
	void Start ()
	{
		anim = GetComponent<Animator> ();
		initialPos = gameObject.transform.position;
		dest = new Vector2(initialPos.x, initialPos.y + hoverDist);
	}
	
	void Update () 
	{
		playerArray = GameObject.FindGameObjectsWithTag ("Player");
		
		if(dest.y < initialPos.y)
		{
			transform.Translate (new Vector2(0, -hoverRate));
			if(gameObject.transform.position.y < dest.y)
				dest = new Vector2(initialPos.x, initialPos.y + hoverDist);
		}
		
		if(dest.y > initialPos.y)
		{
			transform.Translate (new Vector2(0, hoverRate));
			if(gameObject.transform.position.y > dest.y)
				dest = new Vector2(initialPos.x, initialPos.y - hoverDist);
		}
			
		if (!checkpointReached)
		{
			foreach (GameObject playerObject in playerArray)
			{
				if ((playerObject.transform.position - transform.position).sqrMagnitude < 50)
				{
					checkpointReached = true;
				}
			}
		}

		if (playerArray.Length < 2 && playerArray.Length > 0)
		{
			// A player has died!
			foreach (GameObject playerObject in playerArray)
			{
				if ((playerObject.transform.position - transform.position).sqrMagnitude < 50) 
				{
					alivePlayer = playerObject;
					var aliveScript = alivePlayer.gameObject.GetComponent<PlayerControl>();
					if (aliveScript.playerNumber == 1)
					{
						spawnedPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity) as GameObject;
						spawnedPlayer.name = "Olaf 2";
						var spawnedScript = spawnedPlayer.gameObject.GetComponent<PlayerControl>();
						spawnedScript.playerNumber = 2;
					}
					else
					{
						spawnedPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity) as GameObject;
						spawnedPlayer.name = "Olaf";
						var spawnedScript = spawnedPlayer.gameObject.GetComponent<PlayerControl>();
						spawnedScript.playerNumber = 1;
					}
				}
			}
		}
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (!animHasPlayed && col.tag == "Player")
		{
			anim.SetBool("hasEntered", true);
			animHasPlayed = true;
		}
	}
}