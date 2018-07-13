using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;


public class ModifierScreen : MonoBehaviour {

    int modYpos = 0;
    int level = 0;
    bool isActive = false;

    private int previousDirectionH;
    private float previousSwapH;
    private int previousDirectionV;
    private float previousSwapV;

    private Transform active;
    private GameObject activeHilight;
    private Vector3 activeStart;
    // Use this for initialization
    void Start () {
        active = this.transform.Find("modifiers/active");
        activeHilight = active.Find("Image").gameObject;
        activeHilight.SetActive(false);
        activeStart = active.localPosition;
    }

    private string[] arenas = new string[] { "Arena!", "Arena mini!", "Mazed!" };
    private string[] arenaDescriptions = new string[] { "Battle in a classic\narena style\nsurvival challenge",
        "Space is very limited\nin this level.\nMind your step!", "Stuck in a maze?\nFind your\nway out." };
    private string[] arenaLevels = new string[] { "scene_henri_3_neon", "scene_henri_3_neon", "scene_henri_3_neon" };
    
    // Update is called once per frame
    void Update () {
        float hv = CrossPlatformInputManager.GetAxisRaw("p1Horizontal")
            +CrossPlatformInputManager.GetAxisRaw("p2Horizontal")
            + CrossPlatformInputManager.GetAxisRaw("p3Horizontal")
            +CrossPlatformInputManager.GetAxisRaw("p4Horizontal");
        float vv = CrossPlatformInputManager.GetAxisRaw("p1Vertical")+
            CrossPlatformInputManager.GetAxisRaw("p2Vertical")+
            CrossPlatformInputManager.GetAxisRaw("p3Vertical")+
            CrossPlatformInputManager.GetAxisRaw("p4Vertical");
        
        //Horizontal
        if (hv < -0.3 && previousDirectionH == 0)
        {
            //Swap left
            level--;
            if (level < 0) level += arenaDescriptions.Length;
            previousDirectionH = -1;
            previousSwapH = Time.time;
            loadLevel(level);
        }
        else if (hv > 0.3 && previousDirectionH == 0)
        {
            //Swap right
            level++;
            previousDirectionH = 1;
            previousSwapH = Time.time;
            loadLevel(level);
        }
        if (hv == 0 || Time.time - previousSwapH > 0.3f)
        {
            previousDirectionH = 0;
            previousSwapH = Time.time;
        }

        //Vertical
        if (vv > 0.3 && previousDirectionV == 0)
        {
            //Swap up
            previousDirectionV = -1;
            previousSwapV = Time.time;
            modYpos--;
            if (modYpos < 0) modYpos = 0;
        }
        else if (vv < -0.3 && previousDirectionV== 0)
        {
            //Swap down
            previousDirectionV = 1;
            previousSwapV = Time.time;
            modYpos++;
            if (modYpos > 4) modYpos = 4;
        }
        if (vv == 0 || Time.time - previousSwapV > 0.3f)
        {
            previousDirectionV = 0;
            previousSwapV = Time.time;
        }

        if(CrossPlatformInputManager.GetButton("p1Submit") ||
           CrossPlatformInputManager.GetButton("p2SelectItem1") ||
           CrossPlatformInputManager.GetButton("p3SelectItem1") ||
           CrossPlatformInputManager.GetButton("p4SelectItem1"))
        {

        }

        //Update state
        var v = active.localPosition;
        float target = activeStart.y - modYpos * 110f;
        v.y += (target - v.y) / 3;
        if (Mathf.Abs(v.y - target) < 0.2f)
            v.y = target;
        active.localPosition = v;
    }

    void loadLevel(int target)
    {
        var title = arenas[target % arenas.Length];
        var targetScene = arenaLevels[target % arenas.Length];
        var description = arenaDescriptions[target % arenas.Length];

        transform.Find("level/info/title").GetComponent<Text>().text = title;
        transform.Find("level/info/description").GetComponent<Text>().text = description;

    }
}
