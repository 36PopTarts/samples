using UnityEngine;
using System.Collections;

public class GearRotate : MonoBehaviour {

	public float speed = 20;
	public bool right = true;


	void Update () {
		if (right) {
			transform.Rotate(Vector3.back * Time.deltaTime * speed);
		}
		else {
			transform.Rotate(Vector3.forward * Time.deltaTime * speed);
		}
	}
}
