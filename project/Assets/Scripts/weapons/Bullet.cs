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
        if (this.shooter != null && this.shooter.tag == "Enemy" && other.tag == "spawner") return;

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
        if (!GameConfig.friendlyFire && this.shooter != null && this.shooter.tag == "Player" && target.tag == "Player")
            return;
        if(e!= null && target != this.shooter && !e.invulnerable)
        {
            float blocked = this.damage * Mathf.Clamp((e.armor), 0.1f, 1f);
            float before = e.hitPoints;
            e.hitPoints -= this.damage - blocked;
            //Weaken armor by blocked amount. Divider is just some weakening value that needs to be tweaked.
            e.armor -= blocked / 30f;
            if (e.armor < 0) e.armor = 0f;

            if (e.dead && before > 0)
            {
                // Target just dyed

                // who is the killer?
                StatisticManager.PlayerStatistics p = null;
                if (this.shooter != null && this.shooter.tag == "Player")
                {
                    p = StatisticManager.playerStatistics[this.shooter.GetComponent<Player>().playerId];
                }

                // Target killed ++
                switch (target.tag)
                {
                    case "Enemy":
                        StatisticManager.gameStatistics.totalEnemyKills++;
                        break;
                    case "spawner":
                        if (p != null) p.spawnerKills++;
                        StatisticManager.gameStatistics.totalSpawnerKills++;
                        break;
                    case "Player":
                        if (p != null) p.playerKills++;
                        StatisticManager.gameStatistics.totalPlayerKills++;
                        break;
                }

            }
        }
    }
}
