using UnityEngine;
using System.Collections;

public class ActorHealth : MonoBehaviour {
	public int hitPoints = 1; 
    public float invulnerabilityTime = 0.5f; // How long the actor is invulnerable after being hit, in seconds
	private float iTimer = 0f; // timer check for invulnerability
	public float stunnedTime = 0.25f; // How long the actor is stunned after being hit, in seconds
	private float sTimer = 0f; // timer check for stunning
	public float knockbackForce = 250f; // how hard the actor gets knocked back on hit
    private float flashTime = 0.1f; // how quickly the player's sprite flashes while hit/invulnerable
    private float flashTimer = 0f; // timer for counting flashes
    public float blockReductionFactor = .25f; 
    private Color currentCol; // keeps track of color for flashing
	private DeathAnim deathAnimCheck; // Delays destruction if there is a death animation
	private Animator anim; // Changes animation states for death, hit
	private GameObject lastAttacker; // Last thing that hit me, set in attack functions
	private PlayerControl pControl; // Gets player controller if this is attached to a player
    private ActorControl aControl; // Gets actor flags
    private SpriteRenderer sRend; // For flashing after being hit
    private PlayerBasicAttack attackControl; // For knowing the length of attack animations
    private CommunistSummon sumControl; // For knowing the length of summon animation
    private bool invuln = false; // set to true when the actor was just damaged
	private bool stunned = false;

    [HideInInspector] public bool arenaEnemy = false; // sends a message to the arena controller upon death
    [HideInInspector] public ArenaControl aC;
    [HideInInspector] public int maxHp;

	public GameObject LastAttacker {get {return lastAttacker; } set {lastAttacker = value;}}
	void Start()
	{
		deathAnimCheck = gameObject.GetComponent<DeathAnim>();
		if (deathAnimCheck != null)
			anim = gameObject.GetComponent<Animator>();
		if(gameObject.tag == "Player")
			pControl = gameObject.GetComponent<PlayerControl>();
        aControl = gameObject.GetComponent<ActorControl>();
        currentCol = Color.white;
        sRend = gameObject.GetComponent<SpriteRenderer>();
        maxHp = hitPoints;
        if(gameObject.tag == "Player")
        {
        	Debug.Log ("Max HP: " + maxHp);
        	attackControl = gameObject.GetComponent<PlayerBasicAttack>();
        	sumControl = gameObject.GetComponent<CommunistSummon>();
        }
	}
	
	public void Hurt(int damage) // Any damage
	{
		if (aControl == null) // This is a crate taking damage
		{
			hitPoints--;
		}
		else if(!aControl.flags[0]) // If not invulnerable
		{
			
			Damaged();
			if(aControl.flags[3])
				damage = (int)(damage * blockReductionFactor);
			hitPoints -= damage;
			/*if(gameObject.tag == "Enemy")
			{
				Debug.Log (transform.name + " took " + damage + " damage");
			}*/
			if(pControl != null) // Add communism for taking damage
				pControl.communism += (int)(damage * 1.5f);
		}
	}
	
	public void Hurt() // One damage
	{
		if (aControl == null) // This is a crate taking damage
		{
			hitPoints--;
		}
		else if(!aControl.flags[0]) // If not invulnerable
		{
			Damaged();
			hitPoints--;
			if(pControl != null) // Add communism for taking damage
				pControl.communism++;
		}
	}

    public void Damaged() // Sets up variables for invulnerability, stun. knocks back
    {
    	if(gameObject.tag == "Player")
    	{
			invuln = true;
			aControl.flags [0] = true; // become invulnerable
			iTimer = Time.time + invulnerabilityTime;
    	}
    	
    	if(!aControl.flags[3])
    	{
			stunned = true;
			aControl.flags [1] = true; // become stunned
			sTimer = Time.time + stunnedTime;
			if (aControl.FacingRight)
				gameObject.rigidbody2D.AddForce(new Vector2(-knockbackForce, knockbackForce));
			else
				gameObject.rigidbody2D.AddForce(new Vector2(knockbackForce, knockbackForce));
		}
    }
	
	public void SetLastAttacker(GameObject x)
	{
		lastAttacker = x;
	}
	
	void Update()
	{
		hitPoints = Mathf.Clamp (hitPoints, 0, maxHp);
		if(anim != null && !invuln)
		{
			if(anim.GetCurrentAnimatorStateInfo(0).IsTag("PlayerSummon"))
			{
				invuln = true;
				aControl.flags [0] = true; // become invulnerable
				iTimer = Time.time + sumControl.SummonLength;
			}
		}
        if(Time.time < iTimer || Time.time < sTimer) // While we're stunned from being hit...
        {
			bool cSelected = false; 
            if(flashTimer == 0f)
            {
                flashTimer = Time.time + flashTime;
				if (currentCol == Color.red)
				{
					sRend.color = Color.yellow;
					currentCol = Color.yellow;
					cSelected = true;
				}
				if(currentCol == Color.yellow && !cSelected)
				{
					sRend.color = Color.white; // not actually white; removes the color blend so the sprite looks normal
					currentCol = Color.white;
					cSelected = true;
				}
				if(currentCol == Color.white && !cSelected)
				{
					sRend.color = Color.red;
					currentCol = Color.red;
					cSelected = true;
				}
            }
            if(Time.time > flashTimer) // It's not Communism without a party! And every party needs a light show
            {
                flashTimer = 0f;
                cSelected = false;
            }
        }
		if (invuln && Time.time >= iTimer) // stop being invulnerable
		{
			invuln = false;
			aControl.flags[0] = false;
			flashTimer = 0f;
			sRend.color = Color.white;
		}

		if (Time.time >= sTimer && stunned) // stop being stunned
		{ 
			stunned = false;
			lastAttacker = null;
			aControl.flags[1] = false;
			sRend.color = Color.white;
		}
		if(deathAnimCheck != null)
		{
			if(hitPoints < 1)
				anim.SetBool("isDying", true);
            if (deathAnimCheck.GetDeathFrames() >= deathAnimCheck.GetDeathCount())
            {
                if (arenaEnemy)
                    aC.SendMessage("EnemyDied");
				if (gameObject.transform.name == "GluttonyDemon(Clone)")
				{
					gameObject.GetComponent<GluttonyDemon>().destroyedByDamage = true;
				}
                Destroy(gameObject);
            }
		}
		else
		{
			if(hitPoints < 1)
			{
				if(lastAttacker != null)
				{
					if(lastAttacker.tag == "Player") // Add communism for defeating
						lastAttacker.gameObject.GetComponent<PlayerControl>().communism+=10;
				}
                if (arenaEnemy)
                    aC.SendMessage("EnemyDied");
				if (gameObject.transform.name == "GluttonyDemon(Clone)")
				{
					gameObject.GetComponent<GluttonyDemon>().destroyedByDamage = true;
				}
				Destroy(gameObject);
			}
		}
	}
	
	void FixedUpdate()
	{
		if(deathAnimCheck != null)
		{
			if(anim.GetBool("isDying"))
				deathAnimCheck.SetDeathFrames(deathAnimCheck.GetDeathFrames ()+1);
		}
	}
	/*void OnTriggerEnter2D(Collider2D collider)
	{
		// Is this an attack?
		Attack att = collider.gameObject.GetComponent<Attack>();
		if (att!= null)
		{
			Collider2D hitbox = att.hitCollider;
			if (hitbox != null)
			{
				if (collider == hitbox)
				{
					ActorHealth enemyCheck = collider.gameObject.GetComponent<ActorHealth>();
					// Avoid friendly fire
					if(isEnemy != enemyCheck.isEnemy)
						hitPoints -= att.damage;
							
					if (hitPoints <= 0)
					{
						// Dead!
						Destroy(gameObject);
					}
					
				}
			}
		}
	}*/
}

