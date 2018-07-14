using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject win;
    public GameObject lose;

    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;

    public Color inactiveColor;
    public Color weaponColor;
    public Color weapon_selectedColor;
    public Color itemColor;

    private GameObject endscreen;

    // Use this for initialization
    void Start () {
        this.win.SetActive(false);
        this.lose.SetActive(false);
        endscreen = this.transform.Find("endscreen").gameObject;
        endscreen.SetActive(false);
   	}
	
	// Update is called once per frame
	void Update () {

		if(this.endscreen.activeSelf)
        {
            //restarts
            if(CrossPlatformInputManager.GetButton("p1Submit") ||
                CrossPlatformInputManager.GetButton("p2Submit")||
                CrossPlatformInputManager.GetButton("p3Submit")||
                CrossPlatformInputManager.GetButton("p4Submit"))
            {
                Time.timeScale = 1f;
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
            }
        }
        else
        {
            //Update player states
            p1.SetActive(false);
            p2.SetActive(false);
            p3.SetActive(false);
            p4.SetActive(false);

            foreach (GameObject playerGo in MainControl.activePlayers)
            {
                if (!playerGo.activeSelf || playerGo.GetComponent<Player>().hitPoints <= 0)
                {
                    var player = playerGo.GetComponent<Player>();
                    var item = player.playerId == 1 ? p1 : player.playerId == 2 ? p2 : player.playerId == 3 ? p3 : p4;
                    var ho = item.transform.Find("health");
                    ho.gameObject.SetActive(false);
                    ho.localScale = new Vector3(Mathf.Clamp(0f, 0f, 1f), ho.localScale.y, ho.localScale.z);
                }
                else
                {
                    var player = playerGo.GetComponent<Player>();
                    var item = player.playerId == 1 ? p1 : player.playerId == 2 ? p2 : player.playerId == 3 ? p3 : p4;
                    item.SetActive(true);
                    var ho = item.transform.Find("health");
                    ho.gameObject.SetActive(true);
                    ho.localScale = new Vector3(Mathf.Clamp(player.hitPoints / player.maxHealth, 0f, 1f), ho.localScale.y, ho.localScale.z);

                    updateItem(0, player, item);
                    updateItem(1, player, item);
                    updateItem(2, player, item);
                    updateItem(3, player, item);
                }

            }
        }
	}

    private void updateItem(int index, Player player, GameObject item)
    {
        var s = "item" + (index+1);
        var sbg = "item" + (index + 1) + "_bg";
        if (player.items[index] != null)
        {
            item.transform.Find(s).GetComponent<Image>().enabled = true;
            item.transform.Find(s).GetComponent<Image>().sprite = player.items[index].icon;
            item.transform.Find(sbg).GetComponent<Image>().color = (player.items[index].toggleable ?
                (player.items[index] == player.activeItem ? weapon_selectedColor : weaponColor) : itemColor) * player.playerColors[player.playerId-1];
        }
        else
        {
            item.transform.Find(sbg).GetComponent<Image>().color = inactiveColor*player.playerColors[player.playerId-1];
            item.transform.Find(s).GetComponent<Image>().enabled = false;
        }

    }

    public void showWin()
    {
        p1.SetActive(false);
        p2.SetActive(false);
        p3.SetActive(false);
        p4.SetActive(false);

        endscreen.SetActive(true);
        this.gameObject.SetActive(true);
    }

    public void showEnd()
    {
        endscreen.SetActive(true);
        this.gameObject.SetActive(true);
    }
}
