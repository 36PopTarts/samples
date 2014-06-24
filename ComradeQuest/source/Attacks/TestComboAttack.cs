using UnityEngine;
using System.Collections;

public class TestComboAttack : ComboAttack {
	/// <summary>>
	/// Test combo attack for testing combo functionality.
	/// </summary>
	private Vector2 pointA;
	private Vector2 pointB;
	
	public TestComboAttack()
	{
		initialized = false;
		comboLength = 0.5f;
	}
	
	public TestComboAttack (GameObject p)
	{
		damage = 3;
		comboLength = 0.5f;
		player = p;
		pointA = p.transform.position;
		if(p.GetComponent<PlayerControl>().FacingRight)
			pointB = new Vector2(p.transform.position.x + 5f, p.transform.position.y);
		else
			pointB = new Vector2(p.transform.position.x - 5f, p.transform.position.y);
		initialized = true;
	}
	
	public override void Effect(int frame)
	{
		RaycastHit2D[] targets1 = new RaycastHit2D[0];
		targets1 = Physics2D.LinecastAll(pointA, pointB, attackLayer);
		
		foreach(RaycastHit2D col in targets1)
		{
			if(col.collider.gameObject.tag == "Enemy" || col.collider.gameObject.layer == LayerMask.NameToLayer("Crates"))
			{
				Debug.Log ("Successfully found enemy");
				var ah = col.collider.gameObject.GetComponent<ActorHealth>();
				
				if(ah.LastAttacker != player)
					ah.Hurt (damage);
				
				ah.LastAttacker = player;
			}
		}
		Gizmos.color = Color.yellow;
		Gizmos.DrawLine (pointA, pointB);
	}
	
	public override string ToString ()
	{
		return string.Format ("[TestComboAttack]");
	}
}
