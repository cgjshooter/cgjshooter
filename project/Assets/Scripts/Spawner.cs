﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Spawner spawning new enemies constantly.
 */
public class Spawner : MonoBehaviour, ITarget {

    public List<GameObject> enemyTypes;
    public GameObject enemyContainer;
    public float spawnDelay;
    private float previousSpawn;

    public Vector3 move = new Vector3();
    public Vector3 m_Move
    {
        get
        {
            return move;
        }
    }

    // Use this for initialization
    void Start () {
        if (this.enemyContainer == null) this.enemyContainer = GameObject.Find("enemyContainer");
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - previousSpawn >  spawnDelay)
        {
            previousSpawn = Time.time;
            //Pick a random type
            GameObject enemyType = enemyTypes[(int)Mathf.Floor(Random.value * enemyTypes.Count)];
            //TODO - get the spawn place. For now, just use default.
            Instantiate(enemyType, this.transform.position, this.transform.rotation, enemyContainer.transform);
        }
	}

    public void hit(IAmmunition ammunition)
    {

    }
}
