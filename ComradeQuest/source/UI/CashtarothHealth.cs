using UnityEngine;
using System.Collections;

public class CashtarothHealth : MonoBehaviour 
{
	public Sprite[] healthSprites; 

	private SpriteRenderer spriteRenderer;
	private GameObject cashtaroth = null;
	private GameObject[] enemyArray;

	void Start () 
	{
		spriteRenderer = renderer as SpriteRenderer;
	}

	void Update () 
	{
		if (cashtaroth == null)
		{
			enemyArray = GameObject.FindGameObjectsWithTag("Enemy");
			foreach (GameObject enemyObject in enemyArray)
			{
				if (enemyObject.transform.name == "Cashtaroth(Clone)")
				{
					cashtaroth = enemyObject;
				}
			}
			if (cashtaroth == null)
			{
				spriteRenderer.sprite = null;
			}
		}
		else
		{
			spriteRenderer.sprite = healthSprites[(51 - (int)Mathf.Clamp(Mathf.RoundToInt(cashtaroth.GetComponent<ActorHealth>().hitPoints * 51f / cashtaroth.GetComponent<Cashtaroth>().maxHitPoints), 0f, 51f))];
		}
	}
}