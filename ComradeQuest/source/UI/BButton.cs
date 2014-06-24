using UnityEngine;
using System.Collections;

public class BButton : MonoBehaviour 
{
	public Sprite[] bButtonSprites;
	
	private GameObject[] playerArray;
	private bool displayButton = false;
	private int currentSpriteArrayIndex = 1;
	private float nextSwapTime = 0f;
	
	void Update () 
	{
		if (transform.parent.GetComponent<Communism>().displayedCommunism >= 200)
		{
			playerArray = GameObject.FindGameObjectsWithTag ("Player");
			if (playerArray.Length == 2)
			{
				displayButton = true;
			}
			else
			{
				displayButton = false;
			}
		}
		else
		{
			displayButton = false;
		}

		if (!displayButton)
		{
			GetComponent<SpriteRenderer>().sprite = null;
		}
		else
		{
			if (Time.time > nextSwapTime)
			{
				if (currentSpriteArrayIndex == 0)
				{
					GetComponent<SpriteRenderer>().sprite = bButtonSprites[1];
					currentSpriteArrayIndex = 1;
					nextSwapTime = Time.time + 0.25f;
				}
				else
				{
					GetComponent<SpriteRenderer>().sprite = bButtonSprites[0];
					currentSpriteArrayIndex = 0;
					nextSwapTime = Time.time + 0.25f;
				}
			}
		}
	}
}