using UnityEngine;
using System.Collections;

public class BeetScript : MonoBehaviour {

	public int value = 0;

	private GameObject beetCounterLabel;

	void Start ()
	{
		beetCounterLabel = GameObject.Find ("BeetCounterLabel");
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.tag == "Player") {
			beetCounterLabel.GetComponent<BeetCounterLabel> ().UpdateBeetCount (value);
			Destroy (gameObject);
		}
	}
}
