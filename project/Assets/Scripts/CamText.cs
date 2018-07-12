﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamText : MonoBehaviour {

    private TextMesh tm;
    private float start;
    private int startC;

	// Use this for initialization
	void Start () {
        this.tm = this.GetComponent<TextMesh>();
        this.tm.text = "";
        showCounter();
	}
	
	// Update is called once per frame
	void Update () {
		if(Time.time - start > 1f)
        {
            //Advance
            this.startC--;
            if(this.startC < 0)
            {
                this.tm.text = "";
                this.transform.localScale = new Vector3(1, 1, 1)*0.25f;
            }
            else
            {
                if(this.startC == 0)
                {
                    this.tm.text = "BEGIN!";
                    this.transform.localScale = new Vector3(1, 1, 1) * 0.25f;
                }
                else
                {
                    this.transform.localScale = new Vector3(1, 1, 1) * 0.25f;
                    this.tm.text =  this.startC.ToString();
                }
                start = Time.time;
            }
        }
        else if (startC >= 0)
        {
            this.transform.localScale = new Vector3(1, 1, 1) *  (1.0f-(Time.time - start) * 0.25f) * 0.25f;
        }

    }

    public void showWellDone()
    {
        this.tm.text = "Stage complete!";
        start = Time.time+2f;
        //Invoke("HideText", 3.5f);
    }

    public void levelComplete()
    {
        this.tm.text = "Level finished!";
        start = Time.time + 2f;
    }

    public void hideText()
    {

    }

    public void showCounter()
    {
        this.startC = 3;
        start = Time.time;
        this.tm.text = "3";
    }
}
