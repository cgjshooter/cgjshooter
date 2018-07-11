using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour, ITarget {
    public float moveSpeed;
    public float _hitPoints;
	public GameObject weaponPrefab;
	public IItem activeWeapon;
    public GameObject death;
    public float ramDamage;
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

    [Range(0,1)]
    public float _armor;
    public float armor
    {
        get
        {
            return _armor;
        }

        set
        {
            _armor = value;
        }
    }

    public bool _invulnerable;
    public bool invulnerable
    {
        get
        {
            return _invulnerable;
        }

        set
        {
            _invulnerable = value;
        }
    }

    public bool _invisible;
    public bool invisible
    {
        get
        {
            return _invisible;
        }

        set
        {
            _invisible = value;
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
		if(dead)
        {
            //DIE
            if(!this.death.activeSelf)
            {
                this.death.SetActive(true);
                this.GetComponent<MeshRenderer>().enabled = false;
				foreach(MeshRenderer e in transform.GetComponentsInChildren<MeshRenderer>()) e.enabled = false;
				this.GetComponent<BoxCollider> ().enabled = false;
                Invoke("deathDone", 5f);
            }
        }
        
		if (activeWeapon != null && !dead)
		{
			// simple shoot ai
			this.activeWeapon.activate(this.gameObject);
		}
	}

    void deathDone()
    {
        GameObject.Destroy(this.gameObject);
    }

    //Calculate movements on fixed step.
    //TODO - if text mode is wanted. This needs to be moved to logic.
    private void FixedUpdate()
    {
        if (dead) return;
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
        this.transform.position -= move * Time.fixedDeltaTime;

        if(this.activeWeapon == null && Vector3.Magnitude(dMin) < 2.5f && hitPoints > 0)
        {
            //Self destruct.
            this.hitPoints = 0;
            var e = target.GetComponent<Player>();
            float blocked = this.ramDamage * Mathf.Clamp((e.armor), 0.1f, 1f);
            e.hitPoints -= this.ramDamage - blocked;
            //Weaken armor by blocked amount. Divider is just some weakening value that needs to be tweaked.
            e.armor -= blocked / 10f;
            if (e.armor < 0) e.armor = 0f;
            
        }
    }

    public void hit(IAmmunition ammunition)
    {
        ammunition.affect(this.gameObject);
    }
}
