using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControl : MonoBehaviour {

    public List<GameObject> players;

    //Keep players available at any point for other classes.
    public static List<GameObject> activePlayers;
    
    private Vector3 target = new Vector3();

	// Use this for initialization
	void Start () {
        //TODO - initialize player amount based on player selection.
        //TODO - make sure they have correct player ids (controller based).

        //Populate active players
        activePlayers = new List<GameObject>();
        foreach(GameObject player in players)
        {
            if (player != null && player.activeSelf)
                activePlayers.Add(player);
        }
	}
	
	// Update is called once per frame
	void Update () {
		//Sync the camera

        if(activePlayers.Count > 1)
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
        GameObject player1 = activePlayers[0];
        target.Set(Camera.main.transform.position.x + (player1.transform.position.x - Camera.main.transform.position.x) / 10,
            0,
            Camera.main.transform.position.z + (player1.transform.position.z-15 - Camera.main.transform.position.z) / 10+15);
        Camera.main.transform.position= new Vector3( 
            Camera.main.transform.position.x + (player1.transform.position.x-Camera.main.transform.position.x)/10, 
            +30,
            Camera.main.transform.position.z + (player1.transform.position.z-15 - Camera.main.transform.position.z)/10  );
        
        Camera.main.transform.LookAt(target);
    }

    private void updateMultiplayerCam()
    {
        Vector3 dMin = new Vector3(9999999f, 9999999999f, 99999999999f);
        GameObject player = null; ;
        foreach(GameObject player1 in activePlayers)
        {
            foreach(GameObject player2 in activePlayers)
            {
                if (player1 == player2) continue;
                Vector3 distanceBetween = player2.transform.position - player1.transform.position;
                if(Vector3.Magnitude(distanceBetween) < Vector3.Magnitude(dMin))
                {
                    dMin = distanceBetween;
                    player = player1;
                }
            }
            
        }
        

        Vector3 midPoint = player.transform.position + dMin * 0.5f;

        target.Set(Camera.main.transform.position.x + (midPoint.x - Camera.main.transform.position.x) / 10,
            0,
            Camera.main.transform.position.z + (midPoint.z - 15 - Camera.main.transform.position.z) / 10 + 15);
        Camera.main.transform.position = new Vector3(
            Camera.main.transform.position.x + (midPoint.x - Camera.main.transform.position.x) / 10,
            +30 + Mathf.Max(0,(Vector3.Magnitude(dMin)*2f-20f)),
            Camera.main.transform.position.z + (midPoint.z - 15 - Camera.main.transform.position.z) / 10);

        Camera.main.transform.LookAt(target);
    }
}
