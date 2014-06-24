using UnityEngine;
using System.Collections;

public class MainCreditsCollider : MonoBehaviour 
{
	public Sprite[] mainCreditsSprites;
	[HideInInspector] public bool selected = false;
	[HideInInspector] public bool pressed = false;
	
	private GameObject mainMenu;
	
	void Start ()
	{
		mainMenu = GameObject.Find ("MainMenu");
	}
	
	public void ResetSprite()
	{
		transform.parent.GetComponent<SpriteRenderer>().sprite = mainCreditsSprites [0];
	}
	
	void Update()
	{
		if (selected)
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = mainCreditsSprites [1];
		}
		else
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = mainCreditsSprites [0];
		}
		if (pressed)
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = mainCreditsSprites [2];
			pressed = false;
		}
	}
}