﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileWeapon : MonoBehaviour, IItem {
    public bool toggleable
    {
        get
        {
            return true;
        }
    }

    public int useCount
    {
        get
        {
            return -1;
        }
    }

    public bool useOnPickup
    {
        get
        {
            return false;
        }
    }

    public float power
    {
        get
        {
            return bulletDamage;
        }
    }

    public float spread;
    public float bulletSpeedRandomFactor;
    public float firedelay;
    public float bulletspeed;
    public float bulletAmount;
    public float bulletDamage;
    public float heightAngle;
    public float effectRadius;

    public GameObject bulletPrefab;

    private float previousActivation=0f;
    private GameObject bulletContainer;
    
    // Use this for initialization
    void Start () {
        this.bulletContainer = GameObject.Find("bulletContainer");
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void activate(GameObject player)
    {
        if (Time.time - this.previousActivation > this.firedelay)
        {
            //Allow fire
            for(int i = 0; i < bulletAmount; i++)
            {
                //Spawn a bullet
                var go = GameObject.Instantiate(bulletPrefab, player.transform.Find("shootPoint").position,
                    player.transform.rotation
                    , this.bulletContainer.transform);
                
                float angle = this.transform.rotation.eulerAngles.y + (UnityEngine.Random.value - 0.5f) * spread;
                Vector3 add = new Vector3(
                    Mathf.Sin(angle * Mathf.PI / 180f),
                    Mathf.Sin(heightAngle * 180f/Mathf.PI),
                    Mathf.Cos(angle * Mathf.PI / 180f )
                );
                add.Normalize();
                go.GetComponent<IAmmunition>().shooter = player;
                go.GetComponent<IAmmunition>().direction = add * (bulletspeed + UnityEngine.Random.value * bulletSpeedRandomFactor) 
                                                           + player.GetComponent<ITarget>().m_Move;
                go.GetComponent<IAmmunition>().damage = this.bulletDamage;
                go.GetComponent<IAmmunition>().effectRadius = this.effectRadius;
                
            }
            this.previousActivation = Time.time;
        }
    }

    public void deactivate(GameObject player)
    {

    }
}
