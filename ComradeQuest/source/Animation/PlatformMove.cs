using UnityEngine;
using System.Collections;

public class PlatformMove : MonoBehaviour
{

    public GameObject platformSwitch1;
    DoorSwitch platformSwitch_script1;

    public GameObject platformSwitch2;
    DoorSwitch platformSwitch_script2;

    public float speed = .05f;
    public float Xmax = 0;
    public float Xmin = 0;
    public bool up = false;
    public bool resets = false; // Goes back to starting position after a short period of player(s) not standing on the switch
    public float Ymax = 0;
    public float Ymin = 0;

	private float resetTimer = 0f;
	private float resetTime = 3f; // Time (in seconds) it takes to reset the platform 
	private Vector2 initPos; // Initial position of the platform for resetting
    bool movingRight = false;
    bool movingUp = false;
    Vector2 left, right;
    Vector2 dir;
    LayerMask playerLayer;

    void Awake()
    {
        platformSwitch_script1 = platformSwitch1.GetComponent<DoorSwitch>();
        platformSwitch_script2 = platformSwitch2.GetComponent<DoorSwitch>();
        initPos = gameObject.transform.position;
        left = new Vector2();
        right = new Vector2();
        dir = new Vector2();
        playerLayer = 1 << LayerMask.NameToLayer("Player");
    }

    void Update()
    {
        if (platformSwitch_script1.isPressed || platformSwitch_script2.isPressed)
        {
        	if(platformSwitch_script1.simple)
        		platformSwitch_script1.DoorOpened = true;
        	if(platformSwitch_script2.simple)
        		platformSwitch_script2.DoorOpened = true;
        	if(resets) // Stops resetTimer if players touch the switch again
        		resetTimer = 0f;
            if (!up)
            {
                if (transform.position.x >= Xmax)
                    //wait a few seconds
                    movingRight = false;
                if (transform.position.x <= Xmin)
                    //wait a few seconds
                    movingRight = true;

                if (movingRight && transform.position.x <= Xmax)
                    dir = new Vector2(1, 0);
                if (!movingRight && transform.position.x >= Xmin)
                    dir = new Vector2(-1, 0);
                transform.Translate(dir * speed * Time.deltaTime);
            }
            if (up)
            {
                if (transform.position.y >= Ymax)
                    //wait a few seconds
                    movingUp = false;
                if (transform.position.y <= Ymin)
                    //wait a few seconds
                    movingUp = true;

                if (movingUp && transform.position.y <= Ymax)
                    dir = new Vector2(0, 1);
                if (!movingUp && transform.position.y >= Ymin)
                    dir = new Vector2(0, -1);
                transform.Translate(dir * speed * Time.deltaTime);
            }

            left = new Vector2(gameObject.transform.position.x - gameObject.GetComponent<BoxCollider2D>().size.x / 2 * transform.lossyScale.x,
                                gameObject.transform.position.y + gameObject.GetComponent<BoxCollider2D>().size.y / 2 * transform.lossyScale.y);
            right = new Vector2(gameObject.transform.position.x + gameObject.GetComponent<BoxCollider2D>().size.x / 2 * transform.lossyScale.x,
                                gameObject.transform.position.y + gameObject.GetComponent<BoxCollider2D>().size.y / 2 * transform.lossyScale.y);
            Collider2D[] pColliders = Physics2D.OverlapAreaAll(left + (Vector2.up * .2f), right, playerLayer);

            foreach (Collider2D p in pColliders)
            {
                //Debug.Log("Current velocity: " + (dir * speed) + " " + p.gameObject.transform.name + ": " + p.rigidbody2D.velocity);
                if(up&&!movingUp) // Don't add velocity while moving up; innate collision in Unity Engine takes care of it
                    p.rigidbody2D.velocity = new Vector2(p.rigidbody2D.velocity.x + dir.x * speed, p.rigidbody2D.velocity.y + dir.y * speed);
                if (!up) // Only use translate on horizontal platforms. Also, this is probably a temporary solution
                    p.transform.Translate(dir * speed * Time.deltaTime);
            }
        }
        else if (resets) // begin reset if players not in contact with switch
        {
        	if(Time.time > resetTimer && resetTimer != 0f)
        	{
        		if((Vector2)gameObject.transform.position != initPos)
        		{
					dir = initPos - (Vector2)gameObject.transform.position;
					dir.Normalize();
					transform.Translate(dir * speed * Time.deltaTime);
				}
        	}
        	else if (resetTimer == 0f)
        		resetTimer = Time.time + resetTime;
        }
    }

        void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(left + (Vector2.up * .15f), right);
        }
    }