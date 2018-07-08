using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;

public class Player : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float shootDelay;
    private Vector3 m_Move;


    private float lastSpawn;
    private GameObject bulletContainer;
       
    private void Start()
    {
        bulletContainer = GameObject.Find("bulletContainer");
        Debug.Log(bulletContainer);
    }


    private void Update()
    {
        if(Time.realtimeSinceStartup - lastSpawn > shootDelay && CrossPlatformInputManager.GetAxis("Fire1")!=0 && bulletContainer != null)
        {
            //Spawn a bullet
            var go = GameObject.Instantiate(bulletPrefab, this.transform.position,
                this.transform.rotation
                ,this.bulletContainer.transform);
            lastSpawn = Time.realtimeSinceStartup;
        }
    }


    // Fixed update is called in sync with physics
    //TODO - update this to use the position from main game logic which runs based on ticks.
    private void FixedUpdate()
    {
        // read inputs
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");

        float hR = CrossPlatformInputManager.GetAxis("HorizontalR");
        float vR = CrossPlatformInputManager.GetAxis("VerticalR");

        if(Mathf.Abs(hR +vR) > 0)
        {
            this.transform.rotation = Quaternion.Euler(0,
                Mathf.Atan2(vR, hR) * 180f / Mathf.PI
                , 0);
        }
        
        // we use world-relative directions in the case of no main camera
        m_Move = v * Vector3.forward + h * Vector3.right;

        this.transform.position += m_Move*0.1f;
    }
}
