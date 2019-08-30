using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class hfov : MonoBehaviour {
public float fieldOfView = 120;
 
  
void Start () {
    
    GetComponent<Camera>().fieldOfView = fieldOfView *(16f/9f) ;//((float)GetComponent<Camera>().pixelWidth / GetComponent<Camera>().pixelHeight);
    }
}

