using UnityEngine;
using System.Collections;

public class ParticleSort : MonoBehaviour {

	void Start ()
	{
		//Change Foreground to the layer you want it to display on 
		// make a public variable for this later
		particleSystem.renderer.sortingLayerName = "foregroundLayer";
	}

}
