using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour 
{	
	public Sprite[] mainBackgroundSprites;

	private bool mainMenuDisplayed = true;
	private bool optionsDisplayed = false;
	private bool artisticStatementDisplayed = false;
	private GameObject mainBackground, mainArtist, mainCredits, mainExit, mainOptions, mainStart, mainLoad;
	private GameObject mainArtistCollider, mainCreditsCollider, mainExitCollider, mainOptionsCollider, mainStartCollider, mainLoadCollider;
	private Sprite[] mainArtistSprites, mainCreditsSprites, mainExitSprites, mainOptionsSprites, mainStartSprites, mainLoadSprites;
	private float horizontalJoystickInput, verticalJoystickInput, nextSelectionTime;
	private bool aButtonInput, bButtonInput;
	private string[] menuOptions = new string[6];
	private int selectedMenuOption = 0;
	
	void Start ()
	{
		mainBackground = GameObject.Find ("MainBackground");
		mainArtist = GameObject.Find ("MainArtist");
		mainCredits = GameObject.Find ("MainCredits");
		mainExit = GameObject.Find ("MainExit");
		mainOptions = GameObject.Find ("MainOptions");
		mainStart = GameObject.Find ("MainStart");
		mainLoad = GameObject.Find ("MainLoad");
		
		mainArtistCollider = GameObject.Find ("MainArtistCollider");
		mainCreditsCollider = GameObject.Find ("MainCreditsCollider");
		mainExitCollider = GameObject.Find ("MainExitCollider");
		mainOptionsCollider = GameObject.Find ("MainOptionsCollider");
		mainStartCollider = GameObject.Find ("MainStartCollider");
		mainLoadCollider = GameObject.Find ("MainLoadCollider");
		
		menuOptions [0] = "Start Game";
		menuOptions [1] = "Load Game";
		menuOptions [2] = "Artistic Statement";
		menuOptions [3] = "Credits";
		menuOptions [4] = "Options";
		menuOptions [5] = "Exit";

		nextSelectionTime = Time.time;
	}
	
	public void DisplayMainMenu ()
	{
		if (!mainMenuDisplayed)
		{
			mainBackground.GetComponent<SpriteRenderer>().sprite = mainBackgroundSprites[0];
			mainArtist.SetActive (true);
			mainCredits.SetActive (true);
			mainExit.SetActive (true);
			mainOptions.SetActive (true);
			mainStart.SetActive (true);
			mainLoad.SetActive (true);
			mainArtistCollider.GetComponent<MainArtistCollider>().ResetSprite ();
			mainCreditsCollider.GetComponent<MainCreditsCollider>().ResetSprite ();
			mainExitCollider.GetComponent<MainExitCollider>().ResetSprite ();
			mainOptionsCollider.GetComponent<MainOptionsCollider>().ResetSprite ();
			mainStartCollider.GetComponent<MainStartCollider>().ResetSprite ();
			mainLoadCollider.GetComponent<MainLoadCollider>().ResetSprite ();
			mainMenuDisplayed = true;
			optionsDisplayed = false;
			artisticStatementDisplayed = false;
		}
	}
	
	public void DisplayOptions ()
	{
		if (!optionsDisplayed)
		{
			mainBackground.GetComponent<SpriteRenderer>().sprite = mainBackgroundSprites[1];
			mainArtist.SetActive (false);
			mainCredits.SetActive (false);
			mainExit.SetActive (false);
			mainOptions.SetActive (false);
			mainStart.SetActive (false);
			mainLoad.SetActive (false);
			optionsDisplayed = true;
			mainMenuDisplayed = false;
			artisticStatementDisplayed = false;
		}
	}
	
	public void DisplayArtisticStatement ()
	{
		if (!artisticStatementDisplayed)
		{
			mainBackground.GetComponent<SpriteRenderer>().sprite = mainBackgroundSprites[2];
			mainArtist.SetActive (false);
			mainCredits.SetActive (false);
			mainExit.SetActive (false);
			mainOptions.SetActive (false);
			mainStart.SetActive (false);
			mainLoad.SetActive (false);
			artisticStatementDisplayed = true;
			optionsDisplayed = false;
			mainMenuDisplayed = false;
		}
	}
	
	private void MenuSelection (string direction)
	{
		if (Time.time > nextSelectionTime && !artisticStatementDisplayed && !optionsDisplayed)
		{
			nextSelectionTime = Time.time + 0.25f;
			if (direction == "up")
			{
				if (selectedMenuOption == 0 || selectedMenuOption == 1 || selectedMenuOption == 2)
				{
					selectedMenuOption += 3;
				}
				else
				{
					selectedMenuOption -= 3;;
				}
			}
		
			if (direction == "down")
			{
				if (selectedMenuOption == 0 || selectedMenuOption == 1 || selectedMenuOption == 2)
				{
					selectedMenuOption += 3;
				}
				else
				{
					selectedMenuOption -= 3;
				}
			}
		
			if (direction == "left")
			{
				if (selectedMenuOption == 0)
				{
					selectedMenuOption = 5;
				}
				else
				{
					selectedMenuOption -= 1;
				}
			}
		
			if (direction == "right")
			{
				if (selectedMenuOption == 5)
				{
					selectedMenuOption = 0;
				}
				else
				{
					selectedMenuOption += 1;
				}
			}
		}
	}

	private void MenuPress ()
	{
		switch (selectedMenuOption)
		{
			case 0:
				mainStartCollider.GetComponent<MainStartCollider>().pressed = true;
				break;
			case 1:
				mainLoadCollider.GetComponent<MainLoadCollider>().pressed = true;
				break;
			case 2:
				mainArtistCollider.GetComponent<MainArtistCollider>().pressed = true;
				break;
			case 3:
				mainCreditsCollider.GetComponent<MainCreditsCollider>().pressed = true;
				break;
			case 4:
				mainOptionsCollider.GetComponent<MainOptionsCollider>().pressed = true;
				break;
			case 5:
				mainExitCollider.GetComponent<MainExitCollider>().pressed = true;
				break;
		}
	}
	
	void Update () 
	{	
		horizontalJoystickInput = Input.GetAxis ("Horizontal_P1"); // Check P1 input
		if (horizontalJoystickInput != 0) // If there is input...
		{
			if (horizontalJoystickInput < 0)
			{
				// left
				MenuSelection ("left");
			}
			else
			{
				// right
				MenuSelection ("right");
			}
		}
		else
		{
			horizontalJoystickInput = Input.GetAxis ("Horizontal_P2"); // Check P2 input
			
			if (horizontalJoystickInput != 0) // If there is input...
			{
				if (horizontalJoystickInput < 0)
				{
					// left
					MenuSelection ("left");
				}
				else
				{
					// right
					MenuSelection ("right");
				}
			}
		}
		
		verticalJoystickInput = Input.GetAxis ("Vertical_P1"); // Check P1 input
		if (verticalJoystickInput != 0)
		{
			if (verticalJoystickInput < 0) // If there is input...
			{
				// down
				MenuSelection ("down");
			}
			else
			{
				// up
				MenuSelection ("up");
			}
		}
		else
		{
			verticalJoystickInput = Input.GetAxis ("Vertical_P2"); // Check P2 input
			
			if (verticalJoystickInput != 0) // If there is input...
			{
				if (verticalJoystickInput < 0)
				{
					// down
					MenuSelection ("down");
				}
				else
				{
					// up
					MenuSelection ("up");
				}
			}
		}

		aButtonInput = Input.GetButtonDown ("Jump_P1");
		if (aButtonInput != false)
		{
			if (!artisticStatementDisplayed && !optionsDisplayed)
			{
				MenuPress();
			}
		}
		else
		{
			aButtonInput = Input.GetButtonDown ("Jump_P2");
			if (aButtonInput != false)
			{
				if (!artisticStatementDisplayed && !optionsDisplayed)
				{
					MenuPress();
				}
			}
		}

		bButtonInput = Input.GetButtonDown ("Summon_P1");
		if (bButtonInput != false)
		{
			if (artisticStatementDisplayed || optionsDisplayed)
			{
				DisplayMainMenu();
			}
		}
		else
		{
			bButtonInput = Input.GetButtonDown ("Summon_P2");
			if (bButtonInput != false)
			{
				if (artisticStatementDisplayed || optionsDisplayed)
				{
					DisplayMainMenu();
				}
			}
		}
		
		switch (selectedMenuOption)
		{
			case 0:
				mainStartCollider.GetComponent<MainStartCollider>().selected = true;
				mainLoadCollider.GetComponent<MainLoadCollider>().selected = false;
				mainArtistCollider.GetComponent<MainArtistCollider>().selected = false;
				mainCreditsCollider.GetComponent<MainCreditsCollider>().selected = false;
				mainOptionsCollider.GetComponent<MainOptionsCollider>().selected = false;
				mainExitCollider.GetComponent<MainExitCollider>().selected = false;
				break;
			case 1:
				mainStartCollider.GetComponent<MainStartCollider>().selected = false;
				mainLoadCollider.GetComponent<MainLoadCollider>().selected = true;
				mainArtistCollider.GetComponent<MainArtistCollider>().selected = false;
				mainCreditsCollider.GetComponent<MainCreditsCollider>().selected = false;
				mainOptionsCollider.GetComponent<MainOptionsCollider>().selected = false;
				mainExitCollider.GetComponent<MainExitCollider>().selected = false;
				break;
			case 2:
				mainStartCollider.GetComponent<MainStartCollider>().selected = false;
				mainLoadCollider.GetComponent<MainLoadCollider>().selected = false;
				mainArtistCollider.GetComponent<MainArtistCollider>().selected = true;
				mainCreditsCollider.GetComponent<MainCreditsCollider>().selected = false;
				mainOptionsCollider.GetComponent<MainOptionsCollider>().selected = false;
				mainExitCollider.GetComponent<MainExitCollider>().selected = false;
				break;
			case 3:
				mainStartCollider.GetComponent<MainStartCollider>().selected = false;
				mainLoadCollider.GetComponent<MainLoadCollider>().selected = false;
				mainArtistCollider.GetComponent<MainArtistCollider>().selected = false;
				mainCreditsCollider.GetComponent<MainCreditsCollider>().selected = true;
				mainOptionsCollider.GetComponent<MainOptionsCollider>().selected = false;
				mainExitCollider.GetComponent<MainExitCollider>().selected = false;
				break;
			case 4:
				mainStartCollider.GetComponent<MainStartCollider>().selected = false;
				mainLoadCollider.GetComponent<MainLoadCollider>().selected = false;
				mainArtistCollider.GetComponent<MainArtistCollider>().selected = false;
				mainCreditsCollider.GetComponent<MainCreditsCollider>().selected = false;
				mainOptionsCollider.GetComponent<MainOptionsCollider>().selected = true;
				mainExitCollider.GetComponent<MainExitCollider>().selected = false;
				break;
			case 5:
				mainStartCollider.GetComponent<MainStartCollider>().selected = false;
				mainLoadCollider.GetComponent<MainLoadCollider>().selected = false;
				mainArtistCollider.GetComponent<MainArtistCollider>().selected = false;
				mainCreditsCollider.GetComponent<MainCreditsCollider>().selected = false;
				mainOptionsCollider.GetComponent<MainOptionsCollider>().selected = false;
				mainExitCollider.GetComponent<MainExitCollider>().selected = true;
				break;
		}
	}
}