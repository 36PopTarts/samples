using UnityEngine;
using System.Collections;

public class PauseResumeCollider : MonoBehaviour 
{
	public Sprite[] pauseResumeSprites;
	[HideInInspector] public bool selected = false;
	[HideInInspector] public bool pressed = false;

	private GameObject pauseMenu;

	void Start ()
	{
		pauseMenu = GameObject.Find ("PauseMenu");
	}

	public void ResetSprite()
	{
		transform.parent.GetComponent<SpriteRenderer>().sprite = pauseResumeSprites [0];
	}

	void Update()
	{
		if (selected)
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = pauseResumeSprites [1];
		}
		else
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = pauseResumeSprites [0];
		}
		if (pressed)
		{
			transform.parent.GetComponent<SpriteRenderer>().sprite = pauseResumeSprites [2];
			pauseMenu.GetComponent<PauseMenu>().ResumeGame();
			pressed = false;
		}
	}
}