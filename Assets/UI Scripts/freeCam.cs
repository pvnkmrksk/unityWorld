using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freeCam : MonoBehaviour {

	public float speed = 1.0f;
    private float X;
    private float Y;
	public float climbSpeed = 1.0f;
	private GameObject fly;
	private bool follow = false;
	public float factor = 2.0f;

	void Start(){
		fly = GameObject.Find("Fly");
	}
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            transform.Rotate(new Vector3(Input.GetAxis("Mouse Y") * speed, -Input.GetAxis("Mouse X") * speed, 0));
            X = transform.rotation.eulerAngles.x;
            Y = transform.rotation.eulerAngles.y;
            transform.rotation = Quaternion.Euler(X, Y, 0);
        }
		if (Input.GetKeyDown(KeyCode.F))
        {
			//transform.position += transform.up * climbSpeed * Time.deltaTime;
			follow = !follow;
        }
		transform.Translate(Vector3.forward * Input.GetAxis("Mouse ScrollWheel"));
		if(follow){
			factor += Input.GetAxis("Mouse ScrollWheel");
			transform.position = fly.transform.position - transform.forward*factor;
		}
    }
}
