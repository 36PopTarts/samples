using UnityEngine;
using System.Collections;

public class Attack : MonoBehaviour {
	/// <summary>
	/// This class exists so that all attacks may be defined by it, and I can refer to all attacks in collisions
	/// </summary>
	
	public int damage = 1;
	public Collider2D hitCollider; // Set by the editor
	protected Animator attackAnim; // reference animator for attack
}
