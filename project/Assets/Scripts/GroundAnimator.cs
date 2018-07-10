using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAnimator : MonoBehaviour {

    private static int WIDTH = 100;
    private static int HEIGHT = 100;

    public List<GameObject> tiles;

    private List<GameObject> grid;

	// Use this for initialization
	void Start () {
        grid = new List<GameObject>();
        for (int i = 0; i < HEIGHT; i++)
        {
            for(int j = 0; j < WIDTH; j++)
            {
                var pre = tiles[(int)Mathf.Floor(UnityEngine.Random.value * tiles.Count)];
                var go = Instantiate(pre, new Vector3((i-HEIGHT/2)*3, 0, (j-WIDTH/2)*3), this.transform.rotation, this.transform );
                grid.Add(go);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
		foreach(GameObject tile in grid)
        {
            //Calculate nearest player
            var dMin = 999999999f;
            GameObject near = null;
            foreach(GameObject player in MainControl.activePlayers)
            {
                var d = Vector3.Magnitude(player.transform.position - tile.transform.position);
                if(d < dMin)
                {
                    dMin = d;
                    near = player;
                }
            }
            /*
            tile.transform.position = new Vector3(tile.transform.position.x, 
                                                 -Mathf.Clamp(dMin-15f, 0f, 25f), 
                                                  tile.transform.position.z);
            tile.transform.GetComponent<MeshRenderer>().enabled = tile.transform.position.y > -15f;*/
        }
	}
}
