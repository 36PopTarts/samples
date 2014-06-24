using UnityEngine;
using System.Collections;

public class PressureSwitch : Switch {
	/// <summary>
	/// Switch that the player jumps on to activate
	/// </summary>
	
	/// <summary>
	/// The amount to move the switch button when it's pressed. 
	/// </summary>
	public float moveX; // Negative is left, positive is right
	public float moveY; // Negative is down, positive is up
	/// <summary>
	/// Direction for the switch button to move. Only use unit vectors (i.e., 0 0 1, 1 0 0, 0 1 0...)	
	/// </summary>
	public Vector3 direction = new Vector3(0, -1, 0); 
	/// <summary>
	/// Speed factor for button movement
	/// </summary>
	public float speed = 1f;
	private Vector3 currentPos; // Current position
	private Animator anim; // Animator for button animation when pressed
	
	void Start()
	{
		anim = GetComponent<Animator>();
	}
	
	void Update () {
		if (pressed)
		{
			if(!anim.GetBool("isPressed"))
				anim.SetBool("isPressed",true);
		}
	}
}
