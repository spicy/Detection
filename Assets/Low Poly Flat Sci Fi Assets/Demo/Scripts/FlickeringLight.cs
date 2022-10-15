using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//	This script handles flickering for lights
//	It is expected the light contains a true Unity Light source so will turn it on and off and will change the model texture for better effect


[RequireComponent(typeof(MeshRenderer))]
public class FlickeringLight : MonoBehaviour {

	Light light;
	[SerializeField]
	private float minWaitTime	= 0.1f;
	[SerializeField]
	private float maxWaitTime	= 0.5f;
	[SerializeField]
	private int materialIdx ;

	[SerializeField]
	private Material	onMaterial;
	[SerializeField]
	private Material	offMaterial;

	private MeshRenderer	meshRenderer;
	private Material []	materials;

	// Use this for initialization
	void Start () {
		light = GetComponentInChildren <Light>();
		if (light != null) {
			StartCoroutine (FlickerLight ());
		}
		meshRenderer	= GetComponent<MeshRenderer> ();
		materials	= meshRenderer.materials;
	}

	//	Turn on and off the light
	IEnumerator FlickerLight () {
		while (true) {
			yield return new WaitForSeconds(Random.Range (minWaitTime, maxWaitTime));
			light.enabled = ! light.enabled;

			//	Updates the model material based on the real light status
			if (light.enabled) {
				materials [materialIdx]	= onMaterial;
			} else {
				materials [materialIdx]	= offMaterial;
			}

			meshRenderer.materials	= materials;
		}
	}
}
