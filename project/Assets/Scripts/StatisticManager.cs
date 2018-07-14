using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticManager : MonoBehaviour {

    public enum EnemyType { easy, normal, hard };
    public enum Death { byJump, byEnemy, byPlayer };
    public enum Weapon { DualWield, Grenade, GrenadeHand, Mine, MiniGun, Pistol, Rocket, SawnShotgun, Shotgun };
    public enum Powerups { damageboost, health, invulnerability, shield, speed, weaponspeed };
    public enum Targets { player, enemy, spawner, obstacle };

    public class GameStatistics
    {
        public float totalGameTime = 0; //CC
		public int totalEnemyKills = 0; //cc
		public int totalSpawnerKills = 0; //cc
		public int totalPlayerKills = 0; //cc
		public int totalBulletsShot = 0; //cc by players and enemies
		public int totalShots = 0; //cc
		public int totalLevelsCompleted = 0;
    };
    
    public class PlayerStatistics
    {
        public Dictionary<EnemyType, int> enemyKills; //cc
		public int playerKills = 0; //cc
		public int spawnerKills = 0; //cc
		public float damageTaken = 0; //cc
		public float rawDamageTaken = 0; //cc
		public float damageDealt = 0; //cc
		public float rawDamageDealt = 0; //cc
        public Dictionary<Powerups, int> powerupsUsed; //cc
        public float hitpointsHealed = 0;
        public Dictionary<Death, int> deaths; //cc
		public int survivedLevels = 0;
		public int totalBulletsShot = 0; //cc
		public int totalShots = 0; //cc
        public Dictionary<Targets, int> totalHits; // sum = total shots hit
        public Dictionary<Weapon, float> weaponTimeUsed;
    };

    public static Dictionary<int, PlayerStatistics> playerStatistics;

    public static GameStatistics gameStatistics;

	// Use this for initialization
	void Start () {
            //Only create them if they dont exist.
            playerStatistics = new Dictionary<int, PlayerStatistics>();
            gameStatistics = new GameStatistics();

            for(int i = 1; i <= 4; ++i)
            {
                var ps = new PlayerStatistics();
                ps.enemyKills = new Dictionary<EnemyType, int>();
                ps.powerupsUsed = new Dictionary<Powerups, int>();

                ps.deaths = new Dictionary<Death, int>();
                ps.totalHits = new Dictionary<Targets, int>();
                ps.weaponTimeUsed = new Dictionary<Weapon, float>();
                playerStatistics.Add(i, ps);
            }
        init();
	}

    public static void init()
    {
        if (playerStatistics == null) return;
        foreach(PlayerStatistics ps in playerStatistics.Values)
        {

            ps.enemyKills.Add(EnemyType.easy, 0);
            ps.enemyKills.Add(EnemyType.normal, 0);
            ps.enemyKills.Add(EnemyType.hard, 0);

            ps.powerupsUsed.Add(Powerups.damageboost, 0);
            ps.powerupsUsed.Add(Powerups.health, 0);
            ps.powerupsUsed.Add(Powerups.invulnerability, 0);
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
        // save
        // init
    }

    public static void calculatePowerupStatistics(Player player, Powerups powerup)
    {
        playerStatistics[player.playerId].powerupsUsed[powerup]++;
    }

    public static void calculatePlayerDeathStatistics(Player player, Death cause)
    {
        playerStatistics[player.playerId].deaths[cause]++;
    }

    public static void calculateHitStatistics(Player player, Targets type, float increase)
    {
        playerStatistics[player.playerId].totalHits[type]++;
    }

    public static void calculateShotStatistics(Player player, int bulletAmount)
    {
        // player == null -> shot by enemy
        if(player != null)
        {
            playerStatistics[player.playerId].totalBulletsShot += bulletAmount;
            playerStatistics[player.playerId].totalShots += 1;
        }
        gameStatistics.totalShots += 1;
        gameStatistics.totalBulletsShot += bulletAmount;
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
                    if (p != null)
                    {
                        p.playerKills++;
                        calculatePlayerDeathStatistics(target.GetComponent<Player>(), Death.byPlayer);
                    }
                    else
                    {
                        calculatePlayerDeathStatistics(target.GetComponent<Player>(), Death.byEnemy);
                    }
                    gameStatistics.totalPlayerKills++;
                break;
        }
    }
}
