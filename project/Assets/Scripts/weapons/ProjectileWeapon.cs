using System;
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

    public Sprite _icon;
    public Sprite icon
    {
        get
        {
            return this._icon;
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
    public GameObject bulletEnemyPrefab;

    private float previousActivation=0f;
    private GameObject bulletContainer;
    
    private static Dictionary<string, List<GameObject>> bulletPool;
    private static Dictionary<string, int> bulletIndices;
    private static Dictionary<string, List<GameObject>> bulletPoolEnemy;
    private static Dictionary<string, int> bulletIndicesEnemy;

    private static int bulletPoolSize = 800;

    // Use this for initialization
    void Start () {
        this.bulletContainer = GameObject.Find("bulletContainer");

        if (bulletPool == null)
        {
            DontDestroyOnLoad(this.bulletContainer.gameObject);
            bulletPool = new Dictionary<string, List<GameObject>>();
            bulletIndices = new Dictionary<string, int>();
            bulletPoolEnemy = new Dictionary<string, List<GameObject>>();
            bulletIndicesEnemy = new Dictionary<string, int>();
        }
        if (!bulletPool.ContainsKey(this.bulletPrefab.name))
        {
            //Instantiate bullets.
            List<GameObject> bullets = new List<GameObject>();
            List<GameObject> bulletsE = new List<GameObject>();

            for ( int i = 0; i < bulletPoolSize; i++)
            {
                GameObject go = GameObject.Instantiate(bulletPrefab,bulletContainer.transform);
                bullets.Add(go);
                go.SetActive(false);
                GameObject goE = GameObject.Instantiate(bulletEnemyPrefab, bulletContainer.transform);
                bulletsE.Add(goE);
                goE.SetActive(false);
            }

            bulletPool.Add(this.bulletPrefab.name, bullets);
            bulletIndices.Add(this.bulletPrefab.name, 0);
            bulletPoolEnemy.Add(this.bulletEnemyPrefab.name, bulletsE);
            bulletIndicesEnemy.Add(this.bulletEnemyPrefab.name, 0);
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void activate(GameObject player)
    {
        var pl = player.GetComponent<Player>();
        float divider = pl == null ? 1 : pl.weaponSpeedMultiplier;
        if (Time.time - this.previousActivation > this.firedelay/divider)
        {
            //Allow fire
            for(int i = 0; i < bulletAmount; i++)
            {
                //Spawn a bullet
                var go = player.tag == "Enemy"?  bulletPoolEnemy[this.bulletEnemyPrefab.name][(bulletIndicesEnemy[bulletEnemyPrefab.name]++)%bulletPoolSize] :
                    bulletPool[this.bulletPrefab.name][(bulletIndices[bulletPrefab.name]++) % bulletPoolSize];
                go.transform.parent = bulletContainer.transform;
                go.transform.rotation = player.transform.rotation;
                go.transform.position = player.transform.Find("shootPoint").position;
                /*GameObject.Instantiate(bulletPrefab, player.transform.Find("shootPoint").position,
                player.transform.rotation
                , this.bulletContainer.transform);*/
                go.SetActive(true);
                float angle = this.transform.rotation.eulerAngles.y + (UnityEngine.Random.value - 0.5f) * (spread);
                Vector3 add = new Vector3(
                    Mathf.Sin(angle * Mathf.PI / 180f),
                    Mathf.Sin(heightAngle * 180f/Mathf.PI),
                    Mathf.Cos(angle * Mathf.PI / 180f )
                );
                add.Normalize();

                float addSpeed = 1f;
                float addDamage = 1f;
                //TODO - this might be moved to interface at some point, but for now. Lets keep it at player.
                if(pl != null)
                {
                 //   addSpeed = pl.weaponSpeedMultiplier;
                    addDamage = pl.weaponDamageMultiplier;
                }
                else
                {
                    //Enemies shoot halved speed.s
                    addSpeed = 0.5f*GameConfig.difficulty;
                    addDamage *= GameConfig.difficulty;
                }
           //     Debug.Log("Shoot angle: " + add);
                go.GetComponent<IAmmunition>().shooter = player;
                go.GetComponent<IAmmunition>().direction = add * (bulletspeed * addSpeed + UnityEngine.Random.value * bulletSpeedRandomFactor * addSpeed) 
                                                           +0f* player.GetComponent<ITarget>().m_Move;
            //    Debug.Log("final shoot: " + go.GetComponent<IAmmunition>().direction);
                go.GetComponent<IAmmunition>().damage = this.bulletDamage*addDamage;
                go.GetComponent<IAmmunition>().effectRadius = this.effectRadius;
                
            }
            this.previousActivation = Time.time;
        }
    }

    public void deactivate(GameObject player)
    {

    }
}
