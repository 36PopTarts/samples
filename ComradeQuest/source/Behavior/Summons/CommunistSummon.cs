using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CommunistSummon : MonoBehaviour {
	/// <summary>
	/// This script is both the parent/defining class for all communist summons and the additional component for the player object that allows him or her to perform those summons.
	/// </summary>
	private GameObject[] cameras;
	private GameObject mainCamera;
	protected int lifeTime = 10;
	protected int lifeCounter = 0;
	protected float adjustY = 0;
	protected List<GameObject> summonList;
	protected GameObject summoningPlayer;
	protected SpriteRenderer sr;
	private Animator anim;
	private float summonLength = 0.667f;
	public float SummonLength {get {return summonLength;} set {summonLength = value;}}
	
	void Start()
	{
		sr = gameObject.GetComponent<SpriteRenderer>();
		summonList = new List<GameObject>();
		summonList.Add((GameObject)Resources.Load ("Summon_Marx"));
		//summonList.Add((GameObject)Resources.Load ("Summon_Lenin"));
	}
	
	protected void InitializeCamera ()
	{
		cameras = GameObject.FindGameObjectsWithTag("MainCamera");
	}
	
	protected void MoveCamera () {
		foreach(GameObject g in cameras)
		{
			if(g.GetComponent<Camera>().enabled)
				mainCamera = g;
		}
		var size = sr.sprite.bounds.size;
		var corner = mainCamera.camera.ViewportToWorldPoint(new Vector2(0, 0));
		var adjustedPos = new Vector2(corner.x + size.x, corner.y + size.y/2 + adjustY);
		gameObject.transform.position = adjustedPos;
	}
	
	public GameObject ChooseRandom () // Picks a random summon
	{
		int c = Random.Range(0,summonList.Count);
		return summonList[c];
	}
	public float GetAdjust()
	{
		return adjustY;
	}
}
