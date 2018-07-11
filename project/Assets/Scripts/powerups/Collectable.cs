using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

    public GameObject itemPrefab;

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
}
