using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticManager : MonoBehaviour {

    public enum EnemyType { easy, normal, hard };
    public enum Death { byJump, byEnemy, byPlayer };
    public enum Weapon { DualWield, Grenade, GrenadeHand, Mine, MiniGun, Pistol, Rocket, SawnShotgun, Shotgun };
    public enum Powerups { damage, health, shield, speed, weaponspeed };
    public enum Targets { player, enemy, spawner, obstacle };

    public class GameStatistics
    {
        public float totalGameTime=0; //CC
        public int totalEnemyKills = 0; //c
        public int totalSpawnerKills = 0; //c
        public int totalPlayerKills = 0; //c
        public int totalBulletsShot = 0; // by players and enemies
        public int totalShots = 0;
        public int totalLevelsCompleted = 0;
    };

    public class PlayerStatistics
    {
        public Dictionary<EnemyType, int> enemyKills; //c
        public int playerKills = 0; //c
        public int spawnerKills = 0; //c
        public float damageTaken = 0; //c
        public float rawDamageTaken = 0; //c
        public float damageDealt = 0; //c
        public float rawDamageDealt = 0; //c
        public Dictionary<Powerups, int> powerupsCollected;
        public Dictionary<Powerups, int> powerupsUsed;
        public float hitpointsHealed = 0;
        public Dictionary<Death, int> deaths;
        public int survivedLevels = 0;
        public int totalBulletsShot = 0;
        public int totalShots = 0;
        public Dictionary<Targets, int> totalHits; // sum = total shots hit //c
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
            ps.enemyKills = new Dictionary<EnemyType, int>();
            ps.powerupsCollected = new Dictionary<Powerups, int>();
            ps.powerupsUsed = new Dictionary<Powerups, int>();

            ps.deaths = new Dictionary<Death, int>();
            ps.totalHits = new Dictionary<Targets, int>();
            ps.weaponTimeUsed = new Dictionary<Weapon, float>();
            playerStatistics.Add(i, ps);
        }
        init();
	}

    void init()
    {
        foreach(PlayerStatistics ps in playerStatistics.Values)
        {
            ps.enemyKills.Add(EnemyType.easy, 0);
            ps.enemyKills.Add(EnemyType.normal, 0);
            ps.enemyKills.Add(EnemyType.hard, 0);

            ps.powerupsCollected.Add(Powerups.damage, 0);
            ps.powerupsCollected.Add(Powerups.health, 0);
            ps.powerupsCollected.Add(Powerups.shield, 0);
            ps.powerupsCollected.Add(Powerups.speed, 0);
            ps.powerupsCollected.Add(Powerups.weaponspeed, 0);

            ps.powerupsUsed.Add(Powerups.damage, 0);
            ps.powerupsUsed.Add(Powerups.health, 0);
            ps.powerupsUsed.Add(Powerups.shield, 0);
            ps.powerupsUsed.Add(Powerups.speed, 0);
            ps.powerupsUsed.Add(Powerups.weaponspeed, 0);

            ps.deaths.Add(Death.byEnemy, 0);
            ps.deaths.Add(Death.byJump, 0);
            ps.deaths.Add(Death.byPlayer, 0);

            ps.totalHits.Add(Targets.enemy, 0);
            ps.totalHits.Add(Targets.obstacle, 0);
            ps.totalHits.Add(Targets.player, 0);
            ps.totalHits.Add(Targets.spawner, 0);

            ps.weaponTimeUsed.Add(Weapon.DualWield, 0);
            ps.weaponTimeUsed.Add(Weapon.Grenade, 0);
            ps.weaponTimeUsed.Add(Weapon.GrenadeHand, 0);
            ps.weaponTimeUsed.Add(Weapon.Mine, 0);
            ps.weaponTimeUsed.Add(Weapon.MiniGun, 0);
            ps.weaponTimeUsed.Add(Weapon.Pistol, 0);
            ps.weaponTimeUsed.Add(Weapon.Rocket, 0);
            ps.weaponTimeUsed.Add(Weapon.SawnShotgun, 0);
            ps.weaponTimeUsed.Add(Weapon.Shotgun, 0);

        }
    }

    // Update is called once per frame
    void Update () {
		
	}

    void Reset ()
    {

    }

    public static void calculateDamageStatistics(IAmmunition ammo, GameObject target, float damage, float rawDamage)
    {
        if (ammo.shooter != null && ammo.shooter.tag == "Player")
        {
            //p = this.playerStatistics[ammo.shooter.GetComponent<Player>().playerId];
            playerStatistics[ammo.shooter.GetComponent<Player>().playerId].damageDealt += damage; //c
            playerStatistics[ammo.shooter.GetComponent<Player>().playerId].rawDamageDealt += rawDamage; //c
        }

        if (target.tag == "Player")
        {
            playerStatistics[target.GetComponent<Player>().playerId].damageTaken += damage; //c
            playerStatistics[target.GetComponent<Player>().playerId].rawDamageTaken += rawDamage; //c
        }
    }

    public static void calculateKillStatistics(IAmmunition ammo, GameObject target)
    {
        PlayerStatistics p = null;
        if (ammo.shooter != null && ammo.shooter.tag == "Player")
        {
            p = playerStatistics[ammo.shooter.GetComponent<Player>().playerId];
        }

        // Target killed ++ //c
        switch (target.tag)
        {
            case "Enemy":
                if (p != null) p.enemyKills[target.GetComponent<Enemy>().type]++;
                gameStatistics.totalEnemyKills++;
                break;
            case "spawner":
                if (p != null) p.spawnerKills++;
                gameStatistics.totalSpawnerKills++;
                break;
            case "Player":
                if (p != null) p.playerKills++;
                gameStatistics.totalPlayerKills++;
                break;
        }
    }
}
