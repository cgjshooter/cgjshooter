using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public List<GameObject> itemPrefabs;
    public IItem activeItem;
    public List<IItem> items;

    public LayerMask mouseHitMask;
    public float moveSpeed;
    public Vector3 m_Move;

    //TODO - get player id from registration order.
    //p1 - keybaord / steam controller as it maps to kb/mouse without steam overlay
    //p2 - joystick 1
    //p3 - joystick 2
    //p4 - joystick 3
    public String playerId;

    private float lastSpawn;
   
    private void Start()
    {
        //TODO - populate from players weapon selection.
        this.items = new List<IItem>();
        //Just for testing, instantiate the first value and set it as active item.
        var activeGO = Instantiate(itemPrefabs[0], this.transform);
        this.activeItem = activeGO.GetComponent<IItem>();
    }

    private void Update()
    {
        if( CrossPlatformInputManager.GetAxis("p" + playerId + "Fire1")!=0 && activeItem != null)
        {
            this.activeItem.activate(this.gameObject);
        }
    }

    // Fixed update is called in sync with physics
    //TODO - update this to use the position from main game logic which runs based on ticks.
    private void FixedUpdate()
    {
        // read inputs
        float h = CrossPlatformInputManager.GetAxis("p"+playerId+"Horizontal");
        float v = CrossPlatformInputManager.GetAxis("p"+playerId+"Vertical");

        if(this.playerId == "1")//Use mouse to look
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //make sure you have a camera in the scene tagged as 'MainCamera'
            RaycastHit hit;

            if (Physics.Raycast(ray,out hit, 1000f, mouseHitMask.value,QueryTriggerInteraction.Ignore))
            {
                var dx = hit.point.x- this.transform.position.x;
                var dz = hit.point.z- this.transform.position.z;
                var angle = Mathf.Atan2(dx, dz);
                this.transform.rotation = Quaternion.Euler(0,
                        angle * 180f / Mathf.PI - 90
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
                    Mathf.Atan2(vR, hR) * 180f / Mathf.PI
                    , 0);
            }
        }

        //calculate move.
        m_Move = (v * Vector3.forward + h * Vector3.right)*moveSpeed;

        this.transform.position += m_Move;
    }
}
