using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SmasherScript : Attack {

	public float Ymin;
	public float Ymax;
	public float speed = .05f;
	private float penetration = .2f;
	private List<int> penetrationCounter; // How many frames the crusher has penetrated player's collider for; after a certain amount, deals damage
    private int penetrationMaxFrames = 12;
	private List<GameObject> targets; // Players being crushed currently; shares indecies with penetrationCounter (i.e. penetrationCounter[0] is targets[0]'s penetration frame count)
    private Vector2 left, right, lefttop, righttop, dir;
    private BoxCollider2D[] boxes;
    LayerMask playerLayer, groundLayer;
	bool movingUp = false;
	
	void Start()
	{
		penetrationCounter = new List<int>();
		targets = new List<GameObject>();
        left = new Vector2();
        right = new Vector2();
        lefttop = new Vector2();
        righttop = new Vector2();
        dir = new Vector2();
        playerLayer = 1 << LayerMask.NameToLayer("Player");
        groundLayer = 1 << LayerMask.NameToLayer("Ground");
        boxes = gameObject.GetComponents<BoxCollider2D>();
	}

    void Update()
    {
    	//Debug.Log ("Boxes: " + boxes.Length);
        if (transform.position.y >= Ymax)
            //wait a few seconds
            movingUp = false;
        if (transform.position.y <= Ymin)
            //wait a few seconds
            movingUp = true;
        if (movingUp && transform.position.y <= Ymax)
            transform.Translate(new Vector3(0, 1, 0) * speed * Time.deltaTime);
        if (!movingUp && transform.position.y >= Ymin)
            transform.Translate(new Vector3(0, -1, 0) * speed * Time.deltaTime);

        // Collision detection/damage code 
        var pos = gameObject.transform.position;
        var scale = transform.lossyScale;
        if (!movingUp) // Left = top left, middle = top middle, right = top right, other vectors are the midway points between those vectors
        {
            left = new Vector2((pos.x + boxes[0].center.x) - boxes[0].size.x / 2 * scale.x * .9f,
                               (pos.y + boxes[0].center.y * scale.y) - boxes[0].size.y / 2 * scale.y);
            right = new Vector2((pos.x + boxes[0].center.x) + boxes[0].size.x / 2 * scale.x * .9f,
                                (pos.y + boxes[0].center.y * scale.y) - boxes[0].size.y / 2 * scale.y);
            // For moving the players down with the smasher when used as an elevator
			lefttop = new Vector2((pos.x + boxes[1].center.x) - boxes[1].size.x / 2 * scale.x * .9f,
			                      (pos.y + boxes[1].center.y * scale.y * 1.15f) + boxes[1].size.y / 2 * scale.y);
			righttop = new Vector2((pos.x + boxes[1].center.x) + boxes[1].size.x / 2 * scale.x * .9f,
			                       (pos.y + boxes[1].center.y * scale.y * 1.15f) + boxes[1].size.y / 2 * scale.y);
        }
        else // Left = bottom left, middle = bottom middle, right = bottom right, other vectors are the midway points between those vectors
        {
            left = new Vector2((pos.x + boxes[1].center.x) - boxes[1].size.x / 2 * scale.x * .9f,
                               (pos.y + boxes[1].center.y * scale.y) + boxes[1].size.y / 2 * scale.y);
            right = new Vector2((pos.x + boxes[1].center.x) + boxes[1].size.x / 2 * scale.x * .9f,
                                (pos.y + boxes[1].center.y * scale.y) + boxes[1].size.y / 2 * scale.y);
        }
        if (movingUp)
            dir = Vector2.up;
        else
            dir = new Vector2(0, -1);
        Collider2D[] pColliders = Physics2D.OverlapAreaAll(left, right + (dir * penetration), playerLayer);
		Collider2D[] pCollidersTop = new Collider2D[0];
        if(!movingUp)
			pCollidersTop = Physics2D.OverlapAreaAll(lefttop, righttop + (dir * penetration), playerLayer);
        //Debug.Log("Start point: " + left + " End point: " + (right + (dir * penetration)) + "Magnitude: " + (dir * penetration));
        // Check all raycasts, and add new targets; iterate penetration frame count of existing targets
        if(pColliders.Length > 0)
        {
            //Debug.Log("Smasher found player");
            foreach(Collider2D p in pColliders)
            {
                RaycastHit2D crushedWall = CheckFarCollisionBox(p.gameObject.GetComponent<BoxCollider2D>(), movingUp);
                if (crushedWall != null) // Not sure why I need such a deep null check here, but this works.
                {
                    if (crushedWall.collider != null)
                    {
                        if (crushedWall.collider.transform != null)
                        {
                            if (!targets.Contains(p.gameObject))
                            {
                                targets.Add(p.gameObject);
                                penetrationCounter.Add(0);
                            }
                            penetrationCounter[targets.IndexOf(p.gameObject)]++;
                        }
                    }
                }
            }
        }
        else // No players; reset penetration counting
        {
            targets.Clear();
            penetrationCounter.Clear();
        }
        // Prevent "bounciness" when being used as an elevator
        if(pCollidersTop.Length > 0 && !movingUp)
        {
			foreach(Collider2D p in pCollidersTop)
			{
				//Debug.Log ("Player being moved");
				p.rigidbody2D.velocity = new Vector2(p.rigidbody2D.velocity.x + dir.x * speed, p.rigidbody2D.velocity.y + dir.y * speed);
			}
        }
        // Deal damage when there's a player who's been crushed (penetration frame count sufficient)
        if (penetrationCounter.Contains(penetrationMaxFrames))
        {
            for(int x = 0; x < penetrationCounter.Count; x++)
            {
                if (penetrationCounter[x] > 2)
                {
                    GameObject y = targets[x];
                    if (y != null)
                        y.GetComponent<ActorHealth>().Hurt(damage);
                    penetrationCounter[x] = 0;       
                }
            }
        }
    }
    

    // Syntax: bool CheckFarCollisionBox(BoxCollider2D col, bool up); col: the collider to check collision on the opposite side of the smasher
    // up: true if the smasher is moving up, false otherwise
    // returns: RaycastHit2D of the colliding ground on the other side of col
    RaycastHit2D CheckFarCollisionBox(BoxCollider2D col, bool up)
    {
        float y;
        Vector2 terminalOffset;
        if (up)
        {
            y = col.gameObject.transform.position.y + col.gameObject.renderer.bounds.size.y/2;
            terminalOffset = new Vector2(0, 0.2f);
        }
        else
        {
            y = col.gameObject.transform.position.y - col.gameObject.renderer.bounds.size.y/2;
            terminalOffset = new Vector2(0, -0.2f);
        }
        Vector2 start = new Vector2(col.gameObject.transform.position.x, y);
        Vector2 terminal = start + terminalOffset;
        RaycastHit2D crushed = Physics2D.Linecast(start, terminal, groundLayer);
        //Debug.DrawLine(start, terminal);
        return crushed;
    }

    /*
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawLine(left, right + (dir * penetration));
		Gizmos.DrawLine(lefttop, righttop + (dir * penetration));
    }*/
}
