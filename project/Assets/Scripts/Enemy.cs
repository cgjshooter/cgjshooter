using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ITarget {
    public float moveSpeed;
    public float hitPoints;
	public GameObject weaponPrefab;
	public IItem activeWeapon;

    private GameObject player1;
    private GameObject player2;

    // Use this for initialization
    void Start () {
        //Get player references.
        this.player1 = GameObject.Find("Player1");
        this.player2 = GameObject.Find("Player2");

		if (weaponPrefab != null) {
			this.activeWeapon = Instantiate (weaponPrefab, this.transform).GetComponent<IItem> ();
		}
    }

    // Update is called once per frame
    void Update () {
		if(hitPoints < 0)
        {
            //DIE
            GameObject.Destroy(this.gameObject);
        }
        
		if (activeWeapon != null)
		{
			// simple shoot ai
			this.activeWeapon.activate(this.gameObject);
		}
	}

    //Calculate movements on fixed step.
    //TODO - if text mode is wanted. This needs to be moved to logic.
    private void FixedUpdate()
    {
        Vector3 dMin = new Vector3(999999999f, 9999999999f, 9999999999f);
        GameObject target = null;
        foreach(GameObject player in MainControl.activePlayers)
        {
            var d = this.transform.position - player.transform.position;
            if(Vector3.Magnitude(d) < Vector3.Magnitude(dMin))
            {
                dMin = d;
                target = player;
            }
        }
        
		this.transform.LookAt (player1.transform.position);
        this.transform.position -= dMin.normalized * moveSpeed;
    }

    public void hit(IAmmunition ammunition)
    {
        ammunition.affect(this.gameObject);
    }
}
