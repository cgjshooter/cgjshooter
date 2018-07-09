using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public LayerMask mouseHitMask;
    public GameObject bulletPrefab;
    public float shootDelay;
    public float moveSpeed;
    public float bulletShootSpeed;
    private Vector3 m_Move;

    //TODO - get player id from registration order.
    //p1 - keybaord / steam controller as it maps to kb/mouse without steam overlay
    //p2 - joystick 1
    //p3 - joystick 2
    //p4 - joystick 3
    public String playerId;

    private float lastSpawn;
    private GameObject bulletContainer;
       
    private void Start()
    {
        bulletContainer = GameObject.Find("bulletContainer");
        Debug.Log(bulletContainer);
    }


    private void Update()
    {
        if(Time.realtimeSinceStartup - lastSpawn > shootDelay && 
            CrossPlatformInputManager.GetAxis("p" + playerId + "Fire1")!=0 && 
            bulletContainer != null)
        {
            //Spawn a bullet
            var go = GameObject.Instantiate(bulletPrefab, this.transform.position,
                this.transform.rotation
                ,this.bulletContainer.transform);
            Vector3 add = new Vector3(
                Mathf.Sin(this.transform.rotation.eulerAngles.y * Mathf.PI / 180f + Mathf.PI / 2),
                0f,
                Mathf.Cos(this.transform.rotation.eulerAngles.y * Mathf.PI / 180f + Mathf.PI / 2)
            );
            go.GetComponent<Bullet>().direction = add*0.2f + m_Move;
            lastSpawn = Time.realtimeSinceStartup;
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
