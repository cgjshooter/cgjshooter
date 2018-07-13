using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticManager : MonoBehaviour {

    public enum Enemy { easy, normal, hard };
    public enum Death { byJump, byEnemy, byPlayer };
    public enum Weapon { DualWield, Grenade, GrenadeHand, Mine, MiniGun, Pistol, Rocket, SawnShotgun, Shotgun };
    public enum Powerups { damage, health, shield, speed, weaponspeed };
    public enum Targets { player, enemy, spawner, obstacle };

    public class GameStatistics
    {
        public float totalGameTime; //c
        public int totalEnemyKills; //c
        public int totalSpawnerKills; //c
        public int totalPlayerKills; //c
        public int totalBulletsShot; // by players and enemies
        public int totalShots;
        public int totalLevelsCompleted;
    };

    public class PlayerStatistics
    {
        public Dictionary<Enemy, int> enemyKills; //c
        public int playerKills; //c
        public int spawnerKills; //c
        public float damageTaken;
        public float damageDealt;
        public Dictionary<Powerups, int> powerupsCollected;
        public Dictionary<Powerups, int> powerupsUsed;
        public float hitpointsHealed;
        public Dictionary<Death, int> deaths;
        public int survivedLevels;
        public int totalBulletsShot;
        public int totalShots;
        public Dictionary<Targets, int> totalHits; // sum = total shots hit
        public Dictionary<Weapon, float> weaponTimeUsed;
    };

    public static Dictionary<int, PlayerStatistics> playerStatistics;
    public static GameStatistics gameStatistics;

	// Use this for initialization
	void Start () {
        DontDestroyOnLoad(this.gameObject);
        playerStatistics = new Dictionary<int, PlayerStatistics>();
        gameStatistics = new GameStatistics();

        for(int i = 1; i <= 4; ++i)
        {
            var ps = new PlayerStatistics();
            ps.enemyKills = new Dictionary<Enemy, int>();
            ps.powerupsCollected = new Dictionary<Powerups, int>();
            ps.powerupsUsed = new Dictionary<Powerups, int>();
            ps.deaths = new Dictionary<Death, int>();
            ps.totalHits = new Dictionary<Targets, int>();
            ps.weaponTimeUsed = new Dictionary<Weapon, float>();
            playerStatistics.Add(i, ps);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Reset ()
    {

    }
}
