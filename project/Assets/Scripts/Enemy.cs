using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ITarget {
    public float moveSpeed;
    public float _hitPoints;
	public GameObject weaponPrefab;
	public IItem activeWeapon;
    
    private Vector3 move;
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
            return _hitPoints;
        }

        set
        {
            _hitPoints = value;
        }
    }

    public bool dead
    {
        get
        {
            return this.hitPoints <= 0;
        }
    }

    // Use this for initialization
    void Start () {
        
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
        this.move = dMin.normalized * moveSpeed;
        this.transform.LookAt (target.transform.position);
        this.transform.position -= move;
    }

    public void hit(IAmmunition ammunition)
    {
        ammunition.affect(this.gameObject);
    }
}
