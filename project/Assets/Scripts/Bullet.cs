using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public Vector3 direction;
    public float start;

	// Use this for initialization
	void Start () {
        start = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        
        this.transform.position += direction;
        if (Time.time - start > 10f) GameObject.Destroy(this.gameObject);
	}

    private void OnTriggerEnter(Collider other)
    {
        Enemy e = other.GetComponent<Enemy>();
        if (e!=null)
        {
            e.hit();
            GameObject.Destroy(this.gameObject);
        }
        else
        {
            Obstacle o = other.GetComponent<Obstacle>();
            if(o != null)
            {
                o.hit();
                GameObject.Destroy(this.gameObject);
            }
        }

    }
}
