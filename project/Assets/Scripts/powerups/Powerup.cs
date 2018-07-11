using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Powerup : MonoBehaviour, IItem {

    public Sprite _icon;
    public Sprite icon
    {
        get
        {
            return this._icon;
        }
    }

    public bool toggleable
    {
        get
        {
            return false;
        }
    }

    public bool _useOnPickup;
    public bool useOnPickup
    {
        get
        {
            return _useOnPickup;
        }
    }

    public int _useCount;
    public int useCount
    {
        get
        {
            return _useCount;
        }
    }

    public float _power;
    public float power
    {
        get
        {
            return _power;
        }
    }

    public float health;
    public float armor;
    public float weaponspeedmultiplier;
    public float weapondamagemultiplier;
    public float activeTime;

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

    public void activate(GameObject player)
    {
        Debug.Log("ACTIVATE POWERR UP : " + this.health);
        var p = player.GetComponent<Player>();
        p.hitPoints += this.health;
        //TODO - armor
        //TODO - other activations.

        if (this.weapondamagemultiplier > 0f)
        {
            p.weaponDamageMultiplier += this.weapondamagemultiplier-1f;//offset by 1 as 1 is at bottom.
            if (p.weaponDamageMultiplierResetTime > Time.time) p.weaponDamageMultiplierResetTime += this.activeTime;
            else p.weaponDamageMultiplierResetTime = Time.time + this.activeTime;
        }
        if (this.weaponspeedmultiplier > 0f)
        {
            p.weaponSpeedMultiplier += this.weaponspeedmultiplier-1f;
            if (p.weaponSpeedMultiplierResetTime > Time.time) p.weaponSpeedMultiplierResetTime += this.activeTime;
            else p.weaponSpeedMultiplierResetTime = Time.time + this.activeTime;
        }

        this._useCount--;
        if (this.useCount <= 0)
        {
            p.items[p.items.IndexOf(this.gameObject.GetComponent<IItem>())] = null;
            this.gameObject.SetActive(false);
        }
            
            
    }

    public void deactivate(GameObject player)
    {
        
    }
}
