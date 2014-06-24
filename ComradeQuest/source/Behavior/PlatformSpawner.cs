using UnityEngine;
using System.Collections;

public class PlatformSpawner : MonoBehaviour {

	GameObject platformPrefab;
	private GameObject currentPlatform;
	float spawnTimer = 0f;
	public float spawnTime = 0f;
	public bool respawns = false;
	private Vector3 scale;
	private Vector3 pos;
	
	void Start()
	{
		platformPrefab = (GameObject)Resources.Load ("Platform_Breakable");
		currentPlatform = transform.GetChild(0).gameObject;
		scale = currentPlatform.transform.localScale;
		pos = currentPlatform.transform.position;
	}
	
	void Update () {
		if(respawns)
		{
			if(currentPlatform==null && spawnTimer==0f)
				spawnTimer = Time.time + spawnTime;
				
			if(Time.time > spawnTimer && spawnTimer!=0f)
			{
				spawnTimer = 0f;
				currentPlatform = Instantiate(platformPrefab, pos, Quaternion.identity) as GameObject;
				currentPlatform.transform.localScale = scale;
			}
		}
	}
}
