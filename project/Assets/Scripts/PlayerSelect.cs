using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerSelect : MonoBehaviour {

    public List<GameObject> weapons;

    public GameObject p1;
    public GameObject p2;
    public GameObject p3;
    public GameObject p4;

    public static List<PlayerSelections> selections;

    // Use this for initialization
    void Start () {
        selections = new List<PlayerSelections>();
        selections.Add(new PlayerSelections());
        selections.Add(new PlayerSelections());
        selections.Add(new PlayerSelections());
        selections.Add(new PlayerSelections());
        selections[0].weaponIndex = (int)Mathf.Floor(UnityEngine.Random.value * weapons.Count);
        selections[1].weaponIndex = (int)Mathf.Floor(UnityEngine.Random.value * weapons.Count);
        selections[2].weaponIndex = (int)Mathf.Floor(UnityEngine.Random.value * weapons.Count);
        selections[3].weaponIndex = (int)Mathf.Floor(UnityEngine.Random.value * weapons.Count);
        updateIcon(1, p1);
        updateIcon(2, p2);
        updateIcon(3, p3);
        updateIcon(4, p4);
    }

    // Update is called once per frame
    void Update () {
        handlePlayer(1,p1);
        handlePlayer(2,p2);
        handlePlayer(3,p3);
        handlePlayer(4,p4);
    }

    void updateIcon(int id, GameObject p)
    {
        var ps = selections[id - 1];
        ps.weaponIndex = (ps.weaponIndex - 1 + weapons.Count) % weapons.Count;
        var item = weapons[ps.weaponIndex];
        p.transform.Find("weapons/Image").GetComponent<Image>().sprite = item.GetComponent<IItem>().icon;
    }

    void handlePlayer(int id, GameObject p)
    {
        float hv = CrossPlatformInputManager.GetAxisRaw("p" + id + "Horizontal");
        var ps = selections[id - 1];
        if (hv < -0.3 && ps.previousDirection == 0 )
        {
            //Swap left
            ps.weaponIndex = (ps.weaponIndex-1+weapons.Count)%weapons.Count;
            var item = weapons[ps.weaponIndex];
            p.transform.Find("weapons/Image").GetComponent<Image>().sprite = item.GetComponent<IItem>().icon;
            ps.previousDirection = -1;
            ps.previousSwap = Time.time;
        }
        else if(hv > 0.3 && ps.previousDirection== 0 )
        {
            //Swap right
            ps.weaponIndex = (ps.weaponIndex + 1 ) % weapons.Count;
            var item = weapons[ps.weaponIndex];
            p.transform.Find("weapons/Image").GetComponent<Image>().sprite = item.GetComponent<IItem>().icon;
            ps.previousDirection = 1;
            ps.previousSwap = Time.time;
        }
        if(hv == 0 || Time.time - ps.previousSwap > 0.3f )
        {
            ps.previousDirection = 0;
            ps.previousSwap = Time.time;
        }

        if(CrossPlatformInputManager.GetButtonUp("p"+id+"Submit"))
        {
            if(ps.active)
            {
                Invoke("moveToGame", 1f);
                p.transform.Find("Text").GetComponent<Text>().text = "Starting!\nBack\nto cancel";
            }
            else
            {
                ps.active = true;
                if (id == 1)
                    p.transform.Find("Text").GetComponent<Text>().text = "Enter\nagain to\nstart";
                else
                    p.transform.Find("Text").GetComponent<Text>().text = "Start\nagain to\nstart";
            }
        }
        else if (CrossPlatformInputManager.GetButtonUp("p" + id + "Cancel"))
        {
            CancelInvoke();
            ps.active = false;
            p.transform.Find("Text").GetComponent<Text>().text = "Player 1\nenter\nto join";
        }
    }

    void moveToGame()
    {
        //TODO - fade out
        SceneManager.LoadScene("scene_henri_3_neon");
    }
}

public class PlayerSelections
{
    public bool active;
    public int weaponIndex;
    public int previousDirection;
    public float previousSwap;
    public PlayerSelections()
    {
        this.active = false;
        this.weaponIndex = 0;
        this.previousDirection = 0;
        this.previousSwap = 0;
    }
}