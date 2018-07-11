using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoRotation : MonoBehaviour {

    public bool circular;
    public Vector3 speed;
    public Vector3 circularAngles;
    
    private Quaternion originalRot;
    
	// Use this for initialization
	void Start () {
        this.originalRot = this.transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
        if(circular)
            this.transform.rotation = Quaternion.Euler(new Vector3(
                Mathf.Sin(speed.x* Time.time)*circularAngles.x,
                Mathf.Sin(speed.y*Time.time)*circularAngles.y,
                Mathf.Sin(speed.z*Time.time)*circularAngles.z)
                + originalRot.eulerAngles);
        else
            this.transform.rotation = Quaternion.Euler(Time.time*speed + originalRot.eulerAngles);
	}
}
