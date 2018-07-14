using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour {

	// Use this for initialization
	void Start () {
        ps1 = this.transform.Find("ps1").gameObject;
        ps2 = this.transform.Find("ps2").gameObject;
        ps3 = this.transform.Find("ps3").gameObject;
        ps4 = this.transform.Find("ps4").gameObject;
    }

    // Update is called once per frame
    void Update () {
		
	}
    private GameObject ps1;
    private GameObject ps2;
    private GameObject ps3;
    private GameObject ps4;

    private void OnEnable()
    {
        updateStats(ps1, 1);
        updateStats(ps2, 2);
        updateStats(ps3, 3);
        updateStats(ps4, 4);
    }

    void updateStats(GameObject go, int playerId)
    {
        if (go == null) return;
        var exists = false;
        if (playerId != 1)
            go.transform.Find("help/title").GetComponent<Text>().text = "Press back to return to menu";
        if(MainControl.activePlayers != null)
        {
            foreach(GameObject ap in MainControl.activePlayers)
            {
                if (ap.GetComponent<Player>().playerId == playerId)
                    exists = true;
            }
        }
        if (!exists)
        {
            go.SetActive(false);
            return;
        }
        var ekT = StatisticManager.playerStatistics[playerId].enemyKills[StatisticManager.EnemyType.easy] +
            StatisticManager.playerStatistics[playerId].enemyKills[StatisticManager.EnemyType.hard] +
            StatisticManager.playerStatistics[playerId].enemyKills[StatisticManager.EnemyType.normal];

        go.transform.Find("kills/value").GetComponent<Text>().text = ekT.ToString();
        go.transform.Find("spawner kills/value").GetComponent<Text>().text = StatisticManager.playerStatistics[playerId].spawnerKills.ToString();
        var dtT = StatisticManager.playerStatistics[playerId].damageTaken;
        var dtR = StatisticManager.playerStatistics[playerId].rawDamageTaken;
        go.transform.Find("damage taken/value").GetComponent<Text>().text = Mathf.Floor(dtT).ToString();
        go.transform.Find("damage blocked/value").GetComponent<Text>().text = Mathf.Floor(dtT - dtR).ToString();
        var dDT = StatisticManager.playerStatistics[playerId].damageDealt;
        var dDR = StatisticManager.playerStatistics[playerId].rawDamageDealt;
        go.transform.Find("damage dealt/value").GetComponent<Text>().text = Mathf.Floor(dDT).ToString();
        go.transform.Find("dealt damage blocked/value").GetComponent<Text>().text = Mathf.Floor(dDT-dDR).ToString();
        go.transform.Find("hit points healed/value").GetComponent<Text>().text = StatisticManager.playerStatistics[playerId].hitpointsHealed.ToString();
        go.transform.Find("total bullets shot/value").GetComponent<Text>().text = StatisticManager.playerStatistics[playerId].totalBulletsShot.ToString();
        go.transform.Find("total shots/value").GetComponent<Text>().text = StatisticManager.playerStatistics[playerId].totalShots.ToString();
        

        var powers = 0;
        foreach (int v in StatisticManager.playerStatistics[playerId].powerupsUsed.Values) powers+=v;
        go.transform.Find("powerups/value").GetComponent<Text>().text = powers.ToString();

        float hits = 0f;
        float totalShots = StatisticManager.playerStatistics[playerId].totalShots;
        
        foreach (float v in StatisticManager.playerStatistics[playerId].totalHits.Values) hits+=v;
        Debug.Log("Hits: " + hits + ", total: " + totalShots);
        if(totalShots==0)
            go.transform.Find("hitmissratio/value").GetComponent<Text>().text = "-";
        else
            go.transform.Find("hitmissratio/value").GetComponent<Text>().text = Mathf.Floor(hits/totalShots*100).ToString()+"%";

        float favorite = 0;
        StatisticManager.Weapon favWep = StatisticManager.Weapon.DualWield;
        
        foreach(StatisticManager.Weapon weapon in StatisticManager.playerStatistics[playerId].weaponTimeUsed.Keys)
        {
            var val = StatisticManager.playerStatistics[playerId].weaponTimeUsed[weapon];
            if(val > favorite)
            {
                favorite = val;
                favWep = weapon;
            }
        }
        go.transform.Find("favoriteweapon/value").GetComponent<Text>().text = "";// favWep.ToString();

        go.transform.Find("favoriteweapon/title").GetComponent<Text>().text = "";//
    }
}
