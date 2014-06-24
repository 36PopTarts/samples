using UnityEngine;
using System.Collections;
using System.Text;
using System;
using System.Collections.Generic;

public class PlayerBasicAttack : Attack {

	/// <summary>
	/// Parent class for basic attacks for the different player characters
	/// </summary>

	public LayerMask attackLayer;
	protected Queue<byte> comboCounter; // for counting how many moves player is into combo
	protected float comboTimer = 0f; // for counting how long the player has to continue a combo
	public float comboTime = .25f; // how long the player has between button presses to execute a combo
	protected bool comboState = false; // used to declare that we're using a combo attack, not a basic attack
	protected float attack1Cooldown = 0f; // how long it takes until the player can make another basic attack, in seconds
	protected float attack2Cooldown = 0f; // how long it takes until the player can make another basic attack, in seconds
	protected float attack1Delay = 0f; // timing the attack with the proper animation frame
    protected float attack2Delay = 0f;
	protected bool isCooldown = false; // is the attack on cooldown from the last attack
	protected bool hasAttacked = false;
	protected float attack1Timer = 0f;
	protected float attack2Timer = 0f;
	protected float attack1Length = 0f;
	protected float attack2Length = 0f;
	protected float attack1Speed = 0f;
	protected float attack2Speed = 0f;
	protected float cAttackLength = 0f;
	public float CurrentAttackLength {get {return cAttackLength;} set {cAttackLength = value;}}
    /// <summary>
    /// Use these values instead of the "damage" value
    /// </summary>
    public int attack1Damage = 1;
    public int attack2Damage = 1;
    [HideInInspector] public bool attackState = false; // If true, the player is attacking. Used for making sure that enemies don't get hit with multiple frames of the same attack
	protected Dictionary<Queue<byte>,ComboAttack> combos; // list of all the combos the player can perform, defined in child classes
	private ComboAttack currentCombo;
	
	public float ComboTimer
	{
		get
		{
			return comboTimer;
		}
		set
		{
			comboTimer = value;
		}
	}

	public float Attack1Cooldown
	{
		get
		{
			return attack1Cooldown;
		}
	}
	public float Attack2Cooldown
	{
		get
		{
			return attack2Cooldown;
		}
	}
	public float Attack1Timer
	{
		get
		{
			return attack1Timer;
		}
		set
		{
			attack1Timer = value;
		}
	}
	public float Attack2Timer
	{
		get
		{
			return attack2Timer;
		}
		set
		{
			attack2Timer = value;
		}
	}
	public Queue<byte> ComboCounter
	{
		get
		{
			return comboCounter;
		}
	}
	
	protected void ComboUpdate()
	{
		if(Time.time > comboTimer && comboTimer != 0f) // Clear the queue of combo inputs after the input timer expires
		{
			comboTimer = 0f;
			comboCounter.Clear();
			currentCombo = null;
		//	Debug.Log("Combo queue cleared");
		//	Debug.Log ("Combos: " + combos.Keys.Count);
			foreach (Queue<byte> q in combos.Keys)
			{
				string st = "{";
				foreach(byte b in q)
					st += b + ", ";
				st += "}: ";
				ComboAttack c;
				combos.TryGetValue(q, out c);
				st += c;
				Debug.Log (st);
			}
		}
		if(Time.time < comboTimer) // Input for combos is available
		{
			currentCombo = ExecuteCombo(comboCounter);
			string st = "Execute combo called with ";
			foreach(byte b in comboCounter)
				st += b + ", ";
//			Debug.Log (st);	
			if(currentCombo != null)
			{
				if(currentCombo.initialized)
					currentCombo.AttackLayer = attackLayer;
			}
		}
	}
	
	protected void TryComboEffect(int frame) //Wrapper function that retrieves the proper combo attack effect, to be used with animation event
	{
		if(currentCombo != null)
		{
			currentCombo.Effect(frame);
		}
	}
	
	public virtual void Attack1()
	{
		// This function to be implemented in child classes
	}
	
	public virtual void Attack2()
	{
		// This function to be implemented in child classes
	}

	public ComboAttack ExecuteCombo(Queue<byte> combo)
	{
		ComboAttack output = new ComboAttack();
		if(combos!=null)
		{
			if(combos.TryGetValue(combo, out output))
			{
				String o = "Executing combo ";
				foreach (byte b in combo)
					o += b + ", ";
				Debug.Log (o);
			}
		}
		return output;
	}
}
