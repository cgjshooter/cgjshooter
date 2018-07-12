using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeOut : MonoBehaviour {

    public float fadeTime;

    private float startTime;
    private Color basecolor;
	// Use this for initialization
	void Start () {
        if (fadeTime == 0) fadeTime = 1f;
        startTime = Time.time;
        basecolor = this.transform.GetComponent<Image>().color;
	}
	
	// Update is called once per frame
	void Update () {
        if(Time.time-startTime > fadeTime)
        {
            this.gameObject.SetActive(false);
        }
        basecolor.a = 1f-(Time.time - startTime) / fadeTime;
        this.transform.GetComponent<Image>().color = basecolor;
	}
}
