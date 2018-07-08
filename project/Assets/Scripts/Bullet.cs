using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    public float speed;
    public float start;

	// Use this for initialization
	void Start () {
        start = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 add = new Vector3(
                Mathf.Sin(this.transform.rotation.eulerAngles.y * Mathf.PI / 180f+Mathf.PI/2),
                0f,
                Mathf.Cos(this.transform.rotation.eulerAngles.y*Mathf.PI/180f + Mathf.PI / 2)
            );
        
        this.transform.position += add * speed;
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
    }
}
