using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ModifierScreen : MonoBehaviour {

    public Sprite preview1;
    public Sprite preview2;
    public Sprite preview3;


    public GameObject playerscreen;

    int modYpos = 0;
    int level = 0;
    int difficulty = 2;
    int speed = 2;
    bool isActive = false;

    private int previousDirectionH;
    private float previousSwapH;
    private int previousDirectionV;
    private float previousSwapV;

    private Transform active;
    private GameObject activeHilight;
    private Vector3 activeStart;

    private Transform difficultySlider;
    private Transform speedSlider;

    public float exitTime;
    public float exitStart;
    public bool exiting;
    

    // Use this for initialization
    void Start () {
        this.difficultySlider = this.transform.Find("modifiers/mod_difficulty/slider");
        this.speedSlider = this.transform.Find("modifiers/mod_speed/slider");
        active = this.transform.Find("modifiers/active");
        activeHilight = active.Find("Image").gameObject;
        activeHilight.SetActive(false);
        activeStart = active.localPosition;
        arenaImages = new Sprite[] { preview1, preview2, preview3 };
    }

    private string[] arenas = new string[] { "Arena!", "Arena mini!", "Mazed!" };
    private string[] arenaDescriptions = new string[] { "Battle in a classic\narena style\nsurvival challenge",
        "Space is very limited\nin this level.\nMind your step!", "Stuck in a maze?\nFind your\nway out." };
    private string[] arenaLevels = new string[] { "scene_henri_3_neon", "scene_henri_3_neon", "scene_henri_3_neon" };
    private Sprite[] arenaImages;

    // Update is called once per frame
    void Update () {

        if (exiting)
        {
            this.transform.Find("fadeout").GetComponent<Image>().color = new Color(0f, 0f, 0f, Mathf.Clamp((Time.time - exitStart) / exitTime, 0f, 1f));
            this.transform.Find("fadeout/Text").GetComponent<Text>().color = new Color(1f, 1f, 1f, Mathf.Clamp((Time.time - exitStart - 0.75f) / (exitTime - 0.75f), 0f, 1f));
        }
        else
        {
            this.transform.Find("fadeout").GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
            this.transform.Find("fadeout/Text").GetComponent<Text>().color = new Color(1f, 1f, 1f, 0f);
        }

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
            if(isActive)
            {
                if(modYpos == 2)
                {
                    difficulty--;
                    if (difficulty < 0)
                        difficulty = 0;
                }
                else if(modYpos == 3)
                {
                    speed--;
                    if (speed < 0)
                        speed = 0;
                }
            }
            else
            {
                level--;
                if (level < 0) level += arenaDescriptions.Length;
            }
            previousDirectionH = -1;
            previousSwapH = Time.time;
            loadLevel(level);
        }
        else if (hv > 0.3 && previousDirectionH == 0)
        {
            //Swap right
            if(isActive)
            {
                if (modYpos == 2)
                {
                    difficulty++;
                    if (difficulty > 4)
                        difficulty = 4;
                }
                else if (modYpos == 3)
                {
                    speed++;
                    if (speed > 4)
                        speed = 4;
                }
            }
            else
            {
                level++;
            }
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
            isActive = false;
        }
        else if (vv < -0.3 && previousDirectionV== 0)
        {
            //Swap down
            previousDirectionV = 1;
            previousSwapV = Time.time;
            modYpos++;
            if (modYpos > 4) modYpos = 4;
            isActive = false;
        }
        if (vv == 0 || Time.time - previousSwapV > 0.3f)
        {
            previousDirectionV = 0;
            previousSwapV = Time.time;
        }

        if((CrossPlatformInputManager.GetButtonUp("p1Submit") ||
           CrossPlatformInputManager.GetButtonUp("p2SelectItem1") ||
           CrossPlatformInputManager.GetButtonUp("p3SelectItem1") ||
           CrossPlatformInputManager.GetButtonUp("p4SelectItem1")))
        {
            if((modYpos == 2 || modYpos == 3))
                isActive = !isActive;
            else if(modYpos == 0)
            {
                GameConfig.friendlyFire = !GameConfig.friendlyFire;
            }
            else if(modYpos == 1)
            {
                GameConfig.allowPowerups = !GameConfig.allowPowerups;
            }
            else if(modYpos == 4)
            {
                Invoke("moveToGame", 1f);
                fadeOut();
            }
        }
        if ((CrossPlatformInputManager.GetButtonUp("p1Cancel") ||
           CrossPlatformInputManager.GetButtonUp("p2Cancel") ||
           CrossPlatformInputManager.GetButtonUp("p3Cancel") ||
           CrossPlatformInputManager.GetButtonUp("p4Cancel")))
        {
            playerscreen.SetActive(true);
            this.gameObject.SetActive(false);
        }

        //Update mod state
        var v = active.localPosition;
        float target = activeStart.y - modYpos * 110f;
        v.y += (target - v.y) / 3;
        if (Mathf.Abs(v.y - target) < 0.2f)
            v.y = target;
        active.localPosition = v;

        //Update active
        activeHilight.SetActive(isActive);

        //Update data
        v = this.speedSlider.localPosition;
        v.x = 120f + 40 * speed;
        this.speedSlider.localPosition = v;

        v = this.difficultySlider.localPosition;
        v.x = 120f + 40 * difficulty;
        this.difficultySlider.localPosition = v;

        GameConfig.difficulty = difficultyValues[difficulty];
        GameConfig.speedMultiplier = speedValues[speed];

        this.transform.Find("modifiers/mod_friendly_fire/active").GetComponent<Image>().enabled = GameConfig.friendlyFire;
        this.transform.Find("modifiers/mod_allow_powerups/active").GetComponent<Image>().enabled = GameConfig.allowPowerups;
    }
    float[] difficultyValues = { 0.5f, 0.85f, 1f, 1.3f, 1.7f };
    float[] speedValues = { 0.8f, 0.9f, 1f, 1.1f, 1.2f };

    void loadLevel(int target)
    {
        var title = arenas[target % arenas.Length];
        var description = arenaDescriptions[target % arenas.Length];

        transform.Find("level/info/title").GetComponent<Text>().text = title;
        transform.Find("level/info/description").GetComponent<Text>().text = description;
        transform.Find("level/info/Image").GetComponent<Image>().sprite = arenaImages[target%arenaImages.Length];
    }

    void moveToGame()
    {
        var targetScene = arenaLevels[level % arenas.Length];
        SceneManager.LoadScene(targetScene);
    }

    void fadeOut()
    {
        exiting = true;
        exitStart = Time.time;
    }
}
