using System;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public enum PrefabType
    {
        ENEMY_SMALL, 
        ENEMY_MEDIUM,
        ENEMY_LARGE,
        ENEMY_BOSS,
        ITEM_COIN, 
        ITEM_POWER, 
        ITEM_BOOM, 
        PLAYER_BULLET_A, 
        PLAYER_BULLET_B, 
        ENEMY_BULLET_A,
        ENEMY_BULLET_B,
        ENEMY_BULLET_C,
        ENEMY_BULLET_D,
        FOLLOWER_BULLET,
    }

    public GameObject enemySmallPrefab;
    public GameObject enemyMediumPrefab;
    public GameObject enemyLargePrefab;
    public GameObject enemyBossPrefab;
    public GameObject itemCoinPrefab;
    public GameObject itemPowerPrefab;
    public GameObject itemBoomPrefab;
    public GameObject playerBulletObjectAPrefab;
    public GameObject playerBulletObjectBPrefab;
    public GameObject enemyBulletObjectAPrefab;
    public GameObject enemyBulletObjectBPrefab;
    public GameObject enemyBulletObjectCPrefab;
    public GameObject enemyBulletObjectDPrefab;
    public GameObject followerBulletObjectAPrefab;

    GameObject[] enemySmallPool;
    GameObject[] enemyMediumPool;
    GameObject[] enemyLargePool;
    GameObject[] enemyBossPool;
    GameObject[] itemCoinPool;
    GameObject[] itemPowerPool;
    GameObject[] itemBoomPool;
    GameObject[] playerBulletAPool;
    GameObject[] playerBulletBPool;
    GameObject[] enemyBulletAPool;
    GameObject[] enemyBulletBPool;
    GameObject[] enemyBulletCPool;
    GameObject[] enemyBulletDPool;
    GameObject[] followerBulletAPool;

    private void Awake()
    {
        InitializePool();
    }

    GameObject GetPrefab(PrefabType gameObjectType)
    {
        switch (gameObjectType)
        {
            case PrefabType.ENEMY_SMALL:
                return enemySmallPrefab;
            case PrefabType.ENEMY_MEDIUM:
                return enemyMediumPrefab;
            case PrefabType.ENEMY_LARGE:
                return enemyLargePrefab;
            case PrefabType.ENEMY_BOSS:
                return enemyBossPrefab;
            case PrefabType.ITEM_COIN:
                return itemCoinPrefab;
            case PrefabType.ITEM_POWER:
                return itemPowerPrefab;
            case PrefabType.ITEM_BOOM:
                return itemBoomPrefab;
            case PrefabType.PLAYER_BULLET_A:
                return playerBulletObjectAPrefab;
            case PrefabType.PLAYER_BULLET_B:
                return playerBulletObjectBPrefab;
            case PrefabType.ENEMY_BULLET_A:
                return enemyBulletObjectAPrefab;
            case PrefabType.ENEMY_BULLET_B:
                return enemyBulletObjectBPrefab;
            case PrefabType.ENEMY_BULLET_C:
                return enemyBulletObjectCPrefab;
            case PrefabType.ENEMY_BULLET_D:
                return enemyBulletObjectDPrefab;
            case PrefabType.FOLLOWER_BULLET:
                return followerBulletObjectAPrefab;
        }

        throw new Exception("No Prefab for " + gameObjectType);
    }

    GameObject[] GetPool(PrefabType gameObjectType)
    {
        switch (gameObjectType)
        {
            case PrefabType.ENEMY_SMALL:
                return enemySmallPool;
            case PrefabType.ENEMY_MEDIUM:
                return enemyMediumPool;
            case PrefabType.ENEMY_LARGE:
                return enemyLargePool;
            case PrefabType.ENEMY_BOSS:
                return enemyBossPool;
            case PrefabType.ITEM_COIN:
                return itemCoinPool;
            case PrefabType.ITEM_POWER:
                return itemPowerPool;
            case PrefabType.ITEM_BOOM:
                return itemBoomPool;
            case PrefabType.PLAYER_BULLET_A:
                return playerBulletAPool;
            case PrefabType.PLAYER_BULLET_B:
                return playerBulletBPool;
            case PrefabType.ENEMY_BULLET_A:
                return enemyBulletAPool;
            case PrefabType.ENEMY_BULLET_B:
                return enemyBulletBPool;
            case PrefabType.ENEMY_BULLET_C:
                return enemyBulletCPool;
            case PrefabType.ENEMY_BULLET_D:
                return enemyBulletDPool;
            case PrefabType.FOLLOWER_BULLET:
                return followerBulletAPool;
        }

        throw new Exception("No ObjectPool for " + gameObjectType);
    }

    private void InitializePool()
    {
        enemySmallPool = new GameObject[20];
        enemyMediumPool = new GameObject[10];
        enemyLargePool = new GameObject[10];
        enemyBossPool = new GameObject[3];
        itemCoinPool = new GameObject[20];
        itemPowerPool = new GameObject[10];
        itemBoomPool = new GameObject[10];
        playerBulletAPool = new GameObject[100];
        playerBulletBPool = new GameObject[100];
        enemyBulletAPool = new GameObject[100];
        enemyBulletBPool = new GameObject[100];
        enemyBulletCPool = new GameObject[1000];
        enemyBulletDPool = new GameObject[50];
        followerBulletAPool = new GameObject[100];

        foreach (PrefabType gameObjectType in Enum.GetValues(typeof(PrefabType)))
        {
            InstantiateData(GetPool(gameObjectType), GetPrefab(gameObjectType));
        }
    }

    private void InstantiateData(GameObject[] pool, GameObject prefab)
    {
        for (int index = 0; index < pool.Length; index++)
        {
            pool[index] = Instantiate(prefab);
            pool[index].SetActive(false);
        }
    }

    public GameObject GetObject(PrefabType gameObjectType, Vector3 position)
    {
        GameObject[] pool = GetPool(gameObjectType);
        for (int index = 0; index < pool.Length; index++)
        {
            if (!pool[index].activeSelf)
            {
                pool[index].SetActive(true);
                pool[index].transform.position = position;
                pool[index].transform.rotation = Quaternion.identity;
                return pool[index];
            }
        }

        return null;
    }

    public void BoomDamageToAllEnemies()
    {
        BoomDamangeToEnemyByPrefabType(PrefabType.ENEMY_SMALL);
        BoomDamangeToEnemyByPrefabType(PrefabType.ENEMY_MEDIUM);
        BoomDamangeToEnemyByPrefabType(PrefabType.ENEMY_LARGE);
        BoomDamangeToEnemyByPrefabType(PrefabType.ENEMY_BOSS);
    }

    private void BoomDamangeToEnemyByPrefabType(PrefabType prefabType)
    {
        GameObject[] pool = GetPool(prefabType);
        for (int index = 0; index < pool.Length; index++)
        {
            if (pool[index].activeSelf)
            {
                Enemy enemy = pool[index].GetComponent<Enemy>();
                enemy.OnHit(1000);
            }
        }
    }

    public void DestroyAllEnemyBullets()
    {
        DestroyEnemyBulletsByPrefabType(PrefabType.ENEMY_BULLET_A);
        DestroyEnemyBulletsByPrefabType(PrefabType.ENEMY_BULLET_B);
        DestroyEnemyBulletsByPrefabType(PrefabType.ENEMY_BULLET_C);
        DestroyEnemyBulletsByPrefabType(PrefabType.ENEMY_BULLET_D);
    }

    private void DestroyEnemyBulletsByPrefabType(PrefabType prefabType)
    {
        GameObject[] pool = GetPool(prefabType);
        for (int index = 0; index < pool.Length; index++)
        {
            if (pool[index].activeSelf)
                pool[index].SetActive(false);
        }
    }
}
