using UnityEngine;
using System.Collections;

public class MainStartCollider : MonoBehaviour 
{
	public Sprite[] mainStartSprites;
	[HideInInspector] public bool selected = false;
	[HideInInspector] public bool pressed = false;
	
	private GameObject mainMenu;
	
	void Start ()
	{
		mainMenu = GameObject.Find ("MainMenu");
	}
	
	public void ResetSprite()
	{
		transform.parent.GetComponent<SpriteRenderer>().sprite = mainStartSprites [0];
	}
	
	void Update()
	{
		if (selected)
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = mainStartSprites [1];
		}
		else
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = mainStartSprites [0];
		}
		if (pressed)
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = mainStartSprites [2];
			Application.LoadLevel (1);
			pressed = false;
		}
	}
}