using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainControl : MonoBehaviour {

    public List<GameObject> players;
    public List<GameObject> modifiers;
    public GameObject ui;

    public GameObject[] spawners;

    //Keep players available at any point for other classes.
    public static List<GameObject> activePlayers;
    public static List<GameObject> activeModifiers;
    public bool running;

    private Vector3 target = new Vector3();

    private float defaultHeight = 35f;

	// Use this for initialization
	void Start () {
        running = true;
        //TODO - initialize player amount based on player selection.
        //TODO - make sure they have correct player ids (controller based).

        //TODO - populate modifiers based by level generation / modifiers.

        //Populate active players
        activePlayers = new List<GameObject>();
        foreach(GameObject player in players)
        {
            if (player != null && player.activeSelf)
                activePlayers.Add(player);
        }

        //Populate active modifiers
        activeModifiers = new List<GameObject>();
        foreach(GameObject modifier in modifiers)
        {
            if (modifier != null && modifier.activeSelf)
                activeModifiers.Add(modifier);
        }

        this.spawners = GameObject.FindGameObjectsWithTag("spawner");

	}
	
	// Update is called once per frame
	void Update () {
        //Sync the camera
        if (!running) return;

        bool complete = true;
        bool alldead = true;
        foreach(GameObject spawner in spawners)
        {
            if(!spawner.GetComponent<Spawner>().dead)
            {
                complete = false;
                break;
            }
        }
        int aliveCount = 0;
        foreach(GameObject player in activePlayers)
        {
            if(!player.GetComponent<Player>().dead)
            {
                alldead = false;
                aliveCount++;
            }
        }

        if (aliveCount > 1)
        {
            //update two player cam
            this.updateMultiplayerCam();
        }
        else
        {
            //update single player cam
            this.updateSinglePlayerCam();
        }

        if (complete)
        {
            Debug.Log("WINNER!");
            running = false;
            ui.GetComponent<UIManager>().showWin();
        }
        if(alldead)
        {
            running = false;
            ui.GetComponent<UIManager>().showEnd();
        }
	}
    
    //Updates the cam to fit single player in.
    private void updateSinglePlayerCam()
    {
        GameObject player1 = null;
        foreach(GameObject player in activePlayers)
        {
            if (!player.GetComponent<ITarget>().dead)
            {
                player1 = player;
                break;
            }
        }
        if(player1 != null)
        {
            target.Set(Camera.main.transform.position.x + (player1.transform.position.x - Camera.main.transform.position.x) / 10,
                0,
                Camera.main.transform.position.z + (player1.transform.position.z - 15 - Camera.main.transform.position.z) / 10 + 15);
            Camera.main.transform.position = new Vector3(
                Camera.main.transform.position.x + (player1.transform.position.x - Camera.main.transform.position.x) / 10,
                defaultHeight,
                Camera.main.transform.position.z + (player1.transform.position.z - 15 - Camera.main.transform.position.z) / 10);

            Camera.main.transform.LookAt(target);
        }
    }

    //Updates the cam to fit two players in.
    private void updateMultiplayerCam()
    {
        Vector2 dMin = new Vector3(9999999f, 9999999999f);
        GameObject player = null; ;
        foreach(GameObject player1 in activePlayers)
        {
            foreach(GameObject player2 in activePlayers)
            {
                if (player1 == player2) continue;
                Vector2 distanceBetween = new Vector2(player2.transform.position.x, player2.transform.position.z) -
                                          new Vector2(player1.transform.position.x, player1.transform.position.z);
                if(distanceBetween.magnitude < dMin.magnitude)
                {
                    dMin = distanceBetween;
                    player = player1;
                }
            }
        }

        Vector3 midPoint = player.transform.position + new Vector3(dMin.x,0f,dMin.y) * 0.5f;

        target.Set(Camera.main.transform.position.x + (midPoint.x - Camera.main.transform.position.x) / 10,
            0,
            Camera.main.transform.position.z + (midPoint.z - 15 - Camera.main.transform.position.z) / 10 + 15);
        Camera.main.transform.position = new Vector3(
            Camera.main.transform.position.x + (midPoint.x - Camera.main.transform.position.x) / 10,
            defaultHeight + Mathf.Max(0,(Vector3.Magnitude(dMin)*1.9f)),
            Camera.main.transform.position.z + (midPoint.z - 15 - Camera.main.transform.position.z) / 10);

        Camera.main.transform.LookAt(target);
    }
}
