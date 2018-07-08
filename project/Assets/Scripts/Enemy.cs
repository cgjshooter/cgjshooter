using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    public float moveSpeed;
    public int hitPoints;

    private GameObject player1;
    private GameObject player2;

    // Use this for initialization
    void Start () {
        //Get player references.
        this.player1 = GameObject.Find("Player1");
        this.player2 = GameObject.Find("Player2");

    }

    // Update is called once per frame
    void Update () {
		
        
	}

    //Calculate movements on fixed step.
    //TODO - if text mode is wanted. This needs to be moved to logic.
    private void FixedUpdate()
    {
        //Calculate distance to the nearest player
        Vector3 d1 = player1 == null || !player1.activeSelf ? new Vector3(99999999f,999999999f,99999999f) : this.transform.position - player1.transform.position;
        Vector3 d2 = player2 == null || !player1.activeSelf ? new Vector3(99999999f, 999999999f, 99999999f) : this.transform.position - player2.transform.position;
        
        Vector3 targetD;
        if( Vector3.Magnitude(d1) < Vector3.Magnitude(d2))
        {
            targetD = d1.normalized;
        }
        else
        {
            targetD = d2.normalized;
        }

        this.transform.position -= targetD * moveSpeed;
    }

    public void hit()
    {
        this.hitPoints--;
        if (this.hitPoints <= 0)
            GameObject.Destroy(this.gameObject);
    }
}
