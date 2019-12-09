using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMan : MonoBehaviour {

    public float climbSpeed = 4;
    public float accVal = 1;
    public float deltaAngle = -2.00000f;
    private float dc_inc = 0.001f;
    public float DCoffset = 2.0f;
	public float gain = 8f;
	public float gain_inc = 0.01f;
    public bool open_loop = false;
    public bool valve = false;
    public float speed = 0.0f;
	public int caseU = 0;
	public int reset = 0;
	public GameObject AppleTreeL;
	public GameObject AppleTreeR;
	public HelloWorld rossy;
	
	public void subProcessor (string command, string argss)
	{

			System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo();
			procInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
			procInfo.FileName = command; // 'sh' for bash
			procInfo.Arguments = argss; // The Script name
			System.Diagnostics.Process p= System.Diagnostics.Process.Start(procInfo); // Start that process
			//return p;

	}

	// Use this for initialization
    void Start ()
    {
	    rossy = GetComponent<HelloWorld>();
	    AppleTreeL=GameObject.Find("AppleTreeL");
	    AppleTreeR=GameObject.Find("AppleTreeR");
	    
	    Debug.Log("displays connected: " + Display.displays.Length);
	    // Display.displays[0] is the primary, default display and is always ON.
	    // Check if additional displays are available and activate each.
	    //if (Display.displays.Length > 1)
	    //  Display.displays[1].Activate();
	    if (Display.displays.Length > 2)
		    Display.displays[2].Activate();
	    if (Display.displays.Length > 3)
		    Display.displays[3].Activate();



    }
	
	// Update is called once per frame
	void Update ()
	{
		reset = 0;
        transform.position += transform.forward* speed* Time.deltaTime;
        if (Input.GetKey(KeyCode.UpArrow)) { speed += accVal * Time.deltaTime; }
        if (Input.GetKey(KeyCode.DownArrow)) { speed -= accVal * Time.deltaTime; }
        if (Input.GetKey(KeyCode.Q)) { transform.position += transform.up * climbSpeed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.A)) { transform.position -= transform.up * climbSpeed * Time.deltaTime; }
        if (Input.GetKey(KeyCode.S)) { speed = 0.0f; }
        if (Input.GetKey(KeyCode.LeftArrow)) {transform.rotation = transform.rotation * Quaternion.Euler(new Vector3(0, -1, 0));}
        if (Input.GetKey(KeyCode.RightArrow)) { transform.rotation = transform.rotation * Quaternion.Euler(new Vector3(0, 1, 0)); }
        if (Input.GetKey(KeyCode.J)) { DCoffset += dc_inc; }
        if (Input.GetKey(KeyCode.H)) { DCoffset -= dc_inc; }
        if (Input.GetKey(KeyCode.U)) { gain += gain_inc; }
        if (Input.GetKey(KeyCode.Y)) { gain -= gain_inc; }
        if (Input.GetKey(KeyCode.O)) { open_loop = true; }
        if (Input.GetKey(KeyCode.P)) { open_loop = false; }
        if (Input.GetKey(KeyCode.Z)) { valve = false; }
		if (Input.GetKey(KeyCode.Alpha0)){resetPos();}
		if (Input.GetKey(KeyCode.D)) { subProcessor("python", "Assets/stop.py");  }


		if (Input.GetKey("escape"))
		{
			subProcessor("python", "Assets/stop.py");

			Application.Quit();
		}
		//        if (Input.GetKey(KeyCode.X)) { valve = true; }
        if (Input.GetKey(KeyCode.X)) { 
	        
        }

		if (!open_loop)
		{
			transform.rotation = transform.rotation * Quaternion.Euler(new Vector3(0, -gain*(rossy.wbad), 0));
			//Debug.Log(rossy.wbad);
		}
		

		if (Input.GetKey(KeyCode.Keypad0))
		{
			caseU = 0;
			AppleTreeL.SetActive(false);
			AppleTreeR.SetActive(false);
			resetPos();
		}

		if (Input.GetKey(KeyCode.Keypad1))
		{
			caseU = 1;

			AppleTreeL.SetActive(false);
			AppleTreeR.SetActive(true);
			resetPos();
		}
		
		if (Input.GetKey(KeyCode.Keypad2))
		{
			caseU = 2;

			AppleTreeL.SetActive(true);
			AppleTreeR.SetActive(false);
			resetPos();
		}
		
		if (Input.GetKey(KeyCode.Keypad3))
		{
			caseU = 3;

			AppleTreeL.SetActive(true);
			AppleTreeR.SetActive(true);
			resetPos();
		}

		rossy.Publish();
	
	}
		
	
	
	void resetPos()
	{
		reset = 1;
		transform.position=new Vector3(0f,1f,0f);
		transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
	}
}
