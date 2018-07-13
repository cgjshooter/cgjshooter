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
        foreach (SpriteRenderer si in this.GetComponentsInChildren<SpriteRenderer>()) si.enabled = true;
        foreach (MeshRenderer mr in this.GetComponentsInChildren<MeshRenderer>()) mr.enabled = true;

    }
    // Update is called once per frame
    void Update()
    {
        this.transform.position += direction*Time.deltaTime;
        if (Time.time - start > explosionDelay && !exploded)
        {
            this.explode();
        }
    }

    private GameObject curOther;
    private void OnTriggerEnter(Collider other)
    {
        //Block self
        if (this.shooter != null && 
            (other.gameObject == this.shooter || (other.transform.parent != null && other.transform.parent.gameObject == this.shooter)))
        {
            Debug.Log("Block by shooter.");
            return;
        }
        curOther = other.gameObject;
        ITarget e = other.GetComponent<ITarget>();
        if (this.gameObject.name.IndexOf("Mine") >= 0)
            Debug.Log("trigge enter: "+ (e == null));
        if (e != null)
        {
            Debug.Log(other.tag + " vs " + this.shooter.tag);
            if (this.shooter != null && 
                !(other.tag == "spawner" && this.shooter.tag == "Enemy" || other.tag == "Enemy" && this.shooter.tag == "Enemy"))
                explode();
        }
        else if (this.gameObject.name.IndexOf("Rocket")>=0)
        {
            Debug.Log("not itarget");
            explode();
        }
    }

    void explode()
    {
        if(enemiesGameObject == null) this.enemiesGameObject = GameObject.Find("/enemyContainer");
        if (exploded) return;
        Debug.Log("Explode");
        exploded = true;
        //Find out enemies in the range
        this.direction = new Vector3();
        this.explosionParticles.gameObject.GetComponent<ParticleSystem>().Play();
        if(this.smokeParticles != null)
            this.smokeParticles.gameObject.GetComponent<ParticleSystem>().Stop();
        if(enemiesGameObject != null)
        {
            for (int i = 0; i < enemiesGameObject.transform.childCount; i++)
            {
                var co = enemiesGameObject.transform.GetChild(i);
                if (this.shooter != null && (co.tag == "spawner" && this.shooter.tag == "Enemy" || co.tag == "Enemy" && this.shooter.tag == "Enemy"))
                {
                    Debug.Log("skip as is enemy - spawner");
                    continue; 
                }
                var d = this.transform.position - co.transform.position;
                Debug.Log("Distance: " + d + " is other: " + (curOther == co));
                if(Vector3.Magnitude(d) < this.effectRadius || curOther == co)
                {
                    co.GetComponent<ITarget>().hit(this);
                }
            }
            foreach (GameObject co in MainControl.activePlayers)
            {
                var d = this.transform.position - co.transform.position;
                if (Vector3.Magnitude(d) < this.effectRadius)
                {
                    co.GetComponent<ITarget>().hit(this);
                }
            }

            foreach (SpriteRenderer si in this.GetComponentsInChildren<SpriteRenderer>()) si.enabled = false;
            foreach (MeshRenderer mr in this.GetComponentsInChildren<MeshRenderer>()) mr.enabled = false;
        }

        Invoke("clearObject", 1);
    }

    void clearObject()
    {
        exploded = false;
        this.gameObject.SetActive(false);
    }

    public void affect(GameObject target)
    {
        var e = target.GetComponent<ITarget>();
        if (e != null && target != this.shooter && !e.invulnerable)
        {
            var d = this.transform.position - target.transform.position;
            float damage = dampeningFunction.Evaluate(Mathf.Clamp(Vector3.Magnitude(d) / this.effectRadius, 0f, 1f)) * this.damage;
            if (curOther == target)
            {
                damage = this.damage;
                Debug.Log("Use this damage");
            }
            Debug.Log("damage: " + damage);    
            
            float blocked = damage * Mathf.Clamp((e.armor), 0.1f, 1f);
            float before = e.hitPoints;
            float rawDamage = damage - blocked;
            e.hitPoints -= rawDamage;

            StatisticManager.calculateDamageStatistics(this, target, damage, rawDamage);

            //Weaken armor by blocked amount. Divider is just some weakening value that needs to be tweaked.
            e.armor -= blocked / 15f;
            if (e.armor < 0) e.armor = 0f;

            if (e.dead && before > 0)
            {
                // Target just died
                StatisticManager.calculateKillStatistics(this, target);
            }
        }
    }
}
