using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;

public class Player : MonoBehaviour, ITarget
{
    public List<GameObject> itemPrefabs;
    public IItem activeItem;
    public List<IItem> items;

    public LayerMask mouseHitMask;
    public float moveSpeed;
	public float _hitPoints;
    
    //TODO - get player id from registration order.
    //p1 - keybaord / steam controller as it maps to kb/mouse without steam overlay
    //p2 - joystick 1
    //p3 - joystick 2
    //p4 - joystick 3
    public String playerId;

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

    public bool dead
    {
        get
        {
            return this.hitPoints <= 0;
        }
    }

    private void Start()
    {
        //TODO - populate from players weapon selection.
        this.items = new List<IItem>();
        //Just for testing, instantiate the first value and set it as active item.
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            GameObject go = Instantiate(itemPrefabs[i], this.transform);
            this.items.Add(go.GetComponent<IItem>());
        }
        ListUtil.Shuffle<IItem>(this.items);
        
        this.activeItem = this.items[0];
    }

    private void Update()
    {
        if (dead) return;
		if(hitPoints < 0)
		{
			//DIE
			this.die();
		}

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
                    //TODO - Apply usable item
                }

            }
        }

        
    }

	private void die()
	{
        
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
        if (dead) return;

        // read inputs
        float h = CrossPlatformInputManager.GetAxis("p" + playerId + "Horizontal");
        float v = CrossPlatformInputManager.GetAxis("p" + playerId + "Vertical");

        if (this.playerId == "1")//Use mouse to look
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
