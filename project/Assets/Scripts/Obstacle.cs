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
