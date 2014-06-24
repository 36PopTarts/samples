using UnityEngine;
using System.Collections;

public class OlafHead : MonoBehaviour
{
	[HideInInspector] public bool playerOnMyHead = false;

	void OnCollisionEnter2D(Collision2D col)
	{
		if (col.collider.name.Equals("Player1FloorCollider"))
		{
			playerOnMyHead = true;
		}
		if (col.collider.name.Equals("Player2FloorCollider"))
		{
			playerOnMyHead = true;
		}
	}

	void OnCollisionExit2D(Collision2D col)
	{
		if(col.collider.name.Equals("Player1FloorCollider"))
		{
			playerOnMyHead = false;
		}
		if(col.collider.name.Equals("Player2FloorCollider"))
		{
			playerOnMyHead = false;
		}
	}
}
