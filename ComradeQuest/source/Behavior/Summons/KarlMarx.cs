using UnityEngine;
using System.Collections;

public class KarlMarx : CommunistSummon {

	/// <summary>
	/// Governs movement and effects of the Karl Marx summon
	/// </summary>
	private int finalY = 0; // Final adjustment for Marx's position (i.e. how much he rises up)
	private GameObject manifist; // Prefab for manifist projectile object
	private bool fired = false;
	private bool firedWithEnemies = false;
	
	void Start () {
		adjustY = -20;
		InitializeCamera();
		lifeTime = (int)(Time.time + 2.5);
		manifist = (GameObject)Resources.Load ("Manifist");
		sr = gameObject.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
		if(adjustY < finalY)
			adjustY+=.25f; // Speed of rising
		else if (!fired) // Once we're done rising
		{
			int i = 0;
			foreach(GameObject g in GameObject.FindGameObjectsWithTag("Enemy")) // Deal damage to enemies with manifists
			{
				Vector3 enemyVPos = Camera.main.WorldToViewportPoint(g.transform.position);
				
				if (enemyVPos.x >= 0 && enemyVPos.x <= 1 && enemyVPos.y >= 0 && enemyVPos.y <= 1)// If on screen
				{
					i++;
					float randomx, randomy;
					bool vert, side; // vert: if true, fist comes from top/bottom sides of screen instead of left/right. side: if true, top for vert, right for non-vert
					if (Random.Range(0f, 1f) > 0.5f)
						vert = true;
						else
							vert = false;
					if(Random.Range(0f, 1f) > 0.5f)
						side = true;
						else
							side = false;
							
					if(vert)
					{
						randomx = Random.Range (0f, 1f);
						if(side)
							randomy = 1.15f; // just off screen
							else
								randomy = -0.15f;
					}
					else
					{
						randomy = Random.Range (0f, 1f);
						if(side)
							randomx = 1.15f; // just off screen
							else
								randomx = -0.15f;
					}
					
					Vector2 spawnPoint = Camera.main.ViewportToWorldPoint(new Vector3(randomx, randomy, 0));
					//Debug.Log (i + ": "+vert+ ", " + side);
					GameObject m = (GameObject)Instantiate(manifist, spawnPoint, Quaternion.identity);// Spawn fist and deal damage
					m.GetComponent<Manifist>().TargetEnemy = g;
					firedWithEnemies = true;
				}
			}
			fired = true;
		}
		if(!firedWithEnemies && adjustY >= finalY) // If we didn't find any enemies...
		{
			Vector2 spawnPoint = Camera.main.ViewportToWorldPoint(new Vector3(-0.15f, .5f, 0f));
			Vector2 targetPoint = Camera.main.ViewportToWorldPoint(new Vector3(1.25f, .5f, 0f));
			//Debug.Log (i + ": "+vert+ ", " + side);
			GameObject m = (GameObject)Instantiate(manifist, spawnPoint, Quaternion.identity);// Spawn fist and deal damage
			m.GetComponent<Manifist>().TargetEnemy = (GameObject)Instantiate((GameObject)Resources.Load ("ManifistEmptyTarget"), targetPoint, Quaternion.identity);
			firedWithEnemies = true;
		}
		lifeCounter = (int)(lifeTime - Time.time);
		if(lifeCounter < 0)
			Destroy(gameObject);
		
		MoveCamera();
	}
}
