using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grenade : MonoBehaviour, IProjectile{

    public float start;
    public float explosionDelay;
    private GameObject enemiesGameObject;

    public AnimationCurve dampeningFunction;

    Vector3 _direction;
    public Vector3 direction
    {
        get {return _direction;}
        set{_direction = value;}
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

    private GameObject explosionParticles;

    // Use this for initialization
    void Start()
    {
        start = Time.time;
        this.explosionParticles = this.transform.Find("explosion").gameObject;
        this.enemiesGameObject = GameObject.Find("/enemyContainer");
    }
    
    // Update is called once per frame
    void Update()
    {
        this.transform.position += direction;
        if (Time.time - start > explosionDelay)
        {
            //TODO - explosion
            this.explode();
        }
    }

    void explode()
    {
        //Find out enemies in the range

        this.explosionParticles.GetComponent<ParticleSystem>().Play();

        for(int i = 0; i < enemiesGameObject.transform.childCount; i++)
        {
            var co = enemiesGameObject.transform.GetChild(i);
            var d = this.transform.position - co.transform.position;
            if(Vector3.Magnitude(d) < this.effectRadius)
            {
                co.GetComponent<Enemy>().hit(this);
            }
        }

        Invoke("clearObject", 1);
    }

    void clearObject()
    {
        GameObject.Destroy(this.gameObject);
    }

    public void affect(GameObject target)
    {
        var e = target.GetComponent<Enemy>();
        if (e != null)
        {
            var d = this.transform.position - target.transform.position;
            e.hitPoints -= dampeningFunction.Evaluate(Mathf.Clamp(Vector3.Magnitude(d) / this.effectRadius, 0f, 1f))* this.damage;
        }
    }
}
