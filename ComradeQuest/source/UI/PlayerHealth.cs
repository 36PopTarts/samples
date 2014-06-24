using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour 
{
	public GameObject playerToDisplay;
	public Sprite[] healthSprites;
	
	private SpriteRenderer spriteRenderer;
	private int playerNumber;
	private GameObject[] playerArray;
	private Camera UICamera;
	private float origX;
	private float newX;
	
	void Start () 
	{
		spriteRenderer = renderer as SpriteRenderer;
		playerNumber = playerToDisplay.GetComponent<PlayerControl>().playerNumber;
		UICamera = transform.parent.GetComponent<Camera>();
		origX = transform.position.x;
	}
	
	void Update () 
	{
		if (UICamera.aspect == (5f/4f))
		{
			transform.position = new Vector3(origX, transform.position.y, transform.position.z);
		}
		else if (UICamera.aspect == 4f/3f)
		{
			newX = origX - 0.25f;
			transform.position = new Vector3(newX, transform.position.y, transform.position.z);
		}
		else
		{
			newX = origX - 2.80f;
			transform.position = new Vector3(newX, transform.position.y, transform.position.z);
		}
		if (playerToDisplay == null)
		{
			playerArray = GameObject.FindGameObjectsWithTag("Player");
			foreach (GameObject playerObject in playerArray)
			{
				if(playerObject != null)
				{
					if (playerObject.GetComponent<PlayerControl>().playerNumber == playerNumber)
					{
						playerToDisplay = playerObject;
					}
				}
			}
			if (playerToDisplay == null)
			{
				spriteRenderer.sprite = healthSprites[16];
			}
		}
		else
		{
			spriteRenderer.sprite = healthSprites[(16 - (int)Mathf.Clamp(Mathf.RoundToInt(playerToDisplay.GetComponent<ActorHealth>().hitPoints * 16f / 100f), 0f, 16f))];
		}
	}
}