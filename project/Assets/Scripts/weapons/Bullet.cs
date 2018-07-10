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

    private GameObject _shooter;
    public GameObject shooter
    {
        get
        {
            return _shooter;
        }

        set
        {
            _shooter = value;
        }
    }

    // Use this for initialization
    void Start () {
        start = Time.time;
	}

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        start = Time.time;
    }

    // Update is called once per frame
    void Update () {
        
        this.transform.position += direction*Time.deltaTime;
        if (Time.time - start > 10f) this.gameObject.SetActive(false);
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == this.shooter || (other.transform.parent != null && other.transform.parent.gameObject == this.shooter))
            return;

        Debug.Log("Collider enter");
        ITarget e = other.GetComponent<ITarget>();
        
        if (e!=null)
        {
            e.hit(this);
            this.gameObject.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    public void affect(GameObject target)
    {
        var e = target.GetComponent<ITarget>();
        if(e!= null)
            e.hitPoints -= this.damage;
    }
}
