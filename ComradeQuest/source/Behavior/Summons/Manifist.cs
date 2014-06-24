using UnityEngine;
using System.Collections;

public class Manifist : MonoBehaviour {

	/// <summary>
	/// Script that governs the behavior of the fists for the Karl Marx summon
	/// </summary>
	
	private GameObject targetEnemy;
	public GameObject TargetEnemy {get{return targetEnemy;} set{targetEnemy = value;}}
	
	private float destroyTime = 0.35f; // Short delay on removal after damage tick for more organic effect
	private float destroyTimer = 0f;
	private Vector3 lastDir; // stored direction of enemy before it dies
	private bool active = true;
	public int damage = 5;
	public float speed = 2f;
	
	void Start () {
		// Find the closest enemy to our inital position
		var enemies = GameObject.FindGameObjectsWithTag("Enemy");
		var distance = Mathf.Infinity;
		
	}
	
	void Update () {
		if(Time.time > destroyTimer && destroyTimer != 0f)
			Destroy(gameObject);
		
		if(targetEnemy != null)
		{
			// Keep the fist pointed at the target enemy
			float angle = Mathf.Atan2(targetEnemy.transform.position.y - transform.position.y, 
									targetEnemy.transform.position.x - transform.position.x) * 180 / Mathf.PI;
			transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle-90));
			//transform.Rotate(transform.eulerAngles.x, transform.eulerAngles.y, angle);
			
			// Keep the fist moving towards the target enemy
			Vector2 dir = targetEnemy.transform.position - transform.position; 
			dir.Normalize();
			lastDir = dir;
			transform.Translate (dir * speed * Time.deltaTime, Space.World);
		}
		else
		{
			transform.Translate (lastDir * speed, Space.World);
		}
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		if((col.gameObject.tag == "Enemy" || col.gameObject.name == "ManifistEmptyTarget") && active)
		{
			col.gameObject.GetComponent<ActorHealth>().Hurt(damage);
			destroyTimer = Time.time + destroyTime;
			active = false;
			if(col.gameObject.name == "ManifistEmptyTarget")
				Destroy(col.gameObject);
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		if(targetEnemy!=null)
			Gizmos.DrawLine (transform.position, targetEnemy.transform.position);
	}
}
