using UnityEngine;
using System.Collections;

public class MainOptionsCollider : MonoBehaviour 
{
	public Sprite[] mainOptionsSprites;
	[HideInInspector] public bool selected = false;
	[HideInInspector] public bool pressed = false;
	
	private GameObject mainMenu;
	
	void Start ()
	{
		mainMenu = GameObject.Find ("MainMenu");
	}
	
	public void ResetSprite()
	{
		transform.parent.GetComponent<SpriteRenderer>().sprite = mainOptionsSprites [0];
	}
	
	void Update()
	{
		if (selected)
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = mainOptionsSprites [1];
		}
		else
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = mainOptionsSprites [0];
		}
		if (pressed)
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = mainOptionsSprites [2];
			mainMenu.GetComponent<MainMenu>().DisplayOptions ();
			pressed = false;
		}
	}
}