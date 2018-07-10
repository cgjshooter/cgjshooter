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
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void hit(IAmmunition ammunition)
    {

    }
}
