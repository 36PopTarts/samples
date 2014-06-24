using UnityEngine;
using System.Collections;

public class Geyser : MonoBehaviour {

	public int damage = 10;
	public float interval = 3f; // how long between active states
	public float activeLength = 2f; // how long the flame geyser is active
	public float pushForce = 500f; // how far the player gets pushed away upon contact
	float activeTimer = 0f;
	float intervalTimer = 0f;
	bool active = false;
	SpriteRenderer sRend;
		
	void Start () {
		sRend = gameObject.GetComponent<SpriteRenderer>();
	}

	void Update () {
		if(!active && Time.time > intervalTimer)
		{
			activeTimer = Time.time + activeLength;	
			active = true;
		}
			
		if(Time.time > activeTimer && active)
		{
			intervalTimer = Time.time + interval;
			active = false;
		}
		
		if(active) // invisible when inactive
			sRend.enabled = true;
		else
			sRend.enabled = false;
	}
	
	void OnTriggerStay2D(Collider2D col)
	{
		if(active)
		{
			if(col.gameObject.tag == "Player")
			{
				col.gameObject.GetComponent<ActorHealth>().Hurt (damage);
				Vector2 dir = col.transform.position - gameObject.transform.position;
				dir.Normalize();
				if(!col.GetComponent<ActorControl>().flags[3])
					col.gameObject.rigidbody2D.AddForce(dir * pushForce);
			}
		}
	}
}