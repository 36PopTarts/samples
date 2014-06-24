using UnityEngine;
using System.Collections;

public class MainArtistCollider : MonoBehaviour 
{
	public Sprite[] mainArtistSprites;
	[HideInInspector] public bool selected = false;
	[HideInInspector] public bool pressed = false;
	
	private GameObject mainMenu;
	
	void Start ()
	{
		mainMenu = GameObject.Find ("MainMenu");
	}
	
	public void ResetSprite()
	{
		transform.parent.GetComponent<SpriteRenderer>().sprite = mainArtistSprites [0];
	}
	
	void Update()
	{
		if (selected)
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = mainArtistSprites [1];
		}
		else
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = mainArtistSprites [0];
		}
		if (pressed)
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = mainArtistSprites [2];
			mainMenu.GetComponent<MainMenu>().DisplayArtisticStatement ();
			pressed = false;
		}
	}
}