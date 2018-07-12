using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialMainScript : MonoBehaviour
{

    public List<GameObject> weapons;
    public List<GameObject> players;
    public GameObject ui;
    public GameObject moodManager;
    public GameObject[] spawners;

    //Keep players available at any point for other classes.
    public bool running;

    private Vector3 target = new Vector3();

    private float defaultHeight = 95f;
    private float defaultHeightMP = 70f;

    private int mood = 0;

    // Use this for initialization
    void Start()
    {

        //Populate active players
        //VERY UGLY HACK, but makes things much faster to implement.
        MainControl.activePlayers = new List<GameObject>();

        if (PlayerSelect.selections != null && PlayerSelect.selections.Count > 0)
        {
            for (int i = 0; i < PlayerSelect.selections.Count; i++)
            {
                if (PlayerSelect.selections[i].active)
                {
                    MainControl.activePlayers.Add(players[i]);
                    Debug.Log(PlayerSelect.selections[i].weaponIndex);

                }
                else
                    players[i].SetActive(false);
            }
        }
        else
        {
            foreach (GameObject player in players)
            {
                if (player != null && player.activeSelf)
                    MainControl.activePlayers.Add(player);
            }
        }

        //TODO - initialize player amount based on player selection.
        //TODO - make sure they have correct player ids (controller based).

        if (MainControl.activePlayers.Count > 1)
            this.updateMultiplayerCam(true);
        else
            this.updateSinglePlayerCam(true);

        this.spawners = GameObject.FindGameObjectsWithTag("spawner");
        running = true;
    }

    private void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        bool complete = true;
        bool alldead = true;
        foreach (GameObject spawner in spawners)
        {
            if (!spawner.GetComponent<Spawner>().dead)
            {
                complete = false;
                break;
            }
        }
        int aliveCount = 0;
        foreach (GameObject player in MainControl.activePlayers)
        {
            if (!player.GetComponent<Player>().dead)
            {
                alldead = false;
                aliveCount++;
            }
        }


        //Sync the camera
        if (aliveCount > 1)
        {
            //update two player cam
            this.updateMultiplayerCam(false);
        }
        else
        {
            //update single player cam
            this.updateSinglePlayerCam(false);
        }


        if (!running) return;

        if (complete)
        {
            if (mood >= 0)
            {
                PlayerPrefs.SetInt("tutorial_complete", 1);
                running = false;
                Invoke("showWinPre", 3f);
            }
        }
        if (alldead)
        {
            running = false;
            ui.GetComponent<UIManager>().showEnd();
        }
    }

    void showWinPre()
    {
        this.GetComponentInChildren<CamText>().levelComplete();
        Invoke("showWin", 3f);
    }
    void showWin()
    {
        ui.GetComponent<UIManager>().showWin();
    }

    //Updates the cam to fit single player in.
    private void updateSinglePlayerCam(bool instant)
    {
        GameObject player1 = null;
        foreach (GameObject player in MainControl.activePlayers)
        {
            if (!player.GetComponent<ITarget>().dead)
            {
                player1 = player;
                break;
            }
        }
        if (player1 != null)
        {
            float d = instant ? 1f : 10f;
            target.Set(Camera.main.transform.position.x + (player1.transform.position.x - Camera.main.transform.position.x) / d,
                0,
                Camera.main.transform.position.z + (player1.transform.position.z - 25 - Camera.main.transform.position.z) / d + 25);
            Camera.main.transform.position = new Vector3(
                Camera.main.transform.position.x + (player1.transform.position.x - Camera.main.transform.position.x) / d,
                defaultHeight,
                Camera.main.transform.position.z + (player1.transform.position.z - 25 - Camera.main.transform.position.z) / d);

            Camera.main.transform.LookAt(target);
        }
    }

    //Updates the cam to fit two players in.
    private void updateMultiplayerCam(bool instant)
    {
        Vector2 dMax = new Vector3(0f, 0f);
        GameObject player = null; ;
        foreach (GameObject player1 in MainControl.activePlayers)
        {
            foreach (GameObject player2 in MainControl.activePlayers)
            {
                if (player1 == player2) continue;
                Vector2 distanceBetween = new Vector2(player2.transform.position.x, player2.transform.position.z) -
                                          new Vector2(player1.transform.position.x, player1.transform.position.z);
                if (distanceBetween.magnitude > dMax.magnitude)
                {
                    dMax = distanceBetween;
                    player = player1;
                }
            }
        }

        Vector3 midPoint = player.transform.position + new Vector3(dMax.x, 0f, dMax.y) * 0.5f;
        float d = instant ? 1f : 10f;
        target.Set(Camera.main.transform.position.x + (midPoint.x - Camera.main.transform.position.x) / d,
            0,
            Camera.main.transform.position.z + (midPoint.z - 25 - Camera.main.transform.position.z) / d + 25);
        Camera.main.transform.position = new Vector3(
            Camera.main.transform.position.x + (midPoint.x - Camera.main.transform.position.x) / d,
            defaultHeight + Mathf.Max(0, (Vector3.Magnitude(dMax) * 1.9f)),
            Camera.main.transform.position.z + (midPoint.z - 25 - Camera.main.transform.position.z) / d);

        Camera.main.transform.LookAt(target);
    }
}
