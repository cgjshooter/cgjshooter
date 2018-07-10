using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, IAmmunition {

    public float start;

    Vector3 _direction;
    public Vector3 direction
    {
        get { return _direction; }
        set { _direction = value; }
    }

    float _damage;
    public float damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    float _effectRadius;
    public float effectRadius
    {
        get { return _effectRadius; }
        set { _effectRadius = value; }
    }

    // Use this for initialization
    void Start () {
        start = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        
        this.transform.position += direction;
        if (Time.time - start > 10f) GameObject.Destroy(this.gameObject);
	}

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collider enter");
        Enemy e = other.GetComponent<Enemy>();
        if (e!=null)
        {
            e.hit(this);
            GameObject.Destroy(this.gameObject);
        }
        else
        {

            /*
            Obstacle o = other.GetComponent<Obstacle>();
            if(o != null)
            {
                o.hit(this.damage);*/
                GameObject.Destroy(this.gameObject);
            //}
        }
    }

    public void affect(GameObject target)
    {
        var e = target.GetComponent<Enemy>();
        if(e!= null)
            e.hitPoints -= this.damage;
    }
}
