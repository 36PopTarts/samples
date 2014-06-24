using UnityEngine;
using System.Collections;

public class PauseOptionsCollider : MonoBehaviour 
{
	public Sprite[] pauseOptionsSprites;
	[HideInInspector] public bool selected = false;
	[HideInInspector] public bool pressed = false;

	private GameObject pauseMenu;

	void Start ()
	{
		pauseMenu = GameObject.Find ("PauseMenu");
	}

	public void ResetSprite()
	{
		transform.parent.GetComponent<SpriteRenderer>().sprite = pauseOptionsSprites [0];
	}

	void Update()
	{
		if (selected)
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = pauseOptionsSprites [1];
		}
		else
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = pauseOptionsSprites [0];
		}
		if (pressed)
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = pauseOptionsSprites [2];
			pressed = false;
		}
	}
}