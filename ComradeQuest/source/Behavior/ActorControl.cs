using UnityEngine;
using System.Collections;

public class ActorControl : MonoBehaviour {
	///<summary>
	/// Parent class for all actors. Contains flags applicable for all actors
	///</summary>
	public bool[] flags;
	protected bool facingRight;
	public bool FacingRight {get {return facingRight;} set {facingRight = value;}}
	

	protected void InitFlags()
	{
		bool invulnerable = false;
		bool frozen = false;
		bool slow = false;
		bool blocking = false; // similar to frozen, but separate in case the player gets frozen by something other than holding block
		flags = new bool[4] {invulnerable, frozen, slow, blocking};
	}
}
