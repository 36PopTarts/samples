using UnityEngine;
using System.Collections;

public class ComboAttack {
	/// <summary>>
	/// This is the parent class for combo attacks, which are defined in the basic attack scripts for each character.
	/// This is NOT a component. This does not inherit from MonoBehaviour or Attack (which inherits from MonoBehaviour) for this reason.
	/// ComboAttack and its children is used to store attack logic, damage values, etc.
	/// </summary>
	public int damage = 1;
	public bool initialized = false;
	protected LayerMask attackLayer;
	public LayerMask AttackLayer {get {return attackLayer;} set{attackLayer = value;}}
	protected float comboLength;
	public float ComboLength {get {return comboLength;}}
	protected GameObject player;
	
	public ComboAttack()
	{
		initialized = false;
	}
	
	public virtual void Effect(int frame) // Damage effect to be used by animation event
	{
	
	}
	// This space for rent.
}
