using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;

public class Player : MonoBehaviour, ITarget
{
    public float weaponSpeedMultiplier = 1f;
    public float weaponDamageMultiplier = 1f;
    public float weaponSpeedMultiplierResetTime;
    public float weaponDamageMultiplierResetTime;


    public List<GameObject> itemPrefabs;
    public IItem activeItem;
    public List<IItem> items;

    public LayerMask mouseHitMask;
    public float moveSpeed;
	public float _hitPoints;
    public GameObject death;

    public List<Sprite> glowImages;
    public List<Color> playerColors;
    
    //TODO - get player id from registration order.
    //p1 - keybaord / steam controller as it maps to kb/mouse without steam overlay
    //p2 - joystick 1
    //p3 - joystick 2
    //p4 - joystick 3
    public int playerId;

    private float lastSpawn;
    private bool falling;

    public Vector3 move;
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

    public float _maxHealth;
    public float maxHealth
    {
        get
        {
            return _maxHealth;
        }        
    }
    public bool dead
    {
        get
        {
            return this.hitPoints <= 0;
        }
    }

    public float _armor=0f;
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

    private bool _invulnerable;
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

    private bool _invisible;
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

    private void Start()
    {
        if (this.hitPoints > _maxHealth) _maxHealth = this.hitPoints;
        
        //Testing code if no player selection has been made.
             this.items = new List<IItem>();
        if(PlayerSelect.selections != null && PlayerSelect.selections.Count > 0)
        {
            //Populate real weapons
            GameObject go = Instantiate(itemPrefabs[PlayerSelect.selections[playerId-1].weaponIndex], this.transform);
            this.items.Add(go.GetComponent<IItem>());
        }
        else
        {
            GameObject go = Instantiate(itemPrefabs[(int)Mathf.Floor(UnityEngine.Random.value* itemPrefabs.Count)], this.transform);
            this.items.Add(go.GetComponent<IItem>());
            go = Instantiate(itemPrefabs[(int)Mathf.Floor(UnityEngine.Random.value * itemPrefabs.Count)], this.transform);
            this.items.Add(go.GetComponent<IItem>());
            go = Instantiate(itemPrefabs[(int)Mathf.Floor(UnityEngine.Random.value * itemPrefabs.Count)], this.transform);
            this.items.Add(go.GetComponent<IItem>());
        }
        this.activeItem = this.items[0];
        while (items.Count < 4) items.Add(null);


        foreach (SpriteRenderer si in this.GetComponentsInChildren<SpriteRenderer>()) si.sprite = glowImages[this.playerId-1];
        this.GetComponentInChildren<Light>().color = playerColors[this.playerId-1];
        foreach(MeshRenderer mr in this.GetComponentsInChildren<MeshRenderer>()) mr.material.SetColor("_Color", playerColors[this.playerId-1]);

    }

    private void Update()
    {
        if(dead)
		{
			//DIE
			this.die();
            return;
		}

        //Reset powerup modifiers
        if (Time.time > this.weaponDamageMultiplierResetTime) this.weaponDamageMultiplier = 1f;
        if (Time.time > this.weaponSpeedMultiplierResetTime) this.weaponSpeedMultiplier = 1f;

        if( CrossPlatformInputManager.GetAxis("p" + playerId + "Fire1")!=0 && activeItem != null)
        {
            this.activeItem.activate(this.gameObject);
        }

        //Check for item swaps
        bool i1 = CrossPlatformInputManager.GetButtonDown("p" + playerId + "SelectItem1");
        bool i2 = CrossPlatformInputManager.GetButtonDown("p" + playerId + "SelectItem2");
        bool i3 = CrossPlatformInputManager.GetButtonDown("p" + playerId + "SelectItem3");
        bool i4 = CrossPlatformInputManager.GetButtonDown("p" + playerId + "SelectItem4");

        var itemInd = i1 ? 0 : i2 ? 1 : i3 ? 2 : i4 ? 3 : -1;
        if(itemInd >= 0)
        {
            //Select item and activate.
            if(itemInd >= 0 && itemInd < this.items.Count)
            {
                var nextItem = this.items[itemInd];
                if(nextItem != null)
                {
                    if(nextItem.toggleable)
                    {
                        if(this.activeItem != null)
                        {
                            this.activeItem.deactivate(this.gameObject);
                        }
                        activeItem = this.items[itemInd];
                    }
                    else
                    {
                        nextItem.activate(this.gameObject);
                    }
                }

            }
        }
    }

    /**
     * Called when player is near powerup for picking.
     * TODO - check if powerup can be picked / should be.
     * return true if ok, false otherwise.
     **/
    public bool pickPowerup(GameObject powerup)
    {
        //Immediate activation
        if (powerup.GetComponent<IItem>().useOnPickup)
        {
            powerup.GetComponent<IItem>().activate(this.gameObject);
            return true;
        }
        else
        {
            //Check if player has room for new item.
            for(int i = 0; i < 4; i++)
            {
                if (this.items[i] == null)
                {
                    this.items[i] = powerup.GetComponent<IItem>();
                    return true;
                }
            }
        }
        return false;
    }

	private void die()
	{
        if ( !death.activeSelf)
        {
            Debug.Log("DIE");
            death.SetActive(true);
            foreach (MeshRenderer me in this.GetComponentsInChildren<MeshRenderer>()) me.enabled = false;
            foreach (Collider co in this.GetComponentsInChildren<Collider>()) co.enabled = false;
            foreach (SpriteRenderer sr in this.GetComponentsInChildren<SpriteRenderer>()) sr.enabled = false;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.name == "fallTrigger" && !falling)
        {
            //Trigger fall
            falling = true;
            this.transform.Find("Sounds").GetComponent<AudioSource>().Play();
        }
        else if(other.name == "exitTrigger")
        {
            //Game over
            this.hitPoints = 0;
        }
    }

    // Fixed update is called in sync with physics
    //TODO - update this to use the position from main game logic which runs based on ticks.
    private void FixedUpdate()
    {
        if (dead)
        {
            return;
        }

        // read inputs
        float h = CrossPlatformInputManager.GetAxis("p" + playerId + "Horizontal");
        float v = CrossPlatformInputManager.GetAxis("p" + playerId + "Vertical");

        if (this.playerId == 1)//Use mouse to look
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //make sure you have a camera in the scene tagged as 'MainCamera'
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000f, mouseHitMask.value, QueryTriggerInteraction.Ignore))
            {
                var dx = hit.point.x - this.transform.position.x;
                var dz = hit.point.z - this.transform.position.z;
                var angle = Mathf.Atan2(dx, dz);
                this.transform.rotation = Quaternion.Euler(0,
                        angle * 180f / Mathf.PI
                        , 0);
            }
        }
        else //Use controller axis.
        {
            float hR = CrossPlatformInputManager.GetAxis("p" + playerId + "HorizontalR");
            float vR = CrossPlatformInputManager.GetAxis("p" + playerId + "VerticalR");

            if (Mathf.Abs(hR + vR) > 0)
            {
                this.transform.rotation = Quaternion.Euler(0,
                    Mathf.Atan2(vR, hR) * 180f / Mathf.PI + 90
                    , 0);
            }
        }

        //calculate move.
        move = (v * Vector3.forward + h * Vector3.right);
        if (move.magnitude > 1) move.Normalize();
        move *= moveSpeed;

        this.transform.position += move*Time.fixedDeltaTime;
    }

    public void hit(IAmmunition ammunition)
    {
        ammunition.affect(this.gameObject);
    }
}
