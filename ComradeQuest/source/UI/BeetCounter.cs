using UnityEngine;
using System.Collections;

public class BeetCounter : MonoBehaviour 
{
	private Camera UICamera;
	private float origX;
	private float newX;
	
	void Start () 
	{
		UICamera = transform.parent.GetComponent<Camera>();
		origX = transform.position.x;
	}
	
	void Update () 
	{
		if (UICamera.aspect == (5f/4f))
		{
			transform.position = new Vector3(origX, transform.position.y, transform.position.z);
		}
		else if (UICamera.aspect == 4f/3f)
		{
			newX = origX + 0.25f;
			transform.position = new Vector3(newX, transform.position.y, transform.position.z);
		}
		else
		{
			newX = origX + 2.60f;
			transform.position = new Vector3(newX, transform.position.y, transform.position.z);
		}
	}
}