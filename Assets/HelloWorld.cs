using System;
using RosSharp.RosBridgeClient;
using UnityEngine;
using System.Collections;

// run these commands on ROS system:
// roslaunch rosbridge_server rosbridge_websocket.launch
// rostopic echo /talker
// rostopic pub /listener std_msgs/String "World!"



public class HelloWorld: MonoBehaviour
{
	public CameraMan cammy;
    public StandardHeader header;

	public float wbad = 0;
    public float wbas = 0;

	private System.DateTime epochStart = new System.DateTime(1970, 1, 1, 0, 0, 0, System.DateTimeKind.Utc);
	private MsgTrajectory message;
    private String publication_id;
    private RosSocket rosSocket ;



	public void subProcessor (string command, string argss)
	{

            try
            {
            	System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo();
            	procInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            	procInfo.FileName = command; // 'sh' for bash
            	procInfo.Arguments = argss; // The Script name
            	System.Diagnostics.Process p= System.Diagnostics.Process.Start(procInfo); // Start that process
				//return p;

            }
            catch (Exception e)
            {
                //Console.WriteLine(e.Message);
		        Debug.Log(e.Message);

			}
	}
	

	//public void init
    public void Start()
    {
	    subProcessor("rosbag", "record --lz4 --output-name=tada3.bag /rhag_camera/image_raw/compressed /kinefly/image_output /servo_camera/image_raw/compressed /kinefly/flystate /trajectory");
        header = new StandardHeader();
        rosSocket = new RosSocket("ws://localhost:9090");
        cammy = GetComponent<CameraMan>();

		// Publication:
        publication_id = rosSocket.Advertise("/trajectory", "World/MsgTrajectory");
        message = new MsgTrajectory();
        
        // Subscription:
        String subscription_id = rosSocket.Subscribe("/kinefly/flystate", "Kinefly/MsgFlystate", subscriptionHandler);
    }



    public void Publish()
    {
        double time = ((System.DateTime.UtcNow - epochStart).TotalSeconds);
        int secs = (int)time;
        int nsecs = (int)(1000 *(time-secs));
        header.seq++;
        header.stamp.secs = secs;
        header.stamp.nsecs = nsecs;

        message.header = header;
        
        message.pPos.x= Camera.main.gameObject.transform.position.x ;
        message.pPos.y = Camera.main.gameObject.transform.position.z ;
        message.pPos.z = Camera.main.gameObject.transform.position.y ;

        message.pOri.x = - Camera.main.gameObject.transform.eulerAngles.y ;
        message.pOri.y = Camera.main.gameObject.transform.eulerAngles.z ;
        message.pOri.z = Camera.main.gameObject.transform.eulerAngles.x ;

        message.o1Pos.x = cammy.AppleTreeL.transform.position.x;
        message.o1Pos.y = cammy.AppleTreeL.transform.position.z;
        message.o1Pos.z = cammy.AppleTreeL.transform.position.y;
        message.o2Pos.x = cammy.AppleTreeR.transform.position.x;
        message.o2Pos.y = cammy.AppleTreeR.transform.position.z;
        message.o2Pos.z = cammy.AppleTreeR.transform.position.y;

        message.wbad = wbad;
        message.wbas = wbas;

        message.speed = cammy.speed;
        message.gain = cammy.gain;

//        slip=0f;
//        groundSpeed=0f;
        message.DCoffset=cammy.DCoffset;
//        packetFrequency=0f;
//        packetDuration=0f;
//        pfStimState=0f;
//    
        message.headingControl=cammy.open_loop? 0 : 1;
//        speedControl=0;
//        trial=0;
//        runNum=0;
        message.caseU=cammy.caseU;
//        servoAngle=0;
//        valve1=0;
//        valve2=0;
//        valve3=0;
//        quadrant=0;
//        boutFrame=0;
        message.reset=cammy.reset;
//        isFlying=0;
//            
//        impose=0f;
//        imposeResponse=0f;
//        imposeResponseSmooth=0f;
//        imposeHeading=0f;
//        imposeResponseHeading=0f;
//        compensation=0f;
//        key="";

        //Debug.Log("position is"+ message.pPos.x);
        rosSocket.Publish(publication_id, message);
    }

	public void subscriptionHandler(Message message)
    {
        MsgFlystate standardString= (MsgFlystate)message;
        wbad = (standardString.left.angles[0] - standardString.right.angles[0] + cammy.DCoffset);
        wbas = (standardString.left.angles[0] - standardString.right.angles[0] + cammy.DCoffset);
        //Debug.Log("wbad: "+ wbad);
    }

}
