using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class UIManager : MonoBehaviour {

    public GameObject win;
    public GameObject lose;

	// Use this for initialization
	void Start () {
        this.win.SetActive(false);
        this.lose.SetActive(false);
        
	}
	
	// Update is called once per frame
	void Update () {

		if(this.win.activeSelf || lose.activeSelf)
        {
            Debug.Log("wait input");
            //restarts
            if(CrossPlatformInputManager.GetButton("Submit"))
            {
                Debug.Log("submit");
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }
	}

    public void showWin()
    {
        this.gameObject.SetActive(true);
        this.win.SetActive(true);
    }

    public void showEnd()
    {
        this.gameObject.SetActive(true);
        this.lose.SetActive(true);
    }
}
