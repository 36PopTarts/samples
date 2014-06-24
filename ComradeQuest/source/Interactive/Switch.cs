using UnityEngine;
using System.Collections;

public class Switch : MonoBehaviour {
	/// <summary>
	/// This object triggers a GameObject's Interactive component when it is collided with a player.
	///</summary>
	
	/// <summary>
	/// The linked GameObject to activate. This GameObject requires an "Interactive" script. Linked is used if defined; otherwise, linkedArray is used.
	/// </summary>
	public GameObject linked; 
	public GameObject[] linkedArray;
	protected bool pressed = false; // Has this switch been pressed
	
	void OnCollisionEnter2D(Collision2D col) // Place a collider where you want the player to activate the switch
	{
		if(col.gameObject.tag == "Player")
		{
			if(linked != null)
				linked.GetComponent<Interactive>().Activate();
			else
			{
				for(int i = 0; i < linkedArray.Length; i++)
					linkedArray[i].GetComponent<Interactive>().Activate();
			}
			pressed = true;
		}
	}
}
