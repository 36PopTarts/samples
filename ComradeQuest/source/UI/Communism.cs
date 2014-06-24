using UnityEngine;
using System.Collections;

public class Communism : MonoBehaviour 
{
	public Sprite[] communismSprites;
	[HideInInspector] public int displayedCommunism;

	private SpriteRenderer spriteRenderer;
	private int actualCommunism;
	private GameObject[] playerArray;

	void Start () 
	{
		spriteRenderer = renderer as SpriteRenderer;
		displayedCommunism = 0;
		spriteRenderer.sprite = communismSprites [0];
	}

	void Update () 
	{
		actualCommunism = 0;

		playerArray = GameObject.FindGameObjectsWithTag("Player");
		foreach (GameObject playerObject in playerArray)
		{
			if (playerObject != null)
			{
				actualCommunism += playerObject.GetComponent<PlayerControl>().communism;
			}
		}

		if (actualCommunism != displayedCommunism)
		{
			spriteRenderer.sprite = communismSprites[(int)Mathf.Clamp(Mathf.RoundToInt(actualCommunism * 41f / 200f), 0f, 41f)];
			displayedCommunism = actualCommunism;
		}
	}
}
