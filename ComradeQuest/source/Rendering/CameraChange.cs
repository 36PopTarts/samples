using UnityEngine;
using System.Collections;

public class CameraChange : MonoBehaviour 
{
	public float orthographicSize = 13.92f;
	public float moveY = 0;
	public GameObject mainCam;

	private CameraFollow cam;
	private bool hasChanged = false;
	private float originalY = 0;

	void Start()
	{
		cam = mainCam.GetComponent<CameraFollow> ();
		originalY = cam.adjustY;
	}

	void OnTriggerEnter2D (Collider2D other) 
	{
		if (other.tag == "Player")
		{
			Camera.main.orthographicSize = orthographicSize;
			if (!hasChanged)
			{
				cam.adjustY += moveY;
				hasChanged = true;
			}	
		}
	}

	void OnTriggerExit2D (Collider2D other) 
	{
		if (other.tag == "Player")
		{
			Camera.main.orthographicSize = 13.92f;
			cam.adjustY = originalY;
			hasChanged = false;
		}
	}
}