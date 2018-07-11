using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour, ITarget {

    public Vector3 move = new Vector3();
    public Vector3 m_Move
    {
        get
        {
            return move;
        }
    }
    public float hitPoints
    {
        get
        {
            return 1;
        }

        set
        {
            
        }
    }

    public bool dead
    {
        get
        {
            return false;
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
            return true;
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

    public float _maxHealth;
    public float maxHealth
    {
        get
        {
            return _maxHealth;
        }
    }

    // Use this for initialization
    void Start () {
        if (this.hitPoints > _maxHealth) _maxHealth = this.hitPoints;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void hit(IAmmunition ammunition)
    {

    }
}
