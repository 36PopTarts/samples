using UnityEngine;
using System.Collections;

public class PlatformBreakAnim : MonoBehaviour {

	private Animator platformAnimator;
	private Vector2 colliderSize;
	private bool breaking = false;
	private bool[] playersTouching;
	private float animSpeed = 0f;
    public float breakTime = 0.5f;
    private float breakTimer = 0f;
	private GameObject playerOne, playerTwo;
	void Start () {
		platformAnimator = gameObject.GetComponent<Animator>();
		playersTouching = new bool[2];
	}
	
	void Update () {
        if (playersTouching[0] && playersTouching[1])
        {
            if (!breaking)
            {
                breaking = true;
                breakTimer = Time.time + breakTime;
            }
            else if (Time.time > breakTimer && breakTimer != 0f)
            {
                platformAnimator.SetBool("isBreaking", true);
            }
        }
        else if (playersTouching[0])
		{
			if (playerOne != null)
			{
				if (playerOne.rigidbody2D.mass == 2f)
				{
					if (!breaking)
					{
						breaking = true;
						breakTimer = Time.time + breakTime;
					}
					else if (Time.time > breakTimer && breakTimer != 0f)
					{
						platformAnimator.SetBool("isBreaking", true);
					}
				}
				else
				{
					breaking = false;
					breakTimer = 0f;
				}
			}
			else
			{
				breaking = false;
				breakTimer = 0f;
			}
		}
		else if (playersTouching[1])
		{
			if (playerTwo != null)
			{
				if (playerTwo.rigidbody2D.mass == 2f)
				{
					if (!breaking)
					{
						breaking = true;
						breakTimer = Time.time + breakTime;
					}
					else if (Time.time > breakTimer && breakTimer != 0f)
					{
						platformAnimator.SetBool("isBreaking", true);
					}
				}
				else
				{
					breaking = false;
					breakTimer = 0f;
				}
			}
			else
			{
				breaking = false;
				breakTimer = 0f;
			}

		}
		else
        {
            breaking = false;
            breakTimer = 0f;
        }
		AnimatorStateInfo temp = platformAnimator.GetCurrentAnimatorStateInfo(0);
		// Initialize the end marker for the animation
		if(breaking && animSpeed == 0f)
			animSpeed = platformAnimator.speed;
		// At the end of break animation, destroy object (during transition to dummy state)		
		if(temp.IsName("Breaking") && platformAnimator.IsInTransition(0))
		{
			Destroy(gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
        if (col.collider.name.Equals("Player1FloorCollider"))
		{
            playersTouching[0] = true;
			playerOne = col.transform.gameObject;
		}
        if (col.collider.name.Equals("Player2FloorCollider"))
		{
            playersTouching[1] = true;
			playerTwo = col.transform.gameObject;
		}
	}
	void OnCollisionExit2D(Collision2D col)
	{
		if(col.collider.name.Equals("Player1FloorCollider"))
			playersTouching[0] = false;
		if(col.collider.name.Equals("Player2FloorCollider"))
			playersTouching[1] = false;
	}
}
