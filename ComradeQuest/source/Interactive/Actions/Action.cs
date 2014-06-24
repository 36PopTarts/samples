using UnityEngine;
using System.Collections;
[RequireComponent(typeof(Interactive))]
public abstract class Action : MonoBehaviour {
	/// <summary>
	/// This is a framework class for children.
	/// Everything listed here is fully defined in Action's children.
	/// </summary>
	public abstract void Perform(); // What happens when this action ... happens?
	public abstract void Perform(float x); 
}