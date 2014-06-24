using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(PlayerBasicAttack))]
public class PlayerControl : ActorControl {

	public float maxSpeed = 12f;
	protected Animator anim;
	
	public bool moveSlowerWhileAttacking = false; // should we move slower while attacking?
	bool grounded = false;
	public Transform groundCheck;
    public BoxCollider2D playerCollider;
    public CircleCollider2D playerFloorCollider;
	public LayerMask whatIsGround;
	public float jumpForce = 800;
	public int playerNumber = 1;
	public int communism = 0;
	public int special = 100;
	private int pSpecial = 0; // for detecting changes in special points, mainly for debugging
	protected int pCommunism = 0; // for detecting changes in communism, mainly for debugging
	protected float communismTimer; // timer for both players activating the summon
	private PlayerBasicAttack attackRef; // reference for basic attack cooldowns/timers
	private SpriteRenderer spRend; // for changing color to indicate flags (probably temporary)
	private Color normalColor; // Standard color blend, used for reference
	private float communismTime = 0.2f;
    Vector2 pointA, pointB;
    private bool freezeFrame = false;

    public AudioClip gruntSound;
    GameObject tempBlockGraphic;

	void Start () {
		facingRight = true;
		anim = GetComponent<Animator>();
		spRend = GetComponent<SpriteRenderer>();
		normalColor = spRend.color;
		attackRef = GetComponent<PlayerBasicAttack>();
        //playerCollider = GetComponent<BoxCollider2D>();
        //playerFloorCollider = GetComponent<CircleCollider2D>();
		// Name colliders for easier referencing by other objects
		playerCollider.name = "Player"+playerNumber+"Collider";
		playerFloorCollider.name = "Player"+playerNumber+"FloorCollider";
        if (playerNumber == 2)
            anim.SetBool("altOlaf", true);
		InitFlags();
	}

	void FixedUpdate () {
        // upper left corner of ground collision box
        pointA = new Vector2(transform.position.x - (playerCollider.size.x / 2 - 0.25f), transform.position.y + .075f - (playerCollider.size.y / 2 + playerFloorCollider.radius/1.25f));
        // lower right corner of ground collision box
        pointB = new Vector2(transform.position.x + (playerCollider.size.x / 2 - 0.25f), transform.position.y - 0.75f - (playerCollider.size.y / 2 + playerFloorCollider.radius/1.25f));
		int howManyGrounded = Physics2D.OverlapAreaNonAlloc(pointA, pointB, new Collider2D[1], whatIsGround); // rectangular detection usually ends up in a better platformer feel
        if(howManyGrounded>0)
        {
        	grounded = true;
        	//Debug.Log ("Grounded: " + howManyGrounded);
        }
        else
        	grounded = false;
		anim.SetBool("Ground", grounded);

		//anim.SetFloat ("vSpeed", rigidbody2D.velocity.y);
		float move;
		switch(playerNumber)
		{
			case 1: move = Input.GetAxis("Horizontal_P1");
			break;
			case 2: move = Input.GetAxis("Horizontal_P2");
			break;
			default: move = Input.GetAxis("Horizontal_P1");
			break;
		}
		if(!flags[1])
		anim.SetFloat("speed", Mathf.Abs (move));
		Vector2 movement;
		if(attackRef.attackState && moveSlowerWhileAttacking)
			movement = new Vector2 (move * maxSpeed * 0.5f, rigidbody2D.velocity.y);
		else if (flags[3])
			movement = new Vector2 (move * maxSpeed * 0.3f, rigidbody2D.velocity.y);
		else
			movement = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);
		Vector3 cameraLeft = Camera.main.ViewportToWorldPoint (new Vector3 (0, 0, 0));
		Vector3 cameraRight = Camera.main.ViewportToWorldPoint (new Vector3 (1, 0, 0));
		
		if ((transform.position.x <= (cameraLeft.x + (1.5f))) && movement.x < 0)
		{
			movement.x = 0f;
		}

		if ((transform.position.x >= (cameraRight.x - (1.5f))) && movement.x > 0)
		{
			movement.x = 0f;
		}

		if(!flags[1]) // not stunned
		{
			rigidbody2D.velocity = movement;

			if (move > 0 && !facingRight)
				Flip();
			else if (move < 0 && facingRight)
				Flip();
		}
		anim.SetFloat("y_vel",rigidbody2D.velocity.y);
	}

	void Update () {
		communism = Mathf.Clamp(communism, 0, 200); // Clamp communism value
		special = Mathf.Clamp(special, 0, 100); // Clamp special value
		if(attackRef.ComboTimer == Time.time && attackRef.ComboCounter.Count > 1) // Clear combo if timer expires
			attackRef.ComboCounter.Clear();
		if(communism != pCommunism)
			Debug.Log ("Communism: " + communism);
		//if(special != pSpecial)
			//Debug.Log ("Special: " + special);
		// Player input section
		bool jump, summon, attack1, attack2, block;
		switch(playerNumber) // Determine player number for input
		{
			case 1: jump = Input.GetButtonDown("Jump_P1");
					summon = Input.GetButtonDown ("Summon_P1");
					block = Input.GetButton ("Block_P1");
					attack1 = Input.GetButtonDown ("BasicAttack1_P1");
					attack2 = Input.GetButtonDown ("BasicAttack2_P1");
			break;
			case 2: jump = Input.GetButtonDown("Jump_P2");
					summon = Input.GetButtonDown ("Summon_P2");
				    block = Input.GetButton ("Block_P2");
					attack1 = Input.GetButtonDown ("BasicAttack1_P2");
					attack2 = Input.GetButtonDown ("BasicAttack2_P2");
			break;
			default: jump = Input.GetButtonDown("Jump_P1");
					 summon = Input.GetButtonDown ("Summon_P1");
					 block = Input.GetButton ("Block_P1");
					 attack1 = Input.GetButtonDown ("BasicAttack1_P1");
					 attack2 = Input.GetButtonDown ("BasicAttack2_P1");
			break;
		} 

		if (!flags [1]) { // if not stunned
						if (grounded && jump && !flags[3]) {
								anim.SetBool ("Ground", false);
								audio.PlayOneShot (gruntSound);
								rigidbody2D.velocity = new Vector2(0,0); // Stop moving; prevents the jump from being shorter or higher due to rigidbody forces
								rigidbody2D.AddForce (new Vector2 (0, jumpForce));
								if(playerNumber==1)
									anim.Play ("jumping");
								else
									anim.Play("jumping_alt");
						}
						if(block && !attackRef.attackState) {
								flags[3] = true;
								rigidbody2D.velocity = new Vector2(0, rigidbody2D.velocity.y);
								if(tempBlockGraphic == null)
									tempBlockGraphic = (GameObject)Instantiate((GameObject)Resources.Load("Block_TempGraphic"), gameObject.transform.position, Quaternion.identity);
									else
										tempBlockGraphic.transform.position = gameObject.transform.position;
								Debug.Log ("Player " + playerNumber + " holding block");
						}
						else {
							flags[3] = false;
							if(tempBlockGraphic != null)
								Destroy(tempBlockGraphic);
							//Debug.Log ("Player " + playerNumber + " released block");
						}
						if (attack1 && !flags[3]) {
								string st = "Combo queue: ";
								attackRef.Attack1 ();
								attackRef.ComboCounter.Enqueue (1);
								foreach(byte b in attackRef.ComboCounter)
									st += b + ", ";
								Debug.Log (st);
								attackRef.ComboTimer = Time.time + attackRef.comboTime; // One second timer until combo expires
						}
						if (attack2 && !flags[3]) {
								string st = "Combo queue: ";
								attackRef.Attack2 ();
								attackRef.ComboCounter.Enqueue (2);
								foreach(byte b in attackRef.ComboCounter)
									st += b + ", ";
								Debug.Log (st);
								attackRef.ComboTimer = Time.time + attackRef.comboTime; // One second timer until combo expires
						}
				}
		if(attackRef.ComboCounter.Count > 1)
			attackRef.ExecuteCombo(attackRef.ComboCounter);

		int s = 0;
		if(summon)
			communismTimer = Time.time + communismTime;
		bool canSummon = false; // determines whether all players have enough communism to summon
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        int com = 0;
		foreach(GameObject g in players)
		{
			if(g!=null)
			{
                com += g.GetComponent<PlayerControl>().communism;
				if (g.GetComponent<PlayerControl>().communismTimer > Time.time)
					s++;
			}
		}
		if(com > 199 && s==players.Length)	
			canSummon = true;
		
		if (summon && canSummon)
		{
			foreach(GameObject g in players)
			{
				if(g!=null)
				{
					PlayerControl pcont = g.GetComponent<PlayerControl>();
					if(pcont.anim.GetBool("altOlaf"))
						pcont.anim.Play ("summon alt");
					else
						pcont.anim.Play ("summon");	
					pcont.communism = 0;	
				}
			}
		}

        if(Input.GetKeyDown(KeyCode.Semicolon)) // Debug force current arena doors to open
        {
            foreach(GameObject a in GameObject.FindGameObjectsWithTag("Arena"))
            {
                if(a.GetComponent<ArenaControl>().PlayersReady)
                {
                    foreach (DoorOpen d in a.GetComponent<ArenaControl>().doorsToOpen)
                    {
                        d.SendMessage("Open");
                    }
                }
            }
        }
		
		pCommunism = communism;
	}

	void Flip() {
		if(!attackRef.attackState)
		{
			facingRight = !facingRight;
			Vector3 scale = transform.localScale;
			scale.x *= -1;
			transform.localScale = scale;
		}
	}

	void SummonEvent()
	{
		if(playerNumber == 1)
		{
			GameObject x = GetComponent<CommunistSummon>().ChooseRandom();
			Instantiate(x, new Vector3(transform.position.x,transform.position.y+x.GetComponent<CommunistSummon>().GetAdjust(),transform.position.z), Quaternion.identity);
		}
	}
	
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(pointA, pointB);
    }
}