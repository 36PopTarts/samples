using UnityEngine;
using System.Collections;

public class PauseReturnCollider : MonoBehaviour 
{
	public Sprite[] pauseReturnSprites;
	[HideInInspector] public bool selected = false;
	[HideInInspector] public bool pressed = false;

	private GameObject pauseMenu;

	void Start ()
	{
		pauseMenu = GameObject.Find ("PauseMenu");
	}

	public void ResetSprite()
	{
		transform.parent.GetComponent<SpriteRenderer>().sprite = pauseReturnSprites [0];
	}

	void Update()
	{
		if (selected)
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = pauseReturnSprites [1];
		}
		else
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = pauseReturnSprites [0];
		}
		if (pressed)
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = pauseReturnSprites [2];
			pauseMenu.GetComponent<PauseMenu>().ResumeGame();
			Application.LoadLevel (0);
			pressed = false;
		}
	}
}