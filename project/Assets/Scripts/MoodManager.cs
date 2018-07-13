using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodManager : MonoBehaviour {

    public bool waitingForMood = false;
    

    private float current;
    private int target;

    private bool animate;
    private List<List<GameObject>> spawnerList;
    private List<Material> moodMaterials;

    //Dummy way to do mixing, but fast to implement.
    private AudioSource as1;
    private AudioSource as2;
    private AudioSource as3;
    private AudioSource as4;
    private AudioSource as5;

    private float[] targetSpeeds = new float[5] { 1f, 0.7f, 1f, 1.4f, 0.4f };

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

        // get materials
        moodMaterials = GameObject.Find("Ground").GetComponent<Environment>().moodMaterials;

    }

    // Update is called once per frame
    void Update () {
        if(animate)
            this.current += (this.target - this.current) / 80;
        if (Mathf.Abs(this.current - this.target) < 0.01) this.current = this.target;
        Camera.main.GetComponentInChildren<FFTEffects>().blend = Mathf.Clamp( this.current, 0f, 4f);

        float blend = Mathf.Clamp(this.current,0f,4f);
        as1.volume = Mathf.Clamp(1f - blend, 0f, 1f);
        as2.volume = Mathf.Clamp(blend < 1f ? blend : 2f - blend, 0f, 1f);
        as3.volume = Mathf.Clamp(blend < 2f ? blend - 1f : 3f - blend, 0f, 1f);
        as4.volume = Mathf.Clamp(blend < 3f ? blend - 2f : 4f - blend, 0f, 1f);
        as5.volume = Mathf.Clamp(blend < 4f ? blend - 3f : 5f - blend, 0f, 1f);

        int lowInd = (int)Mathf.Floor(blend);

        int highInd = (int)Mathf.Ceil(blend);
        
        if(highInd > targetSpeeds.Length-1) highInd = targetSpeeds.Length-1;
        float dif = blend - lowInd;

        Time.timeScale = Mathf.Lerp(targetSpeeds[lowInd], targetSpeeds[highInd], dif) * GameConfig.speedMultiplier;

        // Material Lerping
        GameObject.Find("Ground").GetComponent<Renderer>().material.Lerp(this.moodMaterials[lowInd], this.moodMaterials[highInd], dif);

        
    }

    public void showMood(int target)
    {
        animate = false;
        Camera.main.GetComponentInChildren<CamText>().showWellDone();

        this.waitingForMood = true;
        this.target = target;
        //TODO - animate mood info text.
        Invoke("spawnersIn", 8f);
        Invoke("counterIn", 6.4f);
    }

    private void counterIn()
    {
        animate = true;
        Camera.main.GetComponentInChildren<CamText>().showCounter();
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

