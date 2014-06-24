using UnityEngine;
using System.Collections;

public class DoorSwitch : MonoBehaviour {
	
	public bool isPressed = false;
	public bool simple = false; // Whether or not the switch resets
    public float resetTime = 2f; // How long the switch takes to unpress
	
	private float resetTimer = 0f;
	private Vector2 lend;
	private Vector2 rend;
	private LayerMask playerLayer;
	private Animator anim; // Animator for button animation when pressed
	private Collider2D[] ps;
    private bool doorOpened = false;

    public bool DoorOpened { get { return doorOpened; } set { doorOpened = value; } }

	void Start()
	{
		anim = GetComponent<Animator>();
		playerLayer = 1 << LayerMask.NameToLayer("Player");
		lend = new Vector2();
		rend = new Vector2();
		ps = new Collider2D[0];
	}

	void Update () {
        var sizex = gameObject.GetComponent<BoxCollider2D>().size.x;
        var sizey = gameObject.GetComponent<BoxCollider2D>().size.y;
        lend = new Vector2(gameObject.transform.position.x - sizex / 2 * transform.lossyScale.x + sizex / 10 * transform.lossyScale.x,
		                   gameObject.transform.position.y + sizey * transform.lossyScale.y + 0.2f);
        rend = new Vector2(gameObject.transform.position.x + sizex / 2 * transform.lossyScale.x - sizex / 10 * transform.lossyScale.x,
		                   gameObject.transform.position.y + sizey * transform.lossyScale.y);
		
		Collider2D[] ps = Physics2D.OverlapAreaAll(lend, rend, playerLayer);
		//Debug.Log (ps.Length);
		if(ps.Length > 0)
		{
			isPressed = true;
			if(!simple)
				resetTimer = Time.time + resetTime; // keep the timer infinitely updating while players are standing on it
		}
		
		if (!doorOpened)
		{
			if(ps.Length < 1 && isPressed && resetTimer == 0f)
			{
				Debug.Log ("Timer started");
				resetTimer = Time.time + resetTime;
			}
		}
		
		if (isPressed && anim != null)
		{
			if(!anim.GetBool("isPressed"))
			{
				anim.SetBool("isPressed",true);
				Debug.Log("Switch animation started");
			}

			if(Time.time > resetTimer && resetTimer != 0f)
			{
				Debug.Log ("Button released");
				isPressed = false;
				anim.SetBool ("isPressed", false);
				resetTimer = 0f;
			}
		}
	}

	
    void OnPressed()
    {
        if (doorOpened) // Remain pressed
            anim.speed = 0;
    }

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawLine (lend, rend);
	}
}
