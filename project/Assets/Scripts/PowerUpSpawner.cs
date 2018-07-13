using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour {

    public float spawnRadius;
    public List<GameObject> powerupPrefabs;
    public float powerUpSpawnMinDelay;
    public float powerUpSpawnMaxDelay;
    public int maxPowerupCount;

    private List<GameObject> robin;

	// Use this for initialization
	void Start () {
        this.robin =new List<GameObject>();
        if(GameConfig.allowPowerups)
           Invoke("spawnNext", UnityEngine.Random.value * (powerUpSpawnMaxDelay-powerUpSpawnMinDelay) + powerUpSpawnMinDelay);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void spawnNext()
    {
        if(robin.Count == 0)
        {
            //Add 3x the list amounts so that items are distributed nicely
            foreach(GameObject prefa in powerupPrefabs)
            {
                robin.Add(prefa);
            }
            ListUtil.Shuffle(robin);
        }
        if(this.transform.childCount < maxPowerupCount)
        {
            var prefa = robin[0];
            robin.RemoveAt(0);
            var go = Instantiate(prefa, this.transform);
            //Determine where to spawn.
            var deg = UnityEngine.Random.value * Mathf.PI * 2;
            var r = UnityEngine.Random.value * spawnRadius;
            var xp = Mathf.Cos(deg) * r;
            var zp = Mathf.Sin(deg) * r;
            //Skip if hits something.
            if(Physics.OverlapSphere(new Vector3(xp,60f,zp) + this.transform.position, 2f).Length > 0 )
            {
                Debug.Log(Physics.OverlapSphere(go.transform.position, 10f).Length);
                Destroy(go);
            }
            else
                go.transform.position = new Vector3(xp, 60f, zp);
        }
        Invoke("spawnNext", UnityEngine.Random.value * (powerUpSpawnMaxDelay-powerUpSpawnMinDelay) + powerUpSpawnMinDelay);
    }
}
