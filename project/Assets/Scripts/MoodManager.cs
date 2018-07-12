using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodManager : MonoBehaviour {

    public bool waitingForMood = false;

    private float current;
    private int target;

    private List<List<GameObject>> spawnerList;

	// Use this for initialization
	void Start () {
        spawnerList = new List<List<GameObject>>();
		for(int i = 1; i <=5; i++)
        {
            var gos = this.transform.Find("mood" + i + "_spawners").transform;
            var l = new List<GameObject>();
            for (int j = 0; j < gos.childCount; j++)
            {
                var o = gos.GetChild(j).gameObject;
                o.SetActive(false);
                l.Add(o);
            }
                
            spawnerList.Add(l);
        }
	}
	
	// Update is called once per frame
	void Update () {
        this.current += (this.target - this.current) / 30;
        Camera.main.GetComponentInChildren<FFTEffects>().blend = Mathf.Clamp( this.current, 0f, 4f);
	}

    public void showMood(int target)
    {
        this.waitingForMood = true;
        this.target = target;
        //TODO - animate mood info text.
        Invoke("spawnersIn", 4f);
    }

    public List<GameObject> getInitialSpawns()
    {
        var l = spawnerList[0];
        spawnerList.RemoveAt(0);
        return l;
    }

    void spawnersIn()
    {
        var l = spawnerList[0];
        spawnerList.RemoveAt(0);
        Camera.main.GetComponent<MainControl>().spawnSpawners(l);
        this.waitingForMood = false;
    }
}

