using UnityEngine;
using System.Collections;

public class DoorOpen : MonoBehaviour {
	
	public GameObject switch1;
	public GameObject switch2;
	public float moveY = 8;
	public float openRate = .5f;
	public float closeRate = -1.5f;
	
	private Vector2 dest;
	private Vector2 init;
	private bool opening;
	private bool closing;
	private bool negative;

	DoorSwitch switch1_script;
	DoorSwitch switch2_script;
	[HideInInspector] public bool hasOpened = false;
	
	public bool sameSide;
	public bool incremental;
	
	bool hasPressed1;
	bool hasPressed2;
	
	void Awake () {
		if(switch1 != null)
			switch1_script = switch1.GetComponent<DoorSwitch> ();
		if(switch2 != null)
			switch2_script = switch2.GetComponent<DoorSwitch> ();

		opening = false;
		closing = false;
		if(moveY < 0)
			negative = true;
		else
			negative = false;
		if(!negative)
			dest = transform.position + new Vector3(0, moveY, 0);
		else 
		{
			openRate = openRate * -1;
			closeRate = closeRate * -1;
			dest = transform.position + new Vector3(0, -moveY, 0);
		}
	}

	void FixedUpdate () {
		if(tag=="Bulletproof")
			Debug.Log ("Closing = "+closing);
		if(opening)
		{
			if(((transform.position.y < dest.y) && !negative) || ((transform.position.y > dest.y) && negative))
			{
				transform.Translate (0, openRate, 0);
			}
			else
			{
				opening = false;
				hasOpened = true;
			}
		}
		
		if(closing)
		{
			Debug.Log ("Closing block reached");
			if(((transform.position.y > init.y) && !negative) || ((transform.position.y < init.y) && negative))
			{
				Debug.Log ("Closing... pos = " + transform.position.ToString());
				transform.Translate (0, closeRate, 0);
			}
			else
			{
				Debug.Log ("Door closed; init = " + init.ToString() + ", position = " + transform.position.ToString());
			 	closing = false;
				hasOpened = false;
			}
		}

		if (!hasOpened) {
			
			if (sameSide && switch1_script != null && switch2_script != null) {
				if (switch1_script.isPressed && switch2_script.isPressed) {
					if(((transform.position.y < dest.y) && !negative) || ((transform.position.y > dest.y) && negative))
					{
						transform.Translate (0, openRate, 0);
						Debug.Log ("Door opening; dest = "+dest.ToString() +", position = "+transform.position.ToString());
					}
					else
					{
						hasOpened = true;
						Debug.Log ("Door opened; dest = "+dest.ToString() +", position = "+transform.position.ToString());
					}
				}
			}
			
			if (incremental && switch1_script != null && switch2_script != null) {
				Vector2 dest2 = new Vector2(dest.x, dest.y + (moveY * 2));
				if (switch1_script.isPressed && !hasPressed1) {
					if(((transform.position.y < dest.y) && !negative) || ((transform.position.y > dest.y) && negative))
						transform.Translate (0, openRate, 0);
					else
						hasPressed1 = true;
				}
				
				if (switch2_script.isPressed && !hasPressed2) {
					if (((transform.position.y < dest2.y) && !negative) || ((transform.position.y > dest2.y) && negative))
						transform.Translate (0, openRate, 0);
					else
						hasPressed2 = true;
				}
				
				if(hasPressed1 && hasPressed2)
					hasOpened = true;
			}
			
		}
		else
		{
			if(switch1_script != null && switch2_script != null)
			{
			if (switch1_script.simple)
				switch1_script.DoorOpened = true;
			if (switch2_script.simple)
				switch2_script.DoorOpened = true;
			}
		}

	}
	
	void Close()
	{
		init = transform.position - new Vector3(0, moveY, 0);
		closing = true;
		Debug.Log ("Closing door; init = " + init.ToString() + ", position = " + transform.position.ToString() + " closing = " + closing);
		Debug.Log (transform.name);
	}
	
	void Open()
	{
		opening = true;
	}
	
	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player")	
			transform.Translate (0, -moveY, 0);
	}
}
