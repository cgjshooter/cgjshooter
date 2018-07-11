using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour, IItem {

    public bool toggleable
    {
        get
        {
            return false;
        }
    }

    public bool _useOnPickup;
    public bool useOnPickup
    {
        get
        {
            return _useOnPickup;
        }
    }

    public int _useCount;
    public int useCount
    {
        get
        {
            return _useCount;
        }
    }

    public float _power;
    public float power
    {
        get
        {
            return _power;
        }
    }

    public float health;
    public float armor;
    public float weaponspeedmultiplier;
    public float weapondamagemultiplier;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("HIT WITH " + other.gameObject.tag);
        if(other.gameObject.tag == "Player")
        {
            //TODO - implement pickup
            bool pickOk = other.gameObject.GetComponent<Player>().pickPowerup(this.gameObject);
            if(pickOk)
                this.gameObject.SetActive(false);
        }
    }

    public void activate(GameObject player)
    {
        Debug.Log("ACTIVATE POWERR UP : " + this.health);
        var p = player.GetComponent<Player>();
        p.hitPoints += this.health;
        //TODO - armor
        //TODO - other activations.
    }

    public void deactivate(GameObject player)
    {
        
    }
}
