using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrow_size : MonoBehaviour {

	public float factor = 1.0f;
	public float baseScale = 1.0f;
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		CameraMan cam = GameObject.Find("Main Camera (Front)").GetComponent<CameraMan>();
		float spd = cam.speed;
		transform.localScale = new Vector3(spd * factor*baseScale,baseScale,baseScale);
		transform.localPosition = new Vector3(0.0f,(-GameObject.Find("Main Camera (Front)").transform.position.y+0.1f),0.0f);
	}
}
