using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject[] entitiesList; // 0-7
    /*
     * 0 - Collectible
     * 1 - BigCannonEnenmy
     * 2 - BlackEnemy
     * 3 - BombEnemy
     * 4 - DiveBombEnemy1
     * 5 - DiveBombEnemy2
     * 6 - ShieldingEnemy
     * 7 - SpinningEnemy
     */

    public GameObject[] spinningEnemySpawners; // 0-3
    public GameObject[] collectibleSpawners; // 0-3
    public GameObject[] shieldEnemySpawners; // 0-1
    public GameObject[] blackEnemySpawner;
    public GameObject diveBombEnemy1Spawner;
    public GameObject diveBombEnemy2Spawner;
    public GameObject[] bombEnemySpawner;
    public GameObject bigCannonEnemySpawner;
    public GameObject planeBoss;
    public GameObject planeBossObject;
    public GameObject nukeBoss;
    public GameObject nukeBossObject;


    public GameObject player;
    public static int numberOfEnemies;
    private float timeTillEnemySpawn;
    public float enemySpawnCooldown = 3F;
    public float enemySpawnDifficulty;
    public float difficultyIncreaser = 30F;
    private float timeTillCollectible;
    public float collectibleCooldown = 15F;
    public static bool shieldOnField;
    public static bool countGreaterThanThousand;
    public static bool countGreaterThanTwo;

    // Start is called before the first frame update
    void Start()
    {
        shieldOnField = false;
    }

    // Update is called once per frame
    void Update()
    {
        //Collectible Spawn
        timeTillCollectible += 1 * Time.deltaTime;
        if ((timeTillCollectible >= collectibleCooldown) && ((AbsorbingShieldDetector.absorbingShieldAnimPlaying == false) && (OneHitShieldDetector.oneHitShieldAnimPlaying == false)
                && (FireModeDetector.fireModeAnimPlaying == false) && (FireSpeedDetector.fireSpeedAnimPlaying == false)))
        {
            SpawnCollectible();
        }
        else if ((timeTillCollectible >= collectibleCooldown) && ((AbsorbingShieldDetector.absorbingShieldAnimPlaying == true) || (OneHitShieldDetector.oneHitShieldAnimPlaying == true)
            || (FireModeDetector.fireModeAnimPlaying == true) || (FireSpeedDetector.fireSpeedAnimPlaying == true)))
        {
            timeTillCollectible = 0;
        }
        //Enemy Spawn

        timeTillEnemySpawn += 1 * Time.deltaTime;
        enemySpawnDifficulty += 1 * Time.deltaTime;
        if ((timeTillEnemySpawn >= enemySpawnCooldown) && (UIManager.isBossTime != true))
        {
            SpawnEnemy();
        }
        if ((enemySpawnDifficulty >= difficultyIncreaser) && (UIManager.isBossTime != true) && (enemySpawnCooldown >= 1.5F) && (!countGreaterThanThousand))
        {
            enemySpawnCooldown -= 0.3F;
            enemySpawnDifficulty = 0;
        }
        else if ((enemySpawnDifficulty >= difficultyIncreaser) && (UIManager.isBossTime != true) && (enemySpawnCooldown >= 1.0F) && (!countGreaterThanTwo))
        {
            enemySpawnCooldown -= 0.3F;
            enemySpawnDifficulty = 0;
        }
        else if ((enemySpawnDifficulty >= difficultyIncreaser) && (UIManager.isBossTime != true) && (enemySpawnCooldown >= 0.6F) && (countGreaterThanTwo))
        {
            enemySpawnCooldown -= 0.3F;
            enemySpawnDifficulty = 0;
        }
    }

    public float RandomNumber ()
    {
        float number = Random.Range(0, 10);
        return number;
    }

    void SpawnCollectible ()
    {
        Transform collectibleSpawnPoint = collectibleSpawners[(int)(Random.Range(0, 3))].transform;
        GameObject clone = Instantiate(entitiesList[0]) as GameObject;
        clone.transform.position = collectibleSpawnPoint.position;
        timeTillCollectible = 0;
    }

    public void SpawnEnemy ()
    {
        timeTillEnemySpawn = 0;
        float toSpawn = Random.Range(0F, 10F);

        if (toSpawn <= 4F)
        {
            int num = (int)Random.Range(0, 4);
            GameObject clone = Instantiate(entitiesList[3]) as GameObject;
            Transform position = bombEnemySpawner[num].transform;
            clone.transform.position = position.position;
            numberOfEnemies += 1;
        }
        else if (toSpawn <= 5F)
        {
            GameObject clone = Instantiate(entitiesList[5]) as GameObject;
            Transform position = diveBombEnemy2Spawner.transform;
            clone.transform.position = position.position;
            numberOfEnemies += 1;
        }
        else if (toSpawn <= 6F)
        {
            GameObject clone = Instantiate(entitiesList[1]) as GameObject;
            Transform position = bigCannonEnemySpawner.transform;
            clone.transform.position = position.position;
            numberOfEnemies += 1;
        }
        else if (toSpawn <= 7F)
        {
            int num = (int)Random.Range(0, 3);
            GameObject clone = Instantiate(entitiesList[2]) as GameObject;
            Transform position = blackEnemySpawner[num].transform;
            clone.transform.position = position.position;
            numberOfEnemies += 1;
        }
        else if (toSpawn <= 9F)
        {
            GameObject clone = Instantiate(entitiesList[4]) as GameObject;
            Transform position = diveBombEnemy1Spawner.transform;
            clone.transform.position = position.position;
            numberOfEnemies += 1;
        }
        else if (toSpawn <= 10F)
        {
            Transform spinSpawn = spinningEnemySpawners[(int)(Random.Range(0, 3))].transform;
            GameObject clone = Instantiate(entitiesList[7]) as GameObject;
            clone.transform.position = spinSpawn.position;
            numberOfEnemies += 1;
        }
        else if (toSpawn <= 11F)
        {
            Transform shieldSpawn = shieldEnemySpawners[(int)(Random.Range(0, 1))].transform;
            GameObject clone = Instantiate(entitiesList[6]) as GameObject;
            clone.transform.position = shieldSpawn.position;
            numberOfEnemies += 1;
        }
    }

    public void SpawnBoss(string boss)
    {
        if (boss == "plane")
        {
            Transform planeBossSpawn = planeBoss.transform;
            GameObject clone = Instantiate(planeBossObject) as GameObject;
            clone.transform.position = planeBossSpawn.position;
            enemySpawnCooldown = 2F;
        }

        else if (boss == "nuke")
        {
            Transform nukeBossSpawn = nukeBoss.transform;
            GameObject clone = Instantiate(nukeBossObject) as GameObject;
            clone.transform.position = nukeBossSpawn.position;
            enemySpawnCooldown = 2F;
        }
    }
}