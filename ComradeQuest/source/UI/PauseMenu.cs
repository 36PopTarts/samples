using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour 
{	
	public GameObject playerPrefab;
	
	private GameObject[] playerArray, checkpointArray;
	private GameObject pauseBackground, pauseOptionsCollider, pauseResumeCollider, pauseReturnCollider;
	private bool gameIsPaused = false;
	private Sprite[] pauseResumeSprites, pauseOptionsSprites, pauseReturnSprites;
	private int highestCheckpointReached = 0;
	private GameObject spawnedPlayer1, spawnedPlayer2;
	private float verticalJoystickInput, nextSelectionTime;
	private string[] menuOptions = new string[3];
	private int selectedMenuOption = 0;
	private bool aButtonInput;
	
	void Start ()
	{
		pauseBackground = GameObject.Find ("PauseBackground");
		pauseOptionsCollider = GameObject.Find ("PauseOptionsCollider");
		pauseResumeCollider = GameObject.Find ("PauseResumeCollider");
		pauseReturnCollider = GameObject.Find ("PauseReturnCollider");

		pauseBackground.SetActive(false);

		menuOptions [0] = "Resume Game";
		menuOptions [1] = "Options";
		menuOptions [2] = "Return To Menu";

		nextSelectionTime = Time.time;
	}
	
	public void PauseGame ()
	{
		if (!gameIsPaused)
		{
			gameIsPaused = true;
			pauseBackground.SetActive(true);
			pauseOptionsCollider.GetComponent<PauseOptionsCollider>().ResetSprite();
			pauseResumeCollider.GetComponent<PauseResumeCollider>().ResetSprite();
			pauseReturnCollider.GetComponent<PauseReturnCollider>().ResetSprite();
			Time.timeScale = 0.00001f;
		}
	}
	
	public void ResumeGame ()
	{
		if (gameIsPaused)
		{
			Time.timeScale = 1.0f;
			gameIsPaused = false;
			pauseBackground.SetActive(false);
		}
	}

	private void MenuSelection (string direction)
	{
		if (Time.time > nextSelectionTime)
		{
			nextSelectionTime = Time.time + 0.0000025f;
			if (direction == "up")
			{
				if (selectedMenuOption == 2)
				{
					selectedMenuOption -= 2;
				}
				else
				{
					selectedMenuOption += 1;
				}
			}
			
			if (direction == "down")
			{
				if (selectedMenuOption == 0)
				{
					selectedMenuOption += 2;
				}
				else
				{
					selectedMenuOption -= 1;
				}
			}
		}
	}
	
	private void MenuPress ()
	{
		switch (selectedMenuOption)
		{
			case 0:
				pauseResumeCollider.GetComponent<PauseResumeCollider>().pressed = true;
				break;
			case 1:
				pauseOptionsCollider.GetComponent<PauseOptionsCollider>().pressed = true;
				break;
			case 2:
				pauseReturnCollider.GetComponent<PauseReturnCollider>().pressed = true;
				break;
		}
	}
	
	void Update () 
	{
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
			MenuPress();
		}
		else
		{
			aButtonInput = Input.GetButtonDown ("Jump_P2");
			if (aButtonInput != false)
			{
				MenuPress();
			}
		}

		switch (selectedMenuOption)
		{
			case 0:
				pauseResumeCollider.GetComponent<PauseResumeCollider>().selected = true;
				pauseOptionsCollider.GetComponent<PauseOptionsCollider>().selected = false;
				pauseReturnCollider.GetComponent<PauseReturnCollider>().selected = false;
				break;
			case 1:
				pauseResumeCollider.GetComponent<PauseResumeCollider>().selected = false;
				pauseOptionsCollider.GetComponent<PauseOptionsCollider>().selected = true;
				pauseReturnCollider.GetComponent<PauseReturnCollider>().selected = false;
				break;
			case 2:
				pauseResumeCollider.GetComponent<PauseResumeCollider>().selected = false;
				pauseOptionsCollider.GetComponent<PauseOptionsCollider>().selected = false;
				pauseReturnCollider.GetComponent<PauseReturnCollider>().selected = true;
				break;
		}
		
		bool Start_P1 = Input.GetButtonDown("Start_P1");
		bool Start_P2 = Input.GetButtonDown("Start_P2");
		
		if (Start_P1 || Start_P2)
		{
			if (gameIsPaused)
			{
				ResumeGame();
			}
			else
			{
				PauseGame();
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if (gameIsPaused)
			{
				ResumeGame();
			}
			else
			{
				PauseGame ();
			}
		}
		
		playerArray = GameObject.FindGameObjectsWithTag ("Player");
		
		if (playerArray.Length < 1)
		{
			checkpointArray = GameObject.FindGameObjectsWithTag ("Checkpoint");
			foreach (GameObject checkpointObject in checkpointArray)
			{
				if (checkpointObject.GetComponent<Checkpoint>().checkpointReached)
				{
					if (checkpointObject.GetComponent<Checkpoint>().checkpointNumber > highestCheckpointReached)
					{
						highestCheckpointReached = checkpointObject.GetComponent<Checkpoint>().checkpointNumber;
					}
				}
			}
			
			foreach (GameObject checkpointObject in checkpointArray)
			{
				if (checkpointObject.GetComponent<Checkpoint>().checkpointNumber == highestCheckpointReached)
				{
					spawnedPlayer1 = Instantiate(playerPrefab, checkpointObject.transform.position, Quaternion.identity) as GameObject;
					spawnedPlayer1.name = "Olaf";
					var spawnedScript1 = spawnedPlayer1.gameObject.GetComponent<PlayerControl>();
					spawnedScript1.playerNumber = 1;
					spawnedPlayer2 = Instantiate(playerPrefab, checkpointObject.transform.position, Quaternion.identity) as GameObject;
					spawnedPlayer2.name = "Olaf 2";
					var spawnedScript2 = spawnedPlayer1.gameObject.GetComponent<PlayerControl>();
					spawnedScript2.playerNumber = 2;
				}
			}
			
			foreach (GameObject g in GameObject.FindGameObjectsWithTag("Arena"))
			{
				if(g.GetComponent<ArenaControl>().PlayersReady)
					g.SendMessage("Reset");
			}
			
			foreach (GameObject g in GameObject.FindGameObjectsWithTag("Enemy")) // Clear remaining arena enemies upon death.
			{
				if (g.GetComponent<ActorHealth>().arenaEnemy)
					Destroy(g);
			}
		}
	}
}