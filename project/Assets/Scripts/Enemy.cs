using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
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
        //Calculate distance to the nearest player
        Vector3 d1 = player1 == null || !player1.activeSelf ? new Vector3(99999999f,999999999f,99999999f) : this.transform.position - player1.transform.position;
        Vector3 d2 = player2 == null || !player1.activeSelf ? new Vector3(99999999f, 999999999f, 99999999f) : this.transform.position - player2.transform.position;
        
        Vector3 targetD;
        if( Vector3.Magnitude(d1) < Vector3.Magnitude(d2))
        {
            targetD = d1.normalized;
			this.transform.LookAt (player1.transform.position);
        }
        else
        {
			targetD = d2.normalized;
			this.transform.LookAt (player2.transform.position);
        }

        this.transform.position -= targetD * moveSpeed;
    }

    public void hit(IProjectile ammunition)
    {
        ammunition.affect(this.gameObject);
    }
}
