using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class OlafBasicAttack : PlayerBasicAttack {

	/// <summary>
	/// Olaf's basic attacks and combos
	/// </summary>
	private BoxCollider2D playerCollider;
	private PlayerControl playerControl;
	private Animator playerAnim; 
	public bool showAnimations = true; // Whether to show attack animations
	private float cooldownTimer = 0f;
	private float animTimer = 0f;
    public float attack1SpeedNormalized = .5f;
    public float attack2SpeedNormalized = .5f;
	public float attack1Recovery = 0.10f; // In seconds
	public float attack2Recovery = 0.10f; // In seconds
	private float attack1Duration = 0f;
	private float attack2Duration = 0f;
	private float anim1Timer = 0f;
	private float anim2Timer = 0f;
    private bool inAttack1State = false;
    private bool inAttack2State = false;
    private bool attacking1 = false;
    private bool attacking2 = false;
    public float attack1RangeFactor = 1f; // Multiplier for attack 1's range.
    public float attack2RangeFactor = 1f; // Multiplier for attack 2's range.
	private Vector2 pointA, pointB;

	public AudioClip attackSound;

	void Start () 
	{
		playerCollider = gameObject.GetComponent<BoxCollider2D>();
		playerControl = gameObject.GetComponent<PlayerControl>();
		playerAnim = gameObject.GetComponent<Animator>();
		attack2Length = 1.5f; // Hardcoded because there is no reliable way to dynamically access this
		attack1Length = 1.222f; // Hardcoded because there is no reliable way to dynamically access this
		pointA = new Vector2();
		pointB = new Vector2();
		comboCounter = new Queue<byte>();
        attack1Speed = (1f + 3f) * attack1SpeedNormalized;
        attack2Speed = (1f + 3f) * attack2SpeedNormalized;
        playerAnim.SetFloat("attack1Speed", attack1SpeedNormalized);
        playerAnim.SetFloat("attack2Speed", attack2SpeedNormalized);
        attack1Cooldown = attack1Recovery;
		attack2Cooldown = attack2Recovery;
        attack1Delay = attack1Length / attack1Speed * .8f;
        attack2Delay = attack2Length / attack2Speed * .72f;
        attack1Duration = attack1Length / attack1Speed;
        attack2Duration = attack2Length / attack2Speed;
        //Debug.Log(" Cooldown: " + attackCooldown + " Delay 1: " + attack1Delay + " Speed 1: " + attack1Speed + " Delay 2: " + attack2Delay + " Speed 2: " + attack2Speed);
        combos = new Dictionary<Queue<byte>, ComboAttack>();
		// combo definitions begin here
		// Example : combos.Add(new Queue<byte>({1,1,2}), new Attack()); // AAB combo
		Queue<byte> combo;
		byte[] b = {1, 1, 2};
		combo = new Queue<byte>(b);
		combos.Add (combo, new TestComboAttack(gameObject));
	}
	
	void Update()
	{
		if(showAnimations)
		{
        	inAttack1State = (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("basic attack 1") || playerAnim.GetCurrentAnimatorStateInfo(0).IsName("basic attack 1 alt"));
        	inAttack2State = (playerAnim.GetCurrentAnimatorStateInfo(0).IsName("basic attack 2") || playerAnim.GetCurrentAnimatorStateInfo(0).IsName("basic attack 2 alt"));
        }
        else 
        {
        	if(attacking1 && !inAttack1State)
        	{
        		inAttack1State = true;
        		cAttackLength = attack1Length;
        		anim1Timer = Time.time + attack1Duration;
          	}
        	if(attacking2 && !inAttack2State)
        	{
        		inAttack2State = true;
				cAttackLength = attack2Length;
        		anim2Timer = Time.time + attack2Duration;
        	}
        	if(Time.time > anim1Timer)
        	{
        		inAttack1State = false;
				cAttackLength = 0f;
        	}
			if(Time.time > anim2Timer)
			{
				inAttack2State = false;
				cAttackLength = 0f;
			}
		}
		
		if(inAttack1State)
		{
            cooldownTimer = Time.time + attack1Cooldown; // Continuously updates to current time until player is out of attacking animation
            attackState = true;
        }
        else if (inAttack2State)
        {
			cooldownTimer = Time.time + attack2Cooldown; // Continuously updates to current time until player is out of attacking animation
			attackState = true;
        }
        else
        	attackState = false;
        
        isCooldown = Time.time <= cooldownTimer;
		if(isCooldown)
		{       
			if(Time.time > attack1Timer && attacking1 && inAttack1State)
			{
				//Attack1Damage();
				attack1Timer = 0f;
                attacking1 = false;
			}
            if (Time.time > attack2Timer && attacking2 && inAttack2State)
            {
                //Attack2Damage();
                attack2Timer = 0f;
                attacking2 = false;
            }
		}	
		
		ComboUpdate();
	}
	
	public override void Attack1()
	{
        //Debug.Log("Attack 1 anim length: " + playerAnim.GetCurrentAnimatorStateInfo(0).length);
        if(!comboState)
        {
			if(!isCooldown)
			{
				if(showAnimations)
				{
		            if(playerControl.playerNumber == 1)
					    playerAnim.Play("basic attack 1"); // begin attack animation
		            else
		                playerAnim.Play("basic attack 1 alt");
	            }
				attack1Timer = Time.time + attack1Delay;
	            cooldownTimer = Time.time + attack1Cooldown;
	            //Debug.Log("Current time: " + Time.time + " Cooldown Time: " + cooldownTimer);
	            attacking1 = true;
				audio.PlayOneShot(attackSound);
			}
		}
	}
	
	/*private void Attack1Damage()
	{
		Debug.Log ("Called, attacked, damage dealt, hidden, reported, called the cops");
		if(playerControl.FacingRight)
		{
			pointA = new Vector2(gameObject.transform.position.x + playerCollider.size.x/2, gameObject.transform.position.y);
			pointB = new Vector2(gameObject.transform.position.x + 2*(playerCollider.size.x + .55f), gameObject.transform.position.y + .5f);
		}
		else
		{
			pointA = new Vector2(gameObject.transform.position.x - playerCollider.size.x/2, gameObject.transform.position.y);
			pointB = new Vector2(gameObject.transform.position.x - 2*(playerCollider.size.x + .55f), gameObject.transform.position.y + .5f);
		}
		Collider2D[] targets = Physics2D.OverlapAreaAll(pointA, pointB, attackLayer);
		foreach(Collider2D col in targets)
		{
			if(col.gameObject.tag == "Enemy" || col.gameObject.layer == LayerMask.NameToLayer("Crates"))
			{
				Debug.Log ("Successfully found enemy");
				col.gameObject.GetComponent<ActorHealth>().Hurt (attack1Damage);
			}
		}
	}*/
	
	private void Attack1HitDetection(int frame)
	{
		if(playerControl.FacingRight)
		{
			switch(frame)
			{
				case 1: pointA = new Vector2(gameObject.transform.position.x + playerCollider.size.x/2, gameObject.transform.position.y + .4f);
				pointB = new Vector2(gameObject.transform.position.x + 2*(playerCollider.size.x)*attack1RangeFactor, gameObject.transform.position.y + .5f);
				break;
				case 2: pointA = new Vector2(gameObject.transform.position.x + playerCollider.size.x/2, gameObject.transform.position.y + .4f);
				pointB = new Vector2(gameObject.transform.position.x + 2*(playerCollider.size.x + .55f)*attack1RangeFactor, gameObject.transform.position.y + .5f); //.55f
				break;
			}
		}
		else
		{
			switch(frame)
			{
				case 1: pointA = new Vector2(gameObject.transform.position.x - playerCollider.size.x/2, gameObject.transform.position.y + .4f);
				pointB = new Vector2(gameObject.transform.position.x - 2*(playerCollider.size.x)*attack1RangeFactor, gameObject.transform.position.y + .5f);
				break;
				case 2: pointA = new Vector2(gameObject.transform.position.x - playerCollider.size.x/2, gameObject.transform.position.y + .4f);
				pointB = new Vector2(gameObject.transform.position.x - 2*(playerCollider.size.x + .55f)*attack1RangeFactor, gameObject.transform.position.y + .5f); // .55f
				break;
			}
		}
		Collider2D[] targets = Physics2D.OverlapAreaAll(pointA, pointB, attackLayer);
		foreach(Collider2D col in targets)
		{
			if(col.gameObject.tag == "Enemy" || col.gameObject.layer == LayerMask.NameToLayer("Crates"))
			{
				Debug.Log ("Successfully found enemy");
				var ah = col.gameObject.GetComponent<ActorHealth>();
				
				if(ah.LastAttacker != gameObject)
					ah.Hurt (attack1Damage);
					
				ah.LastAttacker = gameObject;
			}
		}
		attack1Timer = 0f; 
	}
	
	public override void Attack2()
	{
        //Debug.Log("Attack 2 anim length: " + playerAnim.GetCurrentAnimatorStateInfo(0).length);
        if(!comboState)
        {
	        if (!isCooldown)
	        {
	        	if(showAnimations)
	        	{
	            if (playerControl.playerNumber == 1)
	                playerAnim.Play("basic attack 2"); // begin attack animation
	            else
	                playerAnim.Play("basic attack 2 alt");
	            }
	            attack2Timer = Time.time + attack2Delay;
	            cooldownTimer = Time.time + attack2Cooldown;
	            //Debug.Log("Current time: " + Time.time + " Cooldown Time: " + cooldownTimer);
	            attacking2 = true;
	        }
	        else
	           Debug.Log("On cooldown");
        }
	}
	
	/*private void Attack2Damage()
	{
		Debug.Log ("Called, attacked, damage dealt, hidden, reported, called the cops");
		if(playerControl.FacingRight)
		{
			pointA = new Vector2(gameObject.transform.position.x + playerCollider.size.x/2, gameObject.transform.position.y + .5f);
			pointB = new Vector2(gameObject.transform.position.x + playerCollider.size.x*3, gameObject.transform.position.y);
		}
		else
		{
			pointA = new Vector2(gameObject.transform.position.x - playerCollider.size.x/2, gameObject.transform.position.y + .5f);
			pointB = new Vector2(gameObject.transform.position.x - playerCollider.size.x*3, gameObject.transform.position.y);
		}
		Collider2D[] targets = Physics2D.OverlapAreaAll(pointA, pointB, attackLayer);
		foreach(Collider2D col in targets)
		{
			if(col.gameObject.tag == "Enemy" || col.gameObject.layer == LayerMask.NameToLayer("Crates"))
			{
				Debug.Log ("Successfully found enemy");
				col.gameObject.GetComponent<ActorHealth>().Hurt (attack2Damage);
			}
		}
		attack2Timer = 0f; 
	}*/
	
	private void Attack2HitDetection(int frame)
	{
		if(playerControl.FacingRight)
		{
			switch(frame)
			{
			case 1: pointA = new Vector2(gameObject.transform.position.x - playerCollider.size.x-.5f, gameObject.transform.position.y + 2.75f);
				pointB = new Vector2(gameObject.transform.position.x + playerCollider.size.x*2.4f*attack2RangeFactor, gameObject.transform.position.y+4);
				break;
			case 2: pointA = new Vector2(gameObject.transform.position.x + playerCollider.size.x*2.1f*attack2RangeFactor, gameObject.transform.position.y+4);
				pointB = new Vector2(gameObject.transform.position.x + playerCollider.size.x*2.6f*attack2RangeFactor, gameObject.transform.position.y - .5f);
				break;
			case 3: pointA = new Vector2(gameObject.transform.position.x + playerCollider.size.x*2.25f*attack2RangeFactor, gameObject.transform.position.y - .5f);
				pointB = new Vector2(gameObject.transform.position.x + playerCollider.size.x*2.8f*attack2RangeFactor, gameObject.transform.position.y - 2.25f); 
				break;
			}
			
		}
		else
		{
			switch(frame)
			{
			case 1: pointA = new Vector2(gameObject.transform.position.x + playerCollider.size.x-.5f, gameObject.transform.position.y + 2.75f);
				pointB = new Vector2(gameObject.transform.position.x - playerCollider.size.x*2.4f*attack2RangeFactor, gameObject.transform.position.y+4);
				break;
			case 2: pointA = new Vector2(gameObject.transform.position.x - playerCollider.size.x*2.1f*attack2RangeFactor, gameObject.transform.position.y+4);
				pointB = new Vector2(gameObject.transform.position.x - playerCollider.size.x*2.6f*attack2RangeFactor, gameObject.transform.position.y - .5f);
				break;
			case 3: pointA = new Vector2(gameObject.transform.position.x - playerCollider.size.x*2.25f*attack2RangeFactor, gameObject.transform.position.y - .5f);
				pointB = new Vector2(gameObject.transform.position.x - playerCollider.size.x*2.8f*attack2RangeFactor, gameObject.transform.position.y - 2.25f); 
				break;
			}
		}
		RaycastHit2D[] targets1 = new RaycastHit2D[0];
		Collider2D[] targets2 = new Collider2D[0];
		if(frame < 2) 
			targets1 = Physics2D.LinecastAll(pointA, pointB, attackLayer);
		else
			targets2 = Physics2D.OverlapAreaAll(pointA, pointB, attackLayer);
		
		foreach(RaycastHit2D col in targets1)
		{
			if(col.collider.gameObject.tag == "Enemy" || col.collider.gameObject.layer == LayerMask.NameToLayer("Crates"))
			{
				Debug.Log ("Successfully found enemy");
				var ah = col.collider.gameObject.GetComponent<ActorHealth>();
				
				if(ah.LastAttacker != gameObject)
					ah.Hurt (attack2Damage);
					
				ah.LastAttacker = gameObject;
			}
		}
		foreach(Collider2D col in targets2)
		{
			if(col.gameObject.tag == "Enemy" || col.gameObject.layer == LayerMask.NameToLayer("Crates"))
			{
				Debug.Log ("Successfully found enemy");
				var ah = col.gameObject.GetComponent<ActorHealth>();
				
				if(ah.LastAttacker != gameObject)
					ah.Hurt (attack2Damage);
					
				ah.LastAttacker = gameObject;
			}
		}
		attack2Timer = 0f; 
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(pointA, pointB);
	}
	
}