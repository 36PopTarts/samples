using UnityEngine;
using System.Collections;

public class BeetCounterLabel : MonoBehaviour 
{
	public GameObject beetCounter;
	public Vector3 offset;

	private GUIText displayedText;
	private int beetCount;
	private Camera UICamera;
	
	void Start ()
	{
		displayedText = gameObject.GetComponent<GUIText>();
		beetCount = 0;
		UICamera = transform.parent.GetComponent<Camera>();
	}
	
	void Update () 
	{
		displayedText.text = "" + beetCount;
		transform.position = UICamera.WorldToViewportPoint(beetCounter.transform.position + offset);
	}

	public void UpdateBeetCount (int updateAmount)
	{
		beetCount += updateAmount;
	}
}