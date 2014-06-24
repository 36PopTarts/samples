using UnityEngine;
using System.Collections;

public class Interactive : MonoBehaviour {
 	/// <summary>
 	///	This script defines something that can be activated by switches
 	///</summary>

	/// <summary>
	/// Action to perform upon being activated
	/// </summary>
	public Action triggeredAction;
	// Use this for initialization
	void Start () {
	
	}
	
	public void Activate () // What this thing does when it's activated
	{
		triggeredAction.Perform();
	}
	
	public void Activate (float x)
	{
		triggeredAction.Perform (x);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
