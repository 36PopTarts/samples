using UnityEngine;
using System.Collections;

public class StopAnimAction : Action {

	public override void Perform()
	{
		gameObject.GetComponent<Animator>().speed = 0;
	}
	public override void Perform(float x)
	{
		Perform();
	}
}
