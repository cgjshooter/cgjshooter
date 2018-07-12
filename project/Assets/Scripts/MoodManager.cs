using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodManager : MonoBehaviour {

    public bool waitingForMood = false;

    private float current;
    private int target;

    private List<List<GameObject>> spawnerList;

    //Dummy way to do mixing, but fast to implement.
    private AudioSource as1;
    private AudioSource as2;
    private AudioSource as3;
    private AudioSource as4;
    private AudioSource as5;

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
        var ases = this.GetComponents<AudioSource>();
        as1 = ases[0];
        as2 = ases[1];
        as3 = ases[2];
        as4 = ases[3];
        as5 = ases[4];

    }

    // Update is called once per frame
    void Update () {
        this.current += (this.target - this.current) / 30;
        if (Mathf.Abs(this.current - this.target) < 0.01) this.current = this.target;
        Camera.main.GetComponentInChildren<FFTEffects>().blend = Mathf.Clamp( this.current, 0f, 4f);

        float blend = Mathf.Clamp(this.current,0f,4f);
        as1.volume = Mathf.Clamp(1f - blend, 0f, 1f);
        as2.volume = Mathf.Clamp(blend < 1f ? blend : 2f - blend, 0f, 1f);
        as3.volume = Mathf.Clamp(blend < 2f ? blend - 1f : 3f - blend, 0f, 1f);
        as4.volume = Mathf.Clamp(blend < 3f ? blend - 2f : 4f - blend, 0f, 1f);
        as5.volume = Mathf.Clamp(blend < 4f ? blend - 3f : 5f - blend, 0f, 1f);

        Debug.Log(as1.volume + "," + as2.volume + "," + as3.volume + "," + as4.volume + "," + as5.volume);
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

