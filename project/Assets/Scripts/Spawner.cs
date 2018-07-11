using System;
using System.Collections;
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
    private Transform spawnPos;

    public GameObject death;
    public Vector3 move = new Vector3();
    public Vector3 m_Move
    {
        get
        {
            return move;
        }
    }

    public float _hitPoints;
    public float hitPoints
    {
        get
        {
            return _hitPoints;
        }

        set
        {
            _hitPoints = value;
        }
    }
    public float _maxHealth;
    public float maxHealth
    {
        get
        {
            return _maxHealth;
        }
    }

    public bool dead
    {
        get
        {
            return this.hitPoints <= 0;
        }
    }

    public float armor
    {
        get
        {
            return 0;
        }

        set
        {
           
        }
    }

    public bool invulnerable
    {
        get
        {
            return false;
        }

        set
        {
            
        }
    }

    public bool invisible
    {
        get
        {
            return false;
        }

        set
        {
            
        }
    }

    // Use this for initialization
    void Start () {
        if (this.hitPoints > _maxHealth) _maxHealth = this.hitPoints;
        if (this.enemyContainer == null) this.enemyContainer = GameObject.Find("enemyContainer");
        this.spawnPos = this.transform.Find("spawnPos");
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - previousSpawn >  spawnDelay && !dead)
        {
            previousSpawn = Time.time;
            //Pick a random type
            GameObject enemyType = enemyTypes[(int)Mathf.Floor(UnityEngine.Random.value * enemyTypes.Count)];
            //TODO - get the spawn place. For now, just use default.
            Instantiate(enemyType, this.spawnPos.transform.position, this.transform.rotation, enemyContainer.transform);
        }
        if (dead && !death.activeSelf)
        {
            death.SetActive(true);
            foreach (MeshRenderer me in this.GetComponentsInChildren<MeshRenderer>()) me.enabled = false;
        }
	}

    public void hit(IAmmunition ammunition)
    {
        ammunition.affect(this.gameObject);
    }
}
