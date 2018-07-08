using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControl : MonoBehaviour {

    public GameObject player1;
    public GameObject player2;

    private Vector3 target = new Vector3();

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//Sync the camera
        if(player2 != null && player2.activeSelf)
        {
            //update two player cam
            this.updateMultiplayerCam();
        }
        else
        {
            //update single player cam
            this.updateSinglePlayerCam();
        }
	}

    private void updateSinglePlayerCam()
    {
        target.Set(Camera.main.transform.position.x + (this.player1.transform.position.x - Camera.main.transform.position.x) / 10,
            0,
            Camera.main.transform.position.z + (this.player1.transform.position.z-15 - Camera.main.transform.position.z) / 10+15);
        Camera.main.transform.position= new Vector3( 
            Camera.main.transform.position.x + (this.player1.transform.position.x-Camera.main.transform.position.x)/10, 
            +30,
            Camera.main.transform.position.z + (this.player1.transform.position.z-15 - Camera.main.transform.position.z)/10  );
        
        Camera.main.transform.LookAt(target);
    }

    private void updateMultiplayerCam()
    {

    }
}
