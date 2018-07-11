using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour, IItem {
    public float power
    {
        get
        {
            return 0;
        }
    }

    public bool toggleable
    {
        get
        {
            return false;
        }
    }

    public int useCount
    {
        get
        {
            return 1;
        }
    }

    public bool useOnPickup
    {
        get
        {
            return false;
        }
    }

    public void activate(GameObject player)
    {
        
    }

    public void deactivate(GameObject player)
    {
        
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
