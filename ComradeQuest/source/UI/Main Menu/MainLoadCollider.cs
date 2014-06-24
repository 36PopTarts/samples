using UnityEngine;
using System.Collections;

public class MainLoadCollider : MonoBehaviour 
{
	public Sprite[] mainLoadSprites;
	[HideInInspector] public bool selected = false;
	[HideInInspector] public bool pressed = false;
	
	private GameObject mainMenu;
	
	void Start ()
	{
		mainMenu = GameObject.Find ("MainMenu");
	}
	
	public void ResetSprite()
	{
		transform.parent.GetComponent<SpriteRenderer>().sprite = mainLoadSprites [0];
	}
	
	void Update()
	{
		if (selected)
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = mainLoadSprites [1];
		}
		else
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = mainLoadSprites [0];
		}
		if (pressed)
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = mainLoadSprites [2];
			pressed = false;
		}
	}
}