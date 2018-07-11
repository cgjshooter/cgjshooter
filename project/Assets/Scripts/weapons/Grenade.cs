using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour, IAmmunition{

    public float start;
    public float explosionDelay;
    private GameObject enemiesGameObject;
    private bool exploded;
    public AnimationCurve dampeningFunction;

    Vector3 _direction;
    public Vector3 direction
    {
        get {return _direction;}
        set{
            if (this.gameObject.name.IndexOf("Mine")>=0)
                _direction = new Vector3();
            else
                _direction = value;
        }
    }

    float _damage;
    public float damage
    {
        get{return _damage;}
        set{_damage = value;}
    }

    float _effectRadius;
    public float effectRadius
    {
        get{ return _effectRadius; }
        set{ _effectRadius = value; }
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

    private Transform explosionParticles;
    private Transform smokeParticles;

    // Use this for initialization
    void Start()
    {
        start = Time.time;
        this.explosionParticles = this.transform.Find("explosion");
        this.smokeParticles = this.transform.Find("smoke");
        this.enemiesGameObject = GameObject.Find("/enemyContainer");
    }

    private void OnEnable()
    {
        start = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        this.transform.position += direction*Time.deltaTime;
        if (Time.time - start > explosionDelay)
        {
            //TODO - explosion
            this.explode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == this.shooter || (other.transform.parent != null && other.transform.parent.gameObject == this.shooter))
            return;

        ITarget e = other.GetComponent<ITarget>();

        if (e != null)
        {
            explode();
        }
        else if (this.gameObject.name.IndexOf("Rocket")>=0)
            explode();
    }

    void explode()
    {
        if(enemiesGameObject == null) this.enemiesGameObject = GameObject.Find("/enemyContainer");
        exploded = true;
        //Find out enemies in the range

        this.direction = new Vector3();
        this.explosionParticles.gameObject.GetComponent<ParticleSystem>().Play();
        if(this.smokeParticles != null)
            this.smokeParticles.gameObject.GetComponent<ParticleSystem>().Stop();
        for (int i = 0; i < enemiesGameObject.transform.childCount; i++)
        {
            var co = enemiesGameObject.transform.GetChild(i);
            if (co.tag == "spawner" && this.tag == "Enemy")
                continue; 
            var d = this.transform.position - co.transform.position;
            if(Vector3.Magnitude(d) < this.effectRadius)
            {
                co.GetComponent<ITarget>().hit(this);
            }
        }
        foreach(GameObject co in MainControl.activePlayers)
        {
            var d = this.transform.position - co.transform.position;
            if (Vector3.Magnitude(d) < this.effectRadius)
            {
                co.GetComponent<ITarget>().hit(this);
            }
        }

        Invoke("clearObject", 1);
    }

    void clearObject()
    {
        this.gameObject.SetActive(false);
    }

    public void affect(GameObject target)
    {
        var e = target.GetComponent<ITarget>();
        if (e != null && target != this.shooter)
        {
            var d = this.transform.position - target.transform.position;
            e.hitPoints -= dampeningFunction.Evaluate(Mathf.Clamp(Vector3.Magnitude(d) / this.effectRadius, 0f, 1f))* this.damage;
        }
    }
}
