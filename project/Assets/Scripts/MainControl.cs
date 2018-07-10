using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControl : MonoBehaviour {

    public List<GameObject> players;

    //Keep players available at any point for other classes.
    public static List<GameObject> activePlayers;

    public GameObject player1;
    public GameObject player2;

    private Vector3 target = new Vector3();

	// Use this for initialization
	void Start () {
        //TODO - initialize player amount based on player selection.
        //TODO - make sure they have correct player ids (controller based).

        //Populate active players
        activePlayers = new List<GameObject>();
        foreach(GameObject player in players)
        {
            if (player.activeSelf)
                activePlayers.Add(player);
        }
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
        Vector3 distanceBetween = player2.transform.position - player1.transform.position;
        float dist = Vector3.Magnitude(distanceBetween);

        Vector3 midPoint = player1.transform.position + distanceBetween * 0.5f;

        target.Set(Camera.main.transform.position.x + (midPoint.x - Camera.main.transform.position.x) / 10,
            0,
            Camera.main.transform.position.z + (midPoint.z - 15 - Camera.main.transform.position.z) / 10 + 15);
        Camera.main.transform.position = new Vector3(
            Camera.main.transform.position.x + (midPoint.x - Camera.main.transform.position.x) / 10,
            +30 + Mathf.Max(0,(dist*2f-20f)),
            Camera.main.transform.position.z + (midPoint.z - 15 - Camera.main.transform.position.z) / 10);

        Camera.main.transform.LookAt(target);
    }
}
