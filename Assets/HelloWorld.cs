using System;
using RosSharp.RosBridgeClient;
using UnityEngine;
using System.Collections;

// commands on ROS system:
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

        
    public void Start()
    {
            string command = "rqt";
		    string argss = "";
        
            //string argss = "/home/rhagoletis/test.py";
            //string verb = " ";

            System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo();
            procInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            //procInfo.UseShellExecute = false;
            procInfo.FileName = command; // 'sh' for bash
            procInfo.Arguments = argss; // The Script name
            //procInfo.Verb = verb; // ------------
            //procInfo.RedirectStandardOutput = false;
            System.Diagnostics.Process p= System.Diagnostics.Process.Start(procInfo); // Start that process.
            //string strOutput = p.StandardOutput.ReadToEnd(); 
            //p.WaitForExit();
            //Debug.Log(strOutput);

//
//            System.Diagnostics.ProcessStartInfo psi = new System.Diagnostics.ProcessStartInfo();
//            
//        
////        ProcessStartInfo psi = new ProcessStartInfo(); 
//        psi.FileName = "/home/rhagoletis/test.sh";
////        psi.FileName = Application.dataPath+"/test.sh";
//        psi.UseShellExecute = false; 
//        psi.RedirectStandardOutput = true;
////        psi.Arguments = "arg1 arg2 arg3";
//
////psi.Arguments = "test"; 
//        System.Diagnostics.Process p = System.Diagnostics.Process.Start(psi); 
//        string strOutput = p.StandardOutput.ReadToEnd(); 
//        p.WaitForExit(); 
//        UnityEngine.Debug.Log(strOutput);
////
////            
////        System.Diagnostics.Process.Start("rqt_plot");
//        System.Diagnostics.Process p = new System.Diagnostics.Process();
//        p.StartInfo.FileName = "jupyter";
////        p.StartInfo.FileName = "/home/rhagoletis/catkin/devel/lib/python2.7/dist-packages/python";
////        p.StartInfo.Arguments = "/home/rhagoletis/catkin/src/World/GUI_caller.py";
//        p.StartInfo.Arguments = "notebook";    
//        p.StartInfo.RedirectStandardError=false;
//        p.StartInfo.RedirectStandardOutput = false;
//        p.StartInfo.CreateNoWindow = true;
//
////        p.StartInfo.WorkingDirectory = "/home/rhagoletis/"; 
////        p.StartInfo.CreateNoWindow = false;
//        p.StartInfo.UseShellExecute = true;
//        p.Start();     
////        p.WaitForExit();
//
//
////        System.Diagnostics.Process info = new System.Diagnostics.Process();
////        info.StartInfo.Filename = "python";
////        info.StartInfo.Arguments = "~/catkin/src/World/GUI_caller.py";
////     
////        //use to create no window when running cmd script
////        info.StartInfo.UseShellExecute = true;
////        info.StartInfo.CreateNoWindow = true;
////        info.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
////     
////        info.Start();
////     
////        //if you want program to halt until script is finished
////        info.WaitForExit();
//


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
