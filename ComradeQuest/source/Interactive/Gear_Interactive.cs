using UnityEngine;
using System.Collections;

public class Gear_Interactive : MonoBehaviour {

	public GameObject switch1;

	public float moveY = 8;
	public float moveX= 8;
	
	DoorSwitch switch1_script;
	bool hasMoved = false;

	bool hasPressed1;
	
	void Awake () {
		switch1_script = switch1.GetComponent<DoorSwitch> ();
	}
	
	void FixedUpdate () {
		if (!hasMoved & switch1_script.isPressed) {
			
			transform.position = new Vector3(transform.position.x + moveX, transform.position.y + moveY, 0);
			hasMoved = true;
		}
        if (hasMoved && switch1_script.simple)
            switch1_script.DoorOpened = true;
	}
}
