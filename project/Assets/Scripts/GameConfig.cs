using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : MonoBehaviour {
    //These values get adjusted from custom settings.
    public static bool friendlyFire = false;
    public static float difficulty = 1f;
    public static float speedMultiplier = 1f;



    //This variable is adjusted by the number of players & difficulty
    public static float difficultyMultiplier = 1f;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
