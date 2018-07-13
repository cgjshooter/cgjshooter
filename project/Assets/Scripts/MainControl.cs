using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainControl : MonoBehaviour {

    public List<GameObject> weapons;

    public List<GameObject> players;
    public List<GameObject> modifiers;
    public GameObject ui;
    public GameObject moodManager;
    public GameObject[] spawners;

    //Keep players available at any point for other classes.
    public static List<GameObject> activePlayers;
    public static List<GameObject> activeModifiers;
    public bool running;

    private Vector3 target = new Vector3();

    private float defaultHeight = 95f;
    private float defaultHeightMP = 70f;

    public float cameraOffset = 25f;
    
    private int mood = 0;

    private float startTime;
    

    // Use this for initialization
    void Start () {

        //Populate active players
        activePlayers = new List<GameObject>();
        
        if(PlayerSelect.selections != null && PlayerSelect.selections.Count > 0)
        {
            for (int i = 0; i < PlayerSelect.selections.Count; i++)
            {
                if (PlayerSelect.selections[i].active)
                {
                    activePlayers.Add(players[i]);
                    players[i].SetActive(true);
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
                    activePlayers.Add(player);
            }
        }
        //Update 
        GameConfig.difficultyMultiplier = GameConfig.difficulty * (1f + (activePlayers.Count - 1) / 2);

        //TODO - populate modifiers based by level generation / modifiers.
        
        //Populate active modifiers
        //Modifiers purely in code for now as time didnt allow proper implementatiton to unity editor.
        activeModifiers = new List<GameObject>();
        foreach(GameObject modifier in modifiers)
        {
            if (modifier != null && modifier.activeSelf)
                activeModifiers.Add(modifier);
        }

        if (activePlayers.Count > 1)
            this.updateMultiplayerCam(true);
        else
            this.updateSinglePlayerCam(true);

        Invoke("spawnFirstSpawners", 5f);

        if (PlayerSelect.mode == 1)
        {
            ui.transform.Find("timeattack").GetComponent<Text>().text = "00:00";
        }
        else if (PlayerSelect.mode == 2)
        {
            ui.transform.Find("timeattack").GetComponent<Text>().text = "0";
        }
        else
            ui.transform.Find("timeattack").GetComponent<Text>().text = "";
    }

    private void spawnFirstSpawners()
    {
        foreach (GameObject go in moodManager.GetComponent<MoodManager>().getInitialSpawns())
        {
            go.SetActive(true);
            go.transform.parent = GameObject.Find("/enemyContainer").transform;
        }
        this.spawners = GameObject.FindGameObjectsWithTag("spawner");
        running = true;
        startTime = Time.time;
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update () {
        bool complete = true;
        bool alldead = true;
        int spawnerCount = 0;
        foreach (GameObject spawner in spawners)
        {
            if (!spawner.GetComponent<Spawner>().dead)
            {
                complete = false;
                spawnerCount++;
            }
        }
        GameConfig.spawnSpeedIncrease = spawnerCount<=0? 1f: 1f - (spawners.Length-spawnerCount) / spawners.Length * 0.75f;
        int aliveCount = 0;
        foreach (GameObject player in activePlayers)
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
            if(mood >= 5)
            {
                Debug.Log("WINNER!");
                running = false;
                PlayerPrefs.SetInt("attack_complete", 1);
                Invoke("showWinPre", 3f);
            }
            else if(!moodManager.GetComponent<MoodManager>().waitingForMood)
            {
                mood++;
                if(mood < 5)
                    moodManager.GetComponent<MoodManager>().showMood(mood);
            }
        }
        if (alldead)
        {
            running = false;
            ui.GetComponent<UIManager>().showEnd();
        }

        if(PlayerSelect.mode == 1)
        {
            float t = Time.time - startTime;
            float m = Mathf.Floor(t / 60f);
            string mm = (t < 10 ? "0" : "") + m;
            float s = Mathf.Floor(t % 60f);
            string ss = (s < 10 ? "0" : "") + s;
            ui.transform.Find("timeattack").GetComponent<Text>().text = mm + ":" + ss;
        }
        else if(PlayerSelect.mode == 2)
        {
            ui.transform.Find("timeattack").GetComponent<Text>().text = "score";
        }
        else
            ui.transform.Find("timeattack").GetComponent<Text>().text = "";

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

    public void spawnSpawners(List<GameObject> next)
    {
        List<GameObject> alives = new List<GameObject>();
        foreach (GameObject p in activePlayers)
        {
            if (!p.GetComponent<Player>().dead)
                alives.Add(p);
        }

        //Resurrect players
        foreach (GameObject p in activePlayers)
        {
            if(p.GetComponent<Player>().dead)
            {
                p.transform.position = new Vector3(alives[0].transform.position.x, alives[0].transform.position.y + 15f, alives[0].transform.position.z);
            }
            p.GetComponent<Player>().hitPoints = p.GetComponent<Player>().maxHealth;
        }
        foreach( GameObject go in next)
        {
            go.SetActive(true);
            go.transform.parent = GameObject.Find("/enemyContainer").transform;
        }
        this.spawners = GameObject.FindGameObjectsWithTag("spawner");

    }

    //Updates the cam to fit single player in.
    private void updateSinglePlayerCam(bool instant)
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
            float d = instant ? 1f : 10f;
            target.Set(Camera.main.transform.position.x + (player1.transform.position.x - Camera.main.transform.position.x) / d,
                0,
                Camera.main.transform.position.z + (player1.transform.position.z - cameraOffset - Camera.main.transform.position.z) / d + cameraOffset);
            Camera.main.transform.position = new Vector3(
                Camera.main.transform.position.x + (player1.transform.position.x - Camera.main.transform.position.x) / d,
                defaultHeight,
                Camera.main.transform.position.z + (player1.transform.position.z - cameraOffset - Camera.main.transform.position.z) / d);

            Camera.main.transform.LookAt(target);
        }
    }

    //Updates the cam to fit two players in.
    private void updateMultiplayerCam(bool instant)
    {
        Vector2 dMax = new Vector3(0f, 0f);
        GameObject player = null; ;
        foreach(GameObject player1 in activePlayers)
        {
            foreach(GameObject player2 in activePlayers)
            {
                if (player1 == player2) continue;
                Vector2 distanceBetween = new Vector2(player2.transform.position.x, player2.transform.position.z) -
                                          new Vector2(player1.transform.position.x, player1.transform.position.z);
                if(distanceBetween.magnitude > dMax.magnitude)
                {
                    dMax = distanceBetween;
                    player = player1;
                }
            }
        }

        Vector3 midPoint = player.transform.position + new Vector3(dMax.x,0f,dMax.y) * 0.5f;
        float d = instant ? 1f : 10f;
        target.Set(Camera.main.transform.position.x + (midPoint.x - Camera.main.transform.position.x) / d,
            0,
            Camera.main.transform.position.z + (midPoint.z - cameraOffset - Camera.main.transform.position.z) / d + cameraOffset);
        Camera.main.transform.position = new Vector3(
            Camera.main.transform.position.x + (midPoint.x - Camera.main.transform.position.x) / d ,
            defaultHeight + Mathf.Max(0,(Vector3.Magnitude(dMax)*1.9f)),
            Camera.main.transform.position.z + (midPoint.z - cameraOffset - Camera.main.transform.position.z) / d);

        Camera.main.transform.LookAt(target);
    }
}
