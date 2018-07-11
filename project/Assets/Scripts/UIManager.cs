using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class UIManager : MonoBehaviour {

    public GameObject win;
    public GameObject lose;

    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;


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
        else
        {
            //Update player states
            foreach (GameObject playerGo in MainControl.activePlayers)
            {
                var player = playerGo.GetComponent<Player>();
                var item = player.playerId == 1 ? p1 : player.playerId == 2 ? p2 : player.playerId == 3 ? p3 : p4;

                var ho = item.transform.Find("health");
                ho.localScale = new Vector3(Mathf.Clamp(player.hitPoints/player.maxHealth,0f,1f), ho.localScale.y, ho.localScale.z);
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
