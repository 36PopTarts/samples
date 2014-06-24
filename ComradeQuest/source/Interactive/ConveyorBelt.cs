using UnityEngine;
using System.Collections;

public class ConveyorBelt : MonoBehaviour {

	public bool right = false;
	public float speed = 0f;
    Vector2 lend, rend;
	LayerMask playerLayer;
	
	void Start()
	{
		playerLayer = 1 << LayerMask.NameToLayer("Player");
		lend = new Vector2();
		rend = new Vector2();
	}
	
	void Update () {
		lend = new Vector2(gameObject.transform.position.x - gameObject.GetComponent<BoxCollider2D>().size.x / 2 * transform.lossyScale.x,
		                   gameObject.transform.position.y + gameObject.GetComponent<BoxCollider2D>().size.y / 2 * transform.lossyScale.y);
		rend = new Vector2(gameObject.transform.position.x + gameObject.GetComponent<BoxCollider2D>().size.x / 2 * transform.lossyScale.x,
		                    gameObject.transform.position.y + gameObject.GetComponent<BoxCollider2D>().size.y / 2 * transform.lossyScale.y);
		                    
		Collider2D[] pColliders = Physics2D.OverlapAreaAll(lend + (Vector2.up * .2f), rend, playerLayer);
		Vector2 dir;
		if(right)
			dir = new Vector2(1, 0);
		else
			dir = new Vector2(-1, 0);
		foreach (Collider2D p in pColliders)
		{
			Debug.Log ("Player " + p.GetComponent<PlayerControl>().playerNumber + " touching belt");
			//p.rigidbody2D.velocity = new Vector2(p.rigidbody2D.velocity.x + dir.x * speed, p.rigidbody2D.velocity.y + dir.y * speed);
			p.rigidbody2D.AddForce (new Vector2(dir.x * speed, dir.y * speed));
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.blue;
		Gizmos.DrawLine(lend + (Vector2.up * .15f), rend);
	}
}
